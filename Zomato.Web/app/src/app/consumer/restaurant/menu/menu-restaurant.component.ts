import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from "@angular/core";
import { MenuService } from '../../../service/menu.service';
import { Subscription, Observable, Observer, interval } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { Menu } from '../../../model/menu';
import { CartService } from '../../../service/cart.service';
import { ShoppingCart } from '../../../model/shopping-cart';
import { ICartItemWithItems } from '../../../model/ICartItemWithItems';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './menu-restaurant.component.html',
  styleUrls: ['./menu-restaurant.component.css']
})

export class MenuComponent implements OnInit, OnDestroy {

  cart: Observable<ShoppingCart>
  cartItems: ICartItemWithItems[];
  menus: any[]=[];
  cartSubscription; menuSubscription; updateMenuSubscription: Subscription;
  menuList: any[];
  restaurantName: string;
  public itemCount: number;

  constructor(private menuService: MenuService, private router: Router,
    private cartService: CartService) {
    this.restaurantName = this.router.url.split('/')[2];
    this.cartService.getMenuList(this.restaurantName);
  }

  ngOnInit(): void {
    this.loadMenuList();
    this.loadCart();
    this.updateMenuSubscription = interval(10000).subscribe(
      (val) => {
        this.loadMenuList();
      }
    );
  }

  ngOnDestroy(): void {
    if (this.menuSubscription) {
      this.menuSubscription.unsubscribe();
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
    if (this.updateMenuSubscription) {
      this.updateMenuSubscription.unsubscribe();
    }
  }

  loadMenuList(): void {
    this.menuSubscription = this.menuService.getMenuList(this.restaurantName).subscribe(
      res => {
        if (res != null) {
          this.menuList = res as any[];
          for (let each of this.menuList) {
            for (let menu of each.menus) {
              this.menus.push(menu);
            }
          }
        }
      }, err => {
        console.log(err);
      }
    );
  }

  loadCart(): void {
    this.cart = this.cartService.get();
    this.cartSubscription = this.cart.subscribe((cart) => {
      this.itemCount = cart.items.map((x) => x.ItemQuantity).reduce((p, n) => p + n, 0);
      this.menuService.getMenuList(this.restaurantName).subscribe((res) => {
        let m = res as any[];
        this.menus = [];
        for (let each of m) {
          for (let menu of each.menus) {
            this.menus.push(menu);
          }
        }
        this.cartItems = cart.items.map((item) => {
          const menu = this.menus.find((p) => p.itemId === item.ItemId);
          if (menu == undefined) {
            this.cartService.empty();
          } else {
          return {
            ...item, menu, totalCost: menu.itemPrice * item.ItemQuantity
            };
          }
        });
      });
    });
  }

  addItemToOrder(menu: Menu) {
    //this.cartService.addMenuItem(menu, 1);
    this.cartService.addMenuItem(menu, this.restaurantName, 1);
  }

  removeItemFromCart(menu: Menu): void {
    //this.cartService.addMenuItem(menu, -1);
    this.cartService.addMenuItem(menu, this.restaurantName, -1);
  }

  emptyCart(): void {
    this.cartService.empty();
  }

  itemInCart(menu: any): boolean {
    var a = Observable.create((obs: Observer<boolean>) => {
      const sub = this.cartService
        .get()
        .subscribe((cart) => {
          obs.next(cart.items.some((i) => i.ItemId === menu.itemId));
          obs.complete();
        });
      sub.unsubscribe();
    });
    return a;
  }

  openCheckout() {
    this.router.navigateByUrl('restaurant/' + this.restaurantName+"/checkout")
  }

  //hyphenateUrlParams(str: string) {
  //  return str.replace(' ', '-');
  //}
}

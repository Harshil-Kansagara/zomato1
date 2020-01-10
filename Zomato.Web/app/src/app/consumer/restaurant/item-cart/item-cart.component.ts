import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from "@angular/core";
import { MenuService } from '../../../service/menu.service';
import { Observable, Subscription } from 'rxjs';
import { ShoppingCart } from '../../../model/shopping-cart';
import { CartService } from '../../../service/cart.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-item-cart",
  templateUrl: "./item-cart.component.html",
  styleUrls: ['./item-cart.component.css']
})

export class ItemCartComponent implements OnInit, OnDestroy {

  public cart: Observable<ShoppingCart>;
  public itemCount: number;
  public cartSubscription: Subscription;

  constructor(private cartService: CartService) { }

  ngOnDestroy(): void {
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.cart = this.cartService.get();
    this.cartSubscription = this.cart.subscribe((cart) => {
      this.itemCount = cart.items.map((x) => x.ItemQuantity).reduce((p, n) => p + n, 0);
    });
  }

  emptyCart(): void {
    this.cartService.empty();
  }

}

import { Component, OnInit, OnDestroy} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { ShoppingCart } from '../../../model/shopping-cart';
import { ICartItemWithItems } from '../../../model/ICartItemWithItems';
import { OrderService } from '../../../service/order.service';
import { OrderDetail } from '../../../model/order-detail';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './order-confirm.component.html',
  styleUrls: ['./order-confirm.component.css']
})

export class OrderConfirmComponent implements OnInit, OnDestroy{
  cart: Observable<ShoppingCart>
  cartItems: ICartItemWithItems[];
  restaurantName/*; display*/: string;
  menus: any[] = [];
  cartSubscription: Subscription;
  itemCount: number = 0;
  itemsTotal: number
  orderId: number;
  orderSubscritpion: Subscription;
  orderDetail = new OrderDetail();

  constructor(private activatedRoute: ActivatedRoute, private router: Router,
    private orderService: OrderService, private toastr: ToastrService) {
    this.activatedRoute.params.subscribe(params => {
      this.orderId = parseInt(params.orderId.replace('order', ''));
      this.restaurantName = params.restaurantName;
    });
  }

  ngOnInit(): void {
   //this.loadCart();
    this.loadOrderUserData();
  }

  ngOnDestroy(): void {
    if (this.orderSubscritpion) {
      this.orderSubscritpion.unsubscribe();
    }
  }

  loadOrderUserData() {
    this.orderSubscritpion = this.orderService.getOrderDetail(this.orderId).subscribe(
      res => {
        if (res != null) {
          this.orderDetail = res as OrderDetail;
          for (let each of this.orderDetail.itemDetail) {
            this.itemCount = this.itemCount + each.itemQuantity;
          }
        } //setTimeout(() => { this.display = "Delivered Succefully" }, 8000)
      }, err => {
        console.log(err);
      });
  }

  //loadCart(): void {
  //  this.cart = this.cartService.get();
  //  this.cartSubscription = this.cart.subscribe((cart) => {
  //   // this.itemCount = cart.items.map((x) => x.ItemQuantity).reduce((p, n) => p + n, 0);
  //    this.menuService.getMenuList(this.restaurantName).subscribe((res) => {
  //      let m = res as any[];
  //      for (let each of m) {
  //        for (let menu of each.menus) {
  //          this.menus.push(menu);
  //        }
  //      }
  //      this.cartItems = cart.items.map((item) => {
  //        const menu = this.menus.find((p) => p.itemId === item.ItemId);
  //        return {
  //          ...item, menu, totalCost: menu.itemPrice * item.ItemQuantity
  //        };
  //      });
  //    });
  //  });
  //}

  back(): void {
    this.router.navigateByUrl("/restaurant");
  }

  cancel(): void {
    let promise = this.orderService.deleteOrder(this.orderId).subscribe(
      res => {
        this.toastr.success("Order cancelled successfully");
        this.router.navigateByUrl("/restaurant");
        promise.unsubscribe();
      }, err => {
        console.log(err);
      }
    );
  }
}

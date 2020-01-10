import { Component, OnInit, OnDestroy, Inject } from "@angular/core";
import { Observable, Subscription } from 'rxjs';
import { ShoppingCart } from '../../../model/shopping-cart';
import { CartItem } from '../../../model/cart-item';
import { Menu } from '../../../model/menu';
import { MenuService } from '../../../service/menu.service';
import { CartService } from '../../../service/cart.service';
import { Router } from '@angular/router';
import { UserAddressService } from '../../../service/user-address.service';
import { UserAddress } from '../../../model/userAddress';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import * as jwt_decode from 'jwt-decode';
import { ICartItemWithItems } from '../../../model/ICartItemWithItems';
import { Order } from '../../../model/order';
import { OrderService } from '../../../service/order.service';
import { OrderNotificationService } from '../../../service/order-notification.service';

@Component({
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})

export class CheckoutComponent implements OnInit, OnDestroy {
  cart: Observable<ShoppingCart>
  cartItems: ICartItemWithItems[];
  itemCount: number;
  restaurantName; token; location; userId; decode_token: string = " ";
  menus: any[]= [];
  cartSubscription; addressSubscription: Subscription;
  addressList: UserAddress[];
  order: Order;

  constructor(private menuService: MenuService, private router: Router, private toastr: ToastrService,
    private cartService: CartService, public userAddressService: UserAddressService,
    public dialog: MatDialog, private orderService: OrderService) {

    this.restaurantName = this.router.url.split('/')[2];
  }

  ngOnInit(): void {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      this.decode_token = jwt_decode(this.token)
      this.userId = this.decode_token['UserId'];
    }
    this.addressDataList();
    this.loadCart();
    this.order = this.orderService.initializeOrder();
  }

  ngOnDestroy(): void {
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
    if (this.addressSubscription) {
      this.addressSubscription.unsubscribe();
    }
  }

  loadCart(): void {
    this.cart = this.cartService.get();
    this.cartSubscription = this.cart.subscribe((cart) => {
      this.itemCount = cart.items.map((x) => x.ItemQuantity).reduce((p, n) => p + n, 0);
      this.menuService.getMenuList(this.restaurantName).subscribe((res) => {
        let m = res as any[];
        for (let each of m) {
          for (let menu of each.menus) {
            this.menus.push(menu);
          }
        }
        this.cartItems = cart.items.map((item) => {
          const menu = this.menus.find((p) => p.itemId === item.ItemId);
          return {
            ...item, menu, totalCost: menu.itemPrice * item.ItemQuantity
          };
        });
      });
    });
  }

  public emptyCart(): void {
    this.cartService.empty();
    this.router.navigateByUrl('restaurant/' + this.restaurantName);
  }

  //hyphenateUrlParams(str: string) {
  //  return str.replace(' ', '-');
  //}

  placeOrder(): void {
    this.cart.subscribe((cart) => {
      this.order.addressId = cart.deliveryAddressId;
      this.order.userId = this.userId;
      //this.order.restaurantName = this.restaurantName;
      for (let each of this.cartItems) {
        let item = new CartItem();
        item.ItemId = each.ItemId;
        item.ItemQuantity = each.ItemQuantity;
        this.order.items.push(item);
      }
    });
    //this.orderNotificationService.sendOrder(this.order);
    this.orderService.addOrder(this.restaurantName, this.order).subscribe(
      (res:any) => {
        if (res != null) {
          this.cartService.empty();
          this.router.navigateByUrl("restaurant/" + this.restaurantName + "/confirm/order" + res.orderId);
        }
      }, (err) => { console.log(err); }
    );
  }

  setDeliveryAddress(address: UserAddress): void {
    this.cartService.setDeliveryAddress(address);
  }

  addItem(): void {
    this.router.navigateByUrl("restaurant/" + this.restaurantName);
  }

  addressDataList(): void {
    this.addressSubscription = this.userAddressService.getUserAddressList(this.userId).subscribe(
      res => {
        if (res != null) {
          this.addressList = res as UserAddress[];
        }
      }, err => {
        console.log(err);
      }
    );
  }

  checkUserLogin(): void {
    if (this.token != null) {
      this.openAddDialog();
    }
    else {
      console.log("Token is null");
      this.toastr.error("Please Login first !");
    }
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(addUserAddressDialogComponent, {
      width: '250px',
      data: { Address: "", AddressId: 0 }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        result.UserId = this.userId;
        console.log("Address Added:");
        console.log(result);
        this.addressSubscription = this.userAddressService.addUserAddress(result).subscribe(
          res => {
            this.addressDataList();
          }, err => {
              console.log(err);
            }
          //}
        );
      }
    });
  }
}

@Component({
  templateUrl: 'dialog-add-location.component.html'
})

export class addUserAddressDialogComponent {

  placeHolder: string;

  constructor(private dialogRef: MatDialogRef<addUserAddressDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserAddress, private userAddressService: UserAddressService) {
    //this.data = this.userAddressService.initializeUserAddress();
    if (data.AddressId == 0) {
      this.placeHolder = "Add New Address";
    } else {
      this.placeHolder = "Edit Address";
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

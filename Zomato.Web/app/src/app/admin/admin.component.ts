import { Component, OnInit, NgZone, OnDestroy, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Debuger } from '../service/debug.service';
import { HttpClient } from '@angular/common/http';
import * as jwt_decode from 'jwt-decode';
import { Order } from '../model/order';
import { ToastrService } from 'ngx-toastr';
import { OrderNotificationService } from '../service/order-notification.service';
import { OrderDetail } from '../model/order-detail';
import { Subscription } from 'rxjs';
import { OrderService } from '../service/order.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';

const cmp: string = "Admin Component";

@Component({
  templateUrl: './admin.component.html'
})

export class AdminComponent implements OnInit, OnDestroy {
  orderDetail: OrderDetail;
  pageTitle = "Zomato Admin";
  token: string;
  userName: string;
  userToken: boolean = false;
  itemCount: number;
  orders: Order[] = [];
  notificationCount: number = 0;
  orderSubscritpion: Subscription;

  constructor(private router: Router, private http: HttpClient, private orderService: OrderService,
              private orderNotificationService: OrderNotificationService, public dialog: MatDialog,
    private _ngZone: NgZone, private toastr: ToastrService) {
    this.subscribeToEvents();
    this.orderNotificationService.sendAdminOnlineNotification(true);
  }

  ngOnInit(): void {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      console.log(this.token, "--Token is not null");
      let decode_token = jwt_decode(this.token);
      if (decode_token['UserRole'] == "admin") {
        this.userName = decode_token['UserName'];
        this.userToken = true;
        this.router.navigateByUrl('admin/restaurant');
      } else {
        this.userToken = false;
      }
    }
    else {
      console.log("Token is null")
    }
  }

  ngOnDestroy() {
    if (this.orderSubscritpion) {
      this.orderSubscritpion.unsubscribe();
    }
  }

  private subscribeToEvents(): void {

    this.orderNotificationService.orderReceived.subscribe((order: Order) => {
      this._ngZone.run(() => {
        order.restaurantName = order.restaurantName.replace('%20', ' ');
        this.orders.push(order);
        this.notificationCount = this.orders.length;
        this.toastr.success(order.restaurantName + " has order");
      });
    });
  }

  logOut() {
    localStorage.removeItem('token');
    window.location.href = '/admin';
  }

  private orderDetailDialog(orderId: number) {
    console.log(orderId);
    this.orderSubscritpion = this.orderService.getOrderDetail(orderId).subscribe(
      res => {
        if (res != null) {
          this.orderDetail = res as OrderDetail;
          console.log("Order Detail:");
          console.log(this.orderDetail);
          for (let each of this.orderDetail.itemDetail) {
            this.itemCount = this.itemCount + each.itemQuantity;
          }
          const dialogRef = this.dialog.open(OrderDetailAdminDialogComponent, {
            width: '550px',
            data: {
              OrderId: this.orderDetail['orderId'], Date: this.orderDetail['date'], RestaurantName: this.orderDetail['restaurantName'], UserName: this.orderDetail['userName'],
              DeliveryLocation: this.orderDetail['deliveryLocation'], ItemDetail: this.orderDetail['itemDetail'], TotalAmount: this.orderDetail['totalAmount']
            }
          });
          dialogRef.afterClosed().subscribe(result => {
            if (result == "closed") {
                let element = this.orders.find(x => x.orderId == orderId);
                console.log("Index Of:" + this.orders.indexOf(element));
                this.orders.splice(this.orders.indexOf(element), 1);
              this.notificationCount = this.orders.length;
              this.orderNotificationService.sendDeliveryNotification(this.orderDetail['orderId']);
            }
          });
        } //setTimeout(() => { this.display = "Delivered Succefully" }, 8000)
      }, err => {
        console.log(err);
    });
   
  }
}

@Component({
  templateUrl: 'order-detail-admin.component.html'
})

export class OrderDetailAdminDialogComponent {

  constructor(private dialogRef: MatDialogRef<OrderDetailAdminDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OrderDetail) {
  }

  closeClick(): void {
    this.dialogRef.close("closed");
  }

}

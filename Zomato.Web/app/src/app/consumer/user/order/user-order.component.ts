import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import * as jwt_decode from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { OrderService } from '../../../service/order.service';
import { OrderDetail } from '../../../model/order-detail';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Order } from '../../../model/order';



@Component({
  templateUrl: './user-order.component.html',
  styleUrls: ['./user-order.component.css']
})

export class UserOrderComponent implements OnInit, OnDestroy {

  private token; userId; decode_token; searchText: string;
  private totalOrder: number = 0;
  p: number = 1;
  private orderSubscription: Subscription;
  private orderList: OrderDetail[] = new Array<OrderDetail>();
  
  constructor(private toastr: ToastrService, private orderService: OrderService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getUserId();
    this.getOrderList();
  }

  ngOnDestroy(): void {
    if (this.orderSubscription) {
      this.orderSubscription.unsubscribe();
    }
  }

  private getUserId(): void {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      this.decode_token = jwt_decode(this.token);
      if (this.decode_token['UserRole'] == "user") {
        this.userId = this.decode_token['UserId'];
      }
    }
  }

  private getOrderList(): void {
    this.orderSubscription = this.orderService.getOrderByUser(this.userId).subscribe(
      res => {
        this.orderList = res as OrderDetail[];
        this.totalOrder = this.orderList.length;
      }, err => {
        console.log(err);
      }
    );
  }

  private openDetailDialog(id: number): void {
    const dialogRef = this.dialog.open(OrderDetailDialogComponent, {
      width: '550px',
      data: {
        OrderId: this.orderList[id]['orderId'], Date: this.orderList[id]['date'], RestaurantName: this.orderList[id]['restaurantName'], UserName: this.orderList[id]['userName'],
        DeliveryLocation: this.orderList[id]['deliveryLocation'], ItemDetail: this.orderList[id]['itemDetail'], TotalAmount: this.orderList[id]['totalAmount']
      }
    });
  }
}

@Component({
  templateUrl: './order-detail-dialog.component.html'
})

export class OrderDetailDialogComponent {

  constructor(private dialogRef: MatDialogRef<OrderDetailDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OrderDetail) {
  }

  closeClick(): void {
    this.dialogRef.close();
  }

}

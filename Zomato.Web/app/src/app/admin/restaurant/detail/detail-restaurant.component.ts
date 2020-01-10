 import { Component, OnInit, Inject, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort, MatDialog } from '@angular/material';
import { MenuService } from '../../../service/menu.service';
import { Menu } from '../../../model/menu';
import { Subscription, interval } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { RestaurantService } from '../../../service/restaurant.service';
import { OrderService } from '../../../service/order.service';
import { OrderDetail } from '../../../model/order-detail';
import { OrderDetailDialogComponent } from '../../../consumer/user/order/user-order.component';
import { Review } from '../../../model/review';
import { Like } from '../../../model/like';
import { LikeService } from '../../../service/like.service';
import { CommentService } from '../../../service/comment.service';
import { ReviewService } from '../../../service/review.service';
import { ToastrService } from 'ngx-toastr';
import * as jwt_decode from 'jwt-decode';
import { addCommentDialogComponent } from '../../../consumer/restaurant/review/review-restaurant.component';

@Component({
  templateUrl: './detail-restaurant.component.html',
  styleUrls: ['./detail-restaurant.component.css']
})

export class DetailRestaurantComponent implements OnInit, AfterViewInit, OnDestroy {

  pageTitle = "Detail";
  restaurantName; token; decode_token; userId: string;
  restaurantDetail: any;
  menuList: any[];
  menu: Menu[];
  menuSubscription; orderSubscription; reviewSubscription; likeSubscription; commentSubscription;
    updateOrderSubscription: Subscription;
  displayedColumns: string[] = ['ItemName', 'ItemPrice'];//, 'ItemId'];
  dataSource = new MatTableDataSource<Menu>();
  totalOrder: number = 0;
  orderList: OrderDetail[] = new Array<OrderDetail>();
  reviewList: Review[] = [];
  like: Like;
  p: number = 1;
  reviewToggle: boolean = false;
  toggle: boolean = true;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  tabLoadTimes: number;

  constructor(private menuService: MenuService, private restaurantService: RestaurantService,
    private orderService: OrderService, private reviewService: ReviewService, private toastr: ToastrService,
    private likeService: LikeService, private commentService: CommentService,
    private activiateRoute: ActivatedRoute, private dialog: MatDialog) {

    this.activiateRoute.params.subscribe(params => {
      this.restaurantName = params.restaurantName;
    });
    //this.restaurantName = this.restaurantName.replace('-', ' ');
  }

  ngOnInit(): void {
    this.like = this.likeService.initializeLike();
    this.getRestaurantDetail();
    this.getMenuList();
    this.getOrderList();
    this.getReviewList();
    this.updateOrderSubscription = interval(5000).subscribe(
      (val) => {
        this.getOrderList();
      }
    );
  }

  ngOnDestroy(): void {
    if (this.menuSubscription) {
      this.menuSubscription.unsubscribe();
    }
    if (this.orderSubscription) {
      this.orderSubscription.unsubscribe();
    }
    if (this.reviewSubscription) {
      this.reviewSubscription.unsubscribe();
    }
    if (this.likeSubscription) {
      this.likeSubscription.unsubscribe();
    }
    if (this.commentSubscription) {
      this.commentSubscription.unsubscribe();
    }
    if (this.updateOrderSubscription) {
      this.updateOrderSubscription.unsubscribe();
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  private doFilter(value: string) {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  }

  private getRestaurantDetail() {
    this.restaurantDetail = this.restaurantService.getRestaurantDetail(this.restaurantName).subscribe(
      res => {
        if (res != null) {
          this.restaurantDetail = res;
        }
      }, err => {
        console.log(err);
      }
    );
  }

  private getMenuList(): void {
    this.menuSubscription = this.menuService.getMenuList(this.restaurantName).subscribe(
      res => {
        if (res != null) {
          this.menuList = res as any[];
          this.menu = [];
          for (let item of this.menuList) {
            for (let menu of item.menus) {
              let data = {} as Menu;
              data.ItemId = menu['itemId'];
              data.ItemName = menu['itemName'];
              data.ItemPrice = menu['itemPrice'];
              this.menu.push(data);
            }
          }
          this.dataSource.data = this.menu as Menu[];
        }
      }, err => {
        console.log(err);
      }
    );
  }

  //private deleteMenu(itemId: number): void {
  //  this.menuSubscription = this.menuService.deleteMenu(itemId).subscribe(
  //    res => {
  //      this.getMenuList();
  //    }, err => {
  //      console.log(err);
  //    }
  //  );
  //}

  private getOrderList() {
    this.orderSubscription = this.orderService.getOrderByRestaurant(this.restaurantName).subscribe(
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

  private checkUserStatus() {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      this.decode_token = jwt_decode(this.token)
      this.userId = this.decode_token['UserId'];
      console.log(this.userId);
      return true;
    } else {
      this.toastr.error("Please login first");
      return false;
    }
  }

  private getReviewList(): void {
    this.reviewSubscription = this.reviewService.getReviewList(this.restaurantName).subscribe(
      res => {
        if (res != null) {
          this.reviewList = res as Review[];
          if (this.reviewList.length != 0) {
            this.reviewToggle = true;
          }
          console.log(this.reviewList);
        }
      }, err => {
        console.log(err);
      }
    );
  }

  private deleteReview(reviewId: number) {
    this.reviewSubscription = this.reviewService.deleteReview(reviewId).subscribe(
      res => {
        this.getReviewList();
        this.getRestaurantDetail();
      }, err => {
        console.log(err);
      }
    );
  }

  private addLike(reviewId: number): void {
    console.log("Review Id: "+reviewId);
    if (this.toggle) {
      if (this.checkUserStatus()) {
        this.like.reviewId = reviewId;
        this.like.userId = this.userId;
        this.likeSubscription = this.likeService.addLike(this.like).subscribe(
          res => {
            this.getReviewList();
          }, err => {
            console.log(err)
          }
        );
      } else {
        this.toastr.error("Login first ..!");
      }
    }
    else {
      if (this.checkUserStatus()) {
        this.like.reviewId = reviewId;
        this.like.userId = this.userId;
        this.likeSubscription = this.likeService.addLike(this.like).subscribe(
          res => {
            this.getReviewList();
          }, err => {
            console.log(err)
          }
        );
      } else {
        this.toastr.error("Login first ..!");
      }
    }
    this.toggle = !this.toggle;
  }

  private openAddCommentDialog(reviewId: number): void {
    const dialogRef = this.dialog.open(addCommentDialogComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (this.checkUserStatus()) {
        if (result != null) {
          result.userId = this.userId;
          result.reviewId = reviewId;
          this.commentSubscription = this.commentService.addComment(result).subscribe(
            res => {
              this.getReviewList();
            }, err => {
              console.log(err);
            }
          );
        }
      } else {
        this.toastr.error("Login first for adding comment");
      }
    });
  }
}


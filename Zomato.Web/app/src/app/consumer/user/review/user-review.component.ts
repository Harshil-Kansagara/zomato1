import { Component, OnInit, OnDestroy } from '@angular/core';
import { ReviewService } from '../../../service/review.service';
import { Subscription } from 'rxjs';
import * as jwt_decode from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { Review } from '../../../model/review';
import { LikeService } from '../../../service/like.service';
import { Like } from '../../../model/like';
import { addCommentDialogComponent } from '../../restaurant/review/review-restaurant.component';
import { CommentService } from '../../../service/comment.service';
import { MatDialog } from '@angular/material';


@Component({
  templateUrl: './user-review.component.html',
  styleUrls: ['./user-review.component.css']
})

export class UserReviewComponent implements OnInit, OnDestroy {

  toggle = true;
  token; userId; decode_token: string;
  reviewSubscription; likeSubscription; commentSubscription: Subscription;
  reviewList: Review[] = [];
  like: Like;

  constructor(private reviewService: ReviewService, private toastr: ToastrService,
    private likeService: LikeService, private commentService: CommentService, public dialog: MatDialog) {
    this.like = this.likeService.initializeLike();
  }

  ngOnInit(): void {
    this.getUserId();
    this.getReviewList();
  }

  ngOnDestroy(): void {
    if (this.reviewSubscription) {
      this.reviewSubscription.unsubscribe();
    }
    if (this.likeSubscription) {
      this.likeSubscription.unsubscribe();
    }
    if (this.commentSubscription) {
      this.commentSubscription.unsubscribe();
    }
  }

  getUserId(): void {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      this.decode_token = jwt_decode(this.token);
      if (this.decode_token['UserRole'] == "user") {
        this.userId = this.decode_token['UserId'];
      }
    }
  }

  getReviewList(): void {
    this.reviewSubscription = this.reviewService.getReviewByUser(this.userId).subscribe(
      (res) => {
        if (res != null) {
          this.reviewList = res as Review[];
        }
      }, (err) => {
        console.log(err);
      }
    );
  }

  private deleteReview(reviewId: number) {
    if (confirm("Are you sure you want to delete this review ?")) {
      this.reviewSubscription = this.reviewService.deleteReview(reviewId).subscribe(
        res => {
          this.getReviewList();
        }, err => {
          console.log(err);
        }
      );
    }
  }

  //checkUserStatus() {
  //  this.token = localStorage.getItem('token');
  //  if (this.token != null) {
  //    console.log("Token is not null: ", this.token);
  //    this.decode_token = jwt_decode(this.token)
  //    this.userId = this.decode_token['UserId'];
  //    console.log(this.userId);
  //    return true;
  //  } else {
  //    this.toastr.error("Please login first");
  //    return false;
  //  }
  //}

  addLike(reviewId: number): void {
    if (this.toggle) {
      if (this.userId!=null) {
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
      if (this.userId!=null) {
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

  openAddCommentDialog(reviewId: number): void {
    const dialogRef = this.dialog.open(addCommentDialogComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (this.userId!=null) {
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

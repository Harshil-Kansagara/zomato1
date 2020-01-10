import { Component, OnInit, OnDestroy, Inject } from "@angular/core";
import { Review } from '../../../model/review';
import { ReviewService } from '../../../service/review.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import * as jwt_decode from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { CommentService } from '../../../service/comment.service';
import { Comment } from '../../../model/comment';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { LikeService } from '../../../service/like.service';
import { Like } from '../../../model/like';

@Component({
  templateUrl: './review-restaurant.component.html',
  styleUrls: ['./review-restaurant.component.css']
})

export class ReviewComponent implements OnInit, OnDestroy {
  review: Review;
  comment: Comment;
  reviewList: Review[] = [];
  restaurantName; token; decode_token; userId: string;
  reviewSubscription; likeSubscription; commentSubscription: Subscription;
  toggle: boolean = true;
  like: Like;
  reviewToggle: boolean = false;

  constructor(private reviewService: ReviewService, private router: Router, private likeService: LikeService,
    private toastr: ToastrService, private commentService: CommentService, public dialog: MatDialog) { }

  ngOnInit(): void{
    this.restaurantName = this.router.url.split('/')[2];
    this.review = this.reviewService.initializeReview();
    this.comment = this.commentService.initializeComment();
    this.like = this.likeService.initializeLike();
    this.loadReviewList();
  }

  ngOnDestroy() {
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

  checkUserStatus() {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      console.log("Token is not null: ", this.token);
      this.decode_token = jwt_decode(this.token)
      this.userId = this.decode_token['UserId'];
      console.log(this.userId);
       return true;
    } else {
      this.toastr.error("Please login first");
      return false;
    }
  }

  loadReviewList(): void {
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

  addReview() {
    if (this.checkUserStatus()) {
      this.review.userId = this.userId;
      console.log(this.review);
      this.reviewSubscription = this.reviewService.addNewReview(this.restaurantName, this.review).subscribe(
        res => {
          if (res != null) {
            this.loadReviewList();
          }
        }, err => {
          console.log(err);
        });
    } else {
      this.toastr.error("Login first for adding review");
    }
  }

  addLike(reviewId: number): void {
    if (this.toggle) {
      if (this.checkUserStatus()) {
        this.like.reviewId = reviewId;
        this.like.userId = this.userId;
        this.likeSubscription = this.likeService.addLike(this.like).subscribe(
          res => {
            this.loadReviewList();
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
            this.loadReviewList();
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
      if (this.checkUserStatus()) {
        if (result != null) {
          result.userId = this.userId;
          result.reviewId = reviewId;
          this.commentSubscription = this.commentService.addComment(result).subscribe(
            res => {
              this.loadReviewList();
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

@Component({
  templateUrl: 'dialog-add-comment.component.html'
})

export class addCommentDialogComponent {

  constructor(private dialogRef: MatDialogRef<addCommentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Comment,
    private commentService: CommentService) {
    this.data = this.commentService.initializeComment();
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

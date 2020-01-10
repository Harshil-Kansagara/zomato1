import { Component, OnInit, OnDestroy } from '@angular/core';
import * as jwt_decode from 'jwt-decode';
import { AccountService } from '../../../service/account.service';
import { Subscription } from 'rxjs';
import { FollowService } from '../../../service/follow.service';
import { Follow } from '../../../model/follow';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './user-follower.component.html',
  styleUrls: ['./user-follower.component.css']
})

export class UserFollowerComponent implements OnInit, OnDestroy {

  token; decode_token; userId: string;
  userSubscription; followSubscription: Subscription;
  userList: any;
  follow: Follow;
  following; follower: any[];
  followingCount; followerCount: number;

  constructor(private userService: AccountService, private followService: FollowService,
            private toastr: ToastrService) { }

  ngOnInit(): void {
    this.follow = this.followService.initializeFollow();
    this.getUserId();
    this.getUserList();
    this.getFollowing();
    this.getFollowers();
  }

  ngOnDestroy(): void {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
    if (this.followSubscription) {
      this.followSubscription.unsubscribe();
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

  getUserList(): void {
    this.userSubscription = this.userService.getUsersList(this.userId).subscribe(
      res => {
        if (res != null) {
          this.userList = res;
        }
      }, err => {
        
        console.log(err);
      }
    );
  }

  addFollower(userid: string) {
    this.follow.userId = this.userId;
    this.follow.followingId = userid;
    this.followSubscription = this.followService.addFollower(this.follow).subscribe(
      res => {
        if (res != null) {
          this.toastr.success("Follower added successfully");
          this.getFollowing();
          this.getFollowers();
        }
      }, err => {
        if (err.status == 400) {
          this.toastr.error("You are already following");
        } else {
          console.log(err);
        }
      }
    );
  }

  getFollowers(): void {
    this.followSubscription = this.followService.getFollowers(this.userId).subscribe(
      res => {
        if (res != null) {
          this.follower = res as any[];
          this.followerCount = this.follower.length;
        }
      }, err => {
        console.log(err);
      }
    );
  }

  getFollowing(): void {
    this.followSubscription = this.followService.getFollowing(this.userId).subscribe(
      res => {
        if (res != null) {
          this.following = res as any[];
          this.followingCount = this.following.length;
        }
      }, err => {
        console.log(err);
      }
    );
  }

  deleteFollowing(followerId: string): void {
    this.followSubscription = this.followService.deleteFollower(followerId).subscribe(
      res => {
          this.getFollowing();
      }, err => {
        console.log(err);
      }
    );
  }
}

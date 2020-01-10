import { Component, OnInit, OnDestroy, NgZone } from "@angular/core";
import { ActivatedRoute, Router } from '@angular/router';
import * as jwt_decode from 'jwt-decode';
import { AccountService } from '../../service/account.service';
import { Subscription } from 'rxjs';
import { Register } from '../../model/register';
import { OrderNotificationService } from '../../service/order-notification.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: 'user.component.html',
  styleUrls: ['user.component.css']
})

export class UserComponent implements OnInit, OnDestroy {

  token; userId; decode_token: string;
  userSubscription: Subscription;
  user: Register;

  constructor(private accountService: AccountService, private router: Router,
     private toastr: ToastrService) {
    
  }

  ngOnInit(): void {
    this.getUserId();
    this.getUserData();
  }

  ngOnDestroy(): void {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
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

  getUserData(): void {
    this.user = this.accountService.intializeRegister();
    this.userSubscription = this.accountService.GetUserData(this.userId).subscribe(
      (res) => {
        if (res != null) {
          this.user = res as Register;
          console.log(this.user);
        }
      }
    );
  }

  logoutUser() {
    localStorage.removeItem('token');
    //window.location.reload();
    this.router.navigateByUrl("");
  }

  
}

import { Component, OnInit, OnDestroy } from '@angular/core';
import { Login } from '../../../model/login';
import { Debuger } from '../../../service/debug.service';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../../service/account.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as jwt_decode from 'jwt-decode';

const cmp: string = "Login-Admin.Component";

@Component({
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit, OnDestroy {
  pageTitle = "Login Admin";
  login: Login;
  promise: Subscription;

  constructor(private debug: Debuger, private toastr: ToastrService, private accountService: AccountService, private router: Router) {
    this.debug.loadDebuger(cmp, "constructor", "created", []);
  }

  ngOnInit() {
    this.login = this.accountService.initializeLogin();
  }

  ngOnDestroy(): void {
    if (this.promise) {
      this.promise.unsubscribe();
    }
  }

  submit() {
    console.log(this.login);
    this.promise = this.accountService.LoginAdmin(this.login).subscribe(
      (res: any) => {
        if (res != null) {
          localStorage.setItem('token', res.token);
          this.toastr.success("Login Successful");
          window.location.href = "/admin/restaurant";

          //let decode_token = jwt_decode(res.token);
          //if (decode_token['UserRole'] == 'admin') {
          //} else {
          //  this.toastr.error("You don't have priviledge to access this page");
          //}
        }
      }, err => {
        if (err.status == 400) {
          this.toastr.error('Authentication Failed.');
        } else {
          console.log(err);
        }
      }
    );
  }
}

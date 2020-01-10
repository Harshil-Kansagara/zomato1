import { Component } from '@angular/core';
import { Register } from '../../../model/register';
import { Debuger } from '../../../service/debug.service';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../../service/account.service';
import { Router } from '@angular/router';

const cmp: string = "Register-Admin.Component";

@Component({
  templateUrl: './register.component.html'
})

export class RegisterComponent {
  pageTitle = 'Register Admin';
  register: Register;

  constructor(private debug: Debuger, private toastr: ToastrService, private accountService: AccountService, private router:Router) {
    this.debug.loadDebuger(cmp, "constructor", "created", []);
  }

  ngOnInit(): void {
    this.register = this.accountService.intializeRegister();
  }

  save(): void {
    this.register.UserRole = 'admin';
    console.log(this.register);
    let promise = this.accountService.Register(this.register).subscribe(
      (result: any) => {
        if (result.succeeded) {
          this.toastr.success("Register Successfully");
          promise.unsubscribe();
          this.router.navigateByUrl('admin');
        }
        else {
          for (let i = 0; i < result.errors.length; i++) {
            this.toastr.error(result.errors[i].description);
          }
        }
      }, err => {
        console.log(err);
      }
    );
  }
}

import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../service/account.service';
import { ToastrService } from 'ngx-toastr';
import { Register } from '../../model/register';
import { Subscription, Observable, of } from 'rxjs';
import { Login } from '../../model/login';
import * as jwt_decode from 'jwt-decode';
import { RestaurantService } from '../../service/restaurant.service';
import { startWith, map } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { OrderNotificationService } from '../../service/order-notification.service';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit, OnDestroy {
  registerToken; userToken: boolean = false;
  register: Register;
  login: Login;
  promise: Subscription;
  token; decode_token; userName; searchText; userId: string= "";
  restaurantList: string[] = [];
  restaurants: any[];
  restaurantCtrl: FormControl;
  filteredRestaurants: Observable<any[]>;

  constructor(private toastr: ToastrService, private accountService: AccountService,
    private restaurantService: RestaurantService, private router: Router, private _ngZone: NgZone,
    private orderNotificationService: OrderNotificationService, ) {
    this.restaurantCtrl = new FormControl();
    this.subscribeToEvents();
  }

  ngOnInit() {
    this.register = this.accountService.intializeRegister();
    this.login = this.accountService.initializeLogin();
    this.checkUserLogin();
    this.loadRestaurantList();
  }

  ngOnDestroy(): void {
    if (this.promise) {
      this.promise.unsubscribe();
    }
  }

  checkUserLogin(): void {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      console.log("Token is not null: ", this.token);
      this.decode_token = jwt_decode(this.token)
      if (this.decode_token['UserRole'] == "user") {
        this.userName = this.decode_token['UserName'];
        this.userId = this.decode_token['UserId'];
        this.userToken = true;
      } else {
        this.userToken = false;
      }
    } else {
      console.log("Token is null");
    }
  }

  loadRestaurantList(): void {
    this.promise = this.restaurantService.getListRestaurantDetail().subscribe(
      res => {
        if (res != null) {
          this.restaurants = res as any[];
          for (let i = 0; i < this.restaurants.length; i++) {
            if (i == 7) {
              break;
            }
            this.restaurantList.push(this.restaurants[i]);
          }
          //console.log(this.restaurants);
          this.filteredRestaurants = this.restaurantCtrl.valueChanges
            .pipe(
              startWith(''),
              map(res => res ? this.filterRestaurants(res) : this.restaurants.slice())
            );
        }
      }, err => {
        console.log(err);
      }
    );
  }

  filterRestaurants(name: string) {
    return this.restaurants.filter(each =>
      each.restaurant.restaurantName.toLowerCase().indexOf(name.toLowerCase()) === 0);
  }

  search(): void {
    this.promise = this.restaurantService.checkRestaurant(this.restaurantCtrl.value).subscribe(
      res => {
        this.router.navigateByUrl("restaurant/" + this.restaurantCtrl.value);
      }, err => {
        console.log(err);
      }
    );
  }

  reg(): void {
    this.registerToken = true;
  }
  log(): void {
    this.registerToken = false;
  }

  registerUser(): void {
    this.register.UserRole = "user";
    console.log(this.register);
    this.promise = this.accountService.Register(this.register).subscribe(
      (result: any) => {
        if (result.succeeded) {
          this.toastr.success("Register Successfully");
          this.register = this.accountService.intializeRegister();
          this.registerToken = false;
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

  loginUser(): void {
    console.log(this.login);
    this.promise = this.accountService.LoginUser(this.login).subscribe(
      (res: any) => {
        if (res != null) {
          console.log(res.token);
          this.decode_token = jwt_decode(res.token);
          if (this.decode_token['UserRole'] == 'user') {
            localStorage.setItem('token', res.token);
            window.location.reload();
            this.toastr.success("Login Successful");
            this.userToken = true;
            //window.location.href = "admin/restaurant";
          } else {
            this.toastr.error("You don't have priviledge to access this page");
          }
        }
      }, err => {
        if (err.status == 400) {
          this.toastr.error('Incorrect Username or Password.', 'Authentication Failed.');
        } else {
          console.log(err);
        }
      }
    );
  }

  logoutUser() {
    localStorage.removeItem('token');
    window.location.reload();
  }

  restDetail(restaurantName: string): void {
    this.router.navigateByUrl('restaurant/' + restaurantName);
  }

  navigateUserProfile(): void {
    this.router.navigateByUrl("users/" + this.userName);
  }

  private subscribeToEvents(): void {
    this.orderNotificationService.deliveryReceived.subscribe((data: string) => {
      this._ngZone.run(() => {
        this.toastr.success(data);
      });
    });
  }

  //hyphenateUrlParams(str: string) {
  //  return str.replace(' ', '-');
  //}
}

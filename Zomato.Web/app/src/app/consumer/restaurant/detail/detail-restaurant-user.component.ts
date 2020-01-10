import { Component, OnInit, OnDestroy, Inject, ViewChild } from "@angular/core";
import { RestaurantService } from '../../../service/restaurant.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Restaurant } from '../../../model/restaurant';
import { Subscription, interval } from 'rxjs';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatSidenav } from '@angular/material';
import { UserAddress } from '../../../model/userAddress';
import { UserAddressService } from '../../../service/user-address.service';
import { ToastrService } from 'ngx-toastr';
import * as jwt_decode from 'jwt-decode';

@Component({
  templateUrl: './detail-restaurant-user.component.html',
  styleUrls: ['./detail-restaurant-user.component.css']
})

export class DetailRestaurantUserComponent implements OnInit, OnDestroy {
  restaurantName: string;
  restaurantDetail: Restaurant;
  resturantSubscription; updateRestaurantDataSubscription: Subscription;
  cuisine; location; token; decode_token; userId: string = '';
  addressList: UserAddress[];
  navLinks: any[];
  activeLinkIndex = -1;

  constructor(private router: Router, private restaurantService: RestaurantService, public dialog: MatDialog,
    private activiateRoute: ActivatedRoute, private toastr: ToastrService) {

    this.activiateRoute.params.subscribe(params => {
      this.restaurantName = params.restaurantName;
    });
    //this.restaurantName = this.restaurantName.replace('-', ' ');
    this.navLinks = [
      {
        label: 'Menu',
        link: 'menu',
        index: 0
      }, {
        label: 'Review',
        link: 'review',
        index: 1
      }
    ]
  }

  ngOnInit(): void {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      this.decode_token = jwt_decode(this.token)
      this.userId = this.decode_token['UserId'];
    }
    this.router.events.subscribe((res) => {
      this.activeLinkIndex = this.navLinks.indexOf(this.navLinks.find(tab => tab.link === '.' + this.router.url));
    });
    this.restaurantDetail = this.restaurantService.initializeRestaurant();
    this.restaurantData();
    this.updateRestaurantDataSubscription = interval(10000).subscribe(
      (val) => {
        this.restaurantData();
      }
    );
    //this.addressDataList();
  }

  restaurantData(): void {
    this.resturantSubscription = this.restaurantService.getRestaurantDetail(this.restaurantName).subscribe(
      res => {
        if (res != null) {
          this.restaurantDetail = res as Restaurant;
          //console.log(this.restaurantDetail);
        }
      }, err => {
        console.log(err);
      }
    );
  }

  addAddress(addressName: string) {
    console.log(addressName);
  }

  ngOnDestroy(): void {
    if (this.resturantSubscription) {
      this.resturantSubscription.unsubscribe();
    }
    if (this.updateRestaurantDataSubscription) {
      this.updateRestaurantDataSubscription.unsubscribe();
    }
  }
}


import { Component, OnInit } from '@angular/core';
import { RestaurantService } from '../../../service/restaurant.service';
import { Cuisine } from '../../../model/cuisine';
import { Category } from '../../../model/category';
import { Restaurant, RestaurantResolved } from '../../../model/restaurant';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './add-restaurant.component.html',
  styleUrls: ['./add-restaurant.component.css']
})

export class AddRestaurantComponent implements OnInit {
  pageTitle = "Add Restaurant Detail";
  errorMessage: string;
  private dataIsValid: { [key: string]: boolean } = {};

  private currentRestaurant: Restaurant;
  private originalRestaurant: Restaurant;

  get restaurant(): Restaurant {
    return this.currentRestaurant;
  }

  set restaurant(value: Restaurant) {
    this.currentRestaurant = value;
    this.originalRestaurant = value ? { ...value } : null;
  }

  constructor(private restaurantService: RestaurantService, private route: ActivatedRoute, private router: Router,
    private toastr: ToastrService) {
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      const resolvedData: RestaurantResolved = data['resolvedData'];
      this.errorMessage = resolvedData.error;
      this.onRestaurantRetrieved(resolvedData.restaurant)
    })
  }

  onRestaurantRetrieved(restaurant: Restaurant): void {
    this.restaurant = restaurant;
  }

  isValid(path?: string): boolean {
    this.validate();
    if (path) {
      return this.dataIsValid[path];
    }
    return (this.dataIsValid &&
      Object.keys(this.dataIsValid).every(d => this.dataIsValid[d] === true));
  }

  validate(): void {
    this.dataIsValid = {};

    //info
    if (this.restaurant.RestaurantName && this.restaurant.Location.length !=0) {
      this.dataIsValid['info'] = true;
    } else {
      this.dataIsValid['info'] = false;
    }

    //search tags
    if (this.restaurant.CuisineId.length!=0 && this.restaurant.CategoryId.length!=0) {
      this.dataIsValid['tags'] = true;
    } else {
      this.dataIsValid['tags'] = false;
    }
  }

  saveRestaurant(): void {
    console.log(this.restaurant);
    let promise = this.restaurantService.addRestaurant(this.restaurant).subscribe(
      () => {
        this.toastr.success(this.restaurant.RestaurantName + " restaurant is added successfull");
        promise.unsubscribe();
        this.router.navigateByUrl('admin/restaurant');
      }, err => {
        if (err.status == 400) {
          this.toastr.error("Duplicate Restaurant Name");
        } else {
          console.log(err);
        }
      }
    );
  }
}

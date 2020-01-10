import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { RestaurantService } from '../../../service/restaurant.service';
import { ToastrService } from 'ngx-toastr';
import { MatTableDataSource, MatPaginator, MatSort, MatSidenav } from '@angular/material';
import { Restaurant } from '../../../model/restaurant';
import { Cuisine } from '../../../model/cuisine';
import { CuisineService } from '../../../service/cuisine.service';
import { RestCuisineService } from '../../../service/rest-cuisines.service';
import { RestCategoryService } from '../../../service/rest-category.service';

@Component({
  templateUrl: 'list-restaurant-user.component.html',
  styleUrls: ['./list-restaurant-user.component.css']
})

export class ListRestaurantUserComponent implements OnInit, OnDestroy {
  restaurantList: Restaurant[] = [];
  heading; categoryName: string;
  restCategorySubscription: Subscription;
  searchText: string;
  p: number = 1;
  cuisines: Cuisine[];
  cuisineName: string[] = null;
  cuisineId: number[] = [];

  @ViewChild('sidenav', { static: false }) sidenav: MatSidenav; 

  constructor(private toastr: ToastrService, private restCuisineService: RestCuisineService,
    private cuisineService: CuisineService, private restaurantService: RestaurantService,
    private router: Router, private activatedRoute: ActivatedRoute, private restCategoryService: RestCategoryService) {

    this.activatedRoute.params.subscribe(params => {
      this.categoryName = params.categoryName;
    });
    console.log(this.categoryName);
  }

  ngOnInit(): void {
    this.loadRestaurantList();
    this.restCategorySubscription = this.cuisineService.getCuisineList().subscribe(
      result => {
        if (result != null) {
          this.cuisines = result as Cuisine[];
          //this.tempCuisine = this.cuisines;
          console.log(this.cuisines);
        }
      }
    );
  }

  ngOnDestroy(): void {
    if (this.restCategorySubscription) {
      this.restCategorySubscription.unsubscribe();
    }
  }

  loadRestaurantList(): void {
    if (this.categoryName == 'breakfast' || this.categoryName == 'lunch' || this.categoryName == 'dinner'
      || this.categoryName == 'cafe' || this.categoryName == 'dessert') {
      this.heading = this.categoryName.charAt(0).toUpperCase() + this.categoryName.slice(1) + " Restaurant"
      this.restCategorySubscription = this.restCategoryService.getRestaurantListByCategory(this.categoryName).subscribe(
        res => {
          if (res != null) {
            this.restaurantList = res as Restaurant[];
          }
        }, err => {
          console.log(err);
        }
      );
    } else {
        this.heading = "Restaurants"
      this.restCategorySubscription = this.restaurantService.getListRestaurantDetail().subscribe(
        res => {
          if (res != null) {
            this.restaurantList = res as Restaurant[];
          }
        }, err => {
          console.log(err);
        }
      );
    }
  }

  checkedCuisine(id: number): void {
    if (this.cuisineId.indexOf(id) != -1) {
      this.cuisineId.splice(this.cuisineId.indexOf(id),1);
      console.log(this.cuisineId);
      this.isChecked(id);
    }
    else {
      this.cuisineId.push(id);
      console.log(this.cuisineId);
      this.isChecked(id);
    }
  }

  showRestaurant(): void {
    console.log(this.cuisineId);
    this.restCategorySubscription = this.cuisineService.getCuisineNameListById(this.cuisineId).subscribe(
      res => {
        if (res != null) {
          this.cuisineName = res as string[];
          console.log(this.cuisineName);
        }
      }, err => {
        console.log(err);
      }
    );

    this.restCategorySubscription = this.restCuisineService.getRestaurantListByCuisineIds(this.cuisineId).subscribe(
      res => {
        if (res != null) {
          this.restaurantList = res as Restaurant[];
        }
      }, err => {
        console.log(err);
      }
    );
    this.sidenav.toggle();
  }

  resDetail(restaurantName: string): void {
    this.router.navigateByUrl('restaurant/' + restaurantName);
  }

  unCheck(): void {
    this.cuisineId = [];
    this.isChecked(0);
  }

  clearAll(): void {
    this.unCheck();
    this.loadRestaurantList();
    this.cuisineName = null;
  }

  isChecked(id: number): boolean {
    if (this.cuisineId.includes(id)) {
      return true;
    } else {
      return false;
    }
  }

  //hyphenateUrlParams(str: string) {
  //  return str.replace(/\s+/g, '-');
  //}
}

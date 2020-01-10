import { Component, OnInit } from '@angular/core';
import { RestaurantService } from '../../../service/restaurant.service';
import { Category } from '../../../model/category';
import { Cuisine } from '../../../model/cuisine';
import { Restaurant } from '../../../model/restaurant';
import { ActivatedRoute } from '@angular/router';
import { CuisineService } from '../../../service/cuisine.service';
import { CategoryService } from '../../../service/category.service';

@Component({
  templateUrl: './add-restaurant-search.component.html'
})

export class AddRestaurantSearchComponent implements OnInit {

  category: Category[];
  cuisine: Cuisine[];
  restaurant: Restaurant;

  constructor(private categoryService: CategoryService, private cuisineService: CuisineService, private restaurantService: RestaurantService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    //this.restaurant = this.restaurantService.initializeRestaurant();
    let promise = this.categoryService.getCategoryList().subscribe(
      (result: Category[]) => {
          this.category = result;
          promise.unsubscribe();
        console.log(this.category);
      }
    );
    let promise1 = this.cuisineService.getCuisineList().subscribe(
      result => {
        if (result != null) {
          this.cuisine = result as Cuisine[];
          promise1.unsubscribe;
          console.log(this.cuisine);
        }
      }
    );
    this.route.parent.data.subscribe(data => {
      this.restaurant = data['resolvedData'].restaurant;
    })
  }

  checkedCuisine(id: number): void {
    if (this.restaurant.CuisineId.indexOf(id) != -1) {
      this.restaurant.CuisineId.splice(this.restaurant.CuisineId.indexOf(id));
    }
    else {
      this.restaurant.CuisineId.push(id);
    }
  }

  checkedCategory(id: number): void {
    if (this.restaurant.CategoryId.indexOf(id) != -1) {
      this.restaurant.CategoryId.splice(this.restaurant.CategoryId.indexOf(id));
    }
    else {
      this.restaurant.CategoryId.push(id);
    }
  }

}

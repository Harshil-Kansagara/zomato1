import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Cuisine } from '../model/cuisine';

@Injectable({ providedIn: 'root' })

export class CuisineService{

  baseUrl: string = "api/cuisine/"
  constructor(private http: HttpClient){}

  getCuisineList() {
    return this.http.get(this.baseUrl);
  }

  getCuisineNameListById(cuisineId: number[]) {
    return this.http.post(this.baseUrl, cuisineId);
  }

  getCuisineListByRestaurant(restaurantName: string) {
    return this.http.get(this.baseUrl + restaurantName);
  }

  initializeCuisine(): Cuisine {
    return {
      CuisineId: 0,
      CuisineName: ''
    }
  }
}

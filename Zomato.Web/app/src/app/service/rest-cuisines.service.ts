import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })

export class RestCuisineService {

  baseUrl: string = "api/restcuisines/";

  constructor(private http: HttpClient) { }

  getRestaurantListByCuisineIds(cuisineId: number[]) {
    return this.http.post(this.baseUrl, cuisineId);
  }
}

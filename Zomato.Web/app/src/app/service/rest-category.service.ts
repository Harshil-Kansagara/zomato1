import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: "root" })
export class RestCategoryService {
  baseUrl: string = "api/restcategory/";

  constructor(private http: HttpClient) { }

  getRestaurantListByCategory(categoryName: string) {
    return this.http.get(this.baseUrl + categoryName);
  }
}

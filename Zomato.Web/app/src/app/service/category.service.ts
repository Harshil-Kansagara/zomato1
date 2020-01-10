import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../model/category';

@Injectable({ providedIn:'root' })
export class CategoryService {

  baseUrl: string = "api/category/"

  constructor(private http: HttpClient) { }

  getCategoryList() {
    return this.http.get(this.baseUrl);
  }

  initializeCategory(): Category {
    return {
      CategoryId: 0,
      CategoryName: ''
    }
  }
}

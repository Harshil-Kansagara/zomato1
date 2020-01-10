import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Menu } from '../model/menu';

@Injectable({ providedIn: 'root' })
export class MenuService {

  baseUrl = '/api/menu/';
 

  constructor(private http: HttpClient) { }

  getMenuList(restaurantName: string) {
    return this.http.get(this.baseUrl + restaurantName + '/menu');
  }

  addMenu(restaurantName: string, menu: any) {
    return this.http.post(this.baseUrl + restaurantName + '/menu', menu);
  }

  deleteMenu(menuId: number) {
    return this.http.delete(this.baseUrl + menuId);
  }

  initializeMenu(): Menu {
    return {
      ItemId: 0,
      ItemName: '',
      ItemPrice: 0,
      CuisineId: 0
    }
  }

}

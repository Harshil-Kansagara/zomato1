import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order } from '../model/order';

@Injectable({ providedIn: "root" })
export class OrderService {

  baseUrl: string = "api/order/";

  constructor(private http: HttpClient) { }

  addOrder(restaurantName: string, order: Order) {
    return this.http.post(this.baseUrl + restaurantName + "/order", order);
  }

  getOrderByRestaurant(restaurantName: string) {
    return this.http.get(this.baseUrl + "restaurant/" + restaurantName);
  }

  getOrderByUser(userId: string) {
    return this.http.get(this.baseUrl + "user/" + userId);
  }

  getOrderDetail(orderId: number) {
    return this.http.get(this.baseUrl + orderId);
  }

  deleteOrder(orderId: number) {
    return this.http.delete(this.baseUrl + orderId);
  }

  initializeOrder(): Order {
    return {
      orderId: 0,
      orderDate: null,
      userId: null,
      addressId: 0,
      restaurantId: 0,
      restaurantName: null,
      items: []
    }
  }
}

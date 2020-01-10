import { OrderItemDetail } from './order-item-detail';

export class OrderDetail {
  OrderId: number = 0;
  Date: string = null;
  RestaurantName: string = null;
  UserName: string = null;
  UserNumber: string = null;
  UserEmail: string = null;
  DeliveryLocation: string = null;
  TotalAmount: number = 0;
  itemDetail: OrderItemDetail[] = [];
}

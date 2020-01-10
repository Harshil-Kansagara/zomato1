import { CartItem } from './cart-item';

export interface Order {
  orderId: number;
  userId: string;
  addressId: number;
  restaurantId: number;
  restaurantName: string;
  orderDate: string;
  items: CartItem[];
}

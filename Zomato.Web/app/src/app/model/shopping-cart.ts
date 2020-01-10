import { CartItem } from './cart-item';

export class ShoppingCart {
  public items: CartItem[] = new Array<CartItem>();
  public deliveryAddressId: number = 0;
  public itemsTotal: number = 0;
  public restaurantName: string = null;

  public updateFrom(src: ShoppingCart) {
    this.items = src.items;
    this.deliveryAddressId = src.deliveryAddressId;
    this.itemsTotal = src.itemsTotal;
    this.restaurantName = src.restaurantName;
  }
}

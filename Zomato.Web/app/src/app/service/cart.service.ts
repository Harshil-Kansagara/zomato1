import { Injectable } from "@angular/core";
import { Menu } from '../model/menu';
import { Observable, Observer } from 'rxjs';
import { ShoppingCart } from '../model/shopping-cart';
import { LocalStorageService } from './storage.service';
import { MenuService } from './menu.service';
import { CartItem } from '../model/cart-item';
import { UserAddress } from '../model/userAddress';

const CART_KEY = "cart";

@Injectable({ providedIn: "root" })

export class CartService {
  private storage: Storage;
  items: Menu[] = [];
  restaurantName: string;
  private subscriptionObservable: Observable<ShoppingCart>;
  private subscribers: Array<Observer<ShoppingCart>> = new Array<Observer<ShoppingCart>>();
  menus: any[] = [];

  constructor(private storageService: LocalStorageService, private menuService: MenuService) {
    this.storage = this.storageService.get();
    this.subscriptionObservable = new Observable<ShoppingCart>((observer: Observer<ShoppingCart>) => {
      this.subscribers.push(observer);
      observer.next(this.retrieve());
      return () => {
        this.subscribers = this.subscribers.filter((obs) => obs !== observer);
      };
    });
  }

  public get(): Observable<ShoppingCart> {
    return this.subscriptionObservable;
  }

  getMenuList(restaurantName: string) {
    this.menuService.getMenuList(restaurantName).subscribe((res) => {
      let m = res as any[];
      for (let each of m) {
        for (let menu of each.menus) {
          this.menus.push(menu);
        }
      }
    });
  }

  //addMenuItem(menu: any, quantity: number) {
  //  const cart = this.retrieve();
  //  let item = cart.items.find((p) => p.ItemId === menu.itemId);
  //  if (item == undefined) {
  //    item = new CartItem();
  //    item.ItemId = menu.itemId;
  //    cart.items.push(item);
  //  }

  //  item.ItemQuantity += quantity;
  //  cart.items = cart.items.filter((cartItem) => cartItem.ItemQuantity > 0);

  //  if (cart.items.length === 0) {
  //    cart.deliveryAddressId = 0;
  //  }

  //  this.calculateCart(cart);
  //  this.save(cart);
  //  this.dispatch(cart);
  //}

  addMenuItem(menu: any, restaurantName: string, quantity: number) {
    let cart = this.retrieve();
    if (cart.restaurantName != restaurantName) {
      this.empty();
      cart = this.retrieve();
    }
    cart.restaurantName = restaurantName;
    let item = cart.items.find((p) => p.ItemId === menu.itemId);
    if (item == undefined) {
      item = new CartItem();
      item.ItemId = menu.itemId;
      cart.items.push(item);
    }

    item.ItemQuantity += quantity;
    cart.items = cart.items.filter((cartItem) => cartItem.ItemQuantity > 0);

    if (cart.items.length === 0) {
      cart.deliveryAddressId = 0;
    }

    this.calculateCart(cart);
    this.save(cart);
    this.dispatch(cart);
  }

  public empty(): void {
    const newCart = new ShoppingCart();
    this.save(newCart);
    this.dispatch(newCart);
  }

  public setDeliveryAddress(deliveryAddress: any): void {
    const cart = this.retrieve();
    cart.deliveryAddressId = deliveryAddress.addressId;
    this.save(cart);
    this.dispatch(cart);
  }

  private calculateCart(cart: ShoppingCart): void {
    cart.itemsTotal = cart.items
      .map((item) => item.ItemQuantity * this.menus.find((p) => p.itemId === item.ItemId).itemPrice)
      .reduce((previous, current) => previous + current, 0);
  }

  private save(cart: ShoppingCart): void {
    this.storage.setItem(CART_KEY, JSON.stringify(cart));
  }

  private retrieve(): ShoppingCart {
    const cart = new ShoppingCart();
    const storedCart = this.storage.getItem(CART_KEY);
    if (storedCart) {
      cart.updateFrom(JSON.parse(storedCart));
    }
    return cart;
  }

  private dispatch(cart: ShoppingCart): void {
    this.subscribers
      .forEach((sub) => {
        try {
          sub.next(cart);
        } catch (e) {
          // we want all subscribers to get the update even if one errors.
        }
      });
  }
}

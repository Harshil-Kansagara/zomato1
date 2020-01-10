import { CartItem } from "./cart-item";
import { Menu } from './menu';

export interface ICartItemWithItems extends CartItem {
  menu: Menu;
  totalCost: number
} 

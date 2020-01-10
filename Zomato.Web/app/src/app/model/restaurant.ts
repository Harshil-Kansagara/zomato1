export interface Restaurant {
  RestaurantId: number;
  RestaurantName: string;
  Location: string[];
  CuisineId: number[];
  CategoryId: number[];
}

export interface RestaurantResolved {
  restaurant: Restaurant;
  error?: any;
}

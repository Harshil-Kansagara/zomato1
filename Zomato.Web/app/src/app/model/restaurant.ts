export interface Restaurant {
  RestaurantId: number;
  RestaurantName: string;
  Location: string[];
  CuisineId: number[];
  CategoryId: number[];
  RatingAvg: number;
  Cuisines: string[];
  RestaurantLocation: string;
}

export interface RestaurantResolved {
  restaurant: Restaurant;
  error?: any;
}

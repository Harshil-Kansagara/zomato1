import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { RestaurantResolved } from '../model/restaurant';
import { RestaurantService } from './restaurant.service';

@Injectable({ providedIn: 'root' })

export class RestaurantResolver implements Resolve<RestaurantResolved> {

  constructor(private restaurantService: RestaurantService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<RestaurantResolved> {
    const id = route.paramMap.get('id');
    if (isNaN(+id)) {
      const message = `Restaurant id was not a number: ${id}`;
      console.error(message);
      return of({ restaurant: null, error: message });
    }

    return this.restaurantService.getRestaurant(+id)
      .pipe(
        map(restaurant => ({ restaurant: restaurant })),
        catchError(error => {
          const message = `Retrieval error: ${error}`;
          console.error(message);
          return of({ restaurant: null, error: message });
        })
      );
  }
}

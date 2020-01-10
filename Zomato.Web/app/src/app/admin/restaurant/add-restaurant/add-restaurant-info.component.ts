import { Component, OnInit, ViewChild } from '@angular/core';
import { Restaurant } from '../../../model/restaurant';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  templateUrl: './add-restaurant-info.component.html'
})

export class AddRestaurantInfoComponent implements OnInit {
  @ViewChild(NgForm, { static: true }) restaurantForm: NgForm;

  restaurant: Restaurant
  newLocation = '';
  errorMessage: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.parent.data.subscribe(data => {
      if (this.restaurantForm) {
        this.restaurantForm.reset();
      }
      this.restaurant = data['resolvedData'].restaurant;
    })
  }

  addLocations() {
    if (!this.newLocation) {
      this.errorMessage = 'Enter the Restaurant Location by commas and then press Add';
    } else {
    const tagArray = this.newLocation.split(',')
    this.restaurant.Location = this.restaurant.Location ? this.restaurant.Location.concat(tagArray) : tagArray;
      this.newLocation = '';
      this.errorMessage = '';
    }
  }

  removeLocation(idx: number): void {
    this.restaurant.Location.splice(idx, 1);
  }
}

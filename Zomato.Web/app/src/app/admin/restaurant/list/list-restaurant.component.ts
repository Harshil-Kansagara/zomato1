import { Component, OnInit, AfterViewInit, ViewChild, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { RestaurantService } from '../../../service/restaurant.service';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Restaurant } from '../../../model/restaurant';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './list-restaurant.component.html',
  styleUrls: ['./list-restaurant.component.css']
})

export class ListRestaurantComponent implements OnInit, AfterViewInit, OnDestroy {
  pageTitle = "Restaurant List";
  restaurantList: string[];
  restaurant: Restaurant[];
  promise: Subscription;
  displayedColumns: string[] = ['RestaurantName', 'RestaurantId'];
  dataSource = new MatTableDataSource<Restaurant>();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private router: Router, private restaurantService: RestaurantService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.loadRestaurant();
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  ngOnDestroy(): void {
    if (this.promise) {
      this.promise.unsubscribe();
    }
  }

  doFilter(value: string) {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  }

  loadRestaurant(): void {
    this.promise = this.restaurantService.getListRestaurant().subscribe(
      res => {
        if (res != null) {
          this.restaurantList = res as string[];
          this.restaurant = [];
          for (let item of this.restaurantList) {
            let data = {} as Restaurant;
            data.RestaurantId = item['restaurantId'];
            data.RestaurantName = item['restaurantName'];
            this.restaurant.push(data);
          }
         
          this.dataSource.data = this.restaurant as Restaurant[];
        }
      }
    );
  }

  deleteRestaurant(restaurantId: number): void {
    for (let each of this.restaurant) {
      if (restaurantId == each.RestaurantId) {
        if (confirm("Are you sure you want to delete " + each.RestaurantName + " ?")) {
          this.promise = this.restaurantService.deleteRestaurant(restaurantId).subscribe(
            () => {
              this.toastr.success("Restaurant deleted successfully");
              this.loadRestaurant();
            }, err => {
              console.log(err);
            }
          );
        }
      }
    }
  }

  //hyphenateUrlParams(str: string) {
  //  return str.replace(' ', '-');
  //}

  detailRestaurant(restaurantId: number): void {
    for (let each of this.restaurant) {
      if (restaurantId == each.RestaurantId) {
        this.router.navigateByUrl('admin/restaurant/' + each.RestaurantName);
      }
    }
  }

  editRestaurant(restaurantId: number): void {
    this.router.navigateByUrl('admin/restaurant/' + restaurantId +"/edit");
  }
}

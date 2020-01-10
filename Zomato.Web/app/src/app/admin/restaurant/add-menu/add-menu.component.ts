import { Component, OnInit, OnDestroy } from '@angular/core';
import { MenuService } from '../../../service/menu.service';
import { Menu } from '../../../model/menu';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { CuisineService } from '../../../service/cuisine.service';

@Component({
  templateUrl: './add-menu.component.html'
})

export class AddMenuComponent implements OnInit, OnDestroy {

  pageTitle = "Add Menu";
  menu: Menu;
  menuForm: FormGroup;
  cuisineSubscription; menuSubscription: Subscription;
  restaurantName: string;
  cuisine: string[];

  get menus(): FormArray {
    return this.menuForm.get('menus') as FormArray;
  }

  constructor(private cuisineService: CuisineService, private menuService: MenuService, private activiateRoute: ActivatedRoute, private fb: FormBuilder, private router: Router) {
    this.activiateRoute.params.subscribe(params => {
      this.restaurantName = params.restaurantName;
    });
    this.cuisineSubscription = this.cuisineService.getCuisineListByRestaurant(this.restaurantName).subscribe(
      res => {
        if (res != null) {
          this.cuisine = res as string[]
        }
      }, err => {
        console.log(err);
      }
    );
  }

  ngOnInit(): void {
    this.menu = this.menuService.initializeMenu();
    this.menuForm = this.fb.group({
      menus: this.fb.array([this.buildMenu()])
    });
  }

  ngOnDestroy(): void {
    if (this.cuisineSubscription) {
      this.cuisineSubscription.unsubscribe();
    }
    if (this.menuSubscription) {
      this.menuSubscription.unsubscribe();
    }
  }

  buildMenu(): FormGroup {
    return this.fb.group({
      ItemName: ['', Validators.required],
      ItemPrice: [0, Validators.required],
      CuisineId: [0, Validators.required]
    });
  }

  addMenus(): void {
    this.menus.push(this.buildMenu());
  }

  save(): void {
    console.log(this.menuForm.value.menus);
    this.menuSubscription = this.menuService.addMenu(this.restaurantName, this.menuForm.value.menus).subscribe(
      res => {
        this.router.navigateByUrl("admin/restaurant/" + this.restaurantName);
      }, err => {
        console.log(err);
      }
    );
  }

  cancel(): void {
    this.router.navigateByUrl('admin/restaurant/'+this.restaurantName);
  }

  removeField(i: number): void {
    this.menus.removeAt(i);
  }

}

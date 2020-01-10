import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { Debuger } from '../service/debug.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatInputModule, MatFormFieldModule, MatButtonModule, MatCheckboxModule, MatTableModule, MatPaginatorModule, MatSortModule, MatIconModule, MatCardModule, MatTabsModule, MatDividerModule, MatDialogModule, MatSelectModule, MatOptionModule, MatGridListModule } from '@angular/material';
import { ListRestaurantComponent } from './restaurant/list/list-restaurant.component';
import { AddRestaurantComponent } from './restaurant/add-restaurant/add-restaurant.component';
import { AddRestaurantInfoComponent } from './restaurant/add-restaurant/add-restaurant-info.component';
import { AddRestaurantSearchComponent } from './restaurant/add-restaurant/add-restaurant-search.component';
import { RestaurantResolver } from '../service/restaurant-resolver.service';
import { AddMenuComponent } from './restaurant/add-menu/add-menu.component';
import { DetailRestaurantComponent } from './restaurant/detail/detail-restaurant.component';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { OrderDetailDialogComponent } from '../consumer/user/order/user-order.component';
import { addCommentDialogComponent } from '../consumer/restaurant/review/review-restaurant.component';
import { BarRatingModule } from 'ngx-bar-rating';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuthAdminInterceptor } from '../service/auth-admin/auth-admin.interceptor';
import { AuthAdminGuard } from '../service/auth-admin/auth-admin.guard';
import { OrderDetailAdminDialogComponent } from './admin.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'restaurant',
    canActivate: [AuthAdminGuard],
    component: ListRestaurantComponent,
  },
  {
    path: 'restaurant/create',
    canActivate: [AuthAdminGuard],
    component: AddRestaurantComponent,
    resolve: { resolvedData: RestaurantResolver },
    children: [
      { path: '', redirectTo: 'info', pathMatch: 'full' },
      { path: 'info', component: AddRestaurantInfoComponent },
      { path: 'search', component: AddRestaurantSearchComponent }
    ]
  },
  {
    path: 'restaurant/:restaurantName',
    canActivate: [AuthAdminGuard],
    component: DetailRestaurantComponent
  },
  {
    path: 'restaurant/:restaurantName/add-menu',
    canActivate: [AuthAdminGuard],
    component: AddMenuComponent
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    HttpClientModule,
    FormsModule,
    MatInputModule, MatFormFieldModule, MatButtonModule, MatCheckboxModule, MatTabsModule,
    MatTableModule, MatPaginatorModule, MatSortModule, MatIconModule, MatCardModule,
    MatDividerModule, MatDialogModule, MatSelectModule, MatOptionModule, MatGridListModule,
    ReactiveFormsModule,
    Ng2SearchPipeModule,
    NgxPaginationModule,
    BarRatingModule,
    ToastrModule.forRoot({
      progressBar: true
    })
  ],
  declarations: [
    LoginComponent,
    RegisterComponent,
    ListRestaurantComponent,
    AddRestaurantComponent,
    AddRestaurantInfoComponent,
    AddRestaurantSearchComponent,
    DetailRestaurantComponent,
    AddMenuComponent,
    OrderDetailDialogComponent,
    addCommentDialogComponent
  ],
  providers: [Debuger, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthAdminInterceptor,
    multi: true
  }],
  entryComponents: [OrderDetailDialogComponent, addCommentDialogComponent]
})

export class AdminModule {

}

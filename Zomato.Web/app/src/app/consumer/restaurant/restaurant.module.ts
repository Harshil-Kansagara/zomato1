import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { MatSidenavModule, MatButtonModule, MatInputModule, MatFormFieldModule, MatCardModule, MatGridListModule, MatCheckboxModule, MatDialogModule, MatRadioModule, MatTabsModule } from '@angular/material';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Debuger } from '../../service/debug.service';
import { ListRestaurantUserComponent } from './list/list-restaurant-user.component';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { NgxPaginationModule } from 'ngx-pagination';
import { DetailRestaurantUserComponent} from './detail/detail-restaurant-user.component';
import { MenuComponent } from './menu/menu-restaurant.component';
import { ReviewComponent, addCommentDialogComponent } from './review/review-restaurant.component';
import { BarRatingModule } from "ngx-bar-rating";
import { ItemCartComponent } from './item-cart/item-cart.component';
import { CheckoutComponent, addUserAddressDialogComponent } from './checkout/checkout.component';
import { OrderConfirmComponent } from './order-confirm/order-confirm.component';
import { AuthUserGuard } from '../../service/auth-user/auth-user.guard';
import { AuthUserInterceptor } from '../../service/auth-user/auth-user.interceptor';


const routes: Routes = [
 // { path: ':categoryName', component: ListRestaurantUserComponent },
  { path: '', component: ListRestaurantUserComponent },
  { path: 'restaurant', redirectTo: '', pathMatch:'full' },
  {
    path: ':restaurantName', component: DetailRestaurantUserComponent,
    children: [
      { path: '', redirectTo: 'menu', pathMatch: 'full' },
      { path: 'menu', component: MenuComponent },
      { path: 'review', component: ReviewComponent },
    ]
  },
  {
    path: ':restaurantName/checkout', component: CheckoutComponent
  },
  {
    path: ':restaurantName/confirm/:orderId', component: OrderConfirmComponent,
   canActivate: [AuthUserGuard],
  }
  
]

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    HttpClientModule,
    Ng2SearchPipeModule,
    NgxPaginationModule,
    MatSidenavModule, MatButtonModule, MatInputModule, MatFormFieldModule,
    MatCardModule, MatGridListModule, MatCheckboxModule, MatDialogModule,
    MatRadioModule, MatTabsModule,
    BarRatingModule,
    ToastrModule.forRoot({
      progressBar: true
    })
  ],
  declarations: [
    ListRestaurantUserComponent,
    DetailRestaurantUserComponent,
    //addUserAddressDialogComponent,
    addCommentDialogComponent,
    MenuComponent,
    ReviewComponent,
    ItemCartComponent,
    CheckoutComponent,
    OrderConfirmComponent
  ],
  providers: [Debuger, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthUserInterceptor,
    multi: true
  }],
  entryComponents: [addCommentDialogComponent]//addUserAddressDialogComponent
})
export class RestaurantUserModule {}

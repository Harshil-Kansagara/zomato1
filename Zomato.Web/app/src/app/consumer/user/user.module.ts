import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MatListModule, MatCardModule, MatGridListModule, MatButtonModule, MatDialogModule, MatFormFieldModule, MatInputModule } from '@angular/material';
import { UserAddressComponent} from './address/user-address.component';
import { CommonModule } from '@angular/common';
import { UserOrderComponent, OrderDetailDialogComponent } from './order/user-order.component';
import { UserReviewComponent } from './review/user-review.component';
import { UserFollowerComponent } from './follower/user-follower.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { addUserAddressDialogComponent } from '../restaurant/checkout/checkout.component';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { BarRatingModule } from 'ngx-bar-rating';
import { NgxPaginationModule } from 'ngx-pagination';

const routes: Routes = [
  { path: "address", component: UserAddressComponent },
  { path: "", redirectTo: "address", pathMatch:"full" },
  { path: "order", component: UserOrderComponent },
  { path: "review", component: UserReviewComponent },
  { path: "follow", component: UserFollowerComponent }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    HttpClientModule,
    MatCardModule, MatGridListModule, MatButtonModule, MatFormFieldModule,
    MatInputModule, MatDialogModule,
    BarRatingModule,
    Ng2SearchPipeModule,
    NgxPaginationModule
  ],
  declarations: [
    UserAddressComponent,
    UserOrderComponent,
    UserReviewComponent,
    UserFollowerComponent,
    OrderDetailDialogComponent
  ],
  entryComponents: [OrderDetailDialogComponent]
})

export class UserModule { }

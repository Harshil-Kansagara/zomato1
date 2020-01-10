import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { Debuger } from '../service/debug.service';
import { MatButtonModule, MatInputModule, MatFormFieldModule, MatCardModule, MatGridListModule, MatSidenavModule, MatRadioModule, MatListModule, MatDialogModule, MatAutocompleteModule } from '@angular/material';
import { RestaurantComponent } from './restaurant/restaurant.component';
import { UserComponent } from './user/user.component';
import { addUserAddressDialogComponent } from './restaurant/checkout/checkout.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent
  },
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  {
    path: 'category/:categoryName',
    children: [
      { path: '', redirectTo: 'restaurant', pathMatch:'full' },
      { path: 'restaurant', component: RestaurantComponent, loadChildren: 'src/app/consumer/restaurant/restaurant.module#RestaurantUserModule' },
    ]
  },
  { path: 'users/:userName', component: UserComponent, loadChildren: 'src/app/consumer/user/user.module#UserModule'},
  { path: 'restaurant', component: RestaurantComponent, loadChildren: 'src/app/consumer/restaurant/restaurant.module#RestaurantUserModule' }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatSidenavModule, MatButtonModule, MatInputModule, MatFormFieldModule, MatAutocompleteModule,
    MatCardModule, MatGridListModule, MatRadioModule, MatListModule, MatDialogModule,
    ToastrModule.forRoot({
      progressBar: true
    })
  ],
  declarations: [
    HomeComponent,
    RestaurantComponent,
    UserComponent,
    addUserAddressDialogComponent
  ],
  providers: [Debuger],
  entryComponents: [addUserAddressDialogComponent]
})

export class ConsumerModule {}

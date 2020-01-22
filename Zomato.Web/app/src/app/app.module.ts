import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PageNotFoundComponent } from './page-not-found.component';
import { Debuger } from './service/debug.service';
import { ToastrModule } from 'ngx-toastr';
import { AdminComponent, OrderDetailAdminDialogComponent } from './admin/admin.component';
import { MatButtonModule, MatIconModule, MatDialogModule, MatFormFieldModule, MatInputModule } from '@angular/material';
import { OrderNotificationService } from './service/order-notification.service';
import { addCommentDialogComponent } from './consumer/restaurant/review/review-restaurant.component';
import { OrderDetailDialogComponent } from './consumer/user/order/user-order.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    PageNotFoundComponent,
    addCommentDialogComponent,
    OrderDetailAdminDialogComponent,
    OrderDetailDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatButtonModule, MatIconModule, MatDialogModule, MatFormFieldModule, MatInputModule,
    ToastrModule.forRoot({
      progressBar: true
    }),
    AppRoutingModule
  ],
  providers: [OrderNotificationService],
  entryComponents: [OrderDetailAdminDialogComponent, addCommentDialogComponent, OrderDetailDialogComponent],
  //providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

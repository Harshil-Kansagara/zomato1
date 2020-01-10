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
import { MatButtonModule, MatIconModule, MatDialogModule } from '@angular/material';
import { OrderNotificationService } from './service/order-notification.service';


@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    PageNotFoundComponent,
    OrderDetailAdminDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule, MatIconModule, MatDialogModule,
    ToastrModule.forRoot({
      progressBar: true
    }),
    AppRoutingModule
  ],
  providers: [OrderNotificationService],
  entryComponents: [OrderDetailAdminDialogComponent],
  //providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

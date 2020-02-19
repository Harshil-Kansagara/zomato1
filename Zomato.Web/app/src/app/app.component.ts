import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import * as jwt_decode from 'jwt-decode';
import { OrderNotificationService } from './service/order-notification.service';
import { Order } from './model/order';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent{

}

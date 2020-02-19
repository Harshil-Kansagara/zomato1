import { Injectable, EventEmitter } from "@angular/core";
import { Order } from '../model/order';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ToastrService } from 'ngx-toastr';

Injectable({ providedIn: "root" })
export class OrderNotificationService {

  orderReceived = new EventEmitter<Order>();
  deliveryReceived = new EventEmitter<string>();
  token: string;
  connectionEstablished = new EventEmitter<Boolean>();
  private connectionIsEstablished = false;
  private _hubConnection: HubConnection;
  private _hubConnectionStartPromise: Promise<void>;

  constructor() {
    this.getUserId();
    this.createConnection();
    this.startConnection();
    this.registerOnServerEvents();
  }

  sendAdminOnlineNotification(a: boolean) {
    this._hubConnectionStartPromise.then(() => {
      this._hubConnection.invoke('AdminNotification', a);
    });
  }

  sendDeliveryNotification(orderId: number) {
    this._hubConnectionStartPromise.then(() => {
      this._hubConnection.invoke('DeliveryOrder', orderId);
    });
  }

  public createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      //.withUrl("http://localhost:59227/OrderHub", { accessTokenFactory: () => this.token })
      .withUrl("https://zomato.us-west-1.elasticbeanstalk.com/OrderHub", { accessTokenFactory: () => this.token })
      .build();
  }

  private startConnection(): void {
    this._hubConnectionStartPromise = this._hubConnection
      .start();
    if (this._hubConnectionStartPromise) {
    this._hubConnectionStartPromise
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(function () { this.startConnection(); }, 5000);
      });
    }
  }

  private registerOnServerEvents(): void {
    this._hubConnection.on('OrderReceived', (data: Order) => {
      this.orderReceived.emit(data);
    });
    this._hubConnection.on("DeliverySuccessful", (data: string) => {
      this.deliveryReceived.emit(data);
    });
  }

  private getUserId(): void {
    this.token = localStorage.getItem('token');
  }
}

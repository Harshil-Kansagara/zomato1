import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserAddress } from '../../../model/userAddress';
import { UserAddressService } from '../../../service/user-address.service';
import * as jwt_decode from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { MatDialog} from '@angular/material';
import { addUserAddressDialogComponent } from '../../restaurant/checkout/checkout.component';

@Component({
  templateUrl: './user-address.component.html',
  styleUrls: ['./user-address.component.css']
})

export class UserAddressComponent implements OnInit, OnDestroy {
  restaurantList: string[] = [];
  addressList: UserAddress[];
  addressSubscription: Subscription;
  token; userId;decode_token: string;

  constructor(private addressService: UserAddressService,
    private toastr: ToastrService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.getUserId();
    this.loadAddressList();
  }

  ngOnDestroy(): void {
    if (this.addressSubscription) {
      this.addressSubscription.unsubscribe();
    }
  }

  getUserId(): void {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      this.decode_token = jwt_decode(this.token);
      if (this.decode_token['UserRole'] == "user") {
        this.userId = this.decode_token['UserId'];
      }
    }
  }

  loadAddressList(): void {
    this.addressSubscription = this.addressService.getUserAddressList(this.userId).subscribe(
      res => {
        if (res != null) {
          this.addressList = res as UserAddress[];
        }
      }, err => {
        console.log(err);
      }
    );
  }

  deleteAddress(addressId: number): void {
    this.addressSubscription = this.addressService.deleteUserAddress(addressId).subscribe(
      res => {
        this.toastr.success("Address deleted successfully");
        this.loadAddressList();
      },
      err => {
        console.log(err);
      }
    );
  }

  openAddDialog(address: string, addressid:number): void {
    const dialogRef = this.dialog.open(addUserAddressDialogComponent, {
      width: '250px',
      data: { Address:address, AddressId:addressid }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        result.UserId = this.userId;
        this.addressSubscription = this.addressService.addUserAddress(result).subscribe(
          res => {
            if (result.AddressId === addressid) {
              this.toastr.success("Location Updated Successfully");
            } else {
              this.toastr.success("Location Added Successfully");
            }
            this.loadAddressList();
          }, err => {
            if (err.status == 400) {
              this.toastr.error("Location Already exists");
            } else {
              console.log(err);
            }
          }
        );
      }
    });
  }
}

import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { UserAddress } from '../model/userAddress';

@Injectable({ providedIn: "root" })
export class UserAddressService {

  baseUrl: string = "api/useraddress/";

  constructor(private http: HttpClient) { }

  getUserAddressList(userId: string) {
    return this.http.get(this.baseUrl + userId + "/address");
  }

  addUserAddress(address: UserAddress) {
    return this.http.post(this.baseUrl, address);
  }

  deleteUserAddress(addressId: number) {
    return this.http.delete(this.baseUrl + addressId);
  }

  initializeUserAddress(): UserAddress {
    return {
      AddressId: 0,
      Address: null,
      UserId: null
    }
  }
}

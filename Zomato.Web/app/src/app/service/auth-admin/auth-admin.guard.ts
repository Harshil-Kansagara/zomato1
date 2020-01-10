import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})

export class AuthAdminGuard implements CanActivate {

  token; decode_token; userName; searchText; userId; role: string = "";

  constructor(private router: Router) {
  }

  canActivate(next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    this.token = localStorage.getItem('token');
    if (this.token != null) {
      this.decode_token = jwt_decode(this.token)
      if (this.decode_token['UserRole'] == "admin") {
        return true;
      } else {
        return false;
      }
    } else {
      this.router.navigateByUrl('/admin');
      return false;
    }
  }

}

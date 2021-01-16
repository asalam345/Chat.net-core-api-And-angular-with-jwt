import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { CookieService } from './cookie.service';

@Injectable()

export class AuthGuard implements CanActivate {

  constructor(private router: Router, private cookieService: CookieService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
      if (localStorage.getItem('token') || this.cookieService.cookieIsExist('token')) {
          // logged in so return true
          return true;
      }

      // not logged in so redirect to login page
      this.router.navigate(['login'], { queryParams: { returnUrl: state.url }});
      return false;
  }
}

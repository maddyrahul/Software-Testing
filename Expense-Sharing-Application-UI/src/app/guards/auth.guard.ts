import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isLoggedIn()) {
      // Check user role
      const userRole = this.authService.getUserRole();
      if (route.data && route.data['role'] && route.data['role'] === userRole) { // Use bracket notation here
        return true;
      } else {
        // Unauthorized - redirect to login or access denied page
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
      }
    } else {
      // Not logged in - redirect to login with return URL
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }
  }
}

// La protección de rutas se implementa utilizando el guard Auth Guard (https://www.yourteaminindia.com/blog/angular-authentication-using-route-guards) :
// se utiliza para proteger las rutas de la aplicación y asegurarse de que solo los usuarios autenticados puedan acceder a ellas.
// Para hacer esto, el guard implementa el métod canActivate(), que se llama antes de cargar una ruta.

import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthenticationService,
    private router: Router) {


  }

  // Se verifica si el usuario está autenticado utilizando el servicio AuthenticationService.
  // Si el usuario está autenticado, se permite la navegación a la ruta solicitada.
  // Si el usuario no está autenticado, se redirige al usuario a la página de inicio de sesión.
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }

    // TODO: Revisar Roles

    return true;
  }

}

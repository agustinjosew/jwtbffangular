import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }


  saveUser(user: any) {
    localStorage.setItem('user', JSON.stringify(user));
  }

  getUser() {
    return JSON.parse(localStorage.getItem('user')!);
  }

  // Verifica si el usuario está autenticado comprobando si hay un usuario en el almacenamiento local.
  // Va a devolver true si el usuario está autenticado y false en caso contrario.
  // Si alguien intenta crear un usuario manualmente y guardarlo en localStorage,
  // en efecto este servicio lo considerará "autenticado" pero al querer hacer cualquier operación como usuario autenticado no podrá,
  // ya que no tendrá ninguna cookie válida.
  isAuthenticated() {
    return !!this.getUser();
  }

  // Verifica si el usuario autenticado tiene un rol específico.
  // Comprueba si el usuario está autenticado y si el rol especificado está presente en el array de roles del usuario.
  // También aquí puede suceder que se agreguen roles y se haga logre hacer una escalación de permisos,
  // pero los roles siempre y digo siempre, se deben de revisar en el backend antes de realizar o mostrar información.
  isInRole(role: string) {
    return this.isAuthenticated() && this.getUser().roles.includes(role);
  }

  // Realiza una solicitud POST al endpoint local-login del BFF con las credenciales de inicio de sesión proporcionadas.
  // Si la solicitud es exitosa, el métod guarda el objeto de usuario en el almacenamiento local y devuelve la respuesta del servidor.
  login(username: string, password: string): Observable<any> {
    return this.http.post('local-login', { username, password })
      .pipe(
        catchError(error => {
          console.error('Error en la solicitud de inicio de sesión:', error);
          throw error;
        }),
        tap((response: any) => {
          this.saveUser(response);
        })
      );
  }

  // Realiza una solicitud POST al endpoint local-logout del BFF para cerrar la sesión del usuario.
  // Después de cerrar la sesión, el métod elimina el objeto de usuario del almacenamiento local y redirige al usuario a la página de inicio de sesión.
  logout() {
    this.http.post('local-logout', {}).subscribe(result => {
      localStorage.removeItem('user');
      window.location.href = '/login';
    });
  }
}

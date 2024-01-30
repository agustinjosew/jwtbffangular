import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../shared/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public isBusy = false;
  public loginModel: {
    username?: string,
    password?: string
  } = {};

  constructor(
    private authService: AuthenticationService
  ) { }

  ngOnInit(): void {
  }

  onSubmit() {

    if (this.isBusy) {
      return;
    }

    // El botón de Login se deshabilitará cuando la variable isBusy sea true,
    // con esto evitamos que el usuario haga clic varias veces en el botón mientras se procesa la solicitud de inicio de sesión.
    this.isBusy = true;

    // Post loginModel a /login
    // Al mandar el formulario se llama al servicio de autenticación (Authentication.Service) para realizar la solicitud de inicio de sesión.
    // Si la solicitud es exitosa, se redirige al usuario a la página de inicio.
    this.authService.login(this.loginModel.username!, this.loginModel.password!)
      .subscribe(_ => {
        window.location.href = '/home';
      })
      .add(() => this.isBusy = false);
  }
}

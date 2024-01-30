// Se utiliza para mostrar u ocultar elementos del DOM según el rol del usuario autenticado.

import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthenticationService } from './authentication.service';

@Directive({
  // Permite especificar el rol necesario para mostrar el elemento.
  // Si el usuario autenticado tiene el rol especificado, se muestra el elemento.
  // De lo contrario, se oculta.
  selector: '[appAuthorize]'
})
export class AuthorizeDirective {


  constructor(
    private templateRef: TemplateRef<any>,
    // Se utiliza ViewContainerRef para manipular el contenedor de vistas y mostrar u ocultar elementos del DOM.
    private viewContainer: ViewContainerRef,
    // Se utiliza el servicio AuthenticationService para verificar si el usuario autenticado tiene el rol requerido.
    private authService: AuthenticationService
  ) { }


  @Input() set appAuthorize(roleName: string) {

    if (!this.authService.isInRole(roleName)) {
      this.viewContainer.clear();
    } else {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }

}

La adopción del patrón **Backend for Frontend (BFF)** ha ganado una creciente importancia en la actualidad. 
El BFF se ha convertido en una **_herramienta clave para optimizar la comunicación entre el frontend y el backend_**, 
al proporcionar una capa intermedia que se ajusta específicamente a las necesidades del cliente. 
En este artículo, vamos a explorar la implementación del patrón BFF en un contexto en el que se utiliza Angular como la aplicación cliente y se combina con ASP.NET Core junto con YARP como el backend correspondiente. Además, examinaremos cómo establecer procesos de autenticación y autorización en ambos extremos, es decir, tanto en el BFF como en la API del backend.

### Backend for Frontend

Antes de entrar en detalles en la implementación, es importante que comprendamos bien qué es el patrón Backend for Frontend (BFF).
El BFF es como una especie de intermediario que se pone entre el frontend y el backend principal, con el propósito de amoldarse a lo que precisa el cliente. Básicamente, ofrece una interfaz API especializada y pensada especialmente para el frontend, lo que hace que la comunicación sea más sencilla y mejore su rendimiento.

Para llevar a cabo esta implementación del BFF, voy a utilizar Angular como nuestra aplicación cliente, y para el backend, vamos a recurrir a ASP.NET Core junto con YARP. En este contexto, Angular sería nuestro cliente principal, mientras que el BFF, que está hecho con ASP.NET Core y YARP, se encargará de interactuar con una API protegida.

En este BFF que armamos con Angular, vamos a implementar la autenticación y la autorización utilizando cookies. Las cookies son una manera segura de guardar información de autenticación en el cliente. Angular se va a hacer cargo de la autenticación del usuario, y el BFF va a establecer una cookie segura que contendrá un JWT (JSON Web Token) encriptado. Esta cookie se enviará en cada solicitud que hagamos al BFF.

La API protegida también va a estar construida con ASP.NET Core y nos va a proporcionar servicios y recursos protegidos. Para manejar la autenticación en el backend API, vamos a usar el esquema de autenticación con Bearer Tokens. Cuando el BFF reciba una solicitud autenticada con una cookie, va a extraer el JWT encriptado y lo va a incluir como un token (Bearer token) en la solicitud que va hacia el backend API.

#### Reverse Proxy (Con YARP)

YARP (Yet Another Reverse Proxy) es una biblioteca de .NET que nos va a permitir redirigir y transformar las peticiones del BFF hacia el backend API. Vamos a usar YARP para asegurarnos de que el JWT encriptado en la cookie se envíe correctamente hacia el backend API y así mantener la seguridad en la comunicación.

### Porque es importante aplicar BFF en aplicaciones SPA

La verdad es que implementar el BFF en aplicaciones SPA es algo que tiene mucha importancia. En estas aplicaciones SPA, a veces usamos el almacenamiento local del navegador, como el _**localStorage**_, para guardar el JSON Web Token (JWT) que tiene la info de autenticación del usuario. Pero hay que tener cuidado porque esto puede ser inseguro. Con los ataques de tipo **XSS** (Cross-Site Scripting) y las artimañas de phishing, un atacante podría meterse en el contenido guardado en el navegador y sacar el JWT, lo que le daría la chance de hacerse pasar por el usuario.

Ahora, cuando ponemos en marcha el BFF, armamos una especie de capa intermedia entre el frontend y el backend principal. Esto nos deja controlar y bajar los riesgos de seguridad que vienen con el manejo del JWT. En vez de tener el JWT directo en el cliente, el BFF puede usar métodos más seguros, como cookies seguras, para guardar y mandar la info de autenticación. Con esto, se reduce mucho la exposición del JWT a potenciales ataques.

El BFF también nos da una separación bien clara de tareas entre el frontend y el backend. El frontend se enfoca en darle una buena experiencia al usuario y mostrar los datos, mientras que el BFF se encarga de las cosas específicas del cliente, como la autenticación y autorización. Esta separación hace que el sistema sea más modular y escalable, porque cada parte puede crecer por su cuenta.

### Flujo de la Aplicación

![](D:\Edraw\Gifs\JWTBFFAngular.gif)

Te cuento cómo se da la comunicación entre Angular, el BFF y la API protegida. 

- Todo empieza cuando el cliente Angular le manda una solicitud al Backend for Frontend (BFF) con una cookie que tiene la info de autenticación.

- El BFF recibe la solicitud que vino del cliente Angular y ahí puede meter mano en la cookie, sacando el token de autenticación. Después, reenvía la solicitud a la API.

- Ahí, en la API, se ponen las pilas y verifican el token de autenticación que llegó como Bearer en la solicitud que mandó el BFF.

- Cuando la API termina de revisar el token y ve que todo está en orden, manda una respuesta de vuelta al BFF.

- El BFF recibe esa respuesta de la API y la vuelve a mandar al cliente Angular, que es el que se encarga de procesarla y mostrar los datos o hacer lo que sea que tenga que hacer.

-- Mirá el diagrama, te lo muestra todo más claro. 

- El cliente Angular le tira una solicitud al BFF con una cookie que tiene la info de autenticación. 
- - El BFF, al agarrar la solicitud, chusmea la cookie y saca el token de autenticación para mandárselo a la API. 
- - - Ahí, la API se pone las pilas, verifica el token y le manda la respuesta al BFF. Y el BFF, adiviná qué, le manda la respuesta al cliente Angular, que es el que se encarga de procesarla y mostrar los datos como corresponde.

**Este enfoque asegura que la comunicación entre Angular, el BFF y la API protegida sea segura y efectiva. El uso de cookies para la autenticación del lado del cliente y la autenticación con Bearer Token en la API garantiza la seguridad de la info de autenticación y protege los recursos para que nadie sin permiso se meta a toquetearlos.**
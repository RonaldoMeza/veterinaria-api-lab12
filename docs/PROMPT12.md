# DESARROLLO DEL LABORATORIO 12 – WEB API REST CON ASP.NET CORE Y ENTITY FRAMEWORK CORE

Actúa como un **desarrollador senior especializado en ASP.NET Core, C#, Entity Framework Core, SQL Server y arquitectura de Web API REST**. Tu responsabilidad es desarrollar el proyecto que solicita el Laboratorio 12 del curso, siguiendo estrictamente las indicaciones del profesor y aplicando buenas prácticas de desarrollo.

Quiero que trabajes de manera ordenada, profesional y progresiva. No te limites a generar código: también debes verificar su funcionamiento, ejecutar pruebas y preparar la documentación necesaria para entregar el laboratorio.

---

## 1. CONTEXTO DEL ENTORNO

Actualmente tengo abierto en Visual Studio Code el directorio:

`F:\DAEA\LAB12-DAEA\`

Dentro de este directorio existen dos proyectos:

**Proyecto principal (el que debes desarrollar):**
- Ruta: `F:\DAEA\LAB12-DAEA\veterinaria-api`
- Nombre: `veterinaria-api`
- Framework: .NET 10
- Tipo: ASP.NET Core Web API con controladores.
- Ya fue creado mediante el siguiente comando:

```powershell
dotnet new webapi -n veterinaria-api --use-controllers
```

Actualmente me encuentro trabajando desde:

```powershell
F:\DAEA\LAB12-DAEA\veterinaria-api
```

**Proyecto de referencia del profesor:**
- Ruta: `F:\DAEA\LAB12-DAEA\products-api`
- Nombre: `products-api`

Este segundo proyecto es un ejemplo proporcionado por el profesor. Puedes inspeccionarlo para comprender cómo desarrolla sus proyectos, cómo organiza los controladores, configura Entity Framework Core, implementa los endpoints y estructura su código.

IMPORTANTE:
- No modifiques ningún archivo del proyecto `products-api`.
- Utilízalo únicamente como referencia de aprendizaje.
- No copies su lógica indiscriminadamente, porque el tema del laboratorio será diferente.
- El desarrollo debe realizarse exclusivamente dentro de `veterinaria-api`.

**Documento de requisitos:**

En la raíz del directorio de trabajo se encuentra el archivo:

`LAB12.md`

Este archivo contiene todas las instrucciones del Laboratorio 12, incluyendo:
- Tema y modelo de datos.
- Relación 1:N.
- Generación de modelos mediante Scaffold (Database First).
- Data Annotations.
- CRUD completo.
- Mínimo 10 endpoints.
- Consultas LINQ y operaciones asíncronas.
- ActionResult y códigos HTTP.
- Reglas de negocio con respuesta 409.
- Swagger y CORS.
- Despliegue gratuito de la API y base de datos en la nube.
- Entregables, observaciones y conclusiones.

**Debes leer completamente `LAB12.md` antes de comenzar.** Este documento es la principal fuente de requisitos y ninguna decisión técnica debe contradecirlo.

---

## 2. OBJETIVO DEL PROYECTO

Desarrollar una Web API REST para la gestión de una veterinaria utilizando:

- C#.
- ASP.NET Core Web API.
- .NET 10.
- Entity Framework Core.
- SQL Server.
- LINQ.
- Swagger / SwaggerUI.
- Visual Studio Code.

La API debe permitir administrar propietarios y sus mascotas, utilizando una relación uno a muchos (1:N).

El desarrollo debe cumplir todos los puntos establecidos en `LAB12.md`.

## 3. TEMA Y MODELO DE DATOS

El tema seleccionado es:

**Sistema de gestión de una veterinaria.**

La API permitirá administrar la información de los propietarios y sus mascotas.

Propongo inicialmente las siguientes entidades:

### Entidad: Propietario

Representa a una persona responsable de una o varias mascotas.

Campos sugeridos:
- IdPropietario (PK).
- Nombres.
- Apellidos.
- DNI.
- Telefono.
- Correo.
- Direccion.

### Entidad: Mascota

Representa a una mascota registrada en la veterinaria.

Campos sugeridos:
- IdMascota (PK).
- Nombre.
- Especie.
- Raza.
- Sexo.
- FechaNacimiento.
- Peso.
- IdPropietario (FK).

### Relación

Un propietario puede tener varias mascotas, pero cada mascota pertenece a un solo propietario.

Relación:

`Propietario (1) ───────── (N) Mascota`

La clave foránea será `IdPropietario`.

Puedes ajustar los nombres, tipos de datos y restricciones si encuentras una alternativa técnicamente mejor, siempre que mantengas el propósito y los requisitos del laboratorio.

### Reglas de negocio obligatorias

Implementa como mínimo dos reglas de negocio que devuelvan HTTP 409 Conflict.

Propongo inicialmente:

1. **DNI duplicado:** no permitir registrar dos propietarios con el mismo DNI.
2. **Eliminación de propietarios con mascotas:** no permitir eliminar un propietario que todavía tenga mascotas registradas.

Las reglas deben implementarse correctamente en los controladores y devolver mensajes claros mediante `Conflict(...)`.

También debes controlar que no se registren mascotas con propietarios inexistentes.

---

## 4. FASE 1: INSPECCIÓN Y ANÁLISIS

Antes de programar:

1. Lee completamente `LAB12.md`.
2. Inspecciona la estructura actual de `veterinaria-api`.
3. Inspecciona el proyecto `products-api` para identificar las convenciones utilizadas por el profesor.
4. Comprueba las versiones de .NET y los paquetes instalados.
5. Identifica qué configuraciones ya existen y cuáles deben implementarse.

Después, presenta un pequeño diagnóstico indicando:
- Estructura actual del proyecto.
- Diferencias importantes entre ambos proyectos.
- Paquetes necesarios.
- Arquitectura que propones.
- Archivos que necesitarás crear o modificar.
- Posibles dificultades técnicas.

No reconstruyas el proyecto desde cero, porque ya está creado.

---

## 5. FASE 2: BASE DE DATOS SQL SERVER

El profesor solicita trabajar mediante el enfoque **Database First**, por lo tanto, debes respetarlo.

Primero diseña la base de datos SQL Server.

Requisitos:

1. Crear una base de datos para la veterinaria.
2. Crear las tablas Propietarios y Mascotas.
3. Establecer la relación 1:N mediante claves primarias y foráneas.
4. Definir restricciones de integridad.
5. Implementar las restricciones necesarias para evitar DNI duplicados.
6. Insertar datos de prueba suficientes para realizar todas las operaciones CRUD.
7. Crear índices cuando corresponda.

Genera un script SQL completo y organizado.

Ubicación sugerida:

`database/veterinaria_db.sql`

El script debe incluir:
- Creación de la base de datos.
- Creación de tablas.
- Restricciones.
- Relaciones.
- Índices.
- Datos de ejemplo.

**IMPORTANTE:** primero debes ejecutar y comprobar el script en SQL Server local. Después debes generar las entidades y el DbContext mediante Scaffold.

### Conexión local

La instancia de SQL Server que utilizo es:

`DESKTOP-7S1EHKS\SQLEXPRESS`

No inventes el nombre de la base de datos ni el método de autenticación. Define el nombre de la base de datos de acuerdo con el diseño y comprueba qué autenticación utiliza mi SQL Server.

Si necesitas confirmar alguna configuración de conexión que no puedas inspeccionar, pregúntame antes de continuar.

La cadena de conexión debe mantenerse fuera del repositorio, utilizando configuración local y posteriormente variables de entorno o secretos del proveedor en la nube.

Nunca publiques contraseñas ni credenciales.

### Scaffold

Genera los modelos mediante Scaffold utilizando Entity Framework Core y SQL Server.

Instala los paquetes necesarios y utiliza el comando correspondiente a Database First.

El resultado debe incluir:
- Entidad Propietario.
- Entidad Mascota.
- DbContext.

No implementes las entidades manualmente como sustitución de Scaffold.

Posteriormente, incorpora las Data Annotations solicitadas por el profesor:

- `[Required]`
- `[StringLength]`
- `[Range]`, cuando corresponda.

La propiedad de navegación de la entidad hija debe admitir nulo (`Propietario?`) para permitir registrar una mascota enviando solamente la clave foránea.

Evita ciclos de serialización JSON sin utilizar DTOs. Puedes utilizar `[JsonIgnore]` en la colección de navegación o `ReferenceHandler.IgnoreCycles`, justificando la elección.

Ten cuidado con las regeneraciones de Scaffold para no perder las modificaciones realizadas.

---

## 6. FASE 3: DESARROLLO DE LA WEB API

Desarrolla la API utilizando controladores tradicionales de ASP.NET Core.

No utilices Minimal API.

Estructura sugerida:

```text
veterinaria-api/
│
├── Controllers/
│   ├── PropietariosController.cs
│   └── MascotasController.cs
│
├── Models/
│   ├── Propietario.cs
│   └── Mascota.cs
│
├── Data/
│   └── VeterinariaContext.cs
│
├── database/
│   └── veterinaria_db.sql
│
├── Properties/
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── veterinaria-api.csproj
```

Adapta la estructura a lo que genere Scaffold y a las convenciones del proyecto de referencia.

### Controladores y endpoints

Debes implementar CRUD completo para las dos entidades.

Como mínimo:

**PropietariosController**
- GET: listar propietarios.
- GET por ID: obtener un propietario.
- POST: registrar un propietario.
- PUT: actualizar un propietario.
- DELETE: eliminar un propietario.

**MascotasController**
- GET: listar mascotas.
- GET por ID: obtener una mascota.
- POST: registrar una mascota.
- PUT: actualizar una mascota.
- DELETE: eliminar una mascota.

Esto representa un mínimo de 10 endpoints.

Además, debes implementar:

**Filtros mediante query string**

Por ejemplo:

`GET /api/mascotas?especie=Perro&nombre=Max`

Los filtros deben funcionar realmente mediante LINQ y permitir combinar criterios.

**Ruta anidada**

Por ejemplo:

`GET /api/propietarios/{id}/mascotas`

Debe devolver las mascotas asociadas al propietario indicado.

### Requisitos técnicos

- Utilizar `[ApiController]`.
- Utilizar rutas `api/[controller]`.
- Utilizar `ActionResult` o `ActionResult<T>`.
- Todas las acciones deben ser asíncronas.
- Utilizar LINQ.
- Utilizar `ToListAsync()`.
- Utilizar `FindAsync()`.
- Utilizar `AnyAsync()`.
- Utilizar `Include()` cuando corresponda.
- Utilizar `AsNoTracking()` en consultas de solo lectura cuando corresponda.
- Inyectar directamente el DbContext en los controladores.
- No utilizar DTOs, ya que el laboratorio solicita trabajar directamente con las entidades.

No introduzcas patrones, capas o tecnologías adicionales que compliquen innecesariamente el desarrollo o se alejen de lo solicitado por el profesor.

---

## 7. FASE 4: CÓDIGOS DE RESPUESTA HTTP

Debes utilizar correctamente los siguientes resultados:

| Resultado | Código | Aplicación |
|---|---:|---|
| `Ok(datos)` | 200 | GET exitoso |
| `CreatedAtAction(...)` | 201 | POST exitoso |
| `NoContent()` | 204 | PUT y DELETE exitosos |
| `BadRequest()` o `ValidationProblem(...)` | 400 | Datos inválidos o inconsistencias |
| `NotFound()` | 404 | Recurso inexistente |
| `Conflict(...)` | 409 | Regla de negocio incumplida |

Verifica que las respuestas sean coherentes con cada operación.

No devuelvas siempre HTTP 200.

Los errores deben incluir mensajes comprensibles que permitan identificar el problema.

---

## 8. FASE 5: SWAGGER Y CONFIGURACIONES

Swagger será la herramienta principal para probar todos los endpoints.

Debes configurar correctamente:

1. OpenAPI.
2. SwaggerUI.
3. Visualización de todos los controladores y sus endpoints.
4. Descripciones y parámetros de los endpoints cuando corresponda.
5. Visualización de los esquemas de las entidades.
6. Pruebas de solicitudes GET, POST, PUT y DELETE.

**IMPORTANTE:** quiero que SwaggerUI esté disponible también cuando la API se publique en la nube, no solamente en Development.

Comprueba que la configuración funcione en producción sin exponer información sensible.

### CORS

Configura CORS utilizando un origen concreto.

No utilices:

`AllowAnyOrigin()`

La configuración debe permitir posteriormente el acceso desde un origen específico, sin habilitar indiscriminadamente todos los dominios.

---

## 9. FASE 6: PRUEBAS LOCALES

Antes de desplegar, verifica que la API funcione correctamente en local.

Ejecuta el proyecto y prueba los endpoints mediante SwaggerUI.

Comprueba:

- Listado de propietarios.
- Consulta de propietario por ID.
- Registro de propietario.
- Actualización de propietario.
- Eliminación de propietario.
- Listado de mascotas.
- Consulta de mascota por ID.
- Registro de mascota.
- Actualización de mascota.
- Eliminación de mascota.
- Filtros mediante query string.
- Ruta anidada de mascotas por propietario.
- Validaciones de datos.
- Claves foráneas inexistentes.
- DNI duplicado.
- Eliminación de propietarios con mascotas.
- Códigos HTTP correspondientes.

Corrige los errores encontrados antes de pasar al despliegue.

---

## 10. FASE 7: DESPLIEGUE GRATUITO EN LA NUBE

El laboratorio exige publicar tanto la API como la base de datos en servicios en la nube utilizando planes gratuitos.

Investiga alternativas compatibles con:

- ASP.NET Core .NET 10.
- SQL Server o un proveedor alternativo compatible con EF Core.
- Conexiones entre la API y la base de datos.
- Variables de entorno.
- HTTPS.
- SwaggerUI.

Evalúa las alternativas gratuitas y selecciona una combinación viable.

No asumas que un proveedor es gratuito sin comprobar sus condiciones actuales.

Debes justificar la elección de ambos proveedores.

Si eliges una base de datos diferente de SQL Server:
- Cambia el paquete de EF Core.
- Cambia el método `UseSqlServer()` por el proveedor correspondiente.
- Adapta el script SQL.
- Documenta las diferencias en las conclusiones.

Si mantienes SQL Server, conserva el proveedor correspondiente.

### Seguridad

- No subas cadenas de conexión al repositorio.
- No subas contraseñas.
- No publiques archivos `.env`.
- Configura los secretos mediante variables de entorno o mecanismos del proveedor.
- Comprueba que `.gitignore` proteja los archivos sensibles.

### Publicación

Debes:
1. Crear la base de datos en la nube.
2. Ejecutar el script SQL.
3. Publicar la API.
4. Configurar la cadena de conexión en el hosting.
5. Comprobar la comunicación entre API y base de datos.
6. Acceder al Swagger publicado.
7. Ejecutar nuevamente las pruebas sobre la API de producción.

No consideres terminado el despliegue hasta comprobar que la API realmente responde y obtiene datos de la base de datos en la nube.

---

## 11. FASE 8: DOCUMENTACIÓN Y ENTREGABLES

Además del código fuente, necesito que generes un archivo Markdown independiente que me sirva como guía para completar y presentar el laboratorio.

Nombre sugerido:

`ENTREGABLES_Y_PRUEBAS_LAB12.md`

Debe contener las siguientes secciones:

### A. Explicación del tema

Redacta una explicación clara y natural de la veterinaria:

- Qué problema resuelve el sistema.
- Para qué sirve la API.
- Qué entidades se implementaron.
- Cómo funciona la relación 1:N.
- Qué reglas de negocio se implementaron.

Debe estar redactado de manera que pueda utilizarlo directamente en mi entrega académica.

### B. Enlace del repositorio

Incluye un apartado para colocar:
- URL del repositorio GitHub.
- Estructura general del proyecto.
- Ubicación del script SQL.

No inventes enlaces. Déjalos como pendientes hasta que el repositorio exista y esté publicado.

### C. Enlace de la API en la nube

Incluye:
- URL de la API publicada.
- URL de SwaggerUI.
- Proveedor de hosting.
- Proveedor de base de datos.
- Justificación de la elección.
- Tipo de plan utilizado.

### D. Guía de pruebas de Swagger

Esta es una de las partes más importantes.

Necesito una guía detallada de absolutamente todas las pruebas que debo realizar para adjuntar las capturas solicitadas por el profesor.

Organiza las pruebas por controlador y endpoint.

Para cada prueba especifica:

1. Número de prueba.
2. Nombre de la operación.
3. Método HTTP.
4. URL completa del endpoint.
5. Datos JSON que debo enviar, cuando corresponda.
6. Pasos que debo realizar en Swagger.
7. Resultado esperado.
8. Código HTTP esperado.
9. Qué debe aparecer en la respuesta.
10. Nombre sugerido para guardar la captura de pantalla.

Debes incluir pruebas exitosas y pruebas de errores.

Incluye expresamente:
- GET exitosos (200).
- POST exitosos (201).
- PUT exitosos (204).
- DELETE exitosos (204).
- Datos inválidos (400).
- Recursos inexistentes (404).
- DNI duplicado (409).
- Eliminación de propietario con mascotas (409).
- Filtros mediante query string.
- Ruta anidada.
- Clave foránea inexistente.
- Validaciones Data Annotations.

No te limites a mostrar una lista de endpoints. Quiero saber exactamente qué debo ejecutar, qué información enviar y qué captura debo tomar.

**IMPORTANTE:** organiza las pruebas de forma que no se eliminen datos antes de necesitarlos. Indica el orden correcto para registrar, actualizar, consultar y eliminar registros.

Distingue claramente entre las pruebas que se deben ejecutar en local y las pruebas definitivas que se deben capturar en la API publicada en la nube.

### E. Observaciones

Redacta tres observaciones en primera persona, con lenguaje natural y académico.

Deben estar relacionadas con aspectos reales del desarrollo, por ejemplo:
- Uso de Entity Framework Core y Scaffold.
- Implementación de validaciones y reglas de negocio.
- Despliegue y configuración de servicios en la nube.

### F. Conclusiones

Redacta tres conclusiones en primera persona.

Deben reflejar lo que aprendí y desarrollé durante el laboratorio:
- Desarrollo de una API REST.
- Gestión de datos mediante LINQ y Entity Framework Core.
- Publicación y comprobación de una API conectada a una base de datos en la nube.

No inventes problemas o experiencias que no hayan ocurrido. Redacta conclusiones coherentes con el desarrollo real.

### G. Checklist final de entrega

Incluye una lista de verificación con todos los requisitos y entregables del profesor para que pueda comprobar que no me falta nada.

---

## 12. FORMA DE TRABAJO Y REGLAS IMPORTANTES

Quiero que sigas estas instrucciones durante todo el desarrollo:

1. Trabaja por fases y en orden. No intentes desarrollar todo de golpe.
2. Antes de modificar archivos, inspecciona el proyecto y comprende su estructura.
3. Respeta los requisitos de `LAB12.md`.
4. Utiliza `products-api` únicamente como referencia.
5. No modifiques archivos fuera de `veterinaria-api`, salvo que te solicite expresamente lo contrario.
6. No inventes configuraciones, credenciales, conexiones ni resultados de pruebas.
7. No hardcodees cadenas de conexión ni contraseñas.
8. No generes código innecesariamente complejo.
9. Mantén nombres de clases, métodos, variables y archivos consistentes.
10. Utiliza buenas prácticas de C# y ASP.NET Core.
11. Comprueba que el proyecto compile después de cada fase importante.
12. Ejecuta las pruebas necesarias y corrige los errores antes de avanzar.
13. No afirmes que un endpoint funciona si no lo has comprobado.
14. No afirmes que la API está publicada si no se ha verificado su funcionamiento en la nube.
15. Actualiza la documentación cuando cambie algún endpoint, campo o comportamiento.
16. No elimines datos importantes de la base de datos sin advertirme previamente.
17. No realices operaciones destructivas o despliegues que puedan afectar recursos existentes sin mi autorización.

### Comunicación

Explícame los avances como un desarrollador senior que guía a un desarrollador junior.

Cuando necesite realizar alguna acción manual:
- Indícame exactamente qué debo hacer.
- Indícame en qué terminal ejecutar los comandos.
- Proporciona los comandos completos para PowerShell.
- Explícame brevemente qué hace cada comando.
- Espera mi confirmación cuando la acción requiera intervención mía.

Si encuentras alguna decisión técnica que dependa de mis preferencias o de una configuración que no puedas verificar, consúltame antes de asumirla.

---

## 13. PRIMERA TAREA

Por ahora NO comiences a generar todo el código.

Primero realiza exclusivamente la **FASE 1: INSPECCIÓN Y ANÁLISIS**.

Lee `LAB12.md`, inspecciona `veterinaria-api` y analiza `products-api` como referencia.

Después, preséntame:

1. Diagnóstico del proyecto actual.
2. Requisitos técnicos identificados.
3. Estructura de carpetas propuesta.
4. Modelo de datos propuesto.
5. Paquetes que necesitamos instalar.
6. Plan de implementación por fases.
7. Cualquier configuración que necesites confirmar conmigo.

Espera mi aprobación para comenzar con la implementación.

**El objetivo final es entregar un laboratorio completamente funcional, probado, desplegado en la nube y documentado, cumpliendo todos los requisitos del profesor.**
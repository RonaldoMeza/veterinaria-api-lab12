# Entregables y pruebas — Laboratorio 12

## A. Tema: sistema de gestión de veterinaria

Desarrollé una Web API REST para registrar propietarios y las mascotas que atiende una veterinaria. La API permite consultar, crear, actualizar y eliminar ambos recursos. La relación es uno a muchos: un propietario puede tener varias mascotas, mientras que cada mascota tiene un único propietario mediante `IdPropietario`.

Implementé dos reglas de negocio: no se admite más de un propietario con el mismo DNI y no se puede eliminar a un propietario que todavía tenga mascotas. Ambas responden con HTTP 409 Conflict. La base de datos fue creada primero en SQL Server y el modelo se generó mediante Entity Framework Core Scaffold.

## B. Repositorio y estructura

- URL del repositorio GitHub: https://github.com/RonaldoMeza/veterinaria-api-lab12.git
- Script SQL: `database/veterinaria_db.sql`.
- Modelos Scaffold: `Models/Propietario.cs`, `Models/Mascota.cs` y `Models/VeterinariaContext.cs`.
- Controladores: `Controllers/PropietariosController.cs` y `Controllers/MascotasController.cs`.

## C. API en la nube

- URL de Swagger publicado: https://veterinaria-api-lab12.onrender.com/swagger
- Hosting de la API: Render, mediante un Web Service con Docker en el plan gratuito.
- Base de datos: Azure SQL Database, compatible con SQL Server.

La API se desplegó en Render y se conecta a Azure SQL Database mediante una variable de entorno. La cadena de conexión no se publica en GitHub.

## D. Guía de pruebas en Swagger

### Preparación

Las capturas definitivas deben realizarse sobre la API desplegada: `https://veterinaria-api-lab12.onrender.com/swagger`. Las pruebas locales sirvieron como validación previa; para la entrega se utilizará la URL de Render.

En cada caso: abrir la operación, seleccionar **Try it out**, completar los parámetros o JSON, pulsar **Execute** y capturar Request URL, código y Response body. La URL base de todas las pruebas es `https://veterinaria-api-lab12.onrender.com`.

| # | Operación y solicitud | JSON / pasos específicos | Resultado esperado y captura |
|---:|---|---|---|
| 1 | `GET /api/Propietarios` | Ejecutar sin parámetros. | 200, lista inicial de tres propietarios. `01-get-propietarios-200.png` |
| 2 | `GET /api/Propietarios/1` | Id `1`. | 200, Ana María Torres. `02-get-propietario-id-200.png` |
| 3 | `GET /api/Propietarios/9999` | Id `9999`. | 404 sin recurso. `03-get-propietario-404.png` |
| 4 | `GET /api/Propietarios/1/mascotas` | Id `1`. | 200, Max y Luna. `04-ruta-anidada-200.png` |
| 5 | `POST /api/Propietarios` | `{"idPropietario":0,"nombres":"Diego","apellidos":"Paredes Ruiz","dni":"87654321","telefono":"999111222","correo":"diego.paredes@example.com","direccion":"Av. Perú 100, Lima"}` | 201 y `Location`; guardar el `idPropietario` creado (normalmente 4). `05-post-propietario-201.png` |
| 6 | `POST /api/Propietarios` | Repetir prueba 5 con DNI `45872136`. | 409, detalle “DNI duplicado”. `06-dni-duplicado-409.png` |
| 7 | `POST /api/Propietarios` | Igual a prueba 5 pero con DNI `123`. | 400 por Data Annotations. `07-propietario-invalido-400.png` |
| 8 | `PUT /api/Propietarios/{idNuevo}` | Usar el id de prueba 5 y el mismo JSON, cambiando teléfono a `999222333`. | 204 sin cuerpo. `08-put-propietario-204.png` |
| 9 | `DELETE /api/Propietarios/1` | Ejecutar antes de borrar mascotas semilla. | 409: tiene mascotas registradas. `09-delete-propietario-con-mascotas-409.png` |
| 10 | `GET /api/Mascotas` | Sin filtros. | 200 con las cuatro mascotas y propietario. `10-get-mascotas-200.png` |
| 11 | `GET /api/Mascotas?especie=Perro&nombre=Max` | Especie `Perro`, nombre `Max`. | 200, solo Max; filtro LINQ combinado. `11-filtro-mascotas-200.png` |
| 12 | `GET /api/Mascotas/1` | Id `1`. | 200, Max con su propietario. `12-get-mascota-id-200.png` |
| 13 | `GET /api/Mascotas/9999` | Id `9999`. | 404. `13-get-mascota-404.png` |
| 14 | `POST /api/Mascotas` | `{"idMascota":0,"nombre":"Toby","especie":"Perro","raza":"Mestizo","sexo":"M","fechaNacimiento":"2023-06-15","peso":9.5,"idPropietario":idNuevo}` | 201; conservar `idMascota` devuelto. `14-post-mascota-201.png` |
| 15 | `POST /api/Mascotas` | Igual a prueba 14, con `idPropietario:9999`. | 400 y mensaje de propietario inexistente. `15-fk-inexistente-400.png` |
| 16 | `POST /api/Mascotas` | Igual a prueba 14, pero `peso:0`. | 400 por `[Range]`. `16-mascota-invalida-400.png` |
| 17 | `PUT /api/Mascotas/{idMascotaNuevo}` | Usar id de prueba 14; mismo JSON con `peso:10.2`. | 204 sin cuerpo. `17-put-mascota-204.png` |
| 18 | `PUT /api/Mascotas/{idMascotaNuevo}` | En URL usar el id creado y en JSON enviar `idMascota:999`. | 400 por identificadores distintos. `18-put-id-distinto-400.png` |
| 19 | `DELETE /api/Mascotas/{idMascotaNuevo}` | Borrar solo la mascota creada en prueba 14. | 204. `19-delete-mascota-204.png` |
| 20 | `DELETE /api/Propietarios/{idNuevo}` | Borrar el propietario creado en prueba 5, ya sin mascotas. | 204. `20-delete-propietario-204.png` |

No eliminar propietarios o mascotas semilla durante las pruebas. Las pruebas 19 y 20 limpian únicamente los registros temporales creados en 5 y 14.

## E. Observaciones

1. Utilicé Database First: elaboré el script SQL y después generé las entidades y el contexto mediante Scaffold de Entity Framework Core.
2. Apliqué Data Annotations y validaciones de negocio para que la API responda con códigos HTTP acordes al resultado de cada operación.
3. Configuré Swagger para el entorno publicado y la cadena de conexión como variable de entorno en Render, evitando guardar credenciales en el proyecto.

## F. Conclusiones

1. Aprendí a desarrollar una API REST con controladores, rutas, operaciones asíncronas y respuestas HTTP apropiadas.
2. Implementé consultas LINQ con Entity Framework Core para filtrar, consultar relaciones y administrar datos en SQL Server.
3. Publiqué la API en Render conectada a Azure SQL Database y comprobé que Swagger está disponible en la nube.

## G. Checklist final

- [x] Dos entidades y relación 1:N.
- [x] Script SQL con restricciones, índice y datos de ejemplo.
- [x] Scaffold Database First ejecutado.
- [x] Data Annotations y prevención de ciclo JSON.
- [x] CRUD, filtros, ruta anidada y reglas 409.
- [x] Swagger y CORS con origen específico.
- [x] Compilación local sin errores ni advertencias.
- [x] Pruebas GET locales verificadas.
- [ ] Ejecutar y capturar todas las pruebas de la tabla.
- [x] Repositorio publicado sin credenciales ni cadenas de conexión.
- [x] API desplegada en Render y base de datos desplegada en Azure SQL Database.
- [ ] Ejecutar las pruebas de la tabla en Render y adjuntar sus capturas.

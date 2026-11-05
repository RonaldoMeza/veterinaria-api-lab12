# Configuración de Render

## Tipo de servicio

Crear un **Web Service** desde el repositorio GitHub y seleccionar **Docker** como entorno. Render detectará el `Dockerfile` en la raíz del proyecto.

## Variables de entorno obligatorias

Configurar en Render, en **Environment**, sin crear archivos de secretos dentro del repositorio:

| Clave | Valor |
|---|---|
| `ConnectionStrings__Veterinaria` | Cadena de conexión de Azure SQL Database con usuario y contraseña de Azure SQL. |
| `Cors__AllowedOrigin` | Origen exacto del frontend autorizado. Si no habrá frontend, usar temporalmente una URL concreta propia, por ejemplo `https://tu-sitio.example`. |

Ejemplo de estructura de la cadena, sin credenciales reales:

```text
Server=tcp:sql-veterinaria-lab12.database.windows.net,1433;Initial Catalog=free-sql-db-1820708;User ID=<usuario-azure-sql>;Password=<contrasena>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

En variables de entorno de ASP.NET Core, los dos puntos de `ConnectionStrings:Veterinaria` se escriben con doble guion bajo: `ConnectionStrings__Veterinaria`.

## Desarrollo local

El archivo `.env.example` es solo una plantilla documentada y **no se carga automáticamente** por ASP.NET Core. Para usar Azure SQL localmente, configurar la misma cadena mediante User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:Veterinaria" "<cadena-de-Azure-SQL>"
```

No crear ni subir un `.env` para compilar. Render inyecta sus variables directamente al iniciar el contenedor.

## Configuración de Render

- Branch: la rama publicada que se conectará desde GitHub.
- Runtime: Docker.
- Health check path: `/swagger/index.html`.
- No definir manualmente `PORT`: Render lo inyecta y el Dockerfile lo usa.
- La URL pública será `https://<servicio>.onrender.com`; Swagger quedará en `/swagger`.

## Azure SQL

Antes del primer despliegue, agregar en el firewall lógico de Azure SQL una regla que permita los servicios de Azure o, si Render no logra conectarse, agregar temporalmente la IP de salida indicada por Render. Mantener `Encrypt=True` y `TrustServerCertificate=False`.

No subir esta cadena a GitHub, `appsettings.json`, Dockerfile ni capturas de pantalla.

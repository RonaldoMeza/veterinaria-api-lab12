# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["veterinaria-api.csproj", "./"]
RUN dotnet restore "veterinaria-api.csproj"

COPY . .
RUN dotnet publish "veterinaria-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final mínima para Render
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000

COPY --from=build /app/publish .

# Render proporciona PORT en tiempo de ejecución. El valor 10000 permite
# probar el contenedor también de forma local.
ENTRYPOINT ["sh", "-c", "dotnet veterinaria-api.dll --urls http://0.0.0.0:${PORT:-10000}"]

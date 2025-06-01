# Etapa base: imagen ligera que contiene solo el runtime ASP.NET Core 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Define el directorio de trabajo dentro del contenedor
WORKDIR /app             

# Expone el puerto 8080 para que Cloud Run u otros servicios puedan conectarse  
EXPOSE 8080               

# Etapa build: imagen con SDK completo para compilar y publicar la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establece el directorio de trabajo para la compilación
WORKDIR /src              

# Copia todo el código fuente y archivos del proyecto al contenedor
COPY . .                 


# Cambia al directorio específico del proyecto si tu solución tiene múltiples carpetas
WORKDIR /src/Apis        

# Restaura las dependencias NuGet definidas en el proyecto (descarga paquetes)
RUN dotnet restore        

# Compila y publica la app en modo Release en la carpeta /app/publish
RUN dotnet publish -c Release -o /app/publish  

# Etapa final: imagen que solo contiene el runtime y los archivos publicados
FROM base AS final

# Establece el directorio de trabajo para la app en producción
WORKDIR /app               

# Copia los archivos publicados desde la etapa build a esta imagen final
COPY --from=build /app/publish .  

# Comando para iniciar la aplicación al arrancar el contenedor
ENTRYPOINT ["dotnet", "Apis.dll"]  


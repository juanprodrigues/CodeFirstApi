# Inyección de Dependencias usando Extension Methods

## Introducción

ASP.NET Core incluye un contenedor de Inyección de Dependencias (Dependency Injection) que permite registrar servicios y repositorios para que sean creados automáticamente por el framework.

Una forma simple de registrar dependencias es directamente en `Program.cs`:

```csharp
builder.Services.AddScoped<IBeerService, BeerService>();
builder.Services.AddScoped<IBeerRepository, BeerRepository>();
```

Este enfoque funciona, pero a medida que la aplicación crece, `Program.cs` puede convertirse en un archivo difícil de mantener.

Para solucionar esto se pueden utilizar **Extension Methods** para encapsular la configuración de dependencias.

---

# Objetivo

Separar la configuración de Inyección de Dependencias del archivo principal de la aplicación.

La idea es pasar de:

```
Program.cs
    |
    ├── Services
    ├── Repositories
    ├── Database
    ├── Authentication
    └── External Services
```

a:

```
Program.cs

    |
    |
AddApplicationServices()
```

---

# Estructura del proyecto

Ejemplo:

```
CodeFirstApi

├── Controllers
│
├── Services
│   ├── Interfaces
│   ├── BeerService.cs
│   └── BrandService.cs
│
├── Repositories
│   ├── Interfaces
│   ├── BeerRepository.cs
│   └── BrandRepository.cs
│
├── Extensions
│   └── DependencyInjectionExtensions.cs
│
└── Program.cs
```

---

# Crear Extension Method

Crear el archivo:

```
Extensions/DependencyInjectionExtensions.cs
```

Código:

```csharp
using CodeFirstApi.Services;
using CodeFirstApi.Services.Interfaces;
using CodeFirstApi.Repositories;
using CodeFirstApi.Repositories.Interfaces;

namespace CodeFirstApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {

        services.AddScoped<IBeerService, BeerService>();

        services.AddScoped<IBeerRepository, BeerRepository>();

        services.AddScoped<IBrandService, BrandService>();

        services.AddScoped<IBrandRepository, BrandRepository>();


        return services;
    }
}
```

---

# Uso en Program.cs

Antes:

```csharp
builder.Services.AddScoped<IBeerService, BeerService>();
builder.Services.AddScoped<IBeerRepository, BeerRepository>();
```

Después:

```csharp
builder.Services.AddApplicationServices();
```

El archivo principal queda más limpio.

---

# Flujo de resolución

Cuando ASP.NET recibe una petición:

```
Controller

    |
    |
    solicita

IBeerService

    |
    |
    resuelve

BeerService

    |
    |
    solicita

IBeerRepository

    |
    |
    resuelve

BeerRepository

    |
    |
    usa

Database
```

---

# Ventajas

## Organización

Toda la configuración de dependencias está en un único lugar.

---

## Escalabilidad

Cuando agregamos una nueva dependencia:

Antes:

```csharp
Program.cs

AddScoped<IProductService, ProductService>();
AddScoped<IOrderService, OrderService>();
AddScoped<IEmailService, EmailService>();
```

Después:

```csharp
DependencyInjectionExtensions.cs
```

---

## Separación de responsabilidades

`Program.cs` queda encargado de iniciar la aplicación.

La configuración interna queda encapsulada.

---

# Cuándo usarlo

Recomendado para:

- APIs pequeñas y medianas.
- Proyectos CRUD.
- Aplicaciones donde no se necesita separar proyectos.

Es una solución simple antes de migrar a una arquitectura más grande.

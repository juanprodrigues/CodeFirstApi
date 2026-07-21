# Inyección de Dependencias mediante Assembly Scanning con Scrutor

## Introducción

ASP.NET Core incluye un contenedor de Inyección de Dependencias (Dependency Injection) que permite registrar servicios, repositorios y otras dependencias para que sean resueltas automáticamente en tiempo de ejecución.

Una forma tradicional de registrar dependencias es hacerlo directamente en `Program.cs`:

```csharp
builder.Services.AddScoped<IBeerService, BeerService>();
builder.Services.AddScoped<IBeerRepository, BeerRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
```

Este enfoque funciona correctamente, pero en aplicaciones grandes puede generar un archivo `Program.cs` difícil de mantener.

Para solucionar esto se puede utilizar **Assembly Scanning**, que permite buscar automáticamente clases dentro del ensamblado y registrarlas según ciertas convenciones.

Para esto utilizaremos la librería **Scrutor**.

---

# Instalación

Instalar el paquete NuGet:

```bash
dotnet add package Scrutor
```

---

# Estructura del proyecto

Ejemplo de organización:

```
CodeFirstApi
│
├── Controllers
│
├── Services
│   ├── Interfaces
│   │   ├── IBeerService.cs
│   │   └── IBrandService.cs
│   │
│   ├── BeerService.cs
│   └── BrandService.cs
│
├── Repositories
│   ├── Interfaces
│   │   ├── IBeerRepository.cs
│   │   └── IBrandRepository.cs
│   │
│   ├── BeerRepository.cs
│   └── BrandRepository.cs
│
└── Program.cs
```

---

# Interfaces

Las interfaces representan el contrato que debe cumplir una implementación.

Ejemplo:

```csharp
public interface IBeerService
{
    Task<IEnumerable<Beer>> GetAllAsync();
}
```

La implementación:

```csharp
public class BeerService : IBeerService
{
    public Task<IEnumerable<Beer>> GetAllAsync()
    {
        // lógica del servicio
    }
}
```

La relación es:

```
BeerService
     |
     implementa
     |
IBeerService
```

---

# Configuración mediante Assembly Scanning

En lugar de registrar cada clase manualmente:

```csharp
builder.Services.AddScoped<IBeerService, BeerService>();
```

utilizamos Scrutor:

```csharp
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()

    .AddClasses(classes => classes
        .Where(type =>
            type.Name.EndsWith("Service") ||
            type.Name.EndsWith("Repository")))

    .AsImplementedInterfaces()

    .WithScopedLifetime()
);
```

---

# ¿Qué hace cada parte?

## FromAssemblyOf<Program>()

Indica en qué ensamblado buscar las clases.

```csharp
.FromAssemblyOf<Program>()
```

Busca dentro del proyecto actual clases que puedan registrarse.

---

## AddClasses()

Indica que queremos buscar clases concretas.

```csharp
.AddClasses()
```

Por defecto busca clases públicas instanciables.

---

## Where()

Permite aplicar filtros.

En este caso buscamos solamente:

- Clases que terminan en `Service`
- Clases que terminan en `Repository`

```csharp
.Where(type =>
    type.Name.EndsWith("Service") ||
    type.Name.EndsWith("Repository"))
```

Ejemplos encontrados:

```
BeerService
BrandService
BeerRepository
BrandRepository
```

No registrará:

```
BeerHelper
EmailSender
Logger
```

---

## AsImplementedInterfaces()

Relaciona automáticamente la clase con las interfaces que implementa.

Ejemplo:

Encuentra:

```csharp
public class BeerService : IBeerService
```

y genera internamente:

```csharp
services.AddScoped<IBeerService, BeerService>();
```

Lo mismo para:

```csharp
public class BeerRepository : IBeerRepository
```

Genera:

```csharp
services.AddScoped<IBeerRepository, BeerRepository>();
```

---

## WithScopedLifetime()

Define el ciclo de vida de la dependencia.

```csharp
.WithScopedLifetime()
```

Equivale a:

```csharp
builder.Services.AddScoped<IBeerService, BeerService>();
```

---

# Ciclo de resolución de dependencias

Cuando un controlador solicita un servicio:

```csharp
public class BeerController : ControllerBase
{
    private readonly IBeerService _service;

    public BeerController(IBeerService service)
    {
        _service = service;
    }
}
```

ASP.NET Core realiza:

```
BeerController
        |
        solicita
        |
IBeerService
        |
        resuelve
        |
BeerService
        |
        solicita
        |
IBeerRepository
        |
        resuelve
        |
BeerRepository
        |
        usa
        |
BarContext
```

El controlador nunca conoce la implementación concreta.

---

# Ventajas

## Menor acoplamiento

El código depende de interfaces:

```csharp
IBeerService
IBeerRepository
```

y no de implementaciones:

```csharp
BeerService
BeerRepository
```

---

## Facilidad para cambiar implementaciones

Ejemplo:

Antes:

```
IBeerRepository
        |
        |
BeerRepository
        |
        |
Entity Framework
```

Después:

```
IBeerRepository
        |
        |
BeerDapperRepository
        |
        |
Dapper
```

Solo cambia el registro de dependencias.

La capa de negocio no se modifica.

---

## Facilita pruebas unitarias

Podemos reemplazar la implementación real:

```csharp
public class FakeBeerRepository : IBeerRepository
{
}
```

sin utilizar una base de datos real.

---

# Separar la configuración del Program.cs

En proyectos grandes se recomienda crear una extensión.

Estructura:

```
Extensions
└── DependencyInjectionExtensions.cs
```

Contenido:

```csharp
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<Program>()
            .AddClasses(classes => classes
                .Where(type =>
                    type.Name.EndsWith("Service") ||
                    type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        return services;
    }
}
```

---

Luego en `Program.cs`:

```csharp
builder.Services.AddApplicationServices();
```

El archivo principal queda limpio.

---

# Cuándo utilizar Assembly Scanning

Es recomendable cuando:

- La aplicación tiene muchos servicios.
- Existen muchos repositorios.
- Se trabaja con arquitectura por capas.
- Se quiere reducir configuración repetitiva.

Para proyectos pequeños, el registro explícito puede ser más claro:

```csharp
AddScoped<IBeerService, BeerService>();
```

Para proyectos grandes, Assembly Scanning ayuda a mantener la configuración organizada.

---

# Conclusión

Assembly Scanning permite que ASP.NET Core registre automáticamente implementaciones siguiendo convenciones.

La aplicación queda desacoplada porque:

- Los controladores dependen de interfaces.
- Los servicios dependen de interfaces.
- Los repositorios encapsulan el acceso a datos.
- La implementación concreta puede cambiar sin modificar la lógica de negocio.

Esta práctica aplica los principios SOLID, especialmente:

- Dependency Inversion Principle.
- Single Responsibility Principle.
- Open/Closed Principle.

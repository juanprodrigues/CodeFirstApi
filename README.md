# Inyección de Dependencias con Separación por Capas

## Introducción

En aplicaciones empresariales es común separar la solución en diferentes capas.

Cada capa tiene una responsabilidad específica y registra sus propias dependencias.

Una arquitectura típica:

```
API

 |
 |
Application

 |
 |
Infrastructure

 |
 |
Database
```

---

# Objetivo

Evitar que un único proyecto sea responsable de todo.

Cada módulo conoce únicamente las dependencias que necesita.

---

# Estructura de solución

Ejemplo:

```
Solution

├── CodeFirstApi.API
│
│   └── Program.cs
│
├── CodeFirstApi.Application
│
│   ├── Interfaces
│   ├── Services
│   └── DependencyInjection.cs
│
├── CodeFirstApi.Infrastructure
│
│   ├── Repositories
│   ├── Database
│   └── DependencyInjection.cs
│
└── CodeFirstApi.Domain
    |
    └── Entities
```

---

# Responsabilidad de cada capa

## Domain

Contiene las entidades principales.

Ejemplo:

```csharp
public class Beer
{
    public int Id { get; set; }

    public string Name { get; set; }
}
```

No conoce servicios ni base de datos.

---

# Application

Contiene la lógica de negocio.

Ejemplo:

```
IBeerService

BeerService
```

Código:

```csharp
public class BeerService : IBeerService
{
    private readonly IBeerRepository _repository;


    public BeerService(
        IBeerRepository repository)
    {
        _repository = repository;
    }
}
```

Application depende de abstracciones.

---

# Infrastructure

Contiene detalles técnicos.

Ejemplo:

```
BeerRepository

Entity Framework

SQL Server
```

Implementación:

```csharp
public class BeerRepository : IBeerRepository
{
    private readonly BarContext _context;


    public BeerRepository(
        BarContext context)
    {
        _context = context;
    }
}
```

---

# Registro de dependencias por capa

## Application

Archivo:

```
CodeFirstApi.Application/DependencyInjection.cs
```

Código:

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {

        services.AddScoped<IBeerService, BeerService>();

        return services;
    }
}
```

---

## Infrastructure

Archivo:

```
CodeFirstApi.Infrastructure/DependencyInjection.cs
```

Código:

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {

        services.AddScoped<IBeerRepository, BeerRepository>();

        services.AddDbContext<BarContext>();

        return services;
    }
}
```

---

# Composición en la API

El proyecto API solamente combina las capas:

```csharp
builder.Services
    .AddApplication()
    .AddInfrastructure();
```

---

# Flujo de dependencias

```
Controller

    |
    |
Application

    |
    |
Interface

    |
    |
Infrastructure

    |
    |
Database
```

---

# Regla de dependencias

Las dependencias deben ir hacia el centro:

Correcto:

```
API
 |
Application
 |
Domain
```

Incorrecto:

```
Domain
 |
Infrastructure
```

Las entidades del dominio no deben conocer detalles técnicos.

---

# Ventajas

## Mantenimiento

Cada capa tiene una responsabilidad clara.

---

## Testabilidad

Los servicios pueden probarse usando mocks:

```
BeerService

    |
    |
FakeBeerRepository
```

sin una base de datos real.

---

## Cambio de tecnología

Se puede cambiar:

```
Entity Framework

        ↓

Dapper

        ↓

API externa
```

sin modificar la lógica de negocio.

---

# Cuándo utilizarlo

Recomendado para:

- Sistemas empresariales.
- Aplicaciones grandes.
- Equipos con varios desarrolladores.
- Proyectos que tendrán crecimiento a largo plazo.

Ejemplo:

```
ERP
Sistema bancario
E-commerce
Microservicios
```

---

# Diferencia con un proyecto simple

Proyecto pequeño:

```
API
 |
Services
 |
Repositories
 |
Database
```

Proyecto empresarial:

```
API

Application

Infrastructure

Domain
```

La separación por capas agrega más estructura para controlar la complejidad.

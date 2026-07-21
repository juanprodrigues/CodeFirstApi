# Beer API - ASP.NET Core Web API

## Descripción

Beer API es una aplicación backend desarrollada con ASP.NET Core Web API siguiendo buenas prácticas de arquitectura, separación de responsabilidades e inyección de dependencias.

El proyecto implementa un CRUD para la gestión de cervezas y marcas utilizando Entity Framework Core como ORM y una arquitectura basada en capas.

El objetivo principal es mantener un código desacoplado, escalable y preparado para futuros cambios de infraestructura.

---

# Tecnologías utilizadas

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Dependency Injection
- Repository Pattern
- Service Layer Pattern
- Swagger/OpenAPI

---

# Arquitectura del proyecto

La solución está organizada siguiendo una separación por responsabilidades:

```
BeerApi

├── Controllers
│   ├── Controllers│
├── Services
│   ├── Interfaces
│   ├── Services
├── Repositories
│   ├── Interfaces
│   ├── Repositories
|── Program.cs
|
└── DB
    └── Entities
```

---

# Capas del sistema

## API

Responsable de recibir las peticiones HTTP y devolver respuestas.

Contiene:

- Controllers
- Configuración inicial

Ejemplo:

```
BeerController
```

No contiene lógica de negocio.

---

## Service

Contiene la lógica de negocio de la aplicación.

Responsabilidades:

- Validaciones.
- Reglas de negocio.
- Coordinación entre componentes.

Ejemplo:

```
IBeerService

BeerService
```

---

## Repository

Contiene detalles técnicos de implementación.

Responsabilidades:

- Acceso a datos.
- Entity Framework Core.
- Repositorios.
- Configuración de base de datos.

Ejemplo:

```
IBeerRepository

BeerRepository
```

---

## DB

Contiene las entidades principales del sistema.

Ejemplo:

```csharp
public class Beer
{
    public int BeerID { get; set; }

    public string Name { get; set; }

    public int BrandID { get; set; }
}
```

No depende de ninguna capa externa.

---

# Modelo de datos

Actualmente el sistema maneja:

## Brand

Representa una marca de cerveza.

Propiedades:

```
BrandID
Name
```

Relación:

```
Brand
 |
 |
1:N
 |
 |
Beer
```

---

## Beer

Representa una cerveza.

Propiedades:

```
BeerID
Name
BrandID
```

Cada cerveza pertenece a una marca.

---

# Inyección de Dependencias

ASP.NET Core utiliza un contenedor integrado de Dependency Injection.

Las dependencias se registran utilizando Extension Methods.

Ejemplo:

```csharp
builder.Services.AddScoped<IBeerService, BeerService>();

builder.Services.AddScoped<IBeerRepository, BeerRepository>();

builder.Services.AddScoped<IBrandService, BrandService>();

builder.Services.AddScoped<IBrandRepository, BrandRepository>();
```

---


# Flujo de una petición

Ejemplo:

GET:

```
GET /api/beer
```

Flujo interno:

```
HTTP Request

      |

BeerController

      |

IBeerService

      |

BeerService

      |

IBeerRepository

      |

BeerRepository

      |

Entity Framework

      |

SQL Server
```

---

# Repository Pattern

Los repositorios encapsulan el acceso a datos.

Ejemplo:

```csharp
public interface IBeerRepository
{
    Task<IEnumerable<Beer>> GetAllAsync();
}
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

# Service Layer

Los servicios contienen la lógica de negocio.

Ejemplo:

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

Ventajas:

- Evita lógica en Controllers.
- Facilita pruebas.
- Mantiene responsabilidades separadas.

---

# Entity Framework Core

El acceso a datos se realiza mediante Entity Framework Core.

Ejemplo:

```csharp
public class BarContext : DbContext
{
    public DbSet<Beer> Beers { get; set; }

    public DbSet<Brand> Brands { get; set; }
}
```

---

# Relaciones entre entidades

La relación Beer - Brand es:

```
Brand

1

|

N

Beer
```

Configurada mediante:

```csharp
[ForeignKey("BrandID")]
public virtual Brand Brand { get; set; }
```

---

# Endpoints disponibles

## Beer

### Obtener todas las cervezas

```
GET /api/beer
```

Respuesta:

```json
[
  {
    "beerID": 1,
    "name": "IPA",
    "brandID": 2
  }
]
```

---

### Obtener una cerveza por ID

```
GET /api/beer/{id}
```

---

### Crear cerveza

```
POST /api/beer
```

Body:

```json
{
  "name": "IPA",
  "brandID": 2
}
```

---

### Actualizar cerveza

```
PUT /api/beer/{id}
```

---

### Eliminar cerveza

```
DELETE /api/beer/{id}
```

---

# Configuración de base de datos

Connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
    "Server=localhost;Database=BeerDb;Trusted_Connection=True;"
  }
}
```

---

# Migraciones Entity Framework

Crear migración:

```bash
dotnet ef migrations add InitialCreate
```

Aplicar migración:

```bash
dotnet ef database update
```

---

# Swagger

La API incluye documentación automática mediante Swagger.

Disponible normalmente en:

```
/swagger
```

Permite:

- Ver endpoints.
- Probar operaciones CRUD.
- Revisar modelos.

---

# Principios SOLID aplicados

## Single Responsibility Principle

Cada clase tiene una responsabilidad:

```
Controller
HTTP

Service
Negocio

Repository
Datos
```

---

## Dependency Inversion Principle

Las clases dependen de interfaces:

Correcto:

```
BeerService

   |

IBeerRepository
```

No:

```
BeerService

   |

BeerRepository
```

---

## Open Closed Principle

Permite agregar nuevas implementaciones sin modificar código existente.

Ejemplo:

```
IBeerRepository

        |

---------------------

EF Repository

Dapper Repository

API Repository
```

---

# Próximas mejoras posibles

- DTOs para separar modelos internos y contratos HTTP.
- AutoMapper.
- Validaciones con FluentValidation.
- Autenticación JWT.
- Tests unitarios.
- Logging estructurado.
- Dockerización.
- Clean Architecture completa.

---

# Ejecución del proyecto

Restaurar dependencias:

```bash
dotnet restore
```

Ejecutar:

```bash
dotnet run
```

La API quedará disponible mediante la URL configurada en `launchSettings.json`.

---

# Conclusión

Este proyecto implementa una arquitectura preparada para crecer utilizando:

- Separación de responsabilidades.
- Inyección de dependencias.
- Interfaces.
- Servicios.
- Repositorios.
- Entity Framework Core.

El objetivo es mantener un código mantenible, testeable y desacoplado de detalles de infraestructura.

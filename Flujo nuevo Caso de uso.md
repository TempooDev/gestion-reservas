### Flujo para Añadir un Nuevo Caso de Uso

1. **Definir el Caso de Uso**:
   - Identifica el requerimiento o la funcionalidad que se debe implementar.
   - Define claramente qué debe hacer el caso de uso (por ejemplo, "Crear un nuevo usuario" o "Actualizar el saldo de una cuenta").

2. **Crear el Comando o Consulta (CQRS)**:
   - Si el caso de uso implica una acción que modifica el estado del sistema, crea un **Comando** en la capa de aplicación (`Application/Commands`).
   - Si el caso de uso es una consulta que no modifica el estado, crea una **Consulta** en la capa de aplicación (`Application/Queries`).
   - Ejemplo:
     ```
     Application/
     ├── Commands/
     │   └── CreateUserCommand.cs
     ├── Queries/
     │   └── GetUserByIdQuery.cs
     ```

3. **Definir el DTO (Data Transfer Object)**:
   - Crea un DTO en la capa de aplicación (`Application/DTOs`) para representar los datos de entrada y salida del caso de uso.
   - Ejemplo:
     ```
     Application/
     ├── DTOs/
     │   └── UserDTO.cs
     ```

4. **Implementar el Handler**:
   - En la capa de aplicación, implementa un **Handler** para manejar el comando o consulta.
   - El Handler es responsable de orquestar la lógica entre la capa de aplicación y la capa de dominio.
   - Ejemplo:
     ```
     Application/
     ├── Commands/
     │   └── CreateUserCommandHandler.cs
     ├── Queries/
     │   └── GetUserByIdQueryHandler.cs
     ```

5. **Definir la Lógica de Dominio**:
   - Si el caso de uso requiere lógica de negocio, implementa esta lógica en la capa de dominio (`Domain/Models`, `Domain/Services`, etc.).
   - Asegúrate de que las entidades, agregados y servicios de dominio estén correctamente definidos.
   - Ejemplo:
     ```
     Domain/
     ├── Models/
     │   └── User.cs
     ├── Services/
     │   └── UserService.cs
     ```

6. **Implementar el Repositorio**:
   - Si el caso de uso requiere acceso a datos, implementa el repositorio en la capa de infraestructura (`Infrastructure/Persistence`).
   - Asegúrate de que el repositorio implemente la interfaz definida en la capa de dominio.
   - Ejemplo:
     ```
     Infrastructure/
     ├── Persistence/
     │   └── UserRepository.cs
     ```

7. **Exponer el Caso de Uso en la API**:
   - En la capa de presentación (`Presentation/Controllers`), crea un controlador o endpoint para exponer el caso de uso.
   - El controlador debe llamar al Handler correspondiente en la capa de aplicación.
   - Ejemplo:
     ```
     Presentation/
     ├── Controllers/
     │   └── UserController.cs
     ```

8. **Agregar Pruebas**:
   - Escribe pruebas unitarias para el Handler, la lógica de dominio y el repositorio.
   - Escribe pruebas de integración para el controlador y la interacción entre capas.
   - Ejemplo:
     ```
     tests/
     ├── Unit/
     │   └── Application/Commands/CreateUserCommandHandlerTests.cs
     ├── Integration/
     │   └── Presentation/Controllers/UserControllerTests.cs
     ```

9. **Configurar Dependencias**:
   - Registra las dependencias del nuevo caso de uso en el contenedor de inversión de control (IoC) del proyecto.
   - Ejemplo (en un archivo de configuración de dependencias):
     ```csharp
     services.AddScoped<IUserRepository, UserRepository>();
     services.AddScoped<IUserService, UserService>();
     services.AddScoped<CreateUserCommandHandler>();
     ```

10. **Documentar el Caso de Uso**:
    - Actualiza la documentación del proyecto (por ejemplo, en el `README.md`) para incluir el nuevo caso de uso.
    - Si es necesario, documenta los endpoints de la API (por ejemplo, usando Swagger).

---

### Ejemplo Práctico: Crear un Nuevo Usuario

1. **Comando**:
   ```csharp
   public class CreateUserCommand : IRequest<UserDTO>
   {
       public string Name { get; set; }
       public string Email { get; set; }
   }
   ````

2. **Handler:**

```csharp
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.Name, request.Email);
        await _userRepository.AddAsync(user);
        return new UserDTO { Id = user.Id, Name = user.Name, Email = user.Email };
    }
}
```
3. **Controladoe:**
```csharp
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var userDTO = await _mediator.Send(command);
        return Ok(userDTO);
    }
}
```
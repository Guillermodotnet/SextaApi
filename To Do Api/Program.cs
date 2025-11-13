// En esta línea importo el paquete de Entity Framework Core, 
// ya que lo necesito para configurar mi contexto de base de datos (DbContext) y el proveedor SQL Server.
using Microsoft.EntityFrameworkCore;

// Aquí importo el espacio de nombres donde tengo definido mi contexto de datos (AppDbContext).
using To_Do_Api.Data;

// También importo mi clase de mapeo (MappingProfile) para registrar las reglas de AutoMapper.
using To_Do_Api.Mapper;

// Importo la implementación del servicio de ToDoList, que es la clase concreta que ejecuta la lógica de negocio.
using To_Do_Api.Services.ImplementationServices;

// Finalmente, importo la interfaz del servicio para poder registrarla en la inyección de dependencias.
using To_Do_Api.Services.InterfacesService;

// -----------------------------------------------------------------------------
// En esta parte creo el constructor principal de la aplicación web de ASP.NET Core.
// Uso el método CreateBuilder y paso los argumentos de línea de comandos (args).
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// Aquí empiezo a registrar y configurar los servicios que mi aplicación va a necesitar.
// Todo esto se hace dentro del contenedor de Inyección de Dependencias (DI).
// -----------------------------------------------------------------------------

// --- CONFIGURACIÓN DE BASE DE DATOS (Entity Framework Core) ---

// Primero obtengo la cadena de conexión desde el archivo de configuración (appsettings.json).
// Guardo la cadena en una variable llamada connectionString.
string? connectionString = builder.Configuration.GetConnectionString("Connection");

// Luego registro mi AppDbContext en el contenedor de servicios.
// Esto le dice a ASP.NET Core cómo crear instancias del contexto cuando sean necesarias.
builder.Services.AddDbContext<AppDbContext>(options =>
    // Aquí especifico que voy a usar SQL Server como proveedor de base de datos
    // y le paso la cadena de conexión que acabo de obtener.
    options.UseSqlServer(connectionString)
);

// --- SERVICIOS ESTÁNDAR DE ASP.NET CORE ---

// En esta línea habilito el soporte para controladores API.
// Esto permite que mi aplicación reconozca clases anotadas con [ApiController] y [Route].
builder.Services.AddControllers();

// --- INYECCIÓN DE DEPENDENCIAS DE MI SERVICIO PERSONALIZADO ---

// Registro mi servicio de ToDoList en el contenedor de dependencias.
// Le indico que cada vez que se solicite un IToDoListService, se cree una instancia de ToDoListService.
// Uso AddScoped porque quiero que se cree una nueva instancia por cada petición HTTP.
builder.Services.AddScoped<IToDoListService, ToDoListService>();

// --- CONFIGURACIÓN DE SWAGGER / OPENAPI ---

// Aquí habilito la generación de metadatos de los endpoints de mi API.
// Esto es necesario para que Swagger pueda generar la documentación automáticamente.
builder.Services.AddEndpointsApiExplorer();

// En esta línea activo SwaggerGen, que genera el archivo JSON con la especificación OpenAPI
// y también la interfaz visual de Swagger UI.
builder.Services.AddSwaggerGen();

// --- CONFIGURACIÓN DE AUTOMAPPER ---

// Aquí registro y configuro AutoMapper dentro de los servicios.
builder.Services.AddAutoMapper(cfg =>
{
    // Agrego manualmente mi perfil de mapeo (MappingProfile) que define las reglas de conversión entre DTOs y entidades.
    cfg.AddProfile<MappingProfile>();
}, typeof(Program)); // Especifico el tipo base para que AutoMapper busque en este ensamblado.

// -----------------------------------------------------------------------------
// Una vez que terminé de registrar todos los servicios, construyo la aplicación.
// Esto devuelve un objeto WebApplication que puedo configurar y ejecutar.
WebApplication app = builder.Build();

// -----------------------------------------------------------------------------
// A partir de aquí configuro el PIPELINE de peticiones HTTP, o sea, el flujo de ejecución de la app.
// -----------------------------------------------------------------------------

// Verifico si la aplicación se está ejecutando en modo Desarrollo.
if (app.Environment.IsDevelopment())
{
    // Si estoy en desarrollo, habilito Swagger para poder ver y probar mis endpoints.
    app.UseSwagger();

    // También activo SwaggerUI, que es la interfaz visual en el navegador donde puedo hacer pruebas HTTP fácilmente.
    app.UseSwaggerUI();
}

// Agrego el middleware que fuerza a que todas las peticiones sean redirigidas a HTTPS.
// Esto mejora la seguridad de la aplicación.
app.UseHttpsRedirection();

// Habilito el middleware de autorización.
// Aunque no tenga controladores con autenticación todavía, es buena práctica dejarlo configurado.
app.UseAuthorization();

// Aquí indico que la aplicación debe mapear los controladores.
// Es decir, ASP.NET Core buscará las clases con [ApiController] y definirá las rutas automáticamente.
app.MapControllers();

// Finalmente, ejecuto la aplicación.
// En este punto la API empieza a escuchar peticiones en el puerto configurado.
app.Run();



// Aquí importo el espacio de nombres de Entity Framework Core porque lo necesito para trabajar con bases de datos usando EF.
using Microsoft.EntityFrameworkCore;

// En esta línea importo el espacio de nombres donde tengo definida mi entidad ToDoList.
using To_Do_Api.Models.Entities;

namespace To_Do_Api.Data
{
    // Defino mi clase AppDbContext, que hereda de DbContext. 
    // Esta clase representa la conexión y sesión con la base de datos.
    // A través de ella puedo consultar, insertar, actualizar o eliminar entidades.
    public class AppDbContext : DbContext
    {
        // En este constructor recibo las opciones de configuración del contexto, 
        // por ejemplo la cadena de conexión o el proveedor de base de datos (SQL Server, SQLite, etc.).
        // Luego paso esas opciones al constructor de la clase base DbContext para que las utilice internamente.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Aquí declaro una propiedad de tipo DbSet<ToDoList>, 
        // que representa la tabla ToDoLists dentro de mi base de datos.
        // EF Core usa esta propiedad para hacer consultas y operaciones CRUD sobre esa tabla.
        public DbSet<ToDoList> ToDoLists { get; set; }
    }
}





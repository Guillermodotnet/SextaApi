// Aquí importo el espacio de nombres de Entity Framework Core porque lo necesito para usar
// métodos de extensión como ToListAsync, FindAsync y SaveChangesAsync.
using Microsoft.EntityFrameworkCore;

// En esta línea importo el espacio de nombres donde tengo definido mi contexto de base de datos (AppDbContext).
using To_Do_Api.Data;

// También importo el espacio de nombres donde está mi entidad ToDoList,
// ya que este servicio trabaja directamente con ese modelo.
using To_Do_Api.Models.Entities;

// Finalmente, importo la interfaz del servicio para poder implementarla en esta clase.
using To_Do_Api.Services.InterfacesService;

namespace To_Do_Api.Services.ImplementationServices
{
    // Aquí defino la clase ToDoListService, que implementa el contrato IToDoListService.
    // Esta clase contiene toda la lógica que se encarga de interactuar con la base de datos.
    public class ToDoListService : IToDoListService
    {
        // Declaro un campo privado de solo lectura donde voy a guardar la instancia del contexto de base de datos.
        // Este contexto me permite acceder a las tablas y realizar operaciones CRUD.
        private readonly AppDbContext _context;

        // En el constructor recibo el contexto mediante inyección de dependencias.
        // Así puedo trabajar con la base de datos sin tener que crear el contexto manualmente.
        public ToDoListService(AppDbContext context)
        {
            // Asigno el contexto que recibo al campo privado para poder usarlo en los métodos del servicio.
            _context = context;
        }

        // ------------------------------------------------------------------
        // Método: Obtener todas las tareas
        // ------------------------------------------------------------------
        public async Task<List<ToDoList>> GetAllAsync()
        {
            // Aquí accedo al DbSet "ToDoLists" y uso ToListAsync para convertir todos los registros en una lista.
            // Como el método es asíncrono, no bloqueo el hilo principal mientras consulto la base de datos.
            return await _context.ToDoLists.ToListAsync();
        }

        // ------------------------------------------------------------------
        // Método: Obtener una tarea por su ID
        // ------------------------------------------------------------------
        public async Task<ToDoList?> GetByIdAsync(int id)
        {
            // Uso FindAsync para buscar una tarea por su clave primaria (ID).
            // FindAsync es eficiente porque primero busca en el caché del contexto y, si no la encuentra, consulta la base de datos.
            return await _context.ToDoLists.FindAsync(id);
        }

        // ------------------------------------------------------------------
        // Método: Agregar una nueva tarea
        // ------------------------------------------------------------------
        public async Task<ToDoList> AddAsync(ToDoList toDoList)
        {
            // Agrego la nueva entidad al DbSet, marcándola como "para insertar".
            await _context.ToDoLists.AddAsync(toDoList);

            // Luego guardo los cambios en la base de datos.
            // EF Core genera automáticamente el INSERT y asigna el nuevo ID a la entidad.
            await _context.SaveChangesAsync();

            // Devuelvo la entidad recién creada (ya con su ID generado).
            return toDoList;
        }

        // ------------------------------------------------------------------
        // Método: Actualizar una tarea existente
        // ------------------------------------------------------------------
        public async Task<ToDoList?> UpdateAsync(int id, ToDoList toDoList)
        {
            // Primero busco en la base de datos la tarea que quiero actualizar usando su ID.
            var existing = await _context.ToDoLists.FindAsync(id);

            // Si no encuentro ninguna tarea con ese ID, devuelvo null para indicar que no existe.
            if (existing == null)
                return null;

            // Si sí la encuentro, actualizo sus propiedades con los nuevos valores.
            // Hago esto manualmente para asegurarme de no sobrescribir campos importantes como Id o CreatedAt.
            existing.Title = toDoList.Title;
            existing.FullName = toDoList.FullName;
            existing.Description = toDoList.Description;
            existing.CompletedAt = toDoList.CompletedAt;
            existing.InternalNotes = toDoList.InternalNotes;

            // No modifico el Id ni la fecha de creación porque deben mantenerse iguales.

            // Guardo los cambios en la base de datos.
            // EF Core detecta automáticamente qué propiedades fueron modificadas y genera el UPDATE correspondiente.
            await _context.SaveChangesAsync();

            // Finalmente, devuelvo la entidad actualizada.
            return existing;
        }

        // ------------------------------------------------------------------
        // Método: Eliminar una tarea
        // ------------------------------------------------------------------
        public async Task<bool> DeleteAsync(int id)
        {
            // Primero busco la tarea que quiero eliminar por su ID.
            var toDelete = await _context.ToDoLists.FindAsync(id);

            // Si no encuentro ninguna tarea con ese ID, devuelvo false porque no hay nada que eliminar.
            if (toDelete == null)
                return false;

            // Si la encuentro, la marco para eliminación en el contexto.
            _context.ToDoLists.Remove(toDelete);

            // Luego guardo los cambios para aplicar la eliminación en la base de datos.
            await _context.SaveChangesAsync();

            // Devuelvo true para indicar que la tarea fue eliminada correctamente.
            return true;
        }
    }
}



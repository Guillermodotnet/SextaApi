// Aquí importo el espacio de nombres donde tengo definida mi entidad ToDoList,
// ya que todos los métodos de esta interfaz trabajarán directamente con ese tipo de dato.
using To_Do_Api.Models.Entities;

namespace To_Do_Api.Services.InterfacesService
{
    // En esta parte defino la interfaz IToDoListService.
    // Una interfaz es básicamente un contrato que especifica qué métodos debe implementar cualquier clase 
    // que actúe como servicio de lista de tareas en mi aplicación.
    public interface IToDoListService
    {
        // Declaro un método para obtener todas las tareas de manera asíncrona.
        // Uso Task<List<ToDoList>> porque quiero que el método se ejecute sin bloquear el hilo principal
        // y eventualmente me devuelva una lista de entidades ToDoList.
        Task<List<ToDoList>> GetAllAsync();

        // Declaro un método para obtener una sola tarea por su ID.
        // También lo hago asíncrono para aprovechar el rendimiento y la eficiencia de EF Core.
        // Devuelve una entidad ToDoList si la encuentra, o null si no existe (por eso uso el signo de interrogación '?').
        Task<ToDoList?> GetByIdAsync(int id);

        // Aquí defino el método para agregar una nueva tarea.
        // Recibo un objeto ToDoList con los datos de la nueva tarea,
        // y devuelvo la entidad guardada (normalmente ya con su ID asignado por la base de datos).
        Task<ToDoList> AddAsync(ToDoList toDoList);

        // Este método lo utilizo para actualizar una tarea existente.
        // Recibo el ID de la tarea que quiero actualizar y el nuevo objeto con los cambios.
        // Devuelvo la entidad actualizada, o null si no existe una tarea con ese ID.
        Task<ToDoList?> UpdateAsync(int id, ToDoList toDoList);

        // Finalmente, declaro el método para eliminar una tarea por su ID.
        // Devuelve true si la eliminación fue exitosa y false si no se encontró la tarea.
        Task<bool> DeleteAsync(int id);
    }
}


//Método            Argumentos de Entrada (En los Paréntesis Redondos)	        Tipo de Valor Devuelto (Dentro de < > del Task)
//GetAllAsync()                 Ninguno                                                List<ToDoList>
//GetByIdAsync()                int id                                        ToDoList? (Puede ser ToDoList o null)
//AddAsync()                    ToDoList toDoList	                                  ToDoList
//UpdateAsync()	              int id, ToDoList toDoList	                         ToDoList? (Puede ser ToDoList o null)
//DeleteAsync()	                     int id	                                          bool
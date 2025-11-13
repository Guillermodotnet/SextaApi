// Aquí importo el espacio de nombres que me permite usar los atributos de validación y metadatos,
// como [Required], [Key], [MaxLength], etc., los cuales utilizo para definir reglas sobre mis propiedades.
using System.ComponentModel.DataAnnotations;

namespace To_Do_Api.Models.Entities
{
    // En esta parte defino mi clase ToDoList, que representa una entidad dentro de la base de datos.
    // Básicamente, esta clase es la estructura que EF Core usará para crear la tabla "ToDoLists".
    public class ToDoList
    {
        // Marco esta propiedad como la clave primaria de la tabla usando el atributo [Key].
        // Esto le indica a EF Core que esta columna será el identificador único de cada registro.
        [Key]
        public int Id { get; set; } // Aquí guardo el ID único de cada tarea.

        // Indico que el campo "Title" es obligatorio, así me aseguro de que no se guarden tareas sin título.
        [Required]

        // También defino que el título no puede tener más de 50 caracteres,
        // lo cual me ayuda a mantener los datos consistentes y evitar valores demasiado largos.
        [MaxLength(50)]

        // Esta propiedad almacena el título o nombre de la tarea.
        // Uso "= null!" para decirle al compilador que aunque no lo inicializo aquí, 
        // se asignará más adelante (por ejemplo, cuando se guarde o lea desde la base de datos).
        public string Title { get; set; } = null!;

        // Aseguro que el campo "FullName" también sea obligatorio.
        [Required]

        // Limito la longitud del nombre a un máximo de 30 caracteres.
        [MaxLength(30)]

        // Aquí guardo el nombre completo de la persona responsable o asociada a la tarea.
        public string FullName { get; set; } = null!;

        // También marco la descripción como un campo obligatorio.
        [Required]

        // Le pongo un límite de 300 caracteres para evitar descripciones excesivamente largas.
        [MaxLength(300)]

        // Esta propiedad guarda una descripción más detallada de la tarea.
        public string Description { get; set; } = null!;

        // Indico que el tipo de este campo es una fecha y hora.
        // Esto me permite almacenar cuándo fue creada la tarea.
        [DataType(DataType.DateTime)]

        // Aquí guardo la fecha y hora exacta en que se creó la tarea.
        public DateTime CreatedAt { get; set; }

        // También declaro que este campo es de tipo fecha y hora.
        [DataType(DataType.DateTime)]

        // Agrego el signo de interrogación (?) para indicar que esta propiedad puede ser nula,
        // ya que una tarea recién creada aún no tiene una fecha de finalización.
        public DateTime? CompletedAt { get; set; }

        // Limito este campo a 100 caracteres para las notas internas.
        [MaxLength(100)]

        // El signo de interrogación indica que esta propiedad puede ser nula,
        // porque no todas las tareas tendrán notas internas.
        public string? InternalNotes { get; set; }
    }
}

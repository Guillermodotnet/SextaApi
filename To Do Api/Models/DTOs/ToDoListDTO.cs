// Aquí importo el espacio de nombres que me permite usar los atributos de validación,
// como [Required], [MaxLength], etc., los cuales uso para validar la información que recibo.
using System.ComponentModel.DataAnnotations;

namespace To_Do_Api.Models.DTOs
{
    // En esta parte defino mi clase ToDoListDTO (Data Transfer Object).
    // Esta clase sirve como un “intermediario” entre mi API y el cliente,
    // ya que me permite controlar qué datos envío y qué datos recibo sin exponer directamente la entidad del modelo.
    public class ToDoListDTO
    {
        // Esta propiedad representa el identificador único de cada tarea.
        // Normalmente la uso cuando quiero obtener una tarea específica o actualizar una existente.
        // A diferencia de la entidad, aquí no necesito usar el atributo [Key].
        public int Id { get; set; }

        // Con este atributo indico que el campo "Title" es obligatorio cuando recibo datos del cliente.
        [Required]

        // Aquí limito la longitud máxima del título a 50 caracteres, 
        // para evitar que el usuario envíe cadenas demasiado largas.
        [MaxLength(50)]

        // Esta propiedad almacena el título de la tarea.
        // Uso el operador "null!" para indicar que siempre tendrá un valor (gracias a las validaciones).
        public string Title { get; set; } = null!;

        // Indico que el campo "FullName" también es obligatorio.
        [Required]

        // Limito el tamaño del nombre a un máximo de 30 caracteres.
        [MaxLength(30)]

        // Esta propiedad guarda el nombre de la persona asociada a la tarea.
        // También uso "null!" porque confío en que el atributo [Required] garantiza que no será nulo.
        public string FullName { get; set; } = null!;

        // Aseguro que la descripción también sea un campo obligatorio.
        [Required]

        // Aquí establezco que la descripción no puede superar los 300 caracteres.
        [MaxLength(300)]

        // Esta propiedad almacena la descripción detallada de la tarea.
        public string Description { get; set; } = null!;

        // Finalmente, agrego una propiedad booleana que me indica si la tarea está completada o no.
        // Prefiero usar un booleano aquí en el DTO para simplificar la información para el cliente,
        // en lugar de manejar fechas nulas como en la entidad (CompletedAt).
        public bool IsCompleted { get; set; }
    }
}



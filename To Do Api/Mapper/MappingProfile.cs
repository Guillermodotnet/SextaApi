// Aquí importo el espacio de nombres principal de AutoMapper, 
// ya que lo necesito para definir los perfiles de mapeo entre mis entidades y DTOs.
using AutoMapper;

// En esta línea importo el namespace donde tengo definido mi DTO ToDoListDTO.
using To_Do_Api.Models.DTOs;

// Y aquí importo el namespace donde está mi entidad ToDoList, que representa la tabla en la base de datos.
using To_Do_Api.Models.Entities;

namespace To_Do_Api.Mapper
{
    // Defino la clase MappingProfile, que hereda de Profile (una clase de AutoMapper).
    // Este perfil es donde establezco todas las reglas de conversión entre mis modelos y mis DTOs.
    public class MappingProfile : Profile
    {
        // En el constructor configuro los mapeos que mi aplicación va a usar.
        public MappingProfile()
        {
            // -----------------------------------------------------------------
            // Regla 1: Mapeo de Entidad → DTO (cuando envío datos al cliente)
            // -----------------------------------------------------------------

            // Aquí defino una regla que indica cómo convertir una entidad ToDoList a un DTO ToDoListDTO.
            CreateMap<ToDoList, ToDoListDTO>()
                // Uso ForMember para personalizar el mapeo de una propiedad específica del DTO.
                .ForMember(
                    // Indico que quiero configurar la propiedad "IsCompleted" del DTO como destino.
                    dest => dest.IsCompleted,
                    // Luego defino la lógica de cómo obtener ese valor desde la entidad de origen.
                    opt => opt.MapFrom(
                        // Si la propiedad CompletedAt en la entidad tiene un valor (no es nula),
                        // eso significa que la tarea fue completada, por lo tanto IsCompleted será true.
                        src => src.CompletedAt.HasValue
                    )
                );

            // -----------------------------------------------------------------
            // Regla 2: Mapeo de DTO → Entidad (cuando recibo datos del cliente)
            // -----------------------------------------------------------------

            // Aquí defino la regla inversa, para convertir de un ToDoListDTO a un ToDoList.
            CreateMap<ToDoListDTO, ToDoList>()
                // Uso ForMember nuevamente para personalizar la propiedad "CompletedAt" en la entidad destino.
                .ForMember(
                    // Indico que la propiedad destino es "CompletedAt", que puede ser nula (DateTime?).
                    dest => dest.CompletedAt,
                    // Y aquí defino cómo asignar su valor dependiendo del campo "IsCompleted" del DTO.
                    opt => opt.MapFrom(
                        // Si el DTO tiene IsCompleted en true, le asigno la fecha y hora actual,
                        // ya que eso significa que la tarea se completó.
                        src => src.IsCompleted
                            ? DateTime.Now
                            // Si no está completada, dejo el campo como nulo.
                            // Uso (DateTime?)null para asegurarme de que el tipo sea compatible con DateTime nullable.
                            : (DateTime?)null
                    )
                );
        }
    }
}

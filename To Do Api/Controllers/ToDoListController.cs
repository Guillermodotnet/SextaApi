// Aquí importo AutoMapper porque lo necesito para mapear entre mis entidades y los DTOs.
using AutoMapper;

// En esta parte importo las herramientas del framework necesarias para definir controladores y manejar las respuestas HTTP.
using Microsoft.AspNetCore.Mvc;

// Importo el espacio de nombres donde tengo mis DTOs, que uso para transferir datos entre el cliente y el servidor.
using To_Do_Api.Models.DTOs;

// Importo las entidades de mi modelo, que representan las tablas o estructuras de datos en la base de datos.
using To_Do_Api.Models.Entities;

// Aquí importo la interfaz del servicio, que define las operaciones CRUD que mi controlador va a usar.
using To_Do_Api.Services.InterfacesService;

namespace To_Do_Api.Controllers
{
    // Defino la ruta base de este controlador, de modo que las peticiones comiencen con "api/ToDoList".
    [Route("api/[controller]")]

    // Marco esta clase como un controlador de API, lo que me da funcionalidades automáticas como la validación de modelos.
    [ApiController]

    // Declaro mi controlador principal, que hereda de ControllerBase porque no necesito vistas, solo endpoints de API.
    public class ToDoListController : ControllerBase
    {
        // Declaro una variable de solo lectura donde voy a guardar la referencia al servicio que maneja la lógica de negocio.
        private readonly IToDoListService _services;

        // También declaro una referencia a AutoMapper para poder convertir entre entidades y DTOs fácilmente.
        private readonly IMapper _mapper;

        // En este constructor recibo las dependencias que necesito (el servicio y el mapeador) mediante inyección de dependencias.
        public ToDoListController(IToDoListService services, IMapper mapper)
        {
            // Aquí asigno el servicio recibido al campo privado para poder usarlo en los métodos del controlador.
            _services = services;

            // Y hago lo mismo con el mapeador.
            _mapper = mapper;
        }

        // -------------------------------------------------------------
        // MÉTODOS HTTP
        // -------------------------------------------------------------

        // Defino un endpoint GET para obtener todos los elementos de la lista de tareas.
        [HttpGet]

        // Especifico que este método puede devolver un código HTTP 200 si todo sale bien.
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            // Aquí llamo al servicio para obtener todas las entidades ToDoList almacenadas en la base de datos.
            var list = await _services.GetAllAsync();

            // Luego convierto esa lista de entidades a una lista de DTOs, para no exponer directamente mis modelos internos.
            var dtoList = _mapper.Map<List<ToDoListDTO>>(list);

            // Finalmente regreso la lista de DTOs con una respuesta HTTP 200 (OK).
            return Ok(dtoList);
        }

        // Defino un endpoint GET que obtiene un solo elemento por su ID.
        [HttpGet("{id}")]

        // Si todo sale bien, este método devuelve un 200 OK.
        [ProducesResponseType(StatusCodes.Status200OK)]

        // Si no se encuentra el elemento, puede devolver un 404 Not Found.
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            // Llamo al servicio para buscar un registro por su ID.
            var idItem = await _services.GetByIdAsync(id);

            // Si no encuentro nada, devuelvo una respuesta 404 indicando que no existe.
            if (idItem == null)
            {
                return NotFound();
            }

            // Si lo encuentro, convierto la entidad a un DTO para devolverla de forma más segura y limpia.
            var idDtoItem = _mapper.Map<ToDoListDTO>(idItem);

            // Retorno el DTO con un 200 OK.
            return Ok(idDtoItem);
        }

        // Defino un endpoint POST para crear una nueva tarea en la lista.
        [HttpPost]

        // Indico que si se crea correctamente, devolverá un código 201 Created.
        [ProducesResponseType(StatusCodes.Status201Created)]

        // También puede devolver un 400 Bad Request si el modelo no pasa la validación.
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ToDoListDTO createDto)
        {
            // Primero valido que el modelo cumpla con las reglas de anotaciones (por ejemplo, Required, MaxLength, etc.).
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devuelvo un 400 con los detalles.
                return BadRequest(ModelState);
            }

            // Convierto el DTO recibido en una entidad del modelo para poder guardarla en la base de datos.
            var entity = _mapper.Map<ToDoList>(createDto);

            // Asigno la fecha actual como CreatedAt, para registrar cuándo se creó la tarea.
            entity.CreatedAt = DateTime.Now;

            // Uso el servicio para guardar la nueva entidad en la base de datos.
            var createdEntity = await _services.AddAsync(entity);

            // Después del guardado, mapeo nuevamente la entidad (ya con ID asignado) a un DTO para devolverlo al cliente.
            var createdDto = _mapper.Map<ToDoListDTO>(createdEntity);

            // Finalmente devuelvo una respuesta 201 Created y señalo la ubicación del nuevo recurso usando CreatedAtAction.
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        // Defino un endpoint PUT para actualizar por completo una tarea existente.
        [HttpPut("{id}")]

        // Si la actualización es exitosa, no devuelvo contenido (204 No Content).
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        // Si hay algún problema con la solicitud, puede devolver 400.
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        // Si el elemento a actualizar no existe, devuelvo 404.
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ToDoListDTO updateDto)
        {
            // Primero reviso que el modelo sea válido y que el ID de la URL coincida con el ID dentro del DTO.
            if (!ModelState.IsValid || id != updateDto.Id)
            {
                // Si hay alguna inconsistencia, devuelvo un 400.
                return BadRequest();
            }

            // Busco en la base de datos la entidad que quiero actualizar.
            var entityToUpdate = await _services.GetByIdAsync(id);

            // Si no existe, devuelvo un 404.
            if (entityToUpdate == null)
            {
                return NotFound();
            }

            // Si sí existe, uso AutoMapper para copiar los valores del DTO sobre la entidad existente.
            _mapper.Map(updateDto, entityToUpdate);

            // Luego llamo al servicio para guardar los cambios actualizados.
            var updatedEntity = await _services.UpdateAsync(id, entityToUpdate);

            // Devuelvo un 204 No Content para indicar que la operación fue exitosa pero no hay cuerpo de respuesta.
            return NoContent();
        }

        // Defino un endpoint DELETE para eliminar una tarea por su ID.
        [HttpDelete("{id}")]

        // Si la eliminación es exitosa, devuelvo 204 No Content.
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        // Si no se encuentra el elemento, devuelvo 404 Not Found.
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            // Llamo al servicio para intentar eliminar el registro correspondiente al ID.
            var result = await _services.DeleteAsync(id);

            // Si el servicio devuelve false, significa que no se pudo eliminar o no existía.
            if (!result)
            {
                return NotFound();
            }

            // Si todo sale bien, devuelvo un 204 No Content indicando que el recurso fue eliminado correctamente.
            return NoContent();
        }
    }
}


# ToDo API

Una API RESTful para la gestión de tareas, creada con ASP.NET Core.  
Este proyecto tiene como objetivo **practicar la implementación de AutoMapper**, Entity Framework Core y la creación de una API de tareas un poco más avanzada.

---

## 📝 Descripción General

- API de gestión de tareas (*ToDoList*).  
- Implementa los principales verbos HTTP:
  - `GET /tasks` → traer todas las tareas
  - `GET /tasks/{id}` → traer tarea por ID
  - `POST /tasks` → crear tarea
  - `PUT /tasks/{id}` → actualizar tarea
  - `DELETE /tasks/{id}` → eliminar tarea
- La entidad principal tiene **7 propiedades**, de las cuales **solo 4 son visibles en el DTO**.  
- El DTO incluye una propiedad booleana `IsCompleted` que indica si la tarea está terminada.  
  - Si se marca como `true`, mediante **AutoMapper** esta propiedad se convierte en una fecha en la entidad.  
- Dos propiedades de la entidad no son accesibles directamente por el usuario.

---

🐾 Página Para Salvar Vidas
📚 Proyecto Académico – Aplicación Web con ASP.NET Core MVC
________________________________________
📌 1. Introducción
Página Para Salvar Vidas es una aplicación web desarrollada en ASP.NET Core MVC cuyo objetivo es brindar una solución digital para la gestión y visualización de animales en situación de:
•	🏠 Adopción
•	🤝 Tránsito
•	🐶 Comunidad
            Perdido
El sistema permite administrar información detallada de cada animal, facilitando la organización y mejorando la difusión para promover la adopción responsable.
Este proyecto fue desarrollado con fines académicos aplicando buenas prácticas de desarrollo, arquitectura MVC y gestión de base de datos con Entity Framework Core.
________________________________________
🎯 2. Objetivos del Proyecto
•	Implementar una arquitectura Modelo – Vista – Controlador (MVC).
•	Aplicar Entity Framework Core con migraciones.
•	Incorporar autenticación y autorización con Identity.
•	Desarrollar un sistema ABM completo.
•	Diseñar una interfaz moderna y responsive con Bootstrap 5.
•	Integrar tablas dinámicas utilizando DataTables.
________________________________________
🛠️ 3. Tecnologías Utilizadas
•	ASP.NET Core MVC
•	Entity Framework Core
•	SQL Server
•	ASP.NET Core Identity
•	Bootstrap 5
•	jQuery
•	DataTables
________________________________________
🧱 4. Arquitectura del Sistema
El proyecto está estructurado siguiendo el patrón MVC:
📁 Controllers
📁 Models
📁 Views
📁 Data
📁 wwwroot
🔹 Modelos Principales
•	Animal
•	AnimalEnAdopcion
•	AnimalEnTransito
•	AnimalComunitario
.       AnimalPerdido        
•	Usuario (Identity)
Cada entidad posee su correspondiente controlador y vistas CRUD.
________________________________________
🚀 5. Funcionalidades Implementadas
✔️ Alta, Baja y Modificación de animales
✔️ Gestión separada por categoría (Adopción, Tránsito, Comunitario, Perdido)
✔️ Subida y visualización de imágenes
✔️ Sistema de Login y Registro
✔️ Protección de acciones mediante [Authorize]
✔️ Tablas dinámicas con búsqueda y paginación

________________________________________
📸 6. Capturas del Sistema
🏠 Página Principal

 <img width="653" height="694" alt="image" src="https://github.com/user-attachments/assets/554cd314-bf3d-46a1-84b5-9b4f859709cc" />


4
________________________________________
📋 Listado de Animales (DataTables)
 
<img width="568" height="380" alt="image" src="https://github.com/user-attachments/assets/e0c6e3fb-4b2f-4d05-8a88-8db0b99c4367" />


<img width="568" height="380" alt="image" src="https://github.com/user-attachments/assets/2a61f6fd-be11-40f3-ab2d-d21bea39bae4" />

 

4
________________________________________
➕ Formulario de Registro

 <img width="568" height="380" alt="image" src="https://github.com/user-attachments/assets/00e83371-d095-4f4e-b316-0a12c0aa6908" />


4
________________________________________
⚙️ 7. Instalación y Ejecución
1️⃣ Clonar el repositorio
git clone (https://github.com/caro7712/PaginaparasalvarVidas)
2️⃣ Configurar la Base de Datos
Modificar el archivo appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=PaginaparaSalvarVidas;Trusted_Connection=True;TrustServerCertificate=True;"
}

🔐 8. Seguridad
El sistema implementa:
•	Registro y autenticación de usuarios
•	Protección de rutas mediante autorización
•	Restricción de acceso a acciones sensibles (Create, Edit, Delete)
________________________________________
🔮 9. Conclusión
Este proyecto permitió aplicar conocimientos fundamentales en:
•	Desarrollo Web con .NET
•	Arquitectura MVC
•	Gestión de base de datos relacional
•	Seguridad en aplicaciones web
•	Diseño responsive y experiencia de usuario
Representa una solución funcional y escalable para la gestión digital de animales en situación de vulnerabilidad.
________________________________________
👩💻 Autora
Telma Carolina Pardini

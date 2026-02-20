$(document).ready(function () {

    $('.datatable').DataTable({
        responsive: true,
        pageLength: 10,        // 👈 cantidad por defecto
        lengthMenu: [10, 25, 50, 100], // opciones del selector
        language: {
            search: "🔎 Buscar:",
            lengthMenu: "Mostrar _MENU_ registros",
            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
            paginate: {
                previous: "Anterior",
                next: "Siguiente"
            },
            zeroRecords: "No se encontraron registros coincidentes",
            infoEmpty: "No hay registros disponibles",
            emptyTable: "No hay datos en la tabla"
        }
    });

});

let datatable;

$(document).ready(() => {
    CargarDataTable();
})

const CargarDataTable = () => {
    datatable = $("#tblDatos").DataTable({
        "ajax": {
            "url": "/Admin/Categorias/ObtenerTodos"
        },
        "columns": [
            {"data": "nombre"},
            {
                "data": "estado",
                "render": function (data) {
                    if (data) {
                        return `<span class="badge text-bg-primary">Activo</span>`;
                    } else {
                        return `<span class="badge text-bg-danger">Inactivo</span>`;
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center d-flex flex-column gap-2 flex-md-row">
                                <a href="/Admin/Categorias/Edit/${data}" class="btn btn-secondary btn-sm">
                                    <i class="fa-solid fa-pencil"></i>
                                </a>
                                <button onclick=Delete("/Admin/Categorias/Delete/${data}") class="btn btn-danger btn-sm">
                                    <i class="fa-solid fa-trash-can"></i>
                                </button>
                            </div>`;
                }
            }
        ]
    });
}
const Delete = (url) => {

    Swal.fire({
        title: `¿Está seguro de eliminar la categoria?`,
        text: "Este cambio no se puede revertir.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: `#166534`,
        cancelButtonColor: "#d33",
        confirmButtonText: `<i class="fa-solid fa-check"></i> Sí, eliminar`,
        cancelButtonText: `<i class="fa-solid fa-ban"></i> Cancelar`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        datatable.ajax.reload();
                        Swal.fire(
                            "Eliminado",
                            data.message,
                            "success"
                        );
                    } else {
                        Swal.fire(
                            "Error",
                            data.message,
                            "error"
                        );
                    }
                }
            });
        }
    });
}
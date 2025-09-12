
let datatable;

$(document).ready(() => {
    CargarDataTable();
})

const CargarDataTable = () => {
    datatable = $("#tblDatos").DataTable({
        "ajax": {
            "url": "/Admin/Usuarios/ObtenerTodos"
        },
        "columns": [
            {"data": "email"},
            {"data": "nombre"},
            {"data": "apellido"},
            {
                "data": "phoneNumber",
                "render": function (data) {
                    if (!data) return "";

                    // quitar todo lo que no sea número
                    let num = data.replace(/\D/g, "");

                    // aplicar formato (XXX) XXX-XXXX
                    if (num.length === 10) {
                        return `(${num.substr(0, 3)}) ${num.substr(3, 3)}-${num.substr(6, 4)}`;
                    }

                    // si no tiene 10 dígitos, mostrar tal cual
                    return data;
                }
            },
            {"data": "role"},
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    let hoy = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > hoy) {
                        // usuario bloqueado
                        return `<div class="text-center">
                                <button onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger btn-sm">
                                    <i class="fa-solid fa-lock"></i>
                                </button>
                                </div>`;
                    } else {
                        // usuario desbloqueado
                        return `<div class="text-center">
                                 <button onclick=BloquearDesbloquear('${data.id}') class="btn btn-primary btn-sm">
                                   <i class="fa-solid fa-lock-open"></i>
                                </button>
                                </div>`;
                    }
                }
            }
        ]
    });
}

const BloquearDesbloquear = (id) => {

    $.ajax({
        type: "POST",
        url: "/Admin/Usuarios/BloquearDesbloquear",
        data: JSON.stringify(id),
        contentType: "application/json",        
        success: function (data) {
            if (data.success) {
                datatable.ajax.reload();
                Swal.fire(
                    "Confirmado",
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
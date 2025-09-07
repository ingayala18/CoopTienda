
const validarImagen = () => {
    const imagen = document.getElementById("imagen"); // 👈 sin #

    if (imagen.value === '' || imagen.value == null) {
        Swal.fire({
            title: "Favor de seleccionar una imagen",
            icon: "error", // 👈 logo de error
            showClass: {
                popup: `
              animate__animated
              animate__fadeInUp
              animate__faster
            `
            },
            hideClass: {
                popup: `
              animate__animated
              animate__fadeOutDown
              animate__faster
            `
            }
        });
        return false;
    }

    return true; // ✅
}
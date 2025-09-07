using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using CoopTienda.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace CoopTienda.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcasController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public MarcasController(IUnidadTrabajo unidadTrabajo)
        {
            this.unidadTrabajo = unidadTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            Marca marca = new()
            {
                Estado = true
            };
            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Marca marca)
        {
            if (ModelState.IsValid)
            {
                var marcaExiste = await unidadTrabajo.Marca.ObtenerPrimero(c => c.Nombre.ToLower().Trim() == marca.Nombre.ToLower().Trim());
                if (marcaExiste != null)
                {
                    ModelState.AddModelError("Nombre", "La marca ya existe.");
                    return View(marca);
                }

                await unidadTrabajo.Marca.Agregar(marca);
                TempData[DS.Exitoso] = "Marca creada exitosamente";
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var marca = await unidadTrabajo.Marca.Obtener(id);
            if (id == 0 || marca is null)
            {
                return NotFound();
            }
            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Marca marca)
        {
            var marcaDesdeDb = await unidadTrabajo.Marca.ObtenerPrimero(o => o.Id == marca.Id);

            if (ModelState.IsValid)
            {
                if (marcaDesdeDb is null)
                {
                    return NotFound();
                }

                var marcaExiste = await unidadTrabajo.Marca.ObtenerPrimero(c => c.Nombre.ToLower().Trim() == marca.Nombre.ToLower().Trim() && c.Id != marca.Id);
                if (marcaExiste != null)
                {
                    ModelState.AddModelError("Nombre", "La marca ya existe.");
                    return View(marca);
                }
                await unidadTrabajo.Marca.Actualizar(marca);
                TempData[DS.Exitoso] = "Marca actualizada exitosamente";
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            return Json(new { data = await unidadTrabajo.Marca.ObtenerTodos() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDesdeDb = await unidadTrabajo.Marca.Obtener(id);
            if (marcaDesdeDb is null)
            {
                return Json(new { success = false, message = "Error al borrar la marca" });
            }
            unidadTrabajo.Marca.Remover(marcaDesdeDb);
            await unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitosamente" });
        }
        #endregion
    }
}

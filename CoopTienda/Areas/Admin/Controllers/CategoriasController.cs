using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using CoopTienda.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace CoopTienda.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public CategoriasController(IUnidadTrabajo unidadTrabajo)
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
            Categoria categoria = new()
            {
                Estado = true
            };
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var categoriaExiste = await unidadTrabajo.Categoria.ObtenerPrimero(c => c.Nombre.ToLower().Trim() == categoria.Nombre.ToLower().Trim());
                if (categoriaExiste != null)
                {
                    ModelState.AddModelError("Nombre", "La categoría ya existe.");
                    return View(categoria);
                }

                await unidadTrabajo.Categoria.Agregar(categoria);
                TempData[DS.Exitoso] = "Categoría creada exitosamente";
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await unidadTrabajo.Categoria.Obtener(id);
            if (id == 0 || categoria is null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            var categoriaDesdeDb = await unidadTrabajo.Categoria.ObtenerPrimero(o => o.Id == categoria.Id);

            if (ModelState.IsValid)
            {
                if (categoriaDesdeDb is null)
                {
                    return NotFound();
                }

                var categoriaExiste = await unidadTrabajo.Categoria.ObtenerPrimero(c => c.Nombre.ToLower().Trim() == categoria.Nombre.ToLower().Trim() && c.Id != categoria.Id);
                if (categoriaExiste != null)
                {
                    ModelState.AddModelError("Nombre", "La categoría ya existe.");
                    return View(categoria);
                }
                await unidadTrabajo.Categoria.Actualizar(categoria);
                TempData[DS.Exitoso] = "Categoría actualizada exitosamente";
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            return Json(new { data = await unidadTrabajo.Categoria.ObtenerTodos() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var categoriaDesdeDb = await unidadTrabajo.Categoria.Obtener(id);
            if (categoriaDesdeDb is null)
            {
                return Json(new { success = false, message = "Error al borrar la categoría" });
            }
            unidadTrabajo.Categoria.Remover(categoriaDesdeDb);
            await unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoría borrada exitosamente" });
        }
        #endregion
    }
}

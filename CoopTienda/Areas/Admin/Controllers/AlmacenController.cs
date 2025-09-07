using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using CoopTienda.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoopTienda.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlmacenController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public AlmacenController(IUnidadTrabajo unidadTrabajo)
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
            Almacen almacen = new()
            {
                Estado = true
            };
            return View(almacen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Almacen almacen)
        {
            var existeNombre = await unidadTrabajo.Almacen.ObtenerPrimero(o => o.Nombre == almacen.Nombre);
            if (ModelState.IsValid)
            {
                if (existeNombre != null)
                {
                    ModelState.AddModelError("Nombre", "Ya existe un almacén con este nombre.");
                    return View(almacen);
                }

                await unidadTrabajo.Almacen.Agregar(almacen);
                await unidadTrabajo.Guardar();

                TempData[DS.Exitoso] = "Almacén creado exitosamente";
                return RedirectToAction("Index", "Almacen");
            }
            return View(almacen);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var almacen = await unidadTrabajo.Almacen.Obtener(id);
            if (id == 0 || almacen is null)
            {
                return NotFound();
            }
            return View(almacen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Almacen almacen)
        {
            var almacenDesdeDb = await unidadTrabajo.Almacen.ObtenerPrimero(o => o.Id == almacen.Id);
            if (ModelState.IsValid)
            {
                if (almacenDesdeDb is null)
                {
                    return NotFound();
                }

                almacenDesdeDb.Estado = almacen.Estado;
                almacenDesdeDb.Descripcion = almacen.Descripcion;
                await unidadTrabajo.Almacen.Actualizar(almacenDesdeDb);
                TempData[DS.Exitoso] = "Almacen actualizado exitosamente";
                await unidadTrabajo.Guardar();
                return RedirectToAction("Index", "Almacen");
            }
            return View(almacen);
        }

        #region API
        public async Task<IActionResult> ObtenerTodos()
        {
            return Json(new { data = await unidadTrabajo.Almacen.ObtenerTodos() });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var almacenDesdeDb = await unidadTrabajo.Almacen.Obtener(id);
            if (almacenDesdeDb is null)
            {
                return Json(new { success = false, message = "Error al borrar el almacén" });
            }
            unidadTrabajo.Almacen.Remover(almacenDesdeDb);
            await unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Almacén borrado exitosamente" });
        }
        #endregion
    }
}

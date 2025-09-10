using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using CoopTienda.Modelo.ViewsModels;
using CoopTienda.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace CoopTienda.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductosController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductosController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ProductoVM productoVM = new()
            {
                Producto = new()
                {
                    Estado = true
                },
                ListaCategorias = unidadTrabajo.Producto.DropDownListTodos("Categoria"),
                ListaMarcas = unidadTrabajo.Producto.DropDownListTodos("Marca"),
            };
            return View(productoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var RutaPrincipal = webHostEnvironment.WebRootPath;
                var Archivos = HttpContext.Request.Form.Files;

                if (productoVM.Producto.Id == 0)
                {
                    // Verificar duplicados antes de subir archivo

                    var existeCodigo = await unidadTrabajo.Producto.ObtenerPrimero(
                        o => o.Codigo == productoVM.Producto.Codigo);
                    if (existeCodigo is not null)
                    {
                        ModelState.AddModelError("Producto.Codigo", "Ya existe un producto con este código");
                    }

                    var existeSerial = await unidadTrabajo.Producto.ObtenerPrimero(
                        o => o.Serial == productoVM.Producto.Serial);
                    if (existeSerial is not null)
                    {
                        ModelState.AddModelError("Producto.Serial", "Ya existe un producto con este serial");
                    }

                    if (!ModelState.IsValid)
                    {
                        // Volvemos a llenar los combos si hay error
                        productoVM.ListaCategorias = unidadTrabajo.Producto.DropDownListTodos("Categoria");
                        productoVM.ListaMarcas = unidadTrabajo.Producto.DropDownListTodos("Marca");
                        return View(productoVM);
                    }

                    /* Nuevo Producto */
                    var NombreArchivo = Guid.NewGuid().ToString();
                    var Subida = Path.Combine(RutaPrincipal, @"imagenes\productos");
                    var Extension = Path.GetExtension(Archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(Subida, NombreArchivo + Extension), FileMode.Create))
                    {
                        Archivos[0].CopyTo(fileStreams);
                    }

                    productoVM.Producto.ImagenUrl = @"\imagenes\productos\" + NombreArchivo + Extension;

                    await unidadTrabajo.Producto.Agregar(productoVM.Producto);
                    await unidadTrabajo.Guardar();

                    TempData[DS.Exitoso] = "Producto creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Si llega aquí, hubo error en validación
            productoVM.ListaCategorias = unidadTrabajo.Producto.DropDownListTodos("Categoria");
            productoVM.ListaMarcas = unidadTrabajo.Producto.DropDownListTodos("Marca");
            return View(productoVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ProductoVM productoVM = new()
            {
                Producto = new Producto(),
                ListaCategorias = unidadTrabajo.Producto.DropDownListTodos("Categoria"),
                ListaMarcas = unidadTrabajo.Producto.DropDownListTodos("Marca"),
            };

            if (id is null or 0)
            {
                return NotFound();
            }
            productoVM.Producto = await unidadTrabajo.Producto.ObtenerPrimero(o => o.Id == id);

            if (productoVM.Producto is null)
            {
                return NotFound();
            }
            
            return View(productoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var productoDesdeDb = await unidadTrabajo.Producto.ObtenerPrimero(o => o.Id == productoVM.Producto.Id);

                if (productoDesdeDb is null)
                {
                    return NotFound();
                }

                var existeCodigo = await unidadTrabajo.Producto.ObtenerPrimero(o => o.Codigo == productoVM.Producto.Codigo
                 && o.Id != productoVM.Producto.Id);

                if (existeCodigo is not null)
                {
                    ModelState.AddModelError("Producto.Codigo", "Ya existe un producto con este código");
                }

                var existeNombre = await unidadTrabajo.Producto.ObtenerPrimero(o => o.Nombre == productoVM.Producto.Nombre
                 && o.Id != productoVM.Producto.Id);

                if (existeNombre is not null)
                {
                    ModelState.AddModelError("Producto.Nombre", "Ya existe un producto con este nombre");
                }

                var existeSerial = await unidadTrabajo.Producto.ObtenerPrimero(o => o.Serial == productoVM.Producto.Serial
                 && o.Id != productoVM.Producto.Id);

                if (existeSerial is not null)
                {
                    ModelState.AddModelError("Producto.Serial", "Ya existe un producto con este serial");
                }

                if (!ModelState.IsValid)
                {
                    productoVM.ListaCategorias = unidadTrabajo.Producto.DropDownListTodos("Categoria");
                    productoVM.ListaMarcas = unidadTrabajo.Producto.DropDownListTodos("Marca");
                    return View(productoVM);
                }

                // Actualizar datos del producto
                productoDesdeDb.Nombre = productoVM.Producto.Nombre;
                productoDesdeDb.Descripcion = productoVM.Producto.Descripcion;
                productoDesdeDb.Precio = productoVM.Producto.Precio;
                productoDesdeDb.Costo = productoVM.Producto.Costo;
                productoDesdeDb.Codigo = productoVM.Producto.Codigo;
                productoDesdeDb.Serial = productoVM.Producto.Serial;
                productoDesdeDb.Estado = productoVM.Producto.Estado;
                productoDesdeDb.CategoriaId = productoVM.Producto.CategoriaId;
                productoDesdeDb.MarcaId = productoVM.Producto.MarcaId;

                // Subida de imagen si se cambia
                var RutaPrincipal = webHostEnvironment.WebRootPath;
                var Archivos = HttpContext.Request.Form.Files;

                if (Archivos.Count() > 0)
                {
                    var NombreArchivo = Guid.NewGuid().ToString();
                    var Subida = Path.Combine(RutaPrincipal, @"imagenes\productos");
                    var Extension = Path.GetExtension(Archivos[0].FileName);
                    var rutaImagenAnterior = Path.Combine(RutaPrincipal, productoDesdeDb.ImagenUrl.TrimStart('/', '\\'));

                    // Borrar imagen anterior si existe
                    if (System.IO.File.Exists(rutaImagenAnterior))
                    {
                        System.IO.File.Delete(rutaImagenAnterior);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(Subida, NombreArchivo + Extension), FileMode.Create))
                    {
                        Archivos[0].CopyTo(fileStreams);
                    }

                    productoDesdeDb.ImagenUrl = "/imagenes/productos/" + NombreArchivo + Extension;
                    await unidadTrabajo.Producto.Actualizar(productoDesdeDb);
                    await unidadTrabajo.Guardar();
                    TempData[DS.Exitoso] = "Producto actualizado exitosamente";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    // Si no se sube nueva imagen, mantener la existente
                    productoDesdeDb.ImagenUrl = productoDesdeDb.ImagenUrl;
                }
                await unidadTrabajo.Producto.Actualizar(productoDesdeDb);
                await unidadTrabajo.Guardar();
                TempData[DS.Exitoso] = "Producto actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            // Volver a llenar SelectList si hay errores
            productoVM.ListaCategorias = unidadTrabajo.Producto.DropDownListTodos("Categoria");
            productoVM.ListaMarcas = unidadTrabajo.Producto.DropDownListTodos("Marca");
            return View(productoVM);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "Categoria,Marca");
            return Json(new { data = todos });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete (int? id)
        {
            var producto = await unidadTrabajo.Producto.ObtenerPrimero(o => o.Id == id);
            if (producto is null || id == 0)
            {
                return Json(new { success = false, message = "Error al borrar el producto" });
            }
            // Borrar imagen si existe
            var RutaPrincipal = webHostEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(RutaPrincipal, producto.ImagenUrl.TrimStart('/','\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            unidadTrabajo.Producto.Remover(producto);
            await unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado exitosamente" });
        }
        #endregion
    }
}

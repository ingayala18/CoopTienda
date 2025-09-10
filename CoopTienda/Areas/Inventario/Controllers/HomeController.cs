using System.Diagnostics;
using System.Threading.Tasks;
using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using CoopTienda.Modelo.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoopTienda.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public HomeController(IUnidadTrabajo unidadTrabajo)
        {
            this.unidadTrabajo = unidadTrabajo;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Producto> listaProducto = await unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "Categoria,Marca");
            return View(listaProducto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

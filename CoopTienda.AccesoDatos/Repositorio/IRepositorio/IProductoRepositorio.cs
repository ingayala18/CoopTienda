using CoopTienda.Modelo;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopTienda.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        Task Actualizar(Producto producto);
        IEnumerable<SelectListItem> DropDownListTodos(string tipo);
    }
}

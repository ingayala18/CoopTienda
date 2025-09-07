using CoopTienda.AccesoDatos.Data;
using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopTienda.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task Actualizar(Producto producto)
        {
            var productoDb = await db.Producto.FirstOrDefaultAsync(p => p.Id == producto.Id);
            if (productoDb is not null)
            {
                productoDb.Codigo = producto.Codigo;
                productoDb.Serial = producto.Serial;
                productoDb.Nombre = producto.Nombre;
                productoDb.Descripcion = producto.Descripcion;
                productoDb.Precio = producto.Precio;
                productoDb.Costo = producto.Costo;
                productoDb.Estado = producto.Estado;
                productoDb.CategoriaId = producto.CategoriaId;
                productoDb.MarcaId = producto.MarcaId;
                if (producto.ImagenUrl is not null)
                {
                    productoDb.ImagenUrl = producto.ImagenUrl;
                }
            }
        }

        public IEnumerable<SelectListItem> DropDownListTodos(string tipo)
        {
            if(tipo == "Categoria")
            {
                return db.Categoria.Where(c => c.Estado).Select(c => new SelectListItem()
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            else if (tipo == "Marca")
            {
                return db.Marca.Where(m => m.Estado).Select(m => new SelectListItem()
                {
                    Text = m.Nombre,
                    Value = m.Id.ToString()
                });
            }
            return null;
        }
    }
}

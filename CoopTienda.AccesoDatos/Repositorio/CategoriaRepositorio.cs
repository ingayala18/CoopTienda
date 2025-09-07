using CoopTienda.AccesoDatos.Data;
using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopTienda.AccesoDatos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext db;

        public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task Actualizar(Categoria categoria)
        {
            var categoriaBD = await db.Categoria.FirstOrDefaultAsync(c => c.Id == categoria.Id);
            if (categoriaBD is not null)
            {
                categoriaBD.Nombre = categoria.Nombre;
                categoriaBD.Estado = categoria.Estado;
            }
        }
    }
}

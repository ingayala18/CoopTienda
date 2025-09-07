using CoopTienda.AccesoDatos.Data;
using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopTienda.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext db;

        public IAlmacenRepositorio Almacen { get; private set; }
        public ICategoriaRepositorio Categoria { get; private set; }
        public UnidadTrabajo(ApplicationDbContext db)
        {
            this.db = db;
            Almacen = new AlmacenRepositorio(db);
            Categoria = new CategoriaRepositorio(db);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task Guardar()
        {
            await db.SaveChangesAsync();
        }
    }
}

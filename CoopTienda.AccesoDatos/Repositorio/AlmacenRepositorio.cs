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
    public class AlmacenRepositorio : Repositorio<Almacen>, IAlmacenRepositorio
    {
        private readonly ApplicationDbContext db;

        public AlmacenRepositorio(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task Actualizar(Almacen almacen)
        {
            var almacenDb = await db.Almacen.FirstOrDefaultAsync(a => a.Id == almacen.Id);

            if (almacenDb is not null)
            {
                almacenDb.Nombre = almacen.Nombre;
                almacenDb.Descripcion = almacen.Descripcion;
                almacenDb.Estado = almacen.Estado;
            }
        }
    }
}

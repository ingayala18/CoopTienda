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
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext db;

        public MarcaRepositorio(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task Actualizar(Marca marca)
        {
            var marcaBD = await db.Marca.FirstOrDefaultAsync(c => c.Id == marca.Id);
            if (marcaBD is not null)
            {
                marcaBD.Nombre = marca.Nombre;
                marcaBD.Estado = marca.Estado;
            }
        }
    }
}

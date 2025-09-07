using CoopTienda.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopTienda.AccesoDatos.Configuracion
{
    public class Categoriafiguracion : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(x => x.Nombre).IsRequired(true);
            builder.Property(x => x.Estado).IsRequired(true);
        }
    }
}

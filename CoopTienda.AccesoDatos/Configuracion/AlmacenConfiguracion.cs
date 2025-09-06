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
    public class AlmacenConfiguracion : IEntityTypeConfiguration<Almacen>
    {
        public void Configure(EntityTypeBuilder<Almacen> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(x => x.Nombre).IsRequired(true);
            builder.Property(x => x.Descripcion).IsRequired(true);
            builder.Property(x => x.Estado).IsRequired(true);
        }
    }
}

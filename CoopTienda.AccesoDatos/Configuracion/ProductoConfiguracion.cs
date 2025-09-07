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
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(x => x.Codigo).IsRequired(true);
            builder.Property(x => x.Serial).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.Nombre).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.Descripcion).IsRequired(true).HasMaxLength(200);
            builder.Property(x => x.Precio).IsRequired(true);
            builder.Property(x => x.Costo).IsRequired(true);
            builder.Property(x => x.ImagenUrl).IsRequired(false);
            builder.Property(x => x.Estado).IsRequired(true);
            builder.Property(x => x.CategoriaId).IsRequired(true);
            builder.Property(x => x.MarcaId).IsRequired(true);

            /** Relaciones **/
            builder.HasOne(a => a.Categoria)
                   .WithMany()
                   .HasForeignKey(c => c.CategoriaId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Marca)
                   .WithMany()
                   .HasForeignKey(c => c.MarcaId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

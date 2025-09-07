using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopTienda.Modelo
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Range(1,99999, ErrorMessage = "El codigo debe tener maximo 5 numeros sin empezar en 0.")]
        [Required(ErrorMessage = "El codigo es obligatorio.")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "El serial es obligatorio.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "El serial debe tener entre 6 y 50 caracteres.")]
        public string Serial { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre del producto")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "El nombre debe tener entre 6 y 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "La descripcion debe tener entre 6 y 200 caracteres.")]
        public string Descripcion { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        public double Precio { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0.")]
        [Required(ErrorMessage = "El costo es obligatorio.")]
        public double Costo { get; set; }

        [Display(Name = "Imagen")]
        [DataType(DataType.ImageUrl)]
        public string ImagenUrl { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria.")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }
    }
}

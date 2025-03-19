using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JFGM20250319.AppWebMVC.Models;

public partial class Product
{
    public int ProductId { get; set; }

    [Display(Name = "Nombre del producto")]
    [Required(ErrorMessage = "Es obligaotrio el nombre del producto")]
    public string ProductName { get; set; } = null!;

    [Display(Name = "Descripcion")]
    public string? Description { get; set; }

    [Display(Name = "Precio del producto")]
    [Required(ErrorMessage ="Es obligaotrio el precio del producto")]
    public decimal Price { get; set; }

    [Display(Name = "Categoria")]
    public int? CategoryId { get; set; }
    [Display(Name = "Marca")]
    public int? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }
}

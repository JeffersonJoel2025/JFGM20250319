using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace JFGM20250319.AppWebMVC.Models;

public partial class Brand
{
    public int BrandId { get; set; }
    [Display(Name = "Marca")]
    [Required(ErrorMessage = "Es obligatorio el nombre de la marca")]
    public string BrandName { get; set; } = null!;

    [Display(Name = "Pais de origen de la marca")]
    public string? Country { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

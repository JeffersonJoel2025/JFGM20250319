using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JFGM20250319.AppWebMVC.Models;

public partial class User
{
    public int UserId { get; set; }

    [Display(Name = "Nombre del usuario")]
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Username { get; set; } = null!;

    [Display(Name = "Correo electronico")]
    [Required(ErrorMessage = "El Correo es obligatorio")]
    public string Email { get; set; } = null!;

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "La contraseña es obligatorio")]
    public string PasswordHash { get; set; } = null!;

    [Display(Name = "Rol")]
    [Required(ErrorMessage = "El Rol es obligatorio")]
    public string Role { get; set; } = null!;
}

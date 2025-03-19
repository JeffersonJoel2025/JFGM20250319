using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [StringLength(40, MinimumLength = 5, ErrorMessage = "El passowrd debe tener entre 5 y 50 caracteres.")]
    public string PasswordHash { get; set; } = null!;

    [Display(Name = "Rol")]
    [Required(ErrorMessage = "El Rol es obligatorio")]
    public string Role { get; set; } = null!;

    [NotMapped]
    [StringLength(40, MinimumLength = 5, ErrorMessage = "El password debe tener entre 5 y 50 caracteres.")]
    [Display(Name = "Confirmar Contraseña")]
    [DataType(DataType.Password)]
    [Compare("PasswordHash", ErrorMessage = "Las contraseñas no coinciden.")]
    public string? paswordConfir { get; set; } = null!;
}

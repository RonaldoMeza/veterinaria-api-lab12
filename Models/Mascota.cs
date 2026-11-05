using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace veterinaria_api.Models;

public partial class Mascota
{
    public int IdMascota { get; set; }

    [Required, StringLength(80)]
    public string Nombre { get; set; } = null!;

    [Required, StringLength(50)]
    public string Especie { get; set; } = null!;

    [Required, StringLength(80)]
    public string Raza { get; set; } = null!;

    [Required, StringLength(1), RegularExpression("^[MH]$", ErrorMessage = "El sexo debe ser M o H.")]
    public string Sexo { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    [Range(0.01, 500)]
    public decimal Peso { get; set; }

    public int IdPropietario { get; set; }

    public virtual Propietario? Propietario { get; set; }
}

using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace veterinaria_api.Models;

public partial class Propietario
{
    public int IdPropietario { get; set; }

    [Required, StringLength(80)]
    public string Nombres { get; set; } = null!;

    [Required, StringLength(80)]
    public string Apellidos { get; set; } = null!;

    [Required, StringLength(8), RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
    public string Dni { get; set; } = null!;

    [Required, StringLength(20)]
    public string Telefono { get; set; } = null!;

    [Required, StringLength(120), EmailAddress]
    public string Correo { get; set; } = null!;

    [Required, StringLength(200)]
    public string Direccion { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}

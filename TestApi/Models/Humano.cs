using System.ComponentModel.DataAnnotations;

namespace TestApi.Models;

public class Humano
{
    public int Id { get; set; }

    [MaxLength(150)]
    public string Nombre { get; set; } = null!;

    [MaxLength(6)]
    public string Sexo { get; set; } = null!;

    public int Edad { get; set; }

    public float Altura { get; set; }

    public float Peso { get; set; }
}
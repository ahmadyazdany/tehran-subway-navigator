using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DsDemo.Data.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Station
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Colors { get; set; }
}

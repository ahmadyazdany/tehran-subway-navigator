using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DsDemo.Data.Entities;

public class Way
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int From { get; set; }

    [Required]
    public int To { get; set; }
}

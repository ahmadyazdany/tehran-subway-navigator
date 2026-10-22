using DsDemo.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace DsDemo.ViewModels;

public class StationsViewModel :IValidatableObject
{
    [Required(ErrorMessage = "لطفا مبدا را انتخاب کنید!")]
    public int DepartureId { get; set; }

    [Required(ErrorMessage = "لطفا مقصد را انتخاب کنید!")]
    public int DestinationId { get; set; }

    public List<Station>? Stations { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DepartureId == DestinationId)
            yield return new ValidationResult("لطفا ایستگاه های متفاوت انتخاب کنید!", new[] { nameof(DestinationId) });
    }
}

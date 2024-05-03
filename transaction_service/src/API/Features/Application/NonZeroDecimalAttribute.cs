using System.ComponentModel.DataAnnotations;

namespace API.Features.Application;

public class NonZeroDecimalAttribute : ValidationAttribute
{
    public NonZeroDecimalAttribute()
    {
        ErrorMessage = "Amount must be a non-zero decimal value.";
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return false;
        }

        if (value is decimal amount)
        {
            return amount != 0;
        }

        return false;
    }
}

public class MyModel
{
    [Required(ErrorMessage = "Amount is required.")]
    [NonZeroDecimal]
    public decimal Amount { get; set; }
}
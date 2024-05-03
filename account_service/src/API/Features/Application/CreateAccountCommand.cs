using System.ComponentModel.DataAnnotations;
using CodeContracts.Application;

namespace API.Features.Application;

public class CreateAccountCommand : Command
{
    [Required(ErrorMessage = "CPR number is required.")]
    [StringLength(10, ErrorMessage = "CPR number must be 10 characters long.")]
    public string CPR { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(27, ErrorMessage = "First name must be less than 27 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(27, ErrorMessage = "Last name must be less than 27 characters.")]
    public string LastName { get; set; }
}
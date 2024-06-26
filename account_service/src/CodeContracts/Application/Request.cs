using System.ComponentModel.DataAnnotations;

namespace CodeContracts.Application;

public class Request
{
    [Required(ErrorMessage = "ID is required.")]
    public Guid Id { get; set; }
}
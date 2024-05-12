using System.ComponentModel.DataAnnotations;

namespace CodeContracts.Application;

public record Command : ICommand
{
    [Required(ErrorMessage = "ID is required.")]
    public Guid Id { get; set; }
}
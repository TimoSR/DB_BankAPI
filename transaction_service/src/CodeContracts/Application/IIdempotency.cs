using System.ComponentModel.DataAnnotations;

namespace CodeContracts.Application;

public interface IIdempotency
{
    Guid Id { get; set; }
}
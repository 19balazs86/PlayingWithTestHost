using System.ComponentModel.DataAnnotations;

namespace PlayingWithTestHost;

public record TestConfig
{
    [Required, MinLength(3)]
    public string Key1 { get; init; }

    public bool Key2 { get; init; }
}
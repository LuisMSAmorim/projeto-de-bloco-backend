using System.ComponentModel.DataAnnotations;

namespace FisioFinancials.Domain.Model.DTOs;

public sealed record ReceivedDTO
{
    public int? ReceivedId { get; set; }
    [Required]
    public string PatientName { get; set; }
    [Required]
    public int Value { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Local { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public string? UserId { get; set; }

    public void Deconstruct(out string PatientName, out int Value, out string City, out string Local, out DateTime Date)
    {
        PatientName = this.PatientName;
        Value = this.Value;
        City = this.City;
        Local = this.Local;
        Date = this.Date;
    }
}

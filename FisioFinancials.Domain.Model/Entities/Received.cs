using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FisioFinancials.Domain.Model.Entities;

public class Received 
{
    [Key]
    public int ReceivedId { get; set; }
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
    public string UserId { get; set; }
    [Required]
    [ForeignKey("UserId")]  
    public virtual User User { get; set; }

    public Received(string patientName, int value, string city, string local, DateTime date)
    {
        PatientName = patientName;
        Value = value;
        City = city;
        Local = local;
        Date = date;
    }
}

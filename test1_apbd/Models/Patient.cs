namespace test1_apbd.Models;

using System.ComponentModel.DataAnnotations;

public class Patient
{
    public int PatientId { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}
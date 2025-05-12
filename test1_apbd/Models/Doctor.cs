namespace test1_apbd.Models;

using System.ComponentModel.DataAnnotations;


public class Doctor
{
    public int DoctorId { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    [MaxLength(7)]
    public string PWZ { get; set; }
}
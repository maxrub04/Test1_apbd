namespace test1_apbd.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class AppointmentResponseDTO
{
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public int DoctorId { get; set; }
    [Required]
    [RegularExpression("^[0-9]{7}$")] 
    public string PWZ { get; set; }
    public List<AppointmentServiceDTO> appointmentServices { get; set; }

    public AppointmentResponseDTO()
    { }

    public AppointmentResponseDTO(DateTime Date, string FirstName, string LastName, DateTime DateOfBirth, int DoctorId, string PWZ, List<AppointmentServiceDTO> appointmentServices)
    {
        this.Date = Date;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.DateOfBirth = DateOfBirth;
        this.DoctorId = DoctorId;
        this.PWZ = PWZ;
        this.appointmentServices = appointmentServices;
    }
}
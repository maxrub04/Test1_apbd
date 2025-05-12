namespace test1_apbd.Models.DTOs;

public class AppointmentRequestDTO
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PWZ { get; set; }
    public List<Service> services { get; set; }
}
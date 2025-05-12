using test1_apbd.Models.DTOs;
namespace test1_apbd.Services;


public interface IAppointmentsService
{
    Task<AppointmentResponseDTO> GetAppointmentByIdAsync(int id);
    Task CreateAppointmentAsync(AppointmentRequestDTO appointment);
}
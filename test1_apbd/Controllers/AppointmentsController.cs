using Microsoft.AspNetCore.Http;

namespace test1_apbd.Controllers;

using test1_apbd.Models.DTOs;
using test1_apbd.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService _appointmentsService;

    public AppointmentsController(IAppointmentsService appointmentsService) =>
        _appointmentsService = appointmentsService;
    
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentResponseDTO>> GetAppointmentByIdAsync(int id)
    {
        try
        {
            var appointmentResponseDTO = await _appointmentsService.GetAppointmentByIdAsync(id);
            return Ok(appointmentResponseDTO);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentResponseDTO>> CreateAppointment(
        [FromBody] AppointmentRequestDTO appointmentRequestDTO)
    {
        try
        {
            // async
            await _appointmentsService.CreateAppointmentAsync(appointmentRequestDTO);
            return StatusCode(StatusCodes.Status201Created, "Appointment created successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
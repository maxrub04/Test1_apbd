using Microsoft.Data.SqlClient;
using test1_apbd.Models.DTOs;
using test1_apbd.Services;

namespace GrupaA_Test.Services;

public class AppointmentsService : IAppointmentsService
{
    private readonly string _connectionString =
        "Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Initial Catalog=apbd; Integrated Security=False;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False";

    public async Task<AppointmentResponseDTO> GetAppointmentByIdAsync(int id)
    {
        const string checkAppointmentSQL_Comamnd = "SELECT 1 FROM Appointment WHERE appoitment_id = @AppointmentId";
        const string getAppointmentSQL_Command = """
                                                    SELECT a.date,
                                                        p.first_name,
                                                        p.last_name,
                                                        p.date_of_birth,
                                                        d.doctor_id,
                                                        d.PWZ
                                                    FROM Appointment a
                                                    JOIN Patient p ON a.patient_id = p.patient_id
                                                    JOIN Doctor d ON a.doctor_id = d.doctor_id
                                                    WHERE a.appoitment_id = @AppointmentId;
                                                 """;
        const string getAppointmentServicesSQL_Command = """
                                                            SELECT s.name, 
                                                                    ass.service_fee
                                                            FROM Service s 
                                                            Inner JOIN Appointment_Service ass ON s.service_id = ass.service_id
                                                            WHERE ass.appoitment_id = @AppointmentId;
                                                         """;

        var appointmentServicesList = new List<AppointmentServiceDTO>();
        AppointmentResponseDTO appointmentResponseDTO = new AppointmentResponseDTO();
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    using (var checkAppointmentCMD = new SqlCommand(checkAppointmentSQL_Comamnd, conn, transaction))
                    {
                        checkAppointmentCMD.Parameters.AddWithValue("@AppointmentId", id);
                        if (await checkAppointmentCMD.ExecuteScalarAsync() == null)
                            throw new Exception("Appointment was not found");
                    }

                    using (var appointmentServicesCMD = new SqlCommand(getAppointmentServicesSQL_Command, conn, transaction))
                    {
                        appointmentServicesCMD.Parameters.AddWithValue("@AppointmentId", id);
                        using (var reader = await appointmentServicesCMD.ExecuteReaderAsync()){
                            while (await reader.ReadAsync())
                            {
                                appointmentServicesList.Add(new AppointmentServiceDTO()
                                {
                                    Name = reader.GetString(0),
                                    ServiceFee = reader.GetDecimal(1)
                                });
                            }
                        }
                    }
                    
                    using (var getAppointmentCMD = new SqlCommand(getAppointmentSQL_Command, conn, transaction))
                    {
                        getAppointmentCMD.Parameters.AddWithValue("@AppointmentId", id);
                        using (var reader = await getAppointmentCMD.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                appointmentResponseDTO.Date = reader.GetDateTime(0);
                                appointmentResponseDTO.FirstName = reader.GetString(1);
                                appointmentResponseDTO.LastName = reader.GetString(2);
                                appointmentResponseDTO.DateOfBirth = reader.GetDateTime(3);
                                appointmentResponseDTO.DoctorId = reader.GetInt32(4);
                                appointmentResponseDTO.PWZ = reader.IsDBNull(5) ? null : reader.GetString(5);
                                appointmentResponseDTO.appointmentServices = appointmentServicesList;
                            } else throw new Exception("Appointment details were not retrieved.");
                        }
                    }
                   
                    transaction.Commit();
                    return appointmentResponseDTO;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task CreateAppointmentAsync(AppointmentRequestDTO appointment)
    {
        const string createAppointmentSQL_Command = """
                                                        INSERT INTO Appointment(appoitment_id, patient_id, doctor_id, date) 
                                                        VALUES (@AppointmentId, @PatientId, (SELECT doctor_id FROM Doctor WHERE PWZ = @PWZ), GETDATE());
                                                    """;

        const string insertAppointmentServicesSQL_Command = """
                                                                INSERT INTO Appointment_Service (appoitment_id, service_id, service_fee) 
                                                                VALUES (@AppointmentId, (SELECT service_id FROM Service WHERE name = @ServiceName), @ServiceFee);
                                                            """;

        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    using (var insertAppointmentCMD = new SqlCommand(createAppointmentSQL_Command, conn, transaction))
                    {
                        insertAppointmentCMD.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
                        insertAppointmentCMD.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                        insertAppointmentCMD.Parameters.AddWithValue("@PWZ", appointment.PWZ);
                        await insertAppointmentCMD.ExecuteNonQueryAsync();
                    }

                    foreach (var service in appointment.services)
                    {
                        using (var serviceCMD =
                               new SqlCommand(insertAppointmentServicesSQL_Command, conn, transaction))
                        {
                            serviceCMD.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
                            serviceCMD.Parameters.AddWithValue("@ServiceName", service.ServiceName);
                            serviceCMD.Parameters.AddWithValue("@ServiceFee", service.BaseFee);
                            await serviceCMD.ExecuteNonQueryAsync();
                        }
                    }
                    await transaction.CommitAsync();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
namespace test1_apbd.Models;

using System.ComponentModel.DataAnnotations;

public class Service
{
    public int ServiceId { get; set; }
    [MaxLength(100)]
    public string ServiceName { get; set; }
    public decimal BaseFee { get; set; }
}
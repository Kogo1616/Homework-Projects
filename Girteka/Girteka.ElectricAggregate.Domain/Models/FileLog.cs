namespace Girteka.ElectricAggregate.Domain.Models;

public class FileLog
{
    public int Id { get; set; }

    public string FileName { get; set; }
    
    
    public string Status { get; set; }

    public DateTime CreateDateTime { get; set; }
}
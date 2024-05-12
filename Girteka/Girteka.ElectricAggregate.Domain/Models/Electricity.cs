namespace Girteka.ElectricAggregate.Domain.Models;

public class Electricity
{
    public int Id { get; set; }
    
    public string? Tinklas { get; set; }
    
    public string? Pavadinimas { get; set; }

    public decimal? PPlus { get; set; }
    
    public DateTime? PlT { get; set; }
    
    public decimal? PMinus { get; set; }
}
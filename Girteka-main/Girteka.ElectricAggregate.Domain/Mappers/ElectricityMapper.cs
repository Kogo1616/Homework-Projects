using CsvHelper.Configuration;
using Girteka.ElectricAggregate.Domain.Converter;
using Girteka.ElectricAggregate.Domain.Models;

namespace Girteka.ElectricAggregate.Domain.Mappers;

public class ElectricityMapper : ClassMap<Electricity>
{
    public ElectricityMapper()
    {
        Map(m => m.Tinklas).Index(0).Name("TINKLAS");
        Map(m => m.Pavadinimas).Index(1).Name("OBT_PAVADINIMAS");
        Map(m => m.PPlus).Index(4).Name("P+");
        Map(m => m.PlT).Index(5).Name("PL_T");
        Map(m => m.PMinus).Index(6).Name("P-").TypeConverter<CustomDecimalConverter>();
    }
}
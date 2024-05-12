using Girteka.ElectricAggregate.Domain.Models;

namespace Girteka.ElectricAggregate.Domain;

public interface IElectricity
{
    Task<List<Electricity>> Get();
}
namespace Girteka.ElectricAggregate.Domain;

public interface IFileArchive
{
    Task SaveFileInArchive(string fileName);
}
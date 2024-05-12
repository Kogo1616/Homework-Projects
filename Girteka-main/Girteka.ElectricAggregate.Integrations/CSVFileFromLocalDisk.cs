using Girteka.ElectricAggregate.Domain;

namespace Girteka.ElectricAggregate.Integrations;

public class CSVFileFromLocalDisk : IContext<string, string, Stream>
{
    public Stream Do(string fileName, string path)
    {
        string filePath = Path.Combine(path, fileName);

        return File.Open(filePath, FileMode.Open);
    }
}
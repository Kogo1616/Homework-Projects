namespace Girteka.ElectricAggregate.Domain.Services;

public class DownloadFiles : IContext<string, string, Stream>
{
    private readonly IContext<string, string, Stream> _context;

    public DownloadFiles(IContext<string, string, Stream> context)
    {
        _context = context;
    }

    public Stream Do(string param, string path)
    {
        return _context.Do(param, path);
    }
}
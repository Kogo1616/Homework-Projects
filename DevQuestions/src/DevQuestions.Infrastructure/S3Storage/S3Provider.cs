using DevQuestions.Application.FileStorage;

namespace DevQuestions.Infrastructure.S3Storage;

public class S3Provider : IFileProvider
{
    public Task<string> UploadAsync(Stream stream, string key, string bucket, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
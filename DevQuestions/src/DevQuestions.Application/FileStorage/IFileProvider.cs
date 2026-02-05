namespace DevQuestions.Application.FileStorage;

public interface IFileProvider
{
    public Task<string> UploadAsync(Stream stream, string key, string bucket, CancellationToken cancellationToken);
}
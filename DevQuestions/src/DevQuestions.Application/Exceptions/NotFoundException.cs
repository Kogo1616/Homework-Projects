using System.Text.Json;
using Shared;

namespace DevQuestions.Application.Exceptions;

public class NotFoundException : Exception
{
    protected NotFoundException(Error[] error)
        : base(JsonSerializer.Serialize(error))
    {
    }
}
using System.Text.Json;
using Shared;

namespace DevQuestions.Application.Exceptions;

public class BadRequestException : Exception
{
    protected BadRequestException(Error[] error)
        : base(JsonSerializer.Serialize(error))
    {
    }
}
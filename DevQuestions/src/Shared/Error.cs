using System.Text.Json.Serialization;

namespace Shared
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorType Type { get; set; }
        public string? InvalidField { get; set; }

        [JsonConstructor]
        public Error(string code, string message, ErrorType type, string? invalidField = null)
        {
            Code = code;
            Message = message;
            Type = type;
            InvalidField = invalidField;
        }

        public static Error NotFound(string? code, string message, Guid? id)
            => new(code ?? "record.not.found", message, ErrorType.NOT_FOUND);

        public static Error Validation(string? code, string message, string? invalidField = null)
            => new(code ?? "value.is.invalid", message, ErrorType.VALIDATION, invalidField);

        public static Error Conflict(string? code, string message)
            => new(code ?? "value.is.conflict", message, ErrorType.CONFLICT);

        public static Error Failure(string? code, string message)
            => new(code ?? "value.is.conflict", message, ErrorType.FAILURE);
    }
}
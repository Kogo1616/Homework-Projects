using DevQuestions.Application.Exceptions;
using Shared;

namespace DevQuestions.Application.Questions.Exceptions;

public class QuestionValidationException : BadRequestException
{
    protected QuestionValidationException(Error error)
        : base(error)
    {

    }
}

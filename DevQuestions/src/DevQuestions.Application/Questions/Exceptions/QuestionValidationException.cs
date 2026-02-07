using DevQuestions.Application.Exceptions;
using Shared;

namespace DevQuestions.Application.Questions.Exceptions;

public class QuestionValidationException : BadRequestException
{
    public QuestionValidationException(Error[] errors)
        : base(errors)
    {

    }
}
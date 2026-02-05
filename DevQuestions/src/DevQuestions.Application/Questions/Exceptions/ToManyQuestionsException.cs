using DevQuestions.Application.Exceptions;
using Shared;

namespace DevQuestions.Application.Questions.Exceptions;

public class ToManyQuestionsException : BadRequestException
{
    public ToManyQuestionsException(Error[] errors)
        : base(errors)
    {

    }
}
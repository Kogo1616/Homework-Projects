using DevQuestions.Application.Exceptions;
using DevQuestions.Application.Questions.Fails;
using Shared;

namespace DevQuestions.Application.Questions.Exceptions;

public class ToManyQuestionsException : BadRequestException
{
    public ToManyQuestionsException()
        : base([Errors.Questions.ToManyQuestions()])
    {

    }
}
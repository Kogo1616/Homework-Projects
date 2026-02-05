using DevQuestions.Application.Exceptions;
using Shared;

namespace DevQuestions.Application.Questions.Exceptions;

public class QuestionNotFoundException : NotFoundException
{
    protected QuestionNotFoundException(Error error)
        : base(error)
    {
    }
}
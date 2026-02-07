using Shared;

namespace DevQuestions.Application.Questions.Fails;

public partial class Errors
{
    public static class Questions
    {
        public static Error ToManyQuestions() =>
            Error.Failure("question.too.many", "User can not open more three questions");
    }
}
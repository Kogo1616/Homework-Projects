using DevQuestions.Contracts.Questions;
using FluentValidation;

namespace DevQuestions.Application.Questions;

public class CreateQuestionValidator : AbstractValidator<CreateQuestionRequest>
{
    public CreateQuestionValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500).WithMessage("Title not valid");
        RuleFor(x => x.Text).NotEmpty().MaximumLength(500).WithMessage("Text not valid");
        RuleFor(x => x.UserId).NotEmpty();
        RuleForEach(x => x.TagsIds).NotEmpty();
    }
}
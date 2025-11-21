using FluentValidation;

namespace TaskList.WebApi.Dtos;

public class WorkItemDtoValidator : AbstractValidator<WorkItemDto>
{
    public WorkItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(500);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
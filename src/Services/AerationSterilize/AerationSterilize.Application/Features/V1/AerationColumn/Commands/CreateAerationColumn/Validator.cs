using FluentValidation;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Commands.CreateAerationColumn;

internal sealed class Validator : AbstractValidator<CreateAerationColumnCommand>
{
    public Validator()
    {
        RuleFor(x => x.ColumnName)
            .NotEmpty().WithMessage("ColumnName is required.")
            .MaximumLength(10).WithMessage("ColumnName must not exceed 10 characters.");
    }
}

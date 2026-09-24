using FluentValidation;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Commands.CreateAerationPosition;

internal sealed class Validator : AbstractValidator<CreateAerationPositionCommand>
{
    public Validator()
    {
        RuleFor(x => x.PositionCode)
            .NotEmpty().WithMessage("PositionCode is required.")
            .MaximumLength(10).WithMessage("PositionCode must not exceed 10 characters.");

        RuleFor(x => x.AerationColumnId)
            .GreaterThan(0).WithMessage("AerationColumnId must be greater than 0.");
    }
}

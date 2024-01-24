using Contracts.Shared;
using FluentValidation;
using FluentValidation.Results;

namespace Contracts
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        protected override void RaiseValidationException(ValidationContext<T> context, ValidationResult result)
            => throw new BusinessException(result.Errors?.Select(x => x.ErrorMessage)?.ToList());
    }
}

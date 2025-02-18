using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace RecipeApp.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
{
    #region Feilds
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    #endregion

    #region Constructors
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    #endregion

    #region Handle Functions
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            ValidationContext<TRequest> context = new(request);
            ValidationResult[] validationResults = await
                Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );
            List<ValidationFailure> failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                string? message = failures.Select(x => x.ErrorMessage).FirstOrDefault();
                if (message is null)
                    throw new ArgumentNullException(nameof(message));

                throw new ValidationException(message);
            }
        }
        return await next();
    }
    #endregion

}
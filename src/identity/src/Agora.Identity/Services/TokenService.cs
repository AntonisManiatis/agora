using Agora.Identity.Core;

using ErrorOr;

using FluentValidation;

namespace Agora.Identity.Services;

public interface ITokenService
{
    Task<ErrorOr<string>> IssueAsync(IssueTokenCommand request);
}

internal class TokenService : ITokenService
{
    private readonly IValidator<IssueTokenCommand> validator;
    private readonly IUserRepository userRepository;
    private readonly IJwtGenerator tokenGenerator;

    public TokenService(
        IValidator<IssueTokenCommand> validator,
        IUserRepository userRepository,
        IJwtGenerator tokenGenerator)
    {
        this.validator = validator;
        this.userRepository = userRepository;
        this.tokenGenerator = tokenGenerator;
    }

    public async Task<ErrorOr<string>> IssueAsync(IssueTokenCommand request)
    {
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            return result.Errors
                .ConvertAll(err => Error.Validation(err.PropertyName, err.ErrorMessage));
        }

        var user = await userRepository.GetUserByEmail(request.Email);
        if (user is null)
        {
            return Errors.InvalidCredentials;
        }

        // ! should check against a hash of the password but it's not implemented in registration yet.
        if (!user.Password.Equals(request.Password))
        {
            return Errors.InvalidCredentials;
        }

        return tokenGenerator.GenerateToken(user);
    }
}
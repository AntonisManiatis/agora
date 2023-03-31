using Agora.Identity.Core;

using ErrorOr;

using FluentValidation;

namespace Agora.Identity.Services;

public interface IUserService
{
    Task<ErrorOr<Guid>> RegisterUserAsync(RegisterCommand command);
}

internal class UserService : IUserService
{
    private readonly IValidator<RegisterCommand> validator;
    private readonly IUserRepository userRepository;

    public UserService(
        IValidator<RegisterCommand> validator,
        IUserRepository userRepository)
    {
        this.validator = validator;
        this.userRepository = userRepository;
    }

    public async Task<ErrorOr<Guid>> RegisterUserAsync(RegisterCommand command)
    {
        // TODO: Find a trick to specify it one for all services?
        var result = await validator.ValidateAsync(command);
        if (!result.IsValid)
        {
            return result.Errors
                .ConvertAll(err => Error.Validation(err.PropertyName, err.ErrorMessage));
        }

        var exists = await userRepository.ExistsAsync(command.Email);
        if (exists)
        {
            // ! Okay this could dangerous, we shouldn't reveal this on a web API level
            // ! I assume that the layer above always handles this saying okay check your email!
            // TODO: add message.
            return Error.Conflict();
        }

        var user = new User();
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.Email = command.Email;
        // ! Naive approach, should hash passwords, etc.
        user.Password = command.Password;

        var userId = await userRepository.AddAsync(user);

        // ? Is there a domain/integration event we'd need to publish here?
        // Most likely yes! We'd need to send a verification email and that can be done asynchronously.

        return userId;
    }
}
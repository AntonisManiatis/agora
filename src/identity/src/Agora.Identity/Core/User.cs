namespace Agora.Identity.Core;

// ! shouldn't be public..
public sealed class User
{
    // ! draft, not all of those need to be public.
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
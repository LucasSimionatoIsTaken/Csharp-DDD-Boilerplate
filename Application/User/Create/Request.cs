using MediatR;

namespace Application.User.Create;

public class Request : IRequest<Response>
{
    public Request(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
}
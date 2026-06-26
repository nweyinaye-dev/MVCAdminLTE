using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSS.Domain.Features.Auth;

class AuthModels
{
}
public class AuthRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class AuthResponse
{
    public string AccessToken { get; set; } = "";
    public string Role { get; set; } = "";
    public List<string> Permissions { get; set; } = new();
}

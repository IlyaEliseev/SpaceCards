using Microsoft.AspNetCore.Authentication;

public class TestAuthenticationOption : AuthenticationSchemeOptions
{
    public string Realm = "Test";
}
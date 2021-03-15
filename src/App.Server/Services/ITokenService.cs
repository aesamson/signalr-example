namespace App.Server.Services
{
    public interface ITokenService
    {
        string IssueToken(string nick);
    }
}
using System.Threading.Tasks;

namespace App.Client.Services.Contracts
{
    internal interface ITokenAccessor
    {
        Task<string> Fetch();
    }
}
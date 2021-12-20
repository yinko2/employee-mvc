using System.Threading.Tasks;
using IdentityModel.Client;

namespace EmployeeMVC.Services
{
  public interface ITokenService
  {
    Task<TokenResponse> GetToken(string scope);
  }
}
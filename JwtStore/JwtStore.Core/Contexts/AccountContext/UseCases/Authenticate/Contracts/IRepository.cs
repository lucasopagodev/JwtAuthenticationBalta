using JwtStore.Core.Contexts.AccountContext.Entities;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public interface IRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}

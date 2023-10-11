using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IRepository _repository;

    public Handler(IRepository repository) => _repository = repository;

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region Valida a requisição

        try
        {
            var res = Specification.Ensure(request);
            if (!res.IsValid)
                return new Response("Requisição inválida", 400, res.Notifications);
        }
        catch (System.Exception)
        {
            return new Response("Não foi possível validar sua requisição", 500);
        }

        #endregion

        #region Recupera o perfil

        User? user;

        try
        {
            user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null)
                return new Response("Perfil não econtrado", 404);
        }
        catch (System.Exception)
        {
            return new Response("Não foi possível recuperar seu perfil", 500);
        }

        #endregion

        #region Checa se a senha é válida

        if (!user.Password.Challenge(request.Password))
            return new Response("Usuário ou senha inválidos", 400);

        #endregion

        #region Checa se a conta está verificada

        try
        {
            if (!user.Email.Verification.IsActive)
                return new Response("Conta inativa", 400);
        }
        catch (System.Exception)
        {
            return new Response("Não foi possível verificar seu perfil", 500);
        }

        #endregion

        #region Retorna os dados

        try
        {
            var data =  new ResponseData
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Roles = Array.Empty<string>()
            };

            return new Response(string.Empty, data);
        }
        catch (System.Exception)
        {
            return new Response("Não foi possível obter os dados do perfil", 500);
        }

        #endregion
    }
}

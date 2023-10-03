using System.Drawing;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JwtStore.Core.Contexts.AccountContext.ValueObjects;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create;

public class Handler
{
    private readonly IRepository _repository;
    private readonly IService _service;

    public Handler(IRepository repository, IService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region Valida a requisição

        try
        {
            var res = Specification.Ensure(request);
            if (!res.IsValid)
                return new Response("Requisição inválida", 400, res.Notifications);

            
        }
        catch (Exception e) // Criar um InvalidRequestException
        {
            return new Response("Não foi possível validar sua requisição", 500);
        }

        #endregion

        #region Gera os objetos

        Email email;
        Password password;
        User user;

        try
        {
            email = new(request.Email);
            password = new(request.Password);
            user = new(request.Name, email, password);
        }
        catch (Exception ex)
        {
            return new Response(ex.Message, 400);
        }

        #endregion

        #region Verifica a existência do usuário

        try
        {        
            var exists = await _repository.AnyAsync(request.Email, cancellationToken);
            if (exists)
                return new Response("Este e-mail já está em uso", 400);
        }
        catch
        {
            return new Response("Falha ao verificar e-mail cadastrado", 500);
        }


        #endregion

        #region Persiste os dados

        try
        {
            await _repository.SaveAsync(user, cancellationToken);
        }
        catch
        {
            return new Response("Falha ao persitir dados", 500);
        }

        #endregion

        #region Envia e-mail de ativação

        try
        {
            await _service.SendVerificationEmailAsync(user, cancellationToken);
        }
        catch (System.Exception)
        {
            
            throw;
        }

        #endregion
    }
}

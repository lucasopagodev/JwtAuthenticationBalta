using JwtStore.Core.Contexts.AccountContext.UseCases.Create;
using MediatR;

namespace JwtStore.Api;

public static class AccountContextExtension
{
    public static void AddAccountContext(this WebApplicationBuilder builder)
    {
        #region Create

        builder.Services.AddTransient<
            JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts.IRepository,
            JwtStore.Infra.Contexts.AccountContext.UseCases.Create.Repository
        >();

        builder.Services.AddTransient<
            JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts.IService,
            JwtStore.Infra.Contexts.AccountContext.UseCases.Create.Service
        >();

        #endregion

        #region Authenticate

        builder.Services.AddTransient<
            JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts.IRepository,
            JwtStore.Infra.Contexts.AccountContext.UseCases.Authenticate.Repository
        >();

        #endregion
    }

    public static void MapAccountEndpoints(this WebApplication app)
    {
        #region Create

        app.MapPost("api/v1/users", async () => async (JwtStore.Core.Contexts.AccountContext.UseCases.Create.Request request, 
            IRequestHandler<JwtStore.Core.Contexts.AccountContext.UseCases.Create.Request, 
            JwtStore.Core.Contexts.AccountContext.UseCases.Create.Response> handler) =>
        {
            var result = await handler.Handle(request, new CancellationToken());
            return result.IsSuccess ? Results.Created($"api/v1/users/{result.Data?.Id}", result) : Results.Json(result, statusCode: result.Status);
        });

        #endregion

        #region Authenticate

        app.MapPost("api/v1/authenticate", async () => async (JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Request request, 
            IRequestHandler<JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Request, 
            JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Response> handler) =>
        {
            var result = await handler.Handle(request, new CancellationToken());
            return result.IsSuccess ? Results.Ok(result) : Results.Json(result, statusCode: result.Status);
        });

        #endregion
    }
}
﻿using SiteVantagePro_API.Application.TodoLists.Commands.CreateTodoList;
using SiteVantagePro_API.Application.TodoLists.Commands.DeleteTodoList;
using SiteVantagePro_API.Application.TodoLists.Commands.UpdateTodoList;
using SiteVantagePro_API.Application.TodoLists.Queries.GetTodos;
//using SiteVantagePro_API.WebUI.Shared.TodoLists;

namespace SiteVantagePro_API.Web.Endpoints;

public class TodoLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(nameof(TodoLists))
            .RequireAuthorization("api")
            .MapGet(GetTodoLists)
            .MapPost(CreateTodoList)
            .MapPut(UpdateTodoList, "{id}")
            .MapDelete(DeleteTodoList, "{id}");
    }

    public async Task<TodosVm> GetTodoLists(ISender sender)
    {
        return await sender.Send(new GetTodosQuery());
    }

    public async Task<int> CreateTodoList(ISender sender, CreateTodoListCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<IResult> UpdateTodoList(ISender sender, int id, UpdateTodoListCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteTodoList(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoListCommand(id));
        return Results.NoContent();
    }
}
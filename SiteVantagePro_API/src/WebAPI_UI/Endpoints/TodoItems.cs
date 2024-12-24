﻿using SiteVantagePro_API.Application.Common.Models;
using SiteVantagePro_API.Application.TodoItems.Commands.CreateTodoItem;
using SiteVantagePro_API.Application.TodoItems.Commands.DeleteTodoItem;
using SiteVantagePro_API.Application.TodoItems.Commands.UpdateTodoItem;
using SiteVantagePro_API.Application.TodoItems.Commands.UpdateTodoItemDetail;
using SiteVantagePro_API.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace SiteVantagePro_API.Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(nameof(TodoItems))
            .RequireAuthorization("api")
            .MapGet(GetTodoItemsWithPagination)
            .MapPost(CreateTodoItem)
            .MapPut(UpdateTodoItem, "{id}")
            .MapPut(UpdateTodoItemDetail, "UpdateTodoItemDetail/{id}")
            .MapDelete(DeleteTodoItem, "{id}");
    }

    public async Task<PaginatedList<TodoItemBriefDto>> GetTodoItemsWithPagination(ISender sender, [AsParameters] GetTodoItemsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }

    public async Task<int> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<IResult> UpdateTodoItem(ISender sender, int id, UpdateTodoItemCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> UpdateTodoItemDetail(ISender sender, int id, UpdateTodoItemDetailCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteTodoItem(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));
        return Results.NoContent();
    }
}

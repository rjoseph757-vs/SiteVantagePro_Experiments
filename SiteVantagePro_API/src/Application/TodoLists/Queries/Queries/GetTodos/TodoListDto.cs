using SiteVantagePro_API.Application.Common.Mappings;
using SiteVantagePro_API.Domain.Entities;

namespace SiteVantagePro_API.Application.TodoLists.Queries.GetTodos;

public class TodoListDto : IMapFrom<TodoList>
{
    public TodoListDto()
    {
        Items = Array.Empty<TodoItemDto>();
    }

    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TodoList, TodoListDto>();
    }
}


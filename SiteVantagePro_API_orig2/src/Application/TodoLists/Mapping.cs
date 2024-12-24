using SiteVantagePro_API.Domain.Entities;
using SiteVantagePro_API.WebUI.Shared.TodoLists;

namespace SiteVantagePro_API.Application.TodoLists;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<TodoList, TodoListDto>();
        CreateMap<TodoItem, TodoItemDto>();
    }
}

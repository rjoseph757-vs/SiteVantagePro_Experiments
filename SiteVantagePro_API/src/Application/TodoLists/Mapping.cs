using SiteVantagePro_API.Application.TodoLists.Queries.GetTodos;
using SiteVantagePro_API.Domain.Entities;
namespace SiteVantagePro_API.Application.Common.Models;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<TodoList, TodoListDto>();
        CreateMap<TodoItem, TodoItemDto>();
    }
}

using SiteVantagePro_API.WebUI.Shared.Common;

namespace SiteVantagePro_API.WebUI.Shared.TodoLists;
public class TodosVm
{
    public List<LookupDto> PriorityLevels { get; set; } = new();

    public List<TodoListDto> Lists { get; set; } = new();
}

﻿using SiteVantagePro_API.Application.Common.Exceptions;
using SiteVantagePro_API.Application.TodoItems.Commands.CreateTodoItem;
using SiteVantagePro_API.Application.TodoLists.Commands.CreateTodoList;
using SiteVantagePro_API.Domain.Entities;

namespace SiteVantagePro_API.Application.FunctionalTests.TodoItems.Commands;

using static Testing;

public class CreateTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoItemCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var command = new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Tasks"
        };

        var itemId = await SendAsync(command);

        var item = await FindAsync<TodoItem>(itemId);

        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Title.Should().Be(command.Title);
        item.CreatedBy.Should().Be(userId);
        item.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.UpdatedBy.Should().Be(userId);
        item.UpdatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}

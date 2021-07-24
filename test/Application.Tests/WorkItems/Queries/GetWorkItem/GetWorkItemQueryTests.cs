﻿using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Queries.Common;
using TaskManager.Api.Application.WorkItems.Queries.GetWorkItem;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.WorkItems.Queries.GetWorkItem
{
    public class GetWorkItemQueryTests : TestBase
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should return the specified work item")]
        public async Task Should_Get_WorkItem(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            List<WorkItem> workItems,
            GetWorkItemQueryHandler sut,
            GetWorkItemQuery request
          )
        {
            //Arrange
            var selected = PickRandomElement(workItems);
            request.Id = selected.Id;
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should().BeEquivalentTo(new WorkItemDto()
            {
                Id = selected.Id,
                Name = selected.Name,
                Description = selected.Description,
                AssignedTo = selected.AssignedTo?.Id,
            }, "because it should return the equivalent of the requested work item as a DTO");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should throw NotFoundException if the WorkItem doesn't exist")]
        public void Should_Throw_WorkItem_Not_Found(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            List<WorkItem> workItems,
            GetWorkItemQueryHandler sut,
            GetWorkItemQuery request
            )
        {
            //Arrange
            var toRemove = workItems.First();
            request.Id = toRemove.Id;
            workItems.Remove(toRemove);
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            Func<Task<WorkItemDto>> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().Throw<NotFoundException>("because the specified WorkItem does not exist");
        }
    }
}

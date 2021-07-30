using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Commands.CreateWorkItem;
using TaskManager.Api.Application.WorkItems.Commands.DeleteWorkItem;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.WorkItems.Commands
{
    public class DeleteWorkItemCommandTests : TestBase
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should remove a specified WorkItem")]
        public async Task Should_Remove_WorkItem(
           CancellationTokenSource cancellationSource,
           [Frozen] Mock<IApplicationDbContext> applicationDbContext,
           List<WorkItem> workItems,
           DeleteWorkItemCommandHandler sut,
           DeleteWorkItemCommand request
           )
        {
            //Arrange
            request.Id = workItems.First().Id;

            var workItemSet = workItems
                .AsQueryable()
                .BuildMockDbSet()
                .Object;

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItemSet);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            workItems.Should().NotContain(workItem => workItem.Id == request.Id);
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should persist changes to database")]
        public async Task Should_Persist_To_Database(
           CancellationTokenSource cancellationSource,
           [Frozen] Mock<IApplicationDbContext> applicationDbContext,
           List<WorkItem> workItems,
           DeleteWorkItemCommandHandler sut,
           DeleteWorkItemCommand request
           )
        {
            //Arrange
            request.Id = workItems.First().Id;

            var workItemSet = workItems
                .AsQueryable()
                .BuildMockDbSet()
                .Object;

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItemSet);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            applicationDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), "Should persist changes to database");
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
           DeleteWorkItemCommandHandler sut,
           DeleteWorkItemCommand request
           )
        {
            //Arrange
            var toRemove = workItems.First();
            request.Id = toRemove.Id;
            workItems.Remove(toRemove);

            var workItemSet = workItems
                .AsQueryable()
                .BuildMockDbSet()
                .Object;

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItemSet);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            Func<Task<Unit>> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().Throw<NotFoundException>("because the specified WorkItem does not exist");
        }
    }
}

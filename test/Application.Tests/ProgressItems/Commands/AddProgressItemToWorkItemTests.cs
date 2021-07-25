using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.ProgressItems.Commands.AddProgressItemToWorkItem;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.ProgressItems.Commands
{
    public class AddProgressItemToWorkItemTests : TestBase
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should create progress item")]
        public async Task Should_Create_ProgressItem(
            List<WorkItem> workItems,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AddProgressItemToWorkItemCommandHandler sut,
            AddProgressItemToWorkItemCommand request)
        {
            //Arrange
            var progressItems = new List<ProgressItem>();
            request.WorkItemId = PickRandomElement(workItems, out int workItemIndex).Id;

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.ProgressItems).Returns(progressItems);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(progressItems.Count);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            progressItems.First().Name.Should().Be(request.Name, "because it should have the same name");
            progressItems.First().Complete.Should().Be(request.Complete, "because it should have the same completed value");
        }
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should return the newly created ProgressItem's Id")]
        public async Task Should_Return_Created_Id(
            List<WorkItem> workItems,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AddProgressItemToWorkItemCommandHandler sut,
            AddProgressItemToWorkItemCommand request,
            int progressItemGeneratedId)
        {
            //Arrange
            request.WorkItemId = PickRandomElement(workItems, out int workItemIndex).Id;
            var progressItems = new List<ProgressItem>();

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.ProgressItems).Returns(progressItems);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(progressItems.Count)
                .Callback(() =>
                {
                    var item = progressItems.Last();
                    item.Id = progressItemGeneratedId;
                }); ;

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should().Be(progressItemGeneratedId, "because it should return the newly created progress item's id");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should persist to database")]
        public async Task Should_Persist_To_Database(
            List<WorkItem> workItems,
            List<ProgressItem> progressItems,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AddProgressItemToWorkItemCommandHandler sut,
            AddProgressItemToWorkItemCommand request)
        {
            //Arrange
            request.WorkItemId = PickRandomElement(workItems, out int workItemIndex).Id;

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.ProgressItems).Returns(progressItems);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(progressItems.Count);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            applicationDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()),"Should persist changes to database");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should be mapped to specified WorkItem")]
        public async Task Should_Be_Mapped_To_WorkItem(
            List<WorkItem> workItems,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AddProgressItemToWorkItemCommandHandler sut,
            AddProgressItemToWorkItemCommand request)
        {
            //Arrange
            var progressItems = new List<ProgressItem>();
            request.WorkItemId = PickRandomElement(workItems, out int workItemIndex).Id;

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.ProgressItems).Returns(progressItems);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(progressItems.Count);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            progressItems.First().WorkItem.Id.Should().Be(request.WorkItemId, "because the the ProgressItem should have been assigned to the specified WorkItem");
        }


    }
}

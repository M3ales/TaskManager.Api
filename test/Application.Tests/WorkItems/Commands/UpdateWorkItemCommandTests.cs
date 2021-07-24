using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Queries.Common;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.WorkItems.Commands.UpdateWorkItem
{
    public class UpdateWorkItemCommandTests : TestBase
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should update WorkItem")]
        public async Task Should_Update_WorkItem(
              CancellationTokenSource cancellationSource,
              [Frozen] Mock<IApplicationDbContext> applicationDbContext,
              List<WorkItem> workItems,
              UpdateWorkItemCommandHandler sut,
              UpdateWorkItemCommand request
              )
        {
            //Arrange
            request.Updated.Id = PickRandomElement(workItems).Id;
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.Updated.AssignedTo ?? 0 } });
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(applicationDbContext.Object.WorkItems.Count);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should().BeEquivalentTo(request.Updated, "because the returned value should be the same as the requested update");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should throw a NotFoundException if the specified WorkItem Id does not exist")]
        public void Should_Throw_WorkItem_Not_Found(
               CancellationTokenSource cancellationSource,
               [Frozen] Mock<IApplicationDbContext> applicationDbContext,
               UpdateWorkItemCommandHandler sut,
               UpdateWorkItemCommand request
               )
        {
            //Arrange
            applicationDbContext.Setup(context => context.WorkItems).Returns(new List<WorkItem>() { new WorkItem() { Id = request.Updated.Id + 1 } });
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.Updated.AssignedTo ?? 0 } });

            //Act
            Func<Task<ShallowWorkItemDto>> result = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should().Throw<NotFoundException>("because the requested team member does not exist");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should throw a NotFoundException if the specified TeamMember Id does not exist")]
        public void Should_Throw_TeamMember_Not_Found(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            UpdateWorkItemCommandHandler sut,
            UpdateWorkItemCommand request
            )
        {
            //Arrange
            applicationDbContext.Setup(context => context.WorkItems).Returns(new List<WorkItem>());
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.Updated.AssignedTo + 1 ?? 0 } });

            //Act
            Func<Task<ShallowWorkItemDto>> result = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should().Throw<NotFoundException>("because the requested team member does not exist");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should persist changes to the database")]
        public async Task Should_Persist_To_Database(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            List<WorkItem> workItems,
            UpdateWorkItemCommandHandler sut,
            UpdateWorkItemCommand request
            )
        {
            //Arrange
            request.Updated.Id = PickRandomElement(workItems).Id;
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.Updated.AssignedTo ?? 0 } });
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(applicationDbContext.Object.WorkItems.Count);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            applicationDbContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), "Should persist work item to database storage");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should accept null team member")]
        public void Should_Accept_Null_TeamMember(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            List<WorkItem> workItems,
            UpdateWorkItemCommandHandler sut,
            UpdateWorkItemCommand request
            )
        {
            //Arrange
            request.Updated.Id = PickRandomElement(workItems).Id;
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.Updated.AssignedTo ?? 0 } });
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(applicationDbContext.Object.WorkItems.Count);

            //Act
            Func<Task<ShallowWorkItemDto>> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().NotThrow("because it should accept null assignedTo requests");
        }
    }
}

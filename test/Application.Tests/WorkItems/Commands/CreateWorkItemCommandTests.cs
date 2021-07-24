using AutoFixture.Xunit2;
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
using TaskManager.Api.Application.WorkItems.Commands.CreateWorkItem;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.WorkItems.Commands
{
    public class CreateWorkItemCommandTests : TestBase
    {
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
            CreateWorkItemCommandHandler sut,
            CreateWorkItemCommand request
            )
        {
            //Arrange
            applicationDbContext.Setup(context => context.WorkItems).Returns(new List<WorkItem>());
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.AssignedTo + 1 ?? 0 } });

            //Act
            Func<Task<int>> result = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should().Throw<NotFoundException>("because the requested team member does not exist");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should return the Id assigned by the database to the caller")]
        public async Task Should_Return_Created_WorkItem_Id(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            CreateWorkItemCommandHandler sut,
            CreateWorkItemCommand request,
            int workItemGeneratedId
            )
        {
            //Arrange
            var workItems = new List<WorkItem>();
            // Add some variance to occasionally generate nulls
            if (request.AssignedTo % 16 == 0)
            {
                request.AssignedTo = null;
            }
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.AssignedTo ?? 0 } });
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(applicationDbContext.Object.WorkItems.Count)
                .Callback(() =>
                {
                    workItems[0].Id = workItemGeneratedId;
                });

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should().Be(workItemGeneratedId, "because that is the generated id provided by the underlying database to the saved work item");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should create only a single work item")]
        public async Task Should_Only_Create_One_WorkItem(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            CreateWorkItemCommandHandler sut,
            CreateWorkItemCommand request
            )
        {
            //Arrange
            var workItems = new List<WorkItem>();
            // Add some variance to occasionally generate nulls
            if (request.AssignedTo % 16 == 0)
            {
                request.AssignedTo = null;
            }
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.AssignedTo ?? 0 } });
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(applicationDbContext.Object.WorkItems.Count);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            workItems.Should().ContainSingle("because only a single item should have been added");
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
            CreateWorkItemCommandHandler sut,
            CreateWorkItemCommand request
            )
        {
            //Arrange
            var workItems = new List<WorkItem>();
            // Add some variance to occasionally generate nulls
            if (request.AssignedTo % 16 == 0)
            {
                request.AssignedTo = null;
            }
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.AssignedTo ?? 0 } });
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
        [Theory(DisplayName = "Should accept Assigned To of Null")]
        public void Should_Accept_Null_AssignedTo(
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            CreateWorkItemCommandHandler sut,
            CreateWorkItemCommand request
            )
        {
            //Arrange
            request.AssignedTo = null;
            var workItems = new List<WorkItem>();
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(new List<TeamMember>() { new TeamMember() { Id = request.AssignedTo ?? 0 } });
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(applicationDbContext.Object.WorkItems.Count);

            //Act
            Func<Task<int>> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().NotThrow("because it should accept null assignedTo requests");
        }
    }
}

using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.Tasks.Commands.AssignTaskToTeamMember;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.Tasks.Commands.AssignTaskToTeamMember
{
    public class AssignTaskToTeamMemberTest : TestBase
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Assign Tasks to Team Members")]
        public async Task Should_Assign_Tasks_To_Team_Members(
            List<TaskItem> tasks,
            List<TeamMember> teamMembers,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AssignTaskToTeamMemberCommandHandler sut)
        {
            //Arrange
            var request = new AssignTaskToTeamMemberCommand()
            {
                TaskId = PickRandomElement(tasks, out int taskIndex).Id,
                TeamMemberId = PickRandomElement(teamMembers, out int teamMemberIndex).Id
            };
            applicationDbContext.Setup(context => context.TaskItems).Returns(tasks);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(teamMembers);
            applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0))
                .Verifiable("Must persist the changes to database");

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            tasks[taskIndex].AssignedTo.Should().Be(teamMembers[teamMemberIndex], "because you should assign the team member to the task");
            applicationDbContext.Verify();
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should Throw Team Member Not Found Exception")]
        public void Should_Throw_If_TeamMember_Not_Found(
            List<TaskItem> tasks,
            List<TeamMember> teamMembers,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AssignTaskToTeamMemberCommandHandler sut)
        {
            //Arrange
            var teamMemberToRemove = PickRandomElement(teamMembers);

            var request = new AssignTaskToTeamMemberCommand()
            {
                TaskId = PickRandomElement(tasks).Id,
                TeamMemberId = teamMemberToRemove.Id
            };

            //Remove the chosen element from the list to ensure it will fail
            teamMembers.Remove(teamMemberToRemove);

            applicationDbContext.Setup(context => context.TaskItems).Returns(tasks);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(teamMembers);

            //Act
            Func<Task> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().Throw<NotFoundException>("because the team member does not exist");
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should Throw Task Not Found Exception")]
        public void Should_Throw_If_Task_Not_Found(
            List<TaskItem> tasks,
            List<TeamMember> teamMembers,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AssignTaskToTeamMemberCommandHandler sut)
        {
            //Arrange
            var taskToRemove = PickRandomElement(tasks);

            var request = new AssignTaskToTeamMemberCommand()
            {
                TaskId = taskToRemove.Id,
                TeamMemberId = PickRandomElement(teamMembers).Id
            };

            //Remove the chosen element from the list to ensure it will fail
            tasks.Remove(taskToRemove);

            applicationDbContext.Setup(context => context.TaskItems).Returns(tasks);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(teamMembers);

            //Act
            Func<Task> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().Throw<NotFoundException>("because the task does not exist");
        }
    }
}

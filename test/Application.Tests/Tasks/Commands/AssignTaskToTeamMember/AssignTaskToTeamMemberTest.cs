using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.Tasks.Commands.AssignTaskToTeamMember;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.Tasks.Commands.AssignTaskToTeamMember
{
    public class AssignTaskToTeamMemberTest : TestBase
    {
        [AutoMoqData]
        [Theory(DisplayName = "Assign Tasks to Team Members")]
        public async Task Should_Assign_Tasks_To_Team_Members(
            [Frozen] List<TaskItem> tasks, 
            [Frozen] List<TeamMember> teamMembers, 
            [Frozen] CancellationTokenSource cancellationSource)
        {
            //Arrange
            var request = new AssignTaskToTeamMemberCommand()
            {
                TaskId = PickRandomElement(tasks, out int taskIndex).Id,
                TeamMemberId = PickRandomElement(teamMembers, out int teamMemberIndex).Id
            };

            var _applicationDbContext = new Mock<IApplicationDbContext>();
            _applicationDbContext
                .SetupGet(context => context.TaskItems)
                .Returns(tasks)
                .Verifiable("Must get the task");
            _applicationDbContext
                .SetupGet(context => context.TeamMembers)
                .Returns(teamMembers)
                .Verifiable("Must get the team member");
            _applicationDbContext.Setup(x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0))
                .Verifiable("Must save the resulting change");

            var sut = new AssignTaskToTeamMemberCommandHandler(_applicationDbContext.Object);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            tasks[taskIndex].AssignedTo.Should().Be(teamMembers[teamMemberIndex], "because you should assign the team member to the task");
            _applicationDbContext.Verify();
        }
    }
}

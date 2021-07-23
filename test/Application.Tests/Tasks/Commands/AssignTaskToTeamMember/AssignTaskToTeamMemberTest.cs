using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [Fact]
        public async Task Should_Assign_Tasks_To_Team_Members()
        {
            //Arrange
            // TODO: replace hardcoded values with an autofixture to generate random values
            var teamMembers = new List<TeamMember>() { new TeamMember() { Id = 0 } };
            var tasks = new List<TaskItem>() { new TaskItem() { Id = 0 } };
            var request = new AssignTaskToTeamMemberCommand()
            {
                TaskId = 0,
                TeamMemberId = 0
            };
            var cancellationSource = new CancellationTokenSource();

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
            tasks.Count.Should().Be(1, "because you shouldn't be adding any new tasks");
            teamMembers.Count.Should().Be(1, "because you shouldn't be adding any new team members");
            tasks[0].AssignedTo.Should().Be(teamMembers[0], "because you should assign the team member to the task");
            _applicationDbContext.Verify();
        }
    }
}

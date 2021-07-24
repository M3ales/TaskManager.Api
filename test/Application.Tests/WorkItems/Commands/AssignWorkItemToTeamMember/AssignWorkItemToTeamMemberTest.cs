using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Commands.AssignWorkItemToTeamMember;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.WorkItems.Commands.AssignWorkItemToTeamMember
{
    public class AssignWorkItemToTeamMemberTest : TestBase
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Assign WorkItems to Team Members")]
        public async Task Should_Assign_WorkItems_To_Team_Members(
            List<WorkItem> workItems,
            List<TeamMember> teamMembers,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AssignWorkItemToTeamMemberCommandHandler sut)
        {
            //Arrange
            var request = new AssignWorkItemToTeamMemberCommand()
            {
                WorkItemId = PickRandomElement(workItems, out int workItemIndex).Id,
                TeamMemberId = PickRandomElement(teamMembers, out int teamMemberIndex).Id
            };
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(teamMembers);
            applicationDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0))
                .Verifiable("Must persist the changes to database");

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            workItems[workItemIndex].AssignedTo.Should().Be(teamMembers[teamMemberIndex], "because you should assign the team member to the workitem");
            applicationDbContext.Verify();
        }

        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should Throw Team Member Not Found Exception")]
        public void Should_Throw_If_TeamMember_Not_Found(
            List<WorkItem> workItems,
            List<TeamMember> teamMembers,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AssignWorkItemToTeamMemberCommandHandler sut)
        {
            //Arrange
            var teamMemberToRemove = PickRandomElement(teamMembers);

            var request = new AssignWorkItemToTeamMemberCommand()
            {
                WorkItemId = PickRandomElement(workItems).Id,
                TeamMemberId = teamMemberToRemove.Id
            };

            //Remove the chosen element from the list to ensure it will fail
            teamMembers.Remove(teamMemberToRemove);

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
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
        [Theory(DisplayName = "Should Throw WorkItem Not Found Exception")]
        public void Should_Throw_If_WorkItem_Not_Found(
            List<WorkItem> workItems,
            List<TeamMember> teamMembers,
            CancellationTokenSource cancellationSource,
            [Frozen] Mock<IApplicationDbContext> applicationDbContext,
            AssignWorkItemToTeamMemberCommandHandler sut)
        {
            //Arrange
            var workItemToRemove = PickRandomElement(workItems);

            var request = new AssignWorkItemToTeamMemberCommand()
            {
                WorkItemId = workItemToRemove.Id,
                TeamMemberId = PickRandomElement(teamMembers).Id
            };

            //Remove the chosen element from the list to ensure it will fail
            workItems.Remove(workItemToRemove);

            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);
            applicationDbContext.Setup(context => context.TeamMembers).Returns(teamMembers);

            //Act
            Func<Task> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().Throw<NotFoundException>("because the workitem does not exist");
        }
    }
}

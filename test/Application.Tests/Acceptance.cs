using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TaskManager.Api.Application.Tests
{

    // CQRS
    public interface IApplicationDbContext
    {
        ICollection<TeamMember> TeamMembers { get; }
        ICollection<TaskItem> TaskItems { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class AssignTaskToTeamMemberCommand : IRequest
    {
        public int TaskId { get; set; }
        public int TeamMemberId { get; set; }
    }

    public class AssignTaskToTeamMemberCommandHandler : IRequestHandler<AssignTaskToTeamMemberCommand, Unit>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public AssignTaskToTeamMemberCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Unit> Handle(AssignTaskToTeamMemberCommand request, CancellationToken cancellationToken)
        {
            // Going to pretend its hot path code and do a https://stackoverflow.com/questions/8663897/why-is-linq-wherepredicate-first-faster-than-firstpredicate
            var task = _applicationDbContext.TaskItems
                .Where(task => task.Id == request.TaskId)
                .First();
            var teamMember = _applicationDbContext.TeamMembers
                .Where(teamMember => teamMember.Id == request.TeamMemberId)
                .First();
            task.AssignedTo = teamMember;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public TeamMember AssignedTo { get; set; }
    }

    public class TeamMember
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// The requirements
    /// </summary>
    public class Acceptance
    {
        [Fact]
        public void Should_Modify_Progress_Of_Task()
        {
            //Arrange

            //Act

            //Assert
        }
        [Fact]
        public void Should_Show_Progress_Of_Task()
        {
            //Arrange

            //Act

            //Assert
        }
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
        [Fact]
        public void Should_Add_Tasks()
        {
            //Arrange

            //Act

            //Assert
        }
        [Fact]
        public void Should_Modify_Tasks()
        {
            //Arrange

            //Act

            //Assert
        }
        [Fact]
        public void Should_Remove_Tasks()
        {
            //Arrange

            //Act

            //Assert
        }
        [Fact]
        public void Should_Add_TeamMembers()
        {
            //Arrange

            //Act

            //Assert
        }
        [Fact]
        public void Should_Modify_TeamMembers()
        {
            //Arrange

            //Act

            //Assert
        }
        [Fact]
        public void Should_Remove_TeamMembers()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}

using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.WorkItems.Queries.Common;
using TaskManager.Api.Application.WorkItems.Queries.GetAllWorkItems;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.WorkItems.Queries.GetAllWorkItems
{
    public class GetAllWorkItemsQueryTests
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should all work items")]
        public async Task Should_Get_All_WorkItems(
               CancellationTokenSource cancellationSource,
               [Frozen] Mock<IApplicationDbContext> applicationDbContext,
               IMapper mapper,
               List<WorkItem> workItems,
               GetAllWorkItemsQueryHandler sut,
               GetAllWorkItemsQuery request
             )
        {
            //Arrange
            applicationDbContext.Setup(context => context.WorkItems).Returns(workItems);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            result.Should()
                .BeEquivalentTo(mapper
                    .ProjectTo<ShallowWorkItemDto>(
                     workItems.AsQueryable()).ToList(), "because it should return the equivalent of the requested work item as a DTO");
        }
    }
}

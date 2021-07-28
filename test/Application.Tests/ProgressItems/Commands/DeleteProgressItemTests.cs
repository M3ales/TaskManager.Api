using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Application.ProgressItems.Commands.DeleteProgressItem;
using TaskManager.Api.Domain.Entities;
using Xunit;

namespace TaskManager.Api.Application.Tests.WorkItems.Commands
{
    public class DeleteProgressItemTests
    {
        [AutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [InlineAutoMoqData]
        [Theory(DisplayName = "Should remove a specified Progress Item")]
        public async Task Should_Remove_ProgressItem(
           CancellationTokenSource cancellationSource,
           [Frozen] Mock<IApplicationDbContext> applicationDbContext,
           List<ProgressItem> progressItems,
           DeleteProgressItemCommandHandler sut,
           DeleteProgressItemCommand request
           )
        {
            //Arrange
            request.Id = progressItems.First().Id;
            applicationDbContext.Setup(context => context.ProgressItems).Returns(progressItems);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            var result = await sut.Handle(request, cancellationSource.Token);

            //Assert
            progressItems.Should().NotContain(progressItem => progressItem.Id == request.Id);
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
           List<ProgressItem> progressItems,
           DeleteProgressItemCommandHandler sut,
           DeleteProgressItemCommand request
           )
        {
            //Arrange
            request.Id = progressItems.First().Id;
            applicationDbContext.Setup(context => context.ProgressItems).Returns(progressItems);
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
        [Theory(DisplayName = "Should throw NotFoundException if the ProgressItem doesn't exist")]
        public void Should_Throw_ProgressItem_Not_Found(
           CancellationTokenSource cancellationSource,
           [Frozen] Mock<IApplicationDbContext> applicationDbContext,
           List<ProgressItem> progressItems,
           DeleteProgressItemCommandHandler sut,
           DeleteProgressItemCommand request
           )
        {
            //Arrange
            var toRemove = progressItems.First();
            request.Id = toRemove.Id;
            progressItems.Remove(toRemove);
            applicationDbContext.Setup(context => context.ProgressItems).Returns(progressItems);
            applicationDbContext.Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            Func<Task<Unit>> action = () => sut.Handle(request, cancellationSource.Token);

            //Assert
            action.Should().Throw<NotFoundException>("because the specified ProgressItem does not exist");
        }
    }
}

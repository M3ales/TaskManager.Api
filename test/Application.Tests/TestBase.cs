using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockQueryable.Moq;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.Tests
{
    public class TestBase
    {
        public T PickRandomElement<T>(ICollection<T> collection, out int index)
        {
            index = (int)Math.Floor(new Random((int)DateTime.UtcNow.Ticks).NextDouble() * collection.Count);
            return collection.ElementAt(index);
        }
        public T PickRandomElement<T>(ICollection<T> collection)
        {
            return PickRandomElement(collection, out int _);
        }
        public Mock<DbSet<TEntity>> BuildFunctionalDbSetMockFor<TEntity>(ICollection<TEntity> underlyingDataset) where TEntity : class
        {
            var workItemMock = underlyingDataset
                .AsQueryable()
                .BuildMockDbSet();

            workItemMock
                .Setup(set => set.Add(It.IsAny<TEntity>()))
                .Callback<TEntity>(input => underlyingDataset.Add(input));

            workItemMock
                .Setup(set => set.AsNoTracking())
                .Returns(underlyingDataset.AsQueryable());

            workItemMock
                .Setup(set => set.Remove(It.IsAny<TEntity>()))
                .Callback<TEntity>(input => underlyingDataset.Remove(input));

            workItemMock
                .Setup(set => set.AddRange(It.IsAny<TEntity[]>()))
                .Callback<TEntity[]>(input => {
                    foreach (var inputItem in input)
                    {
                        underlyingDataset.Add(inputItem);
                    }
                });

            workItemMock
                .Setup(set => set.RemoveRange(It.IsAny<TEntity[]>()))
                .Callback<TEntity[]>(input => {
                    foreach (var inputItem in input)
                    {
                        underlyingDataset.Remove(inputItem);
                    }
                });
            return workItemMock;
        }
    }
}

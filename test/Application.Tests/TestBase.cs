using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

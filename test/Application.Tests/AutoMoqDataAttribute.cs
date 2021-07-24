using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Application.Tests
{
    // From https://gist.github.com/Kusken/ea7b57658129346848e8fde1fb29109f
    /// <summary>
    /// Generates AutoMoq data for the method's arguments
    /// </summary>
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            return fixture;
        })
        { }
    }
}

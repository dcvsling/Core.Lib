using Core.Lib.Reflections.Observables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Core.Lib.Tests
{
    public class SubjectTests
    {
        [Fact]
        async public Task RegisterOnNextTest()
        {
            var expect = "expect";
            var isAssert = false;
            var services = new ServiceCollection().AddSubject().BuildServiceProvider();
            var mediator = services.GetRequiredService<IMediator>();
            mediator.Subscribe<TestContext>(ctx => isAssert = ctx.Msg == expect);
            mediator.Send(new TestContext { Msg = expect });

            Assert.True(isAssert);
        }

        [Fact]
        async public Task BroadcastErrorTest()
        {
            var expect = "error";
            var isAssert = false;
            var services = new ServiceCollection().AddSubject().BuildServiceProvider();

            var mediator = services.GetRequiredService<IMediator>();
            mediator.Subscribe<Exception>(_ => isAssert = true);
            mediator.Subscribe<TestContext>(ctx => throw new Exception());
            mediator.Send(new TestContext { Msg = expect });

            Assert.True(isAssert);
        }

        [Fact]
        async public Task DisposeCompleteTest()
        {
            var expect = "complete";
            var isAssert = false;
            var services = new ServiceCollection().AddSubject().BuildServiceProvider();
            var mediator = services.GetRequiredService<IMediator>();
            mediator.Subscribe<TestContext>(ctx => isAssert = ctx.Msg == expect);
            mediator.Send(new TestContext { Msg = expect });

            Assert.True(isAssert);
        }


        private class TestContext
        {
            public string Msg { get; set; }
        }
    }
}

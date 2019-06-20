using Core.Lib.Reflections.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Core.Lib.Tests
{
    public class MediatorTests
    {
        [Fact]
        async public Task RegisterMediatorTest()
        {
            var expect = "expect";
            var isAssert = false;
            var services = new ServiceCollection().AddMediator().BuildServiceProvider();
            var mediator = services.GetRequiredService<MediatorStore>();

            var disposer = mediator.Register<TestContext>("1", ctx => { Assert.Equal(expect, ctx.Msg); isAssert = true; return Task.CompletedTask; });
            await mediator.Send(new TestContext { Msg = expect }, "1");

            Assert.True(isAssert);
        }

        [Fact]
        async public Task BroadcastMediatorTest()
        {
            var expect = "expect";
            var isAssert = false;
            var services = new ServiceCollection().AddMediator().BuildServiceProvider();
            var mediator = services.GetRequiredService<MediatorStore>();

            var disposer = mediator.Register<TestContext>("1", ctx => { Assert.Equal(expect, ctx.Msg); isAssert = true; return Task.CompletedTask; });
            await mediator.Send(new TestContext { Msg = expect });

            Assert.True(isAssert);
        }

        [Fact]
        async public Task DisposeMediatorTest()
        {
            var expect = "expect";
            var isRun = false;
            var services = new ServiceCollection().AddMediator().BuildServiceProvider();
            var mediator = services.GetRequiredService<MediatorStore>();

            var disposer = mediator.Register<TestContext>("1", _ => Task.FromResult(isRun = true));
            disposer.Dispose();
            await mediator.Send(new TestContext { Msg = expect });

            Assert.False(isRun);
        }


        private class TestContext
        {
            public string Msg { get; set; }
        }
    }
}

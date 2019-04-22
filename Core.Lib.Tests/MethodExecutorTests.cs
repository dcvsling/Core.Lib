using Core.Lib.MethodExecutor;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ESI.Core.Tests
{
    public class MethodExecutorTests
    {
        [Fact]
        public void test_executor_members()
        {
            var isAssert = false;
            Action action = () => isAssert = true;
            var executor = MethodExecutor.CreateExecutor(action.Method);

            Assert.Equal(action.Method, executor.MethodInfo);
            Assert.Equal(action.Target.GetType().GetTypeInfo(), executor.TargetTypeInfo);
            Assert.True(action.Method.GetParameters().SequenceEqual(executor.MethodParameters));
            Assert.Equal(action.Method.ReturnType, executor.MethodReturnType);

        }

        private static bool Invoke(bool input) => input;

        [Fact]
        public void execute_static_method_with_args_and_return()
        {
            var method = typeof(MethodExecutorTests).GetMethod(nameof(Invoke), (BindingFlags)(0 - 1));
            var isAssert = (bool)MethodExecutor.CreateExecutor(method).Execute(target: null, parameters: true);
            Assert.True(isAssert);
        }

        [Fact]
        public void execute_method_without_args_and_return()
        {
            var isAssert = false;
            Action action = () => isAssert = true;
            MethodExecutor.CreateExecutor(action.Method).Execute(action.Target);
            Assert.True(isAssert);
        }

        [Fact]
        public void execute_method_without_return_with_args()
        {
            var isAssert = false;
            Action<bool> action = _ => isAssert = _;
            MethodExecutor.CreateExecutor(action.Method).Execute(action.Target, true);
            Assert.True(isAssert);
        }

        [Fact]
        public void execute_method_without_args_with_return()
        {
            var isAssert = false;
            Func<bool> action = () => true;
            isAssert = (bool)MethodExecutor.CreateExecutor(action.Method).Execute(action.Target);
            Assert.True(isAssert);
        }

        [Fact]
        public void execute_method_with_args_and_return()
        {
            var isAssert = false;
            Func<bool, bool> action = _ => _;
            isAssert = (bool)MethodExecutor.CreateExecutor(action.Method).Execute(action.Target, true);
            Assert.True(isAssert);
        }
    }
}

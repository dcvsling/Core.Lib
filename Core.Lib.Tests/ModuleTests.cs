using Core.Lib.Module;
using Core.Lib.Module.DependencyInjection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using System.Threading.Tasks;
using TestModule;
using Xunit;

namespace Core.Lib.Tests
{
    public class ModuleTests
    {

        public static IEnumerable<object[]> TestData
            => new[] {
                new [] {
                    new Dictionary<object,string> {
                        ["\"test\""] = @"asset\version\test",
                        [2] = @"asset\version\2",
                        [new object()] = @"asset\version\new"
                    }
                },
                //new [] {
                //    new Dictionary<object,string> {
                //        ["ns20"] = @"asset\platform\netstandard2.0",
                //        ["ns21"] = @"asset\platform\netstandard2.0",
                //        ["nc21"] = @"asset\platform\netcoreapp2.1",
                //        ["nc22"] = @"asset\platform\netcoreapp2.2",
                //        ["nc30"] = @"asset\platform\netcoreapp3.0"
                //    }
                //}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void execute_diff_lib(IDictionary<object, string> data)
        {
            var services = data
                .Aggregate(
                    (IServiceCollection)new ServiceCollection(),
                    (srv, pair) => srv.AddModule(pair.Key.ToString()).AddAssembly(pair.Value, "**/*.dll").Services)
                .BuildServiceProvider();

            var context = services.GetRequiredService<IModuleContext>();

            var result = data
                .Select(x => (value: x.Key, assert: false))
                .Select(x =>
                {
                    context.Invoke<IModuleService>(x.value.ToString(), ms => x.assert = ms.Assert(x.value));
                    return x.assert;
                });

            Assert.Collection(result, Enumerable.Repeat<Action<bool>>(Assert.True, data.Count).ToArray());
        }
    }
}

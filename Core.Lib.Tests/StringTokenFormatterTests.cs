using Core.Lib.Helper;
using System.Collections.Generic;
using Xunit;

namespace Core.Lib.Tests
{

    public class StringTokenFormatterTests
    {
        [Fact]
        public void TestTokenFormat()
        {
            var format = "Value:{Value} Member:{Member.Value} arrayMember:{Members[0].Value} compute:{Member.Value * Members[0].Value}";
            var expect = "Value:1 Member:2 arrayMember:3 compute:6";
            var actual = new NestValue {
                Value = 1,
                Member = new NestValue { Value = 2 },
                Members = {
                    new NestValue { Value = 3 },
                    new NestValue { Value = 4 }
                }
            }.Format(format);

            Assert.Equal(expect, actual);
        }

        [Theory]
        [MemberData("TestData")]
        public void TestScriptParser(NestValue target, string script, object expect)
            => Assert.Equal(expect, script.Parse<NestValue, int>().Invoke(target));

        public static IEnumerable<object[]> TestData()
        {
            var target = new NestValue {
                Value = 1,
                Member = new NestValue { Value = 2 },
                Members = {
                    new NestValue { Value = 3 },
                    new NestValue { Value = 4 }
                }
            };
            yield return new object[] { target, "Value", 1 };
            yield return new object[] { target, "Member.Value", 2 };
            yield return new object[] { target, "Members[0].Value", 3 };
            yield return new object[] { target, "Member.Value * Members[0].Value", 6 };
        }
        public class NestValue
        {
            public int Value { get; set; }
            public NestValue Member { get; set; }
            public List<NestValue> Members { get; set; } = new List<NestValue>();
        }
    }


}

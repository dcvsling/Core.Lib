using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.Ast.Benchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<SeekStringRunner>();
        }
    }

    [MemoryDiagnoser]
    public class SeekStringRunner
    {
        public const string Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_1234567890-=[]\\;',./";
        public const string Words = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_";
        [Params(1_000)]
        public int Total { get; set; }
        [Params(10)]
        public int PerLength { get; set; }
        public Random Random { get; set; }
        private string Content { get; set; }
        private ReadOnlySpan<char> SpanContent => Content.AsSpan();
        private IEnumerable<char> CharContent => Content.AsEnumerable();
        [GlobalSetup]
        public void Init()
        {
            Random = new Random();
            Func<char> getChar = () => Words[Random.Next(Words.Length - 1)];
            Content = Enumerable.Repeat<Func<string>>(() => Words[Random.Next(Words.Length - 1)].ToString(), Total)
                .Select(x => x())
                .Aggregate(string.Concat);
        }
        [GlobalCleanup]
        public void Clean()
        {
            Content = string.Empty;
        }

        [Benchmark]
        public void SeekByString()
        {
            int count = 0;
            var texts = new List<string>();
            while (Content.Length > count)
            {
                if (Content.Length == count)
                {
                    texts.Add(Content);
                    return;
                }
                else if (count == PerLength)
                {
                    texts.Add(Content.Substring(0, PerLength));
                    Content = Content.Substring(PerLength);
                    count = 0;
                }
                else
                {
                    var check = Content.Substring(0, count) != default;
                    count++;
                }
            }
        }
        [Benchmark]
        public void SeekByEnumeratorChar()
        {
            int count = 0;
            var texts = new List<string>();
            var content = CharContent;
            var TotalLength = content.Count();
            while (TotalLength > count)
            {
                if (TotalLength == count)
                {
                    texts.Add(content.ToString());
                    return;
                }
                else if (count == PerLength)
                {
                    texts.Add(content.Take(PerLength).ToString());
                    content = content.Skip(PerLength);
                    TotalLength -= count;
                    count = 0;
                }
                else
                {
                    var check = content.Take(count).ToString() != default;
                    count++;
                }
            }
        }

        [Benchmark]
        public void SeekBySpanChar()
        {
            int count = 0;
            var texts = new List<string>();
            var span = SpanContent;

            while (span.Length > count)
            {
                if (span.Length == count)
                {
                    texts.Add(span.ToString());
                    return;
                }
                else if (count == PerLength)
                {
                    texts.Add(span.Slice(0, PerLength).ToString());
                    span = span.Slice(PerLength);
                    count = 0;
                }
                else
                {
                    var check = span.Slice(0, count).ToString() != default;
                    count++;
                }
            }
        }
    }
}

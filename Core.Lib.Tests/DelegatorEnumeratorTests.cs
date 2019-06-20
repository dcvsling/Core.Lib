namespace Core.Lib.Tests
{
    public class DelegatorEnumeratorTests
    {
        //[Fact]
        //public void TestEach()
        //{
        //    var input = new List<IEnumerator<int>> { GetRangeEnumerator(1, 3), GetRangeEnumerator(4, 3), GetRangeEnumerator(7, 3) };

        //    var container = new Enumerator(input.GetEnumerator());
        //    var actual = new List<int>();
        //    while (container.MoveNext())
        //    {
        //        actual.Add(container.Current);
        //    }
        //    var asserts = Enumerable.Range(1, 9).Select(x => (Action<int>)(i => Assert.Equal(i, x))).ToArray();
        //    Assert.Collection(actual, asserts);
        //}

        //private static IEnumerator<Func<>> GetRangeEnumerator(int start, int count)
        //    => Enumerable.Range(start, count).GetEnumerator();
    }
}

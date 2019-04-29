using System.Collections.Generic;
using Xunit;

namespace Core.Lib.Tests
{
    /// <summary>
    /// The accessor tests class.
    /// </summary>
    public class AccessorTests
    {
        /// <summary>
        ///  --> Test model -->  -->  -->  -->  --> 
        /// </summary>
        [Fact]
        public void Get_TestModel_Member_value_will_get_Ok()
        {
            var expect = "Ok";
            var model = new TestModel { Member = expect };
            var accessor = Accessor.Get(typeof(TestModel));

            Assert.Equal(expect, accessor.Get(model, nameof(TestModel.Member)));

        }

        /// <summary>
        ///  -->  -->  -->  -->  --> Test model --> 
        /// </summary>
        [Fact]
        public void Accessor_can_set_ok_To_TestModel_Member()
        {
            var expect = "Ok";
            var model = new TestModel();
            Assert.NotEqual(expect, model.Member);
            var accessor = Accessor.Get(typeof(TestModel));
            accessor.Set(model, nameof(TestModel.Member), expect);
            Assert.Equal(expect, model.Member);
        }

        /// <summary>
        ///  --> Test model -->  -->  -->  --> 
        /// </summary>
        [Fact]
        public void Get_TestModel_number_will_get_1()
        {
            var expect = 1;
            var model = new TestModel { Number = expect };
            var accessor = Accessor.Get(typeof(TestModel));

            Assert.Equal(expect, accessor.Get(model, nameof(TestModel.Number)));

        }

        /// <summary>
        ///  -->  -->  -->  -->  --> Test model --> 
        /// </summary>
        [Fact]
        public void Accessor_can_set_2_To_TestModel_Number()
        {
            var expect = 2;
            var model = new TestModel();
            Assert.NotEqual(expect, model.Number);
            var accessor = Accessor.Get(typeof(TestModel));
            accessor.Set(model, nameof(TestModel.Number), expect);
            Assert.Equal(expect, model.Number);
        }

        /// <summary>
        ///  -->  -->  -->  -->  --> 
        /// </summary>
        [Fact]
        public void Accessor_null_property_will_get_null()
        {
            var model = new TestModel();
            var accessor = Accessor.Get(typeof(TestModel));
            accessor.Set(model, nameof(TestModel.Member), default(string));
            Assert.Null(model.Member);
        }

        /// <summary>
        ///  -->  --> I enumerable -->  -->  --> 
        /// </summary>
        [Fact]
        public void Can_set_IEnumerable_to_close_List()
        {
            var model = new TestModel();
            var accessor = Accessor.Get(typeof(TestModel));
            accessor.Set(model, nameof(TestModel.List), new[] { new TestModel(), new TestModel() });
            Assert.Equal(2, model.List.Count);
        }

        /// <summary>
        ///  -->  -->  -->  --> 
        /// </summary>
        [Fact]
        public void Set_null_will_do_nothing()
        {
            var expect = default(string);
            var model = new TestModel() { Member = "test" };
            var accessor = Accessor.Get(typeof(TestModel));
            accessor.Set(model, nameof(TestModel.Member), null);
            Assert.Equal(expect, model.Member);
        }

        /// <summary>
        ///  -->  --> change type -->  -->  -->  -->  --> 
        /// </summary>
        [Fact]
        public void Use_convert_changeType_when_assign_different_base_struct()
        {
            var expectmember = 3;
            var expectnumb = "2";
            var model = new TestModel();
            var accessor = Accessor.Get(typeof(TestModel));
            accessor.Set(model, nameof(TestModel.Member), expectmember);
            accessor.Set(model, nameof(TestModel.Number), expectnumb);
            Assert.Equal("3", model.Member);
            Assert.Equal(2, model.Number);
        }

        /// <summary>
        ///  -->  -->  -->  -->  -->  -->  -->  -->  --> 
        /// </summary>
        [Fact]
        public void Null_assign_to_struct_base_type_will_give_default_value()
        {
            var model = new TestModel();
            var accessor = Accessor.Get(typeof(TestModel));
            accessor.Set(model, nameof(TestModel.Number), null);
            Assert.Equal(default, model.Number);
        }

        /// <summary>
        /// The test model class.
        /// </summary>
        public class TestModel
        {
            /// <summary>
            /// Gets the list.
            /// </summary>
            public List<TestModel> List { get; } = new List<TestModel>();

            /// <summary>
            /// Gets or sets the member.
            /// </summary>
            public string Member { get; set; }

            /// <summary>
            /// Gets or sets the number.
            /// </summary>
            public int Number { get; set; }
        }
    }
}

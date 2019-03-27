using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Lib.Reflections
{

    /// <summary>
    /// The expression factory class.
    /// </summary>
    internal static partial class ExpressionFactory
    {

        /// <summary>
        /// model => (object)model.Property
        /// </summary>
        /// <param name="propertyInfo">The propertyInfo.</param>
        /// <returns>The <see cref="T:Func{object, object}"/>.</returns>
        internal static Func<object, object> CreatePropertyGetter(this PropertyInfo propertyInfo)
        {
            var param = Expression.Parameter(typeof(object));
            var convert = Expression.Convert(param, propertyInfo.DeclaringType);
            var property = Expression.Property(convert, propertyInfo);
            var toobj = Expression.Convert(property, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(
                toobj,
                new[] { param });
            return lambda.Compile();
        }

        /// <summary>
        /// (model,val) => model.Property = val;
        /// </summary>
        /// <param name="propertyInfo">The propertyInfo.</param>
        /// <returns>The <see cref="T:Action{object, object}"/>.</returns>
        internal static Action<object, object> CreatePropertySetter(this PropertyInfo propertyInfo)
        {
            var param = Expression.Parameter(typeof(object));
            var convert = Expression.Convert(param, propertyInfo.DeclaringType);
            var value = Expression.Parameter(typeof(object));
            var prop = Expression.Property(convert, propertyInfo);
            var assign = prop.Type.IsCloseList()
                ? prop.CollectionAdd(value) 
                : prop.PropertyStructAssign(value);
            var lambda = Expression.Lambda<Action<object, object>>(assign, new [] { param, value });
            return lambda.Compile();
        }

        /// <summary>
        /// The is close list.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool IsCloseList(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

        /// <summary>
        /// The property assign.
        /// </summary>
        /// <param name="to">The to.</param>
        /// <param name="from">The from.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        private static Expression PropertyAssign(this Expression to, Expression from) => Expression.IfThen(
                Expression.TypeIs(from, to.Type),
                Expression.Assign(to,
                    Expression.Convert(from, to.Type))
            );

        /// <summary>
        /// The property struct assign.
        /// </summary>
        /// <param name="to">The to.</param>
        /// <param name="from">The from.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        private static Expression PropertyStructAssign(this Expression to ,Expression from)
        {
            if ( !to.Type.IsValueType && to.Type != typeof(string) )
                return PropertyAssign(to, from);
            var changetype = typeof(Convert).GetMethod(nameof(Convert.ChangeType), new[] { typeof(object), typeof(Type) });
            var fromOrDefault = Expression.Condition(
                Expression.Equal(from, Expression.Default(from.Type)),
                Expression.TypeAs(Expression.Default(to.Type),typeof(object)),
                Expression.Call(changetype, from, Expression.Constant(to.Type)),
                typeof(object));
            return Expression.Assign(to, Expression.Convert(fromOrDefault, to.Type));
        }

        /// <summary>
        /// The collection add.
        /// </summary>
        /// <param name="to">The to.</param>
        /// <param name="from">The from.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        private static Expression CollectionAdd(this Expression to, Expression from)
        {
            var seq = Expression.Convert(from, typeof(IEnumerable));
            var oftypeMethod = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.OfType))
                .MakeGenericMethod(new[] { to.Type.GenericTypeArguments[0] });
            var oftype = Expression.Call(oftypeMethod, seq);
            return Expression.Call(
                to,
                to.Type.GetMethod(nameof(List<object>.AddRange)),
                new Expression[] { oftype });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Lib.Reflections.Executors
{

    internal class DefaultMethodExecutor : IMethodExecutor
    {
        private Func<object, object[], object> _invoker;
        public DefaultMethodExecutor(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
            _invoker = ExpressionFactory.CreateExecutor(MethodInfo);
        }
        public MethodInfo MethodInfo { get; }
        public IEnumerable<ParameterInfo> MethodParameters => MethodInfo.GetParameters();
        public TypeInfo TargetTypeInfo => MethodInfo.DeclaringType.GetTypeInfo();
        public Type MethodReturnType => MethodInfo.ReturnType;

        public object Execute(object target, params object[] parameters) => _invoker(target, parameters);
    }
}

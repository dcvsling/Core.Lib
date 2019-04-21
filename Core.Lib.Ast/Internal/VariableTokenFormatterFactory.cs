// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Options;

namespace Core.Lib.Ast.Internal
{

    internal class VariableTokenFormatterFactory : IVariableTokenFormatterFactory
    {
        private readonly IOptionsSnapshot<VariableTokenOptions> _snapshot;

        public VariableTokenFormatterFactory(IOptionsSnapshot<VariableTokenOptions> snapshot)
        {
            _snapshot = snapshot;
        }

        public IVariableTokenFormatter Create(string name, string format)
            => new VariableTokenFormatter(_snapshot.Get(name), format);
    }
}

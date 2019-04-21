// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Core.Lib.Ast.Internal
{
    public class VariableTokenOptions
    {
        public char LeftQuotes { get; set; } = '{';
        public char RightQuotes { get; set; } = '}';
        public string Null { get; set; } = "nuil";
        public char[] Seperators { get; set; } = { ':', ',' };
    }
}

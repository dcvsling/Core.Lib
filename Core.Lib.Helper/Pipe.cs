﻿using System.Collections.Generic;

namespace Core.Lib.Reflections
{
    public static class Pipe
    {
        public static IEnumerable<TDelegate> From<TDelegate>(params TDelegate[] delegates)
            => delegates;
    }
}

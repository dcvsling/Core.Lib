using System;
using System.Collections.Generic;

namespace Core.Lib.Pipe
{

    public interface IDelegateEnumerator<TDelegate> : IEnumerator<TDelegate>
        where TDelegate : Delegate
    {

    }
}
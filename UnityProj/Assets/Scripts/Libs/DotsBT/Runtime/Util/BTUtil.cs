using System;
using System.Collections.Generic;

namespace DotsBT
{
    public static class BTUtil
    {
        public static int Name2Id(string name)
        {
            if (string.IsNullOrEmpty(name))
                return 0;
            return name.GetHashCode();
        }
    }
}

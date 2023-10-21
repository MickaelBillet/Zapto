using System;

namespace Framework.Core.Base
{
    public static class UtilityLibrary
    {
        public static bool IsSerializable(this object obj)
        {
            if (obj == null)
                return false;

            Type t = obj.GetType();
            return t.IsSerializable;
        } 
    }
}

using System;
using System.Diagnostics;

namespace Framework.Core.Base
{
    public static class ClassHelper
    {
        public static string GetCallerClassAndMethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame callerFrame = stackTrace.GetFrame(1);
            Type callerType = callerFrame.GetMethod().DeclaringType;
            string callerClassName = callerType.FullName;
            string callerMethodName = callerFrame.GetMethod().Name;

            return $"{callerClassName}:{callerMethodName}" ;
        }
    }
}

using System;

namespace Framework.Core.Base
{
    public class TaskProgressPercent : Progress<int?>, ITaskProgress
    {
        public int InitialValue { get; set; } = 0;

        public int DurationStep { get; set; } = 100;

        public void Report<T>(T value)
        {
            this.OnReport((value as int?).Value);
        }
    }

    public class TaskProgressMessage : Progress<string>, ITaskProgress
    {
        public void Report<T>(T value)
        {
            this.OnReport(value as string);
        }
    }

    public interface ITaskProgress
    {
        void Report<T>(T value);
    }

}

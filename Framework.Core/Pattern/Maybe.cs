using System;

namespace Framework.Core.Base
{
    public abstract class Maybe<T> where T : class
    {
        public abstract void Match(Action<T> someAction, Action<string> errorAction, Action noneAction);

        private Maybe()
        {

        }

        public sealed class Some : Maybe<T>
        {
            public Some(T value) => Value = value;
            public T Value { get; }

            public override void Match(Action<T> someAction, Action<string> errorAction, Action noneAction) => someAction(Value);
        }

        public sealed class Error : Maybe<T>
        {
            public Error(string value) => Value = value;
            public string Value { get; }

            public override void Match(Action<T> someAction, Action<string> errorAction, Action noneAction) => errorAction(Value);
        }

        public sealed class None : Maybe<T>
        {
            public override void Match(Action<T> someAction, Action<string> errorAction, Action noneAction) => noneAction();
        }
    }
}

using System;

namespace PnP.PowerShell.Commands.InvokeAction
{
    internal class InvokeActionParameter<T>
    {
        public Action<T> Action { get; set; }
        public Func<T, bool> ShouldProcessAction {get; set;}
        public Action<T> PostAction { get; set; }
        public Func<T, bool> ShouldProcessPostAction { get; set; }
        public string[] Properties { get; set; }

        public bool HasAction => Action != null;
        public bool HasPostAction => PostAction != null;
        public bool HasAnyAction => Action != null || PostAction != null;

        public bool ShouldProcessAnyAction(T item) => ShouldProcess(item) || ShouldProcessPost(item);

        public bool ShouldProcess(T item)
        {
            if (ShouldProcessAction == null)
                return true;
            else
                return ShouldProcessAction(item);
        }

        public bool ShouldProcessPost(T item)
        {
            if (ShouldProcessPostAction == null)
                return true;
            else
                return ShouldProcessPostAction(item);
        }

        public InvokeActionParameter<T> ShallowCopy() => (InvokeActionParameter<T>) MemberwiseClone();
    }
}

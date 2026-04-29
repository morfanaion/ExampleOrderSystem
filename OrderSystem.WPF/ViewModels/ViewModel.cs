using OrderSystem.Data.Classes;

namespace OrderSystem.WPF.ViewModels
{
    internal abstract class ViewModel : BaseNotifier, IDisposable
    {
        public event Action<bool?>? Finish;
        public abstract string ViewTitle { get; }
        
        protected virtual void Commit()
        {
            Finish?.Invoke(true);
        }

        protected virtual void Discard()
        {
            Finish?.Invoke(false);
        }

        internal virtual bool CancelAllowed() => true;

        public virtual void Dispose()
        {
        }
    }
}

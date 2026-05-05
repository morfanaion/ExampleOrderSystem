using OrderSystem.Data.Classes;

namespace OrderSystem.Business.ViewModels
{
    public abstract class ViewModel : BaseNotifier, IDisposable
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

        public virtual bool CancelAllowed() => true;

        public virtual void Dispose()
        {
        }
    }
}

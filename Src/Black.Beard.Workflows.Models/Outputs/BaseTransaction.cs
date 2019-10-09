using Bb.Workflows.Outputs;

namespace Bb.Workflows.Outputs
{

    public abstract class BaseTransaction : ITransaction
    {

        public virtual void Commit()
        {
            Commit_Impl();
            Child?.Commit();
            commited = true;
        }

        public void Rollback()
        {
            Rollback_Impl();
            Child?.Rollback();
            roolbacked = true;
        }

        protected abstract void Commit_Impl();


        protected abstract void Rollback_Impl();


        public ITransaction Child { get; set; }


        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                if (!commited && roolbacked)
                    Rollback();

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PushBusActionTransaction()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        private bool disposedValue = false; // To detect redundant calls
        private bool commited = false; // To detect redundant calls
        private bool roolbacked = false; // To detect redundant calls

        #endregion


    }

}

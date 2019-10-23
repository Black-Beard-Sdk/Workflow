using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using Bb.Workflows.Outputs;
using Bb.Workflows.Outputs.Memories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bb.Workflows
{
    public class WorkflowEngine
    {

        public WorkflowEngine()
        {

        }

        public void EvaluateEvent(string payload)
        {

            var msg = this.Serializer.Unserialize(payload);

            var key = ThreadSafeKey(msg);
            uint crc = Crc32.Calculate(key);

            using (var l = Lock(crc))
            {

                Processor.EvaluateEvent(msg);

            }

        }

        private LockProcess Lock(uint crc)
        {
            return new LockProcess(crc, this.Locker);
        }

        public IWorkflowSerializer Serializer { get; set; }

        public WorkflowProcessor Processor { get; set; }

        /// <summary>
        /// By default the used key is ExternalId
        /// </summary>
        public Func<IncomingEvent, string> ThreadSafeKey { get; set; } = (e) => e.ExternalId;

        /// <summary>
        /// Locker object too keep safe process for context
        /// </summary>
        public ILockObject Locker { get; set; } = new LockerMemory();

        private class LockProcess : IDisposable
        {

            public LockProcess(uint crc, ILockObject locker)
            {
                this._locker = locker;
                this._crc = crc;
                Task.Run(() => Unlock());
                this._locker.Lock(this._crc);
            }

            private void Unlock()
            {

                var dateMax = WorkflowClock.Now().AddMinutes(5);
                
                while (WorkflowClock.Now() < dateMax)
                {

                    if (this.disposedValue)
                        return;

                    Thread.Yield();

                }

                throw new TimeoutException();

            }

            #region IDisposable Support


            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                        this._locker.UnLock(this._crc);

                    disposedValue = true;
                }
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                Dispose(true);
            }

            #endregion

            private bool disposedValue = false; // To detect redundant calls
            private readonly ILockObject _locker;
            private readonly uint _crc;
        
        }

    }

}

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
            this._resource = new UseResource();
        }

        public void EvaluateEvent(string payload, string incomingType = null)
        {

            IncomingEvent msg;
            try
            {
                msg = this.Processor.Factory.CreateBaseIncomingEvent(payload, incomingType);
            }
            catch (Exception e)
            {
                throw new Exceptions.InvalidIncomingEventFormatException("Invalid format", e);
            }

            var key = ThreadSafeKey(msg);
            uint crc = Crc32.Calculate(key);

            using (var l = Lock(crc))
                Processor.EvaluateEvent(msg);

        }

        private LockProcess Lock(uint crc)
        {
            return new LockProcess(crc, this.Locker, this._resource);
        }

        public WorkflowProcessor Processor { get; set; }

        /// <summary>
        /// By default the used key is ExternalId
        /// </summary>
        public Func<IncomingEvent, string> ThreadSafeKey { get; set; } = (e) => e.ExternalId;

        /// <summary>
        /// Locker object too keep safe process for context
        /// </summary>
        public ILockObject Locker { get; set; } = new LockerMemory();

        public bool CanBeRemoved
        {
            get
            {
                return _resource.Value() == 0;
            }
        }

        private class LockProcess : IDisposable
        {

            public LockProcess(uint crc, ILockObject locker, UseResource res)
            {
                this._locker = locker;
                this._crc = crc;
                Task.Run(() => Unlock());
                this._locker.Lock(this._crc);
                this._res = res;
                this._res.Increment();
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
                    {
                        this._locker.UnLock(this._crc);
                        this._res.Decrement();
                    }

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
            private readonly UseResource _res;
        }

        private class UseResource
        {

            public void Increment()
            {
                Interlocked.Increment(ref midValueCount);
            }

            public void Decrement()
            {
                Interlocked.Increment(ref midValueCount);
            }

            public long Value()
            {
                return Interlocked.Read(ref midValueCount);
            }

            private long midValueCount;

        }

        private readonly UseResource _resource;

    }

}

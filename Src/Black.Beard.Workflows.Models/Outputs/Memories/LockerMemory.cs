using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Bb.Workflows.Outputs.Memories
{


    internal class LockerMemory : ILockObject
    {


        public LockerMemory(int timeout = 3)
        {
            this._timeout = timeout;
        }

        public void Lock(uint crc)
        {

            var d = WorkflowClock.Now().AddSeconds(_timeout);

            while (true)
            {

                if (WorkflowClock.Now() > d)
                    throw new Exceptions.LockFailedException(crc);

                if (_crc.TryAdd(crc, WorkflowClock.Now()))
                    break;

                Thread.Yield();

            }

        }


        public void UnLock(uint crc)
        {
            _crc.TryRemove(crc, out DateTimeOffset d);
        }

        private readonly ConcurrentDictionary<uint, DateTimeOffset> _crc = new ConcurrentDictionary<uint, DateTimeOffset>();
        private readonly int _timeout;
    
    }

}
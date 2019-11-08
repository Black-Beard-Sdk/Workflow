using Bb.Dao;
using Bb.Workflows;
using Bb.Workflows.Outputs;
using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Threading;

namespace Bb.Workflows.Outputs.Sql
{

    public class LockerSqlServer : ILockObject
    {

        public LockerSqlServer(SqlManager manager, int timeout = 3)
        {
            this._timeout = timeout;
            _manager = manager;

        }

        public void Lock(uint crc)
        {

            var d = WorkflowClock.Now().AddSeconds(_timeout);

            while (true)
            {

                if (WorkflowClock.Now() > d)
                    throw new Bb.Workflows.Exceptions.LockFailedException(crc);

                try
                {
                    var result = _manager.Update($"INSERT INTO [Locks] ([Uuid], [InsertedDate]) VALUES (@uuid, CURRENT_TIMESTAMP)",
                                 _manager.CreateParameter("uuid", System.Data.DbType.Int64, crc)
                        
                        );
                    break;
                }
                catch (Exception)
                {

                }

                Thread.Yield();

            }

        }


        public void UnLock(uint crc)
        {

            var result = _manager.Update($"DELETE FROM [Locks] WHERE [Uuid] = @uuid",
                         _manager.CreateParameter("uuid", System.Data.DbType.Int64, crc)
                );

        }


        /// <summary>
        /// Keep last specified minutes
        /// </summary>
        /// <param name="minute"></param>
        public void Purge(int minute)
        {

            var m = 0 - minute;
            var result = _manager.Update($"DELETE FROM [Locks] WHERE [InsertedDate] < DATEADD(minute, @minute, CURRENT_TIMESTAMP);",
                         _manager.CreateParameter("minute", System.Data.DbType.Int64, m)
                );

        }

        private readonly ConcurrentDictionary<uint, DateTimeOffset> _crc = new ConcurrentDictionary<uint, DateTimeOffset>();
        private readonly int _timeout;
        private readonly SqlManager _manager;
    
    }


}

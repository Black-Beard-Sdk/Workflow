using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{


    public class IncomingEvent
    {


        public IncomingEvent()
        {

        }

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public string ExternalId { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public DynObject ExtendedDatas { get; set; } = new DynObject();

        internal Event Map()
        {

            var result = new Event()
            {
                Uuid = Guid.NewGuid(),
                ExternalId = this.ExternalId,
                EventDate = this.EventDate,
                Name = this.Name,
                CreationDate = WorkflowClock.Now(),
                ExtendedDatas = ExtendedDatas.Clone(),
                
            };

            return result;

        }

    }

}

using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{


    public class IncomingEvent : IExtendedDatas
    {


        public IncomingEvent()
        {

        }

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public string ExternalId { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public DynObject ExtendedDatas() { return _extendedDatas; }

        private DynObject _extendedDatas = new DynObject();

    }

}

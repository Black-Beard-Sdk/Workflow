using Bb.Workflows.Models;
using System;

namespace Black.Beard.Workflows.Outputs.Sql
{
    public class ActionByKey : PushedAction
    {

        public Guid EventUuid { get; set; }

    }


}

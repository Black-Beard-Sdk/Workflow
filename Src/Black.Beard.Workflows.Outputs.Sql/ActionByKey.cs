using Bb.Workflows.Models;
using System;

namespace Bb.Workflows.Outputs.Sql
{
    public class ActionByKey : PushedAction
    {

        public Guid EventUuid { get; set; }

    }


}

using System;
using System.Collections.Generic;
using System.Text;
using Bb.Workflows.Models;

namespace Bb.Workflows.Converters
{

    public interface IWorkflowSerializer
    {


        IncomingEvent Unserialize(string payload);

        string Serialize(IncomingEvent @event);



        string Serialize(Workflow workflow);



        string Serialize(PushedAction pushedAction);

    }


}

using System;
using System.Collections.Generic;
using System.Text;
using Bb.Workflows.Models;

namespace Bb.Workflows.Converters
{

    public interface IWorkflowSerializer
    {

        string Serialize(object instance);

        void Populate(object instance, string payload);

    }


}

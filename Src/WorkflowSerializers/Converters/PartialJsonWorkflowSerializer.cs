using Newtonsoft.Json;
using Bb.Workflows.Models;
using System;
using Bb.ComponentModel;
using Bb.Workflows.Models.Messages;

namespace Bb.Workflows.Converters
{


    public class PartialJsonWorkflowSerializer : IWorkflowSerializer
    {


        public PartialJsonWorkflowSerializer()
        {

        }

        public string Serialize(object instance)
        {

            if (instance == null)
                return string.Empty;

            switch (instance)
            {

                case MessageRaw m:
                    var visitor = new VisitorJsonSerialiser();
                    var tokens = visitor.VisitMessageRaw(m);
                    return tokens.ToString(Formatting.None);

                case Workflow w:
                    return DynObjectSerializer.Serialize(w, typeof(Workflow));

                case Event e:
                    return DynObjectSerializer.Serialize(e, typeof(Event));

                case IncomingEvent i:
                    return DynObjectSerializer.Serialize(i, typeof(IncomingEvent));

                default:
                    return TypeDiscovery.Instance.SerializeObject(instance);

            }

        }

        public void Populate(object instance, string payload)
        {

            if (instance != null && !string.IsNullOrEmpty(payload))
                switch (instance)
                {

                    case MessageRaw m:

                        break;

                    case Workflow w:
                        DeserializeExtendedDatas(payload, w);
                        break;

                    case Event e:
                        DeserializeExtendedDatas(payload, e);
                        break;

                    case IncomingEvent i:
                        DeserializeExtendedDatas(payload, i);
                        break;

                    default:
                        TypeDiscovery.Instance.PopulateObject(payload, instance);
                        break;

                }

        }

        public void DeserializeExtendedDatas(string payload, IExtendedDatas instance)
        {
            TypeDiscovery.Instance.PopulateObject(payload, instance);
            DynObjectSerializer.Deserialize(instance, payload);
        }

    }


}

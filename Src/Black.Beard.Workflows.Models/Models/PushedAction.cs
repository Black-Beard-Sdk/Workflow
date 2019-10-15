using Bb.Workflows.Models.Messages;
using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{

    public class PushedAction
    {


        public PushedAction()
        {

        }



        public Guid Uuid { get; set; }

        public string Name { get; set; }



        public MessageRaw ExecuteMessage { get; set; } 
        
        public MessageRaw ResultMessage { get; set; } 
        


        public MessageRaw CancelMessage { get; set; } 
        
        public MessageRaw ResultCancelMessage { get; set; }
        


    }


}

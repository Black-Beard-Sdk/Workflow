﻿namespace Bb.Workflows.Parser
{
    public class ErrorModel
    {

        public ErrorModel()
        {

        }

        public string Message { get; set; }
        public string Text { get; internal set; }
        public int Column { get; internal set; }
        public int StartIndex { get; internal set; }
        public int Line { get; internal set; }
        public string Filename { get; internal set; }

        public override string ToString()
        {
            return Message.ToString();
        }

    }

}

namespace Bb.Workflows.Models.Messages
{
    public abstract class MessageVisitor<T>
    {
        public abstract T VisitMessageRaw(MessageRaw m);
        public abstract T VisitMessageHeader(MessageHeader m);
        public abstract T VisitMessageBlock(MessageBlock m);
        public abstract T VisitMessageProperty(MessageProperty m);
        public abstract T VisitMessageValue(MessageValue m);

    }

}

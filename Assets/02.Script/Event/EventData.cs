public abstract  class EventData
{
    public object Sender { get; private set; }

    public EventData(object sender = null) {
        Sender = sender;
    }
}
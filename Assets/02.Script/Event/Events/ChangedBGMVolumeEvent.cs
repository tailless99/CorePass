public class ChangedBGMVolumeEvent : EventData
{
    public float Volum { get; }

    public ChangedBGMVolumeEvent(float volum) {
        Volum = volum;
    }
}

using UnityEngine;

public class PlaySoundEvent : EventData
{
    public PlaySoundData Data { get; }

    public PlaySoundEvent(PlaySoundData data) { Data = data; }
}

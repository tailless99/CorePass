using UnityEngine;

public class PlayOneShotEvent : EventData
{
    public PlaySoundData Data { get; }

    public PlayOneShotEvent(PlaySoundData data) { Data = data; }
}

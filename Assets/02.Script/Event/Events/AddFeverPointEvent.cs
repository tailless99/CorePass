using UnityEngine;

public class AddFeverPointEvent : EventData
{
    public int Score { get; }
    public AddFeverPointEvent(int score) { Score = score; }
}

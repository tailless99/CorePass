public class AddScoreEvent : EventData
{
    public int Score { get; }
    public AddScoreEvent(int score) { Score = score; }
}

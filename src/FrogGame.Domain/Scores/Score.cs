namespace FrogGame.Domain.Scores;

public readonly record struct Score
{
    public static Score Zero => new(0);

    public Score(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    public int Value { get; }

    public Score Add(int points) => new(checked(Value + points));
}

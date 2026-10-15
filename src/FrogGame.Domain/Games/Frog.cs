namespace FrogGame.Domain.Games;

public sealed record Frog(FrogColor Color)
{
    public int Points => 10;
}

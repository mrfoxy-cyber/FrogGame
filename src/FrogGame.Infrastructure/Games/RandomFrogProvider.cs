using FrogGame.Application.Games;
using FrogGame.Domain.Games;

namespace FrogGame.Infrastructure.Games;

public sealed class RandomFrogProvider : IFrogProvider
{
    private static readonly FrogColor[] Colors = Enum.GetValues<FrogColor>();

    public Frog Next(int level)
    {
        var availableColors = Math.Min(Colors.Length, 3 + level);
        return new Frog(Colors[Random.Shared.Next(availableColors)]);
    }
}

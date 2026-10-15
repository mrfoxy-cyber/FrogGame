using FrogGame.Domain.Games;

namespace FrogGame.Application.Games;

public sealed record CreateGameCommand(int Rows, int Columns);

public sealed record PlayMoveCommand(Guid GameId, int Column);

public sealed record GameState(
    Guid Id,
    int Rows,
    int Columns,
    FrogColor?[][] Board,
    int Score,
    int BestCombo,
    int Level,
    FrogColor NextFrog,
    GameStatus Status);

public sealed record ChainReactionResult(
    int Chain,
    int Awarded,
    IReadOnlyList<BoardPosition> Matches,
    FrogColor?[][] BoardAfterGravity);

public sealed record MoveResult(
    BoardPosition PlacedAt,
    FrogColor Frog,
    IReadOnlyList<ChainReactionResult> Reactions,
    GameState State);

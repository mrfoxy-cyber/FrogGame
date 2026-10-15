using FrogGame.Domain.Games;

var blue = new Frog(FrogColor.Blue);
var calculator = new ClassicScoreCalculator();

var game = new GameSession(rows: 4, columns: 4, blue);
MoveResolution? fourthMove = null;
for (var column = 0; column < 4; column++)
{
    fourthMove = game.Play(column, calculator);
    game.SetNextFrog(blue);
}

Check(game.Score.Value == 40, "Four connected frogs should score 40 points.");
Check(game.BestCombo.Value == 40, "The first combo should become the best combo.");
Check(fourthMove!.Reactions.Count == 1, "Four connected frogs should create one reaction.");
Check(fourthMove.Reactions[0].Matches.Count == 4, "The reaction should remove four frogs.");
Check(game.Board.Snapshot().SelectMany(row => row).All(cell => cell is null), "Matched frogs should leave the board empty.");

var board = new GameBoard(rows: 4, columns: 4);
for (var column = 0; column < 4; column++)
{
    board.Drop(blue, column);
}

var matches = board.FindMatches();
Check(calculator.Calculate(board, matches, chain: 2).Value == 80, "The second chain should double its points.");

var narrowBoard = new GameBoard(rows: 3, columns: 2);
narrowBoard.Drop(new Frog(FrogColor.Blue), 0);
narrowBoard.Drop(new Frog(FrogColor.Green), 0);
narrowBoard.Drop(new Frog(FrogColor.Orange), 0);
try
{
    narrowBoard.Drop(blue, 0);
    throw new Exception("A full column accepted another frog.");
}
catch (GameRuleException)
{
    // Expected domain rule.
}

Console.WriteLine("Passed: domain placement, matching, scoring, gravity, combo, and full-column rules.");

static void Check(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception(message);
    }
}

namespace FrogGame.Domain.Games;

public sealed class GameBoard
{
    private readonly Frog?[,] _cells;

    public GameBoard(int rows, int columns)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(rows, 3);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rows, 30);
        ArgumentOutOfRangeException.ThrowIfLessThan(columns, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(columns, 30);
        Rows = rows;
        Columns = columns;
        _cells = new Frog?[rows, columns];
    }

    public int Rows { get; }
    public int Columns { get; }
    public Frog? this[BoardPosition position] => _cells[position.Row, position.Column];

    public BoardPosition Drop(Frog frog, int column)
    {
        if (column < 0 || column >= Columns)
        {
            throw new GameRuleException($"Column must be between 0 and {Columns - 1}.");
        }

        for (var row = Rows - 1; row >= 0; row--)
        {
            if (_cells[row, column] is not null)
            {
                continue;
            }

            _cells[row, column] = frog;
            return new BoardPosition(row, column);
        }

        throw new GameRuleException("That column is full.");
    }

    public IReadOnlyList<BoardPosition> FindMatches(int minimumGroupSize = 4)
    {
        var matches = new List<BoardPosition>();
        var visited = new HashSet<BoardPosition>();

        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                var start = new BoardPosition(row, column);
                if (visited.Contains(start) || this[start] is null)
                {
                    continue;
                }

                var group = FindGroup(start);
                foreach (var position in group)
                {
                    visited.Add(position);
                }

                if (group.Count >= minimumGroupSize)
                {
                    matches.AddRange(group);
                }
            }
        }

        return matches;
    }

    public void Remove(IEnumerable<BoardPosition> positions)
    {
        foreach (var position in positions)
        {
            _cells[position.Row, position.Column] = null;
        }
    }

    public void ApplyGravity()
    {
        for (var column = 0; column < Columns; column++)
        {
            var destination = Rows - 1;
            for (var row = Rows - 1; row >= 0; row--)
            {
                if (_cells[row, column] is not { } frog)
                {
                    continue;
                }

                _cells[destination, column] = frog;
                if (destination != row)
                {
                    _cells[row, column] = null;
                }

                destination--;
            }

            while (destination >= 0)
            {
                _cells[destination--, column] = null;
            }
        }
    }

    public bool IsFull => Enumerable.Range(0, Columns).All(column => _cells[0, column] is not null);

    public FrogColor?[][] Snapshot() => Enumerable.Range(0, Rows)
        .Select(row => Enumerable.Range(0, Columns)
            .Select(column => _cells[row, column]?.Color)
            .ToArray())
        .ToArray();

    private List<BoardPosition> FindGroup(BoardPosition start)
    {
        var color = this[start]!.Color;
        var group = new List<BoardPosition>();
        var discovered = new HashSet<BoardPosition>();
        var pending = new Stack<BoardPosition>();
        pending.Push(start);

        while (pending.TryPop(out var current))
        {
            if (!IsInside(current) || !discovered.Add(current) || this[current]?.Color != color)
            {
                continue;
            }

            group.Add(current);
            pending.Push(current with { Row = current.Row + 1 });
            pending.Push(current with { Row = current.Row - 1 });
            pending.Push(current with { Column = current.Column + 1 });
            pending.Push(current with { Column = current.Column - 1 });
        }

        return group;
    }

    private bool IsInside(BoardPosition position) =>
        position.Row >= 0 && position.Row < Rows && position.Column >= 0 && position.Column < Columns;
}

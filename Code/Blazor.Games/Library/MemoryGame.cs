namespace Blazor.Games;

public class MemoryGame
{
    private const string title = "Memory Game";
    private const int size = 4;
    private readonly int[,] _board = new int[size, size];
    private readonly FluentEmojiType[,] _assets = new FluentEmojiType[size, size];
    private readonly List<int> _matches = new();
    private static readonly Dictionary<int, FluentEmojiType> _options = new()
    {
        { 1, FluentEmojiType.NewMoon },
        { 2, FluentEmojiType.WaxingCrescentMoon },
        { 3, FluentEmojiType.FirstQuarterMoon },
        { 4, FluentEmojiType.WaxingGibbousMoon },
        { 5, FluentEmojiType.FullMoon },
        { 6, FluentEmojiType.WaningGibbousMoon },
        { 7, FluentEmojiType.LastQuarterMoon },
        { 8, FluentEmojiType.WaningCrescentMoon }
    };
    private readonly Random _random = new((int)DateTime.UtcNow.Ticks);

    private int _moves = 0;
    private int _clicks = 0;
    private int _firstId = 0;
    private int _secondId = 0;
    private int? _row = null;
    private int? _column = null;
    private (int row, int column)? _first;
    private (int row, int column)? _second;

    private List<int> Choose(int minimum, int maximum, int total) =>
         Enumerable.Range(minimum, maximum)
            .OrderBy(r => _random.Next(minimum, maximum))
                .Take(total).ToList();

    private void SetAsset(int row, int column)
    {
        Display.Set(row, column, _assets[row, column]);
        Updated?.Invoke();
    }

    private void Match()
    {
        _matches.Add(_firstId);
        _matches.Add(_secondId);
        if (_matches.Count == size * size)
            Message = $"{title} - Matched in {_moves} moves!";
    }

    private void NoMatch()
    {
        if (_first != null)
        {
            _assets[_first.Value.row, _first.Value.column] = 
                FluentEmojiType.PurpleSquare;
            SetAsset(_first.Value.row, _first.Value.column);
            _first = null;
        }
        if (_second != null)
        {
            _assets[_second.Value.row, _second.Value.column] = 
                FluentEmojiType.PurpleSquare;
            SetAsset(_second.Value.row, _second.Value.column);
            _second = null;
        }
    }

    private async void Compare()
    {
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        if (_firstId == _secondId)
            Match();
        else
            NoMatch();
        _first = null;
        _second = null;
        _moves++;
        _firstId = 0;
        _secondId = 0;
        _clicks = 0;
    }

    public void Click(Position position)
    {
        var row = position.Row;
        var column = position.Column;
        int option = _board[row, column];
        if (_clicks <= 1 && _matches.IndexOf(option) < 0)
        {
            // First Choice
            if (_row == null && _column == null)
            {
                _clicks++;
                _firstId = option;
                _first = (row, column);
                _assets[row, column] = _options[option];
                SetAsset(row, column);
                _row = row;
                _column = column;
            }
            // Second Choice
            else if (!(_row == row && _column == column))
            {
                _clicks++;
                _secondId = option;
                _second = (row, column);
                _assets[row, column] = _options[option];
                SetAsset(row, column);
                Compare();
                _row = null;
                _column = null;
            }
        }
    }

    private void Layout()
    {
        int counter = 0;
        List<int> values = new();
        while (values.Count <= size * size)
        {
            List<int> numbers = Choose(1, size * 2, size * 2);
            for (int number = 0; number < size * 2; number++)
                values.Add(numbers[number]);
        }
        List<int> indices = Choose(1, size * size, size * size);
        for (int column = 0; column < size; column++)
        {
            for (int row = 0; row < size; row++)
            {
                _board[column, row] = values[indices[counter] - 1];
                _assets[column, row] = FluentEmojiType.PurpleSquare;
                counter++;
            }
        }
        Display = new(size, size, FluentEmojiType.PurpleSquare);
    }

    public void New()
    {
        Layout();
        _moves = 0;
        _clicks = 0;
        _row = null;
        _column = null;
        _matches.Clear();
        Message = title;
    }

    public string Message { get; private set; } = string.Empty;

    public Display Display { get; private set; } =
        new(size, size, FluentEmojiType.PurpleSquare);

    public Action? Updated { get; set; }

    public MemoryGame() => New();
}

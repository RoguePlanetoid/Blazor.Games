namespace Blazor.Games;

public class HitOrMiss
{
    private const int score = 18;
    private const int size = 6;
    private const string hit = "X";
    private const string miss = "O";
    private readonly string[,] _board = new string[size, size];
    private readonly Random _random = new((int)DateTime.UtcNow.Ticks);

    private int _go = 0;
    private int _hits = 0;
    private int _misses = 0;
    private bool _won = false;

    private List<int> Choose(int minimum, int maximum, int total) =>
        Enumerable.Range(minimum, maximum)
            .OrderBy(r => _random.Next(minimum, maximum))
                .Take(total).ToList();

    private void Layout()
    {
        for (int row = 0; row < size; row++)
            for (int column = 0; column < size; column++)
                _board[row, column] = string.Empty;
        Display = new(size, size, FluentEmojiType.PurpleSquare);
    }

    public void Click(Position position)
    {
        if (!_won)
        {
            var selected = _board[position.Row, position.Column];
            var item = Display.Get(position);
            if (item?.Emoji == FluentEmojiType.PurpleSquare)
            {
                var emoji = selected == hit ? FluentEmojiType.Collision : FluentEmojiType.Hole;
                Display.Set(position, emoji);
                if (selected == hit)
                    _hits++;
                else if (selected == miss)
                    _misses++;
                _go++;
            }
            if (_go < (size * size) && _misses < score)
            {
                if (_hits == score)
                {
                    Message = $"You Won! With {_hits} hits and {_misses} misses";
                    _won = true;
                }
            }
            else
            {
                Message = $"You Lost! With {_hits} hits and {_misses} misses";
                _won = true;
            }
        }
    }

    public void New()
    {
        Layout();
        _go = 0;
        _hits = 0;
        _misses = 0;
        _won = false;
        int index = 0;
        List<string> values = new();
        while (values.Count < (size * size))
        {
            values.Add(hit);
            values.Add(miss);
        }
        List<int> indices = Choose(1, size * size, size * size);
        for (int column = 0; column < size; column++)
        {
            for (int row = 0; row < size; row++)
            {
                _board[column, row] = values[indices[index] - 1];
                index++;
            }
        }
        Message = string.Empty;
    }

    public string Message { get; private set; } = string.Empty;

    public Display Display { get; private set; } =
        new(size, size, FluentEmojiType.PurpleSquare);

    public HitOrMiss() => New();
}

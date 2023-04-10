namespace Blazor.Games;

public class FlagsGame
{
    private const string space = " ";
    private const int size = 3;

    private readonly Random _random = new((int)DateTime.UtcNow.Ticks);
    private readonly List<FlagType> _flags = new()
    {
        { FlagType.Armenia },
        { FlagType.Austria },
        { FlagType.Belgium },
        { FlagType.Bulgaria },
        { FlagType.Estonia },
        { FlagType.France },
        { FlagType.Gabon },
        { FlagType.Germany },
        { FlagType.Guinea },
        { FlagType.Ireland },
        { FlagType.Italy },
        { FlagType.Lithuania },
        { FlagType.Luxembourg },
        { FlagType.Mali },
        { FlagType.Netherlands },
        { FlagType.Nigeria },
        { FlagType.Romania },
        { FlagType.Hungary },
        { FlagType.SierraLeone },
        { FlagType.Yemen }
    };

    private List<int> _indexes = new();
    private List<int> _choices = new();
    private FlagType _flag;
    private int _turns;
    private bool _over;

    private List<int> Choose(int minimum, int maximum, int total) =>
        Enumerable.Range(minimum, maximum)
            .OrderBy(r => _random.Next(minimum, maximum))
                .Take(total).ToList();

    private string Name(FlagType flag) =>
        Enum.GetName(typeof(FlagType), flag) ?? string.Empty;

    private string Country(FlagType flag) =>
        string.Join(space, new Regex(@"\p{Lu}\p{Ll}*")
            .Matches(Name(flag))
                .Select(s => s.Value));

    private void Select()
    {
        var choice = _choices[_turns];
        var index = _indexes[choice];
        _flag = _flags[index];
        Message = Country(_flag);
        _turns++;
    }

    private void Layout()
    {
        var index = 0;
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                Display.Set(row, column, _flags[_indexes[index]]);
                index++;
            }
        }
    }

    public void Click(Position position)
    {
        if (!_over)
        {
            if (_flag == Display.Get(position)?.Flag)
            {
                Display.Set(position, FluentEmojiType.PurpleSquare);
                if (_turns < size * size)
                    Select();
                else
                    Message = "You Won!";
            }
            else
                _over = true;
        }
        if (_over)
            Message = "Game Over!";
    }

    public void New()
    {
        _turns = 0;
        _over = false;
        _indexes = Choose(0, _flags.Count, _flags.Count);
        _choices = Choose(0, size * size, size * size);
        Layout();
        Select();
    }

    public string Message { get; private set; } = string.Empty;

    public Display Display { get; private set; } =
        new(size, size, FluentEmojiType.PurpleSquare);

    public FlagsGame() => New();
}

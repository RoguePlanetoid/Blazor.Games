namespace Blazor.Games;

public class FruitGame
{
    private const int size = 3;
    private const int rows = 1;
    private const int columns = 3;
    private readonly Dictionary<int, FluentEmojiType> _options = new()
    {
        { 0, FluentEmojiType.SlotMachine },
        { 1, FluentEmojiType.GreenApple },
        { 2, FluentEmojiType.Grapes },
        { 3, FluentEmojiType.Lemon },
        { 4, FluentEmojiType.Cherries },
        { 5, FluentEmojiType.Banana },
        { 6, FluentEmojiType.Melon },
        { 7, FluentEmojiType.Tangerine },
        { 8, FluentEmojiType.Bell }
    };

    private readonly Random _random = new((int)DateTime.UtcNow.Ticks);

    private int _spins;

    private List<int> Choose(int minimum, int maximum, int total)
    {
        var choose = new List<int>();
        var values = Enumerable.Range(minimum, maximum).ToList();
        for (int index = 0; index < total; index++)
            choose.Add(values[_random.Next(0, values.Count)]);
        return choose;
    }

    public void Click(Position _)
    {
        var values = Choose(1, _options.Count - 1, size);
        for (int index = 0; index < size; index++)
            for (int option = 1; option <= values[index]; option++)
                Display.Set(0, index, _options[option]);
        _spins++;
        if (values.All(a => a.Equals(values.First())))
        {
            Message = $"Spin {_spins} matched";
            _spins = 0;
        }
    }

    public void New()
    {
        _spins = 0;
        Message = string.Empty;
        Display = new(rows, columns, FluentEmojiType.SlotMachine);
    }

    public string Message { get; private set; } = string.Empty;

    public Display Display { get; private set; } =
        new(rows, columns, FluentEmojiType.SlotMachine);

    public FruitGame() => New();
}

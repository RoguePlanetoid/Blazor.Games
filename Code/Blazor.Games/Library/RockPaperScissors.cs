namespace Blazor.Games;

public class RockPaperScissors
{
    private const int size = 3;
    private const int lost = 0;
    private const int win = 1;
    private const int draw = 2;
    private const int rows = 1;
    private const int columns = 3;

    private static readonly int[,] _match = new int[size, size]
    {
        { draw, lost, win },
        { win, draw, lost },
        { lost, win, draw }
    };
    private static readonly FluentEmojiType[] _assets = new FluentEmojiType[]
    {
        FluentEmojiType.Rock,
        FluentEmojiType.PageWithCurl,
        FluentEmojiType.Scissors
    };
    private static readonly string[] _values = new string[]
    {
        "Rock", "Paper", "Scissors"
    };
    private static readonly string[] _results = new string[]
    {
        "Lost!", "Win!", "Draw!"
    };
    private readonly Random _random = new((int)DateTime.UtcNow.Ticks);

    public void Click(Position position)
    {
        for(int i = 0; i < _assets.Length; i++)
            Computer.Set(0, i, FluentEmojiType.BlackLargeSquare);
        var option = position.Column;
        int computer = _random.Next(0, size);
        var result = _match[option, computer];
        Computer.Set(0, computer, _assets[computer]);
        PlayerMessage = $"You Picked - {_values[option]} - {_results[result]}";
        ComputerMessage = $"Computer Picked - {_values[computer]}";
    }

    public void New()
    {
        Computer = new(rows, columns, FluentEmojiType.BlackLargeSquare);
        PlayerMessage = "Your Pick";
        ComputerMessage = "Computer Pick";
    }

    public string PlayerMessage { get; private set; } = string.Empty;

    public string ComputerMessage { get; private set; } = string.Empty;

    public Display Player { get; private set; } = 
        new(rows, columns, _assets);

    public Display Computer { get; private set; } =
        new(rows, columns, FluentEmojiType.BlackLargeSquare);

    public RockPaperScissors() => New();
}

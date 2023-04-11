namespace Blazor.Games;

public class EmojiGame
{
    private const string space = " ";
    private const int rounds = 12;
    private const int options = 2;
    private const int size = 3;

    private readonly Random _random = new((int)DateTime.UtcNow.Ticks);
    private readonly List<FluentEmojiType> _emoji = new()
    {
        FluentEmojiType.GrinningFace,
        FluentEmojiType.BeamingFaceWithSmilingEyes,
        FluentEmojiType.FaceWithTearsOfJoy,
        FluentEmojiType.GrinningSquintingFace,
        FluentEmojiType.WinkingFace,
        FluentEmojiType.FaceSavoringFood,
        FluentEmojiType.SmilingFace,
        FluentEmojiType.HuggingFace,
        FluentEmojiType.ThinkingFace,
        FluentEmojiType.FaceWithRaisedEyebrow,
        FluentEmojiType.NeutralFace,
        FluentEmojiType.ExpressionlessFace,
        FluentEmojiType.FaceWithRollingEyes,
        FluentEmojiType.PerseveringFace,
        FluentEmojiType.FaceWithOpenMouth,
        FluentEmojiType.HushedFace,
        FluentEmojiType.SleepyFace,
        FluentEmojiType.TiredFace,
        FluentEmojiType.SleepingFace,
        FluentEmojiType.RelievedFace,
        FluentEmojiType.UnamusedFace,
        FluentEmojiType.PensiveFace,
        FluentEmojiType.ConfusedFace,
        FluentEmojiType.AstonishedFace,
        FluentEmojiType.FrowningFace,
        FluentEmojiType.ConfoundedFace,
        FluentEmojiType.DisappointedFace,
        FluentEmojiType.WorriedFace,
        FluentEmojiType.FaceWithSteamFromNose,
        FluentEmojiType.AnguishedFace,
        FluentEmojiType.FearfulFace,
        FluentEmojiType.FlushedFace,
        FluentEmojiType.ZanyFace,
        FluentEmojiType.FaceExhaling,
        FluentEmojiType.AngryFace,
        FluentEmojiType.NerdFace    
    };
    private readonly List<FluentEmojiType> _items = new();

    private List<int> _selected = new();
    private List<int> _options = new();
    private List<int> _indexes = new();
    private FluentEmojiType _correct;
    private int _round;
    private bool _over;

    private List<int> ChooseValues(int minimum, int maximum, int total)
    {
        var choose = new List<int>();
        var values = Enumerable.Range(minimum, maximum).ToList();
        for (int index = 0; index < total; index++)
            choose.Add(values[_random.Next(0, values.Count)]);
        return choose;
    }

    private List<int> ChooseUnique(int minimum, int maximum, int total) =>
        Enumerable.Range(minimum, maximum)
            .OrderBy(r => _random.Next(minimum, maximum))
                .Take(total).ToList();

    private static string Name(FluentEmojiType item) =>
        Enum.GetName(typeof(FluentEmojiType), item) ?? string.Empty;

    private static string GetQuestion(FluentEmojiType item) =>
        string.Join(space, new Regex(@"\p{Lu}\p{Ll}*")
            .Matches(Name(item))
                .Select(s => s.Value));

    private static List<int> Indexes(IEnumerable<FluentEmojiType> items) =>
        items.Select(item => Array.IndexOf(items.ToArray(), item))
            .ToList();

    private bool Next()
    {
        if (_round < rounds)
        {
            _items.Clear();
            _correct = _emoji[_selected[_round]];
            Message = GetQuestion(_correct);
            var incorrect = ChooseUnique(0, _options.Count - 1, options);
            var indexOne = _options[incorrect.First()];
            var indexTwo = _options[incorrect.Last()];
            var one = _emoji[indexOne];
            var two = _emoji[indexTwo];
            _options.Remove(indexOne);
            _options.Remove(indexTwo);
            var indexes = ChooseUnique(0, options + 1, options + 1);
            var dict = new Dictionary<int, FluentEmojiType>()
            {
                { indexes[0], _correct },
                { indexes[1], one },
                { indexes[2], two }
            };
            _items.AddRange(dict.OrderBy(o => o.Key)
                .Select(item => item.Value));
            Display = new Display(1, size, _items.ToArray());
            _round++;
            return true;
        }
        return false;
    }

    private bool Correct(FluentEmojiType emoji)
    {
        for (int index = 0; index < _items.Count; index++)
            if (_correct == _items[index])
                Display.Set(0, index, GetQuestion(_items[index]));
        return _correct == emoji;
    }

    public void Click(Position position)
    {
        int index = position.Column;
        if (!_over)
        {
            if (Correct(_items[index]))
            {
                if (!Next())
                {
                    Message = "Game Over, You Won";
                    _over = true;
                }
            }
            else
            {
                Message = "Incorrect, You Lost!";
                _over = true;
            }
        }
        else
            Message = "Game Over";
    }

    public void New()
    {
        _round = 0;
        _over = false;
        Message = string.Empty;
        _indexes = Indexes(_emoji);
        _selected = ChooseValues(0, _indexes.Count, rounds);
        _options = _indexes.Where(index => !_selected.Any(selected => selected == index)).ToList();
        Next();
    }

    public string Message { get; private set; } = string.Empty;

    public Display Display { get; private set; } =
        new(1, size, FluentEmojiType.PurpleSquare);

    public EmojiGame() => New();
}


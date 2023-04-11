namespace Blazor.Games;

public class FourInRow
{
    private const int total = 3;
    private const int size = 7;
    private readonly string[] _players = { string.Empty, "Yellow", "Red" };
    private readonly int[,] _board = new int[size, size];

    private int _value = 0;
    private int _amend = 0;
    private int _player = 0;
    private bool _won = false;

    private bool CheckVertical(int row, int column)
    {
        _value = 0;
        do
        {
            _value++;
        }
        while (row + _value < size &&
        _board[column, row + _value] == _player);
        return _value > total;
    }

    private bool CheckHorizontal(int row, int column)
    {
        _value = 0;
        _amend = 0;
        // From Left
        do
        {
            _value++;
        }
        while (column - _value >= 0 &&
        _board[column - _value, row] == _player);
        if (_value > total)
            return true;
        // Deduct Middle - Prevent double count
        _value -= 1;
        // Then Right
        do
        {
            _value++;
            _amend++;
        }
        while (column + _amend < size &&
        _board[column + _amend, row] == _player);
        return _value > total;
    }

    private bool CheckDiagonalTopLeft(int row, int column)
    {
        _value = 0;
        _amend = 0;
        // From Top Left
        do
        {
            _value++;
        }
        while (column - _value >= 0 && row - _value >= 0 &&
        _board[column - _value, row - _value] == _player);
        if (_value > total)
            return true;
        _value -= 1; // Deduct Middle - Prevent double count
                     // To Bottom Right
        do
        {
            _value++;
            _amend++;
        }
        while (column + _amend < size && row + _amend < size &&
        _board[column + _amend, row + _amend] == _player);
        return _value > total;
    }

    private bool CheckDiagonalTopRight(int row, int column)
    {
        _value = 0;
        _amend = 0;
        // From Top Right
        do
        {
            _value++;
        }
        while (column + _value < size && row - _value >= 0 &&
        _board[column + _value, row - _value] == _player);
        if (_value > total)
            return true;
        _value -= 1; // Deduct Middle - Prevent double count
                     // To Bottom Left
        do
        {
            _value++;
            _amend++;
        }
        while (column - _amend >= 0 &&
        row + _amend < size &&
        _board[column - _amend,
        row + _amend] == _player);
        return _value > total;
    }

    private bool Winner(int row, int column)
    {
        bool vertical = CheckVertical(row, column);
        bool horizontal = CheckHorizontal(row, column);
        bool diagonalTopLeft = CheckDiagonalTopLeft(row, column);
        bool diagonalTopRight = CheckDiagonalTopRight(row, column);
        return vertical || horizontal ||
        diagonalTopLeft || diagonalTopRight;
    }

    private bool Full()
    {
        for (int row = 0; row < size; row++)
            for (int column = 0; column < size; column++)
                if (_board[column, row] == 0)
                    return false;
        return true;
    }

    private void SetAsset(int row, int column) =>
        Display.Set(row, column, _board[column, row] switch
        {
            1 => new Asset(FluentEmojiType.YellowCircle),
            2 => new Asset(FluentEmojiType.RedCircle),
            _ => new Asset(FluentEmojiType.PurpleSquare)
        });

    private void Set(int row, int column)
    {
        for (int i = size - 1; i > -1; i--)
        {
            if (_board[column, i] == 0)
            {
                _board[column, i] = _player;
                SetAsset(i, column);
                row = i;
                break;
            }
        }
        if (Winner(row, column))
        {
            _won = true;
            Message = $"{_players[_player]} has won!";
        }
        else if (Full())
            Message = "Board Full!";
        else
        {
            _player = _player == 1 ? 2 : 1;
            Message = $"{_players[_player]} turn";
        }
    }

    private void Layout()
    {
        for (int row = 0; row < size; row++)
            for (int column = 0; column < size; column++)
                _board[row, column] = 0;
        Display = new(size, size, FluentEmojiType.PurpleSquare);
    }

    public void New()
    {
        Layout();
        _won = false;
        _player = 1;
        Message = $"{_players[_player]} start";
    }

    public void Click(Position position)
    {
        var row = position.Row;
        var column = position.Column;
        if (!_won)
        {
            if (_board[column, 0] == 0)
                Set(row, column);
        }
        else
            Message = "Game Over!";
    }


    public string Message { get; private set; } = string.Empty;

    public Display Display { get; private set; } =
        new(size, size, FluentEmojiType.PurpleSquare);

    public FourInRow() => New();
}
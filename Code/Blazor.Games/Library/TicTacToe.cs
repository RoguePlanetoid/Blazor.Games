namespace Blazor.Games;

public class TicTacToe
{
    private const string blank = " ";
    private const string nought = "O";
    private const string cross = "X";
    private const int size = 3;

    private readonly string[,] _board = new string[size, size];
    private string _piece = nought;
    private bool _won = false;

    private bool Winner() =>
        _board[0, 0] == _piece && _board[0, 1] ==
        _piece && _board[0, 2] == _piece ||
        _board[1, 0] == _piece && _board[1, 1] ==
        _piece && _board[1, 2] == _piece ||
        _board[2, 0] == _piece && _board[2, 1] ==
        _piece && _board[2, 2] == _piece ||
        _board[0, 0] == _piece && _board[1, 0] ==
        _piece && _board[2, 0] == _piece ||
        _board[0, 1] == _piece && _board[1, 1] ==
        _piece && _board[2, 1] == _piece ||
        _board[0, 2] == _piece && _board[1, 2] ==
        _piece && _board[2, 2] == _piece ||
        _board[0, 0] == _piece && _board[1, 1] ==
        _piece && _board[2, 2] == _piece ||
        _board[0, 2] == _piece && _board[1, 1] ==
        _piece && _board[2, 0] == _piece;
    private bool Drawn() =>
        _board[0, 0] != blank && _board[0, 1] !=
        blank && _board[0, 2] != blank &&
        _board[1, 0] != blank && _board[1, 1] !=
        blank && _board[1, 2] != blank &&
        _board[2, 0] != blank && _board[2, 1] !=
        blank && _board[2, 2] != blank;

    private void SetAsset(Position position) =>
        Display.Set(position, _board[position.Row, position.Column] switch
        {
            nought => new Asset(FluentEmojiType.HollowRedCircle),
            cross => new Asset(FluentEmojiType.CrossMark),
            _ => new Asset(FluentEmojiType.PurpleSquare)
        });

    private void Layout()
    {
        for (int row = 0; row < size; row++)
            for (int column = 0; column < size; column++)
                _board[row, column] = blank;
        Display = new(size, size, FluentEmojiType.PurpleSquare);
    }

    public void Click(Position position)
    {
        var row = position.Row; 
        var column = position.Column;
        if (!_won)
        {
            if (_board[row, column] == blank)
            {
                _board[row, column] = _piece;
                SetAsset(position);
                if (Winner())
                {
                    Message = $"{_piece} wins!";
                    _won = true;
                }
                else if (Drawn())
                    Message = "Draw!";
                else
                {
                    _piece = _piece == cross ? nought : cross;
                    Message = $"{_piece} turn";
                }
            }
        }
        else
            Message = "Game Over!";
    }

    public void New()
    {
        Layout();
        _won = false;
        _piece = nought;
        Message = $"{_piece} start";
    }

    public string Message { get; private set; } = string.Empty;

    public Display Display { get; private set; } =
        new(size, size, FluentEmojiType.PurpleSquare);

    public TicTacToe() => New();
}
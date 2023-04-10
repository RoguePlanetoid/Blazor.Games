namespace Blazor.Games;

public class Display
{
    public Display() { }

    public Display(int rows, int columns, FluentEmojiType emoji) : 
        this(rows, columns, new Asset(emoji)) { }

    public Display(int rows, int columns, Asset? asset)
    {
        Rows.Clear();
        for (int row = 0; row < rows; row++)
        {
            var item = new Row();
            for (int column = 0; column < columns; column++)
                item.Columns.Add(new Column(asset));
            Rows.Add(item);
        }
    }

    public Display(int rows, int columns, FluentEmojiType[] emojis) :
        this(rows, columns, emojis.Select(emoji => new Asset(emoji)).ToArray()) { }

    public Display(int rows, int columns, Asset[]? assets)
    {
        if (rows * columns == assets?.Length)
        {
            int index = 0;
            Rows.Clear();
            for (int row = 0; row < rows; row++)
            {
                var item = new Row();
                for (int column = 0; column < columns; column++)
                {
                    item.Columns.Add(new Column(assets[index]));
                    index++;
                }
                Rows.Add(item);
            }
        }
    }

    public List<Row> Rows { get; private set; } = new();

    public void Set(int row, int column, FluentEmojiType emoji) =>
        Set(row, column, new Asset(emoji));

    public void Set(Position position, FluentEmojiType emoji) =>
        Set(position.Row, position.Column, new Asset(emoji));

    public void Set(int row, int column, Asset? asset)
    {
        if (row <= Rows.Count && column <= Rows[row].Columns.Count)
            Rows[row].Columns[column].Asset = asset;
    }

    public void Set(int row, int column, string? message)
    {
        if (row <= Rows.Count && column <= Rows[row].Columns.Count)
            Rows[row].Columns[column].Message = message;
    }

    public void Set(Position position, Asset? asset) =>
        Set(position.Row, position.Column, asset);

    public void Set(int row, int column, FlagType flag) =>
        Set(row, column, new Asset(flag));

    public Asset? Get(int row, int column) => 
        row <= Rows.Count && column <= Rows[row].Columns.Count ? 
            Rows[row].Columns[column].Asset : null;

    public Asset? Get(Position position) =>
        Get(position.Row, position.Column);
}

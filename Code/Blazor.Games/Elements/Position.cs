namespace Blazor.Games;

public class Position
{
    public Position(int row, int column) =>
        (Row, Column) = (row, column);

    public int Row {  get; set; }

    public int Column { get; set; } 
}

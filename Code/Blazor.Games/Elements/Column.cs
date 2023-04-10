namespace Blazor.Games;

public class Column
{
    public Column(Asset? asset) => Asset = asset;

    public Asset? Asset { get; set; }

    public string? Message { get; set; }
}
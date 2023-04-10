namespace Blazor.Games;

public class Asset
{
    public Asset(FlagType flag) =>
        Flag = flag;

    public Asset(FluentEmojiType emoji) => 
        Emoji = emoji;

    public FlagType? Flag { get; set; }
    public FlagSet FlagSet { get; set; } = FlagSet.Square;
    public FluentEmojiType? Emoji { get; set; }

    public AssetResource ToAssetResource()
    {
        if (Flag != null)
            return Comentsys.Assets.Flags.Flag.Get(FlagSet, Flag.Value);
        if (Emoji != null)
            return ShadedFluentEmoji.Get(Emoji.Value);
        else
            return new();
    }
}

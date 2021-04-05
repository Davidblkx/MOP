namespace MOP.Terminal.Models.Messages
{
    internal static class OutActorMessages
    {
        public static Print Print(string toPrint) => new(toPrint, false);
        public static Print PrintAndExit(string toPrint) => new(toPrint, true);
        public static Clear Clear() => new();
    }

    /// <summary>
    /// Message to print, if exit = true, call for termination after
    /// </summary>
    internal record Print(string ToPrint, bool Exit);

    /// <summary>
    /// Message to clear output
    /// </summary>
    internal class Clear { }
}

namespace MOP.Core.Akka
{
    /// <summary>
    /// Allow to set the priority of a message
    /// </summary>
    public interface IPriorityMessage
    {
        public int Priority { get; }
    }
}

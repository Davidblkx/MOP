namespace MOP.Infra.Domain.Events
{
    /// <summary>
    /// Empty object to use on event commands without body
    /// </summary>
    public struct Unit {
        public int Value { get; set; }
    }
}

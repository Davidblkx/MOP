using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;

namespace MOP.Core.Akka
{
    public class GenericPriorityMailbox : UnboundedPriorityMailbox
    {
        public static string Name => "generic-priority-mailbox";

        public GenericPriorityMailbox(Settings settings, Config config)
            : base(settings, config) { }

        protected override int PriorityGenerator(object message)
        {
            if (message is IPriorityMessage e)
                return e.Priority;

            return 10;
        }
    }
}

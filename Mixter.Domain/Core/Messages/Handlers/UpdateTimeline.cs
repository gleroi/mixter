using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Domain.Core.Messages.Handlers
{
    [Handler]
    public class UpdateTimeline : IEventHandler<MessageQuacked>
    {
        private readonly ITimelineMessageRepository repository;

        public UpdateTimeline(ITimelineMessageRepository repository) {
            this.repository = repository;
        }

        public void Handle(MessageQuacked evt)
        {
            var tlMsg = new TimelineMessageProjection(evt.Author, evt.Author, evt.Content, evt.Id);
            repository.Save(tlMsg);
        }

    }
}

using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Aggregate]
    public class Subscription
    {
        private DecisionProjection _projection = new DecisionProjection();

        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee) {
            var evt = new UserFollowed(new SubscriptionId(follower, followee));
            eventPublisher.Publish(evt);
        }

        public Subscription(IDomainEvent[] events) {
            foreach (var @event in events)
            {
                _projection.Apply(@event);
            }
        }

        public void Unfollow(IEventPublisher eventPublisher) {
            var evt = new UserUnfollowed(_projection.Id);
            eventPublisher.Publish(evt);
        }

        private class DecisionProjection : DecisionProjectionBase
        {
            public DecisionProjection() {
                AddHandler<UserFollowed>(When);
            }

            public SubscriptionId Id { get; private set; }

            void When(UserFollowed evt) {
                this.Id = evt.SubscriptionId;
            }
        }
    }
}

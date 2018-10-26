using Mixter.Domain.Core.Messages;
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

        public void NotifyFollower(IEventPublisher eventPublisher, MessageId msgId) {
            if (!_projection.IsActive) {
                return;
            }
            
            var evt = new FolloweeMessageQuacked(_projection.Id, msgId);
            eventPublisher.Publish(evt);
        }


        private class DecisionProjection : DecisionProjectionBase
        {
            public DecisionProjection() {
                AddHandler<UserFollowed>(When);
            }

            public SubscriptionId Id { get; private set; }
            public bool IsActive {get; private set;}

            void When(UserFollowed evt) {
                this.Id = evt.SubscriptionId;
                this.IsActive = true;
            }

            void When(UserUnfollowed evt) {
                if (this.Id.Equals(evt.SubscriptionId)) {
                    throw new DomainException("invalid subscription id for unfollow");
                }
                this.IsActive = false;
            }

        }
    }
}

using Mixter.Domain.Core.Subscriptions.Events;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    [Handler]
    public class UpdateFollowers : 
        IEventHandler<UserFollowed>,
        IEventHandler<UserUnfollowed>
    {
        private IFollowersRepository _followersRepository;

        public UpdateFollowers(IFollowersRepository followersRepository)
        {
            _followersRepository = followersRepository;
        }

        public void Handle(UserFollowed evt)
        {
            _followersRepository.Save(new FollowerProjection(evt.SubscriptionId.Followee, evt.SubscriptionId.Follower));
        }

        public void Handle(UserUnfollowed evt)
        {
            _followersRepository.Remove(new FollowerProjection(evt.SubscriptionId.Followee, evt.SubscriptionId.Follower));
        }
    }
}

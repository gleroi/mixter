using Mixter.Domain.Core.Subscriptions.Events;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    [Handler]
    public class UpdateFollowers :
        IEventHandler<UserFollowed>
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
    }
}

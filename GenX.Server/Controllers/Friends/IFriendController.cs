namespace GenX.Server.Controllers.Friends;

public interface IFriendController
{
    Task<List<FriendResult>> GetFriendsAsync(int userId);

    Task<bool> AddFriendAsync(int fromUserId, int toUserId);

    Task<bool> AcceptFriendAsync(int userId, int friendid);

    Task<bool> DenyFriendAsync(int userId, int friendid);
}
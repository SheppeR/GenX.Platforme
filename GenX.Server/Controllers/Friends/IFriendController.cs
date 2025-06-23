using GenX.Server.Database;

namespace GenX.Server.Controllers.Friends;

public interface IFriendController
{
    Task<List<DbUser>> GetFriendsAsync(int userId);

    Task<bool> AddFriendAsync(int fromUserId, int toUserId);

    Task<bool> AcceptFriendAsync(int userId, int requesterId);

    Task<List<DbUser>> GetPendingFriendRequestsAsync(int userId);
}
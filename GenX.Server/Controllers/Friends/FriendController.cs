using GenX.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace GenX.Server.Controllers.Friends;

public class FriendController(IAppDBContext appDbContext) : IFriendController
{
    public async Task<List<DbUser>> GetFriendsAsync(int userId)
    {
        var friendIds = await appDbContext.DbFriend
            .Where(f => (f.UserId == userId || f.FriendId == userId) && f.IsAccepted)
            .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
            .ToListAsync();

        return await appDbContext.DbUser
            .Where(u => friendIds.Contains(u.ID))
            .ToListAsync();
    }

    public async Task<List<DbUser>> GetPendingFriendRequestsAsync(int userId)
    {
        var friendIds = await appDbContext.DbFriend
            .Where(f => (f.UserId == userId || f.FriendId == userId) && !f.IsAccepted)
            .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
            .ToListAsync();

        return await appDbContext.DbUser
            .Where(u => friendIds.Contains(u.ID))
            .ToListAsync();
    }

    public async Task<bool> AddFriendAsync(int fromUserId, int toUserId)
    {
        if (fromUserId == toUserId)
            return false;

        var exists = await appDbContext.DbFriend.AnyAsync(f =>
            (f.UserId == fromUserId && f.FriendId == toUserId) ||
            (f.UserId == toUserId && f.FriendId == fromUserId));

        if (exists)
            return false;

        appDbContext.DbFriend.Add(new DbFriend
        {
            UserId = fromUserId,
            FriendId = toUserId,
            IsAccepted = false
        });

        await appDbContext.SaveChanges();
        return true;
    }

    public async Task<bool> AcceptFriendAsync(int userId, int requesterId)
    {
        var request = await appDbContext.DbFriend.FirstOrDefaultAsync(f =>
            f.UserId == requesterId && f.FriendId == userId && !f.IsAccepted);

        if (request == null)
            return false;

        request.IsAccepted = true;
        await appDbContext.SaveChanges();
        return true;
    }
}
using GenX.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace GenX.Server.Controllers.Friends;

public class FriendController(IAppDBContext appDbContext) : IFriendController
{
    public async Task<List<FriendResult>> GetFriendsAsync(int userId)
    {
        var outgoingFriends = appDbContext.DbFriend
            .Where(f => f.UserId == userId)
            .Select(f => new FriendResult
            {
                Friend = f.Friend,
                IsAccepted = f.IsAccepted
            });

        var incomingFriends = appDbContext.DbFriend
            .Where(f => f.FriendId == userId)
            .Select(f => new FriendResult
            {
                Friend = f.User,
                IsAccepted = f.IsAccepted
            });

        var friends = await outgoingFriends
            .Concat(incomingFriends)
            .OrderBy(f => f.Friend.Pseudo)
            .ToListAsync();

        return friends;
    }

    public async Task<bool> AddFriendAsync(int fromUserId, int toUserId)
    {
        if (fromUserId == toUserId)
        {
            return false;
        }

        var exists = await appDbContext.DbFriend.AnyAsync(f =>
            (f.UserId == fromUserId && f.FriendId == toUserId) ||
            (f.UserId == toUserId && f.FriendId == fromUserId));

        if (exists)
        {
            return false;
        }

        appDbContext.DbFriend.Add(new DbFriend
        {
            UserId = fromUserId,
            FriendId = toUserId,
            IsAccepted = false
        });

        await appDbContext.SaveChanges();
        return true;
    }

    public async Task<bool> AcceptFriendAsync(int userId, int friendid)
    {
        var request = await appDbContext.DbFriend.FirstOrDefaultAsync(f =>
            f.UserId == userId && f.FriendId == friendid && !f.IsAccepted);

        if (request == null)
        {
            return false;
        }

        request.IsAccepted = true;
        await appDbContext.SaveChanges();
        return true;
    }

    public async Task<bool> DenyFriendAsync(int userId, int friendid)
    {
        var request = await appDbContext.DbFriend.FirstOrDefaultAsync(f =>
            f.UserId == userId && f.FriendId == friendid && !f.IsAccepted);
        if (request == null)
        {
            return false;
        }

        appDbContext.DbFriend.Remove(request);
        await appDbContext.SaveChanges();


        return true;
    }
}

public class FriendResult
{
    public DbUser Friend { get; set; } = null!;
    public bool IsAccepted { get; set; }
}
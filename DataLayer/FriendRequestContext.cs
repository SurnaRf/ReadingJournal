using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public class FriendRequestContext : IDb<FriendRequest, string>
	{
		private readonly ReadingJournalDbContext dbContext;

		public FriendRequestContext(ReadingJournalDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task CreateAsync(FriendRequest item)
		{
			try
			{
				User senderFromDb = await dbContext.Users.FindAsync(item.SenderId);
				User receiverFromDb = await dbContext.Users.FindAsync(item.ReceiverId);

				if (senderFromDb != null)
				{
					item.Sender = senderFromDb;
				}

				if (receiverFromDb != null)
				{
					item.Receiver = receiverFromDb;
				}

				dbContext.FriendRequests.Add(item);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<FriendRequest> ReadAsync(string key, bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<FriendRequest> query = dbContext.FriendRequests;

				if (useNavigationalProperties)
				{
					query = query.Include(fr => fr.Sender).Include(fr => fr.Receiver);
				}

				if (isReadOnly)
				{
					query = query.AsNoTracking();
				}

				return await query.FirstOrDefaultAsync(fr => fr.RequestId == key);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<ICollection<FriendRequest>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
		{
			try
			{
				IQueryable<FriendRequest> query = dbContext.FriendRequests;

				if (useNavigationalProperties)
				{
					query = query.Include(fr => fr.Sender).Include(fr => fr.Receiver);
				}

				if (isReadOnly)
				{
					query = query.AsNoTracking();
				}

				return await query.ToListAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task UpdateAsync(FriendRequest item, bool useNavigationalProperties = false)
		{
			try
			{
				FriendRequest requestFromDb = await ReadAsync(item.RequestId, useNavigationalProperties, false);

				if (requestFromDb == null)
				{
					throw new ArgumentException("Friend request not found!");
				}

				dbContext.Entry(requestFromDb).CurrentValues.SetValues(item);

				if (useNavigationalProperties)
				{
					User senderFromDb = await dbContext.Users.FindAsync(item.SenderId);
					User receiverFromDb = await dbContext.Users.FindAsync(item.ReceiverId);

					if (senderFromDb != null)
					{
						requestFromDb.Sender = senderFromDb;
					}

					else
					{
						requestFromDb.Sender = item.Sender;
					}

					if (receiverFromDb != null)
					{
						requestFromDb.Receiver = receiverFromDb;
					}

					else
					{
						requestFromDb.Receiver = item.Receiver;
					}
				}

				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task DeleteAsync(string key)
		{
			try
			{
				FriendRequest requestFromDb = await ReadAsync(key, false, false);

				if (requestFromDb == null)
				{
					throw new ArgumentException("Friend request not found!");
				}

				dbContext.FriendRequests.Remove(requestFromDb);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

        public async Task<List<FriendRequest>> GetFriendRequestsForUserAsync(string userId, bool useNavigationalProperties = false)
        {
            IQueryable<FriendRequest> query = dbContext.FriendRequests
									.Where(fr => fr.ReceiverId == userId && !fr.IsAccepted);

            if (useNavigationalProperties)
            {
                query = query.Include(fr => fr.Sender).Include(fr => fr.Receiver);
            }


			return await query.ToListAsync();
		}
    }
}

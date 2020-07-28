using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess
{
    public class ConversationManager : IConversationManager
    {
        private ApplicationDbContext db;
        public ConversationManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public async Task<Conversation> AddConversation(Conversation conversation)
        {
            if (conversation.Participations.Count < 2)
                return null;
            if (!conversation.IsGroup)
            {
                //only 1 private conversation allowed
                var userIds = conversation.Participations.Select(cu => cu.UserId).ToList();
                var check = await (from c in db.Conversations
                                   where c.Participations.Count() <= 2 &&
                                    c.Participations.All(cu => cu.UserId == userIds[0] || cu.UserId == userIds[1])
                                   select c).SingleOrDefaultAsync();
                if (check != null)
                    return null;
            }
            db.Entry(conversation).State = EntityState.Added;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return conversation;
        }

        public async Task<List<Conversation>> GetConversations(string userId, int count, DateTime? olderThan)
        {
            var query = from c in db.Conversations
                        join cu in db.ConversationUsers on c.Id equals cu.ConversationId
                        where cu.UserId == userId
                        select c;
            var groupedQuery = from c in query
                               join m in db.Messages on c.Id equals m.ConversationId
                               into conversationMessages
                               select new { conversation = c, messages = conversationMessages };

            if (olderThan != null)
                groupedQuery = groupedQuery.Where(g => g.messages.Max(m => m.SendTime < olderThan));

            var sorted = groupedQuery.OrderByDescending(g => g.messages.Max(m => m.SendTime));

            var limited = sorted.Take(count).Select(g => g.conversation);

            var conversations = await limited
                .Include(c => c.Participations)
                .ThenInclude(cu => cu.User).ToListAsync();

            return conversations;
        }

        public async Task<Message> AddMessage(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
                return null;
            db.Entry(message).State = EntityState.Added;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return message;
        }

        public async Task<List<Message>> GetMessages(long conversationId, string userId, int count, DateTime? olderThan)
        {
            bool userAllowed = await (from cu in db.ConversationUsers
                                     join c in db.Conversations on cu.ConversationId equals c.Id
                                     where c.Id == conversationId && cu.UserId == userId
                                     select cu).CountAsync() == 1;
            if (!userAllowed)
                return new List<Message>();
            var query = db.Messages.Where(m => m.ConversationId == conversationId);
            if (olderThan != null)
                query = query.Where(m => m.SendTime < olderThan);
            var messages = await query.OrderBy(m => m.SendTime).Take(count).ToListAsync();
            return messages;
        }
    }
}

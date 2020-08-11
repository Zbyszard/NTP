using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
            db.ConversationUsers.AddRange(conversation.Participations);
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
                        where cu.UserId == userId && c.Messages.Count() > 0
                        select c;
            if (olderThan != null)
                query = from c in query
                        where c.Messages.Max(m => m.SendTime < olderThan)
                        select c;
            var conversations = await query.OrderByDescending(c => c.Messages.Max(m => m.SendTime))
                .Include(c => c.Participations)
                .ThenInclude(cu => cu.User)
                .ToListAsync();
            var cIds = conversations.Select(c => c.Id);
            var lastMessages = await (from c in db.Conversations
                                      where cIds.Contains(c.Id)
                                      select c.Messages.Where(m => m.SendTime == c.Messages.Max(m => m.SendTime)).First()).ToListAsync();
            conversations = conversations.Join(lastMessages, c => c.Id, m => m.ConversationId,
                (c, m) =>
                {
                    c.Messages = new List<Message> { m };
                    return c;
                }).ToList();
            return conversations;
        }

        public async Task<Conversation> GetConversation(long id)
        {
            var conv = await (from c in db.Conversations
                              select new Conversation
                              {
                                  Id = c.Id,
                                  IsGroup = c.IsGroup,
                                  Name = c.Name,
                                  Participations = db.ConversationUsers
                                  .Where(cu => cu.ConversationId == id)
                                  .Include(cu => cu.User).ToList(),
                                  Messages = c.Messages.OrderByDescending(m => m.SendTime).Take(1).ToList()
                              })
                              .SingleOrDefaultAsync(c => c.Id == id);
            return conv;
        }

        public async Task<List<Conversation>> SearchConversations(string searchString, string loggedUserName)
        {
            var userConvs = from c in db.Conversations
                            where c.Participations.Any(cu => cu.User.UserName == loggedUserName)
                            select c;
            var convs = await (from c in userConvs
                               where
                               c.IsGroup && (
                               c.Name != null && c.Name.ToLower().Contains(searchString.ToLower()) ||
                               c.Name == null && c.Participations.Any(cu => cu.User.UserName.ToLower().Contains(searchString.ToLower()))) ||
                               !c.IsGroup &&
                               c.Participations.Where(cu => cu.User.UserName != loggedUserName)
                               .Any(cu => cu.User.UserName.ToLower().Contains(searchString.ToLower()))
                               select c)
                               .Include(c => c.Participations)
                               .ThenInclude(cu => cu.User)
                               .Take(30).ToListAsync();
            return convs;
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
            var messages = await query.OrderByDescending(m => m.SendTime).Take(count).ToListAsync();
            messages = messages.OrderBy(m => m.SendTime).ToList();
            return messages;
        }
    }
}

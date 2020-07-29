using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kopyw.DTOs;
using Kopyw.Services;
using Kopyw.Services.DTOs.Interfaces;
using Kopyw.Services.Notifiers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationDTOManager conversationManager;
        private readonly IMessageNotifier messageNotifier;
        private readonly UserFinder userFinder;
        public ConversationController(IConversationDTOManager conversationDTOManager,
            IMessageNotifier messageNotifier,
            UserFinder userFinder)
        {
            conversationManager = conversationDTOManager;
            this.messageNotifier = messageNotifier;
            this.userFinder = userFinder;
        }

        [Route("range/{count}/{olderThan?}")]
        [HttpGet]
        public async Task<ActionResult<List<ConversationDTO>>> GetConversations(int count, DateTime? olderThan = null)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var conversations = await conversationManager.GetConversations(user.Id, count, olderThan);
            if (conversations.Count == 0)
                return NotFound();
            return Ok(conversations);
        }

        [Route("create")]
        [HttpPost]
        public async Task<ActionResult<ConversationDTO>> CreateConversation(ConversationDTO conversation)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var added = await conversationManager.AddConversation(conversation, user);
            if (added == null)
                return Conflict();
            return Ok(added);
        }

        [Route("messages/{conversationId}/{count}/{olderThan?}")]
        [HttpGet]
        public async Task<ActionResult<List<MessageDTO>>> GetMessages(long conversationId, int count, DateTime? olderThan = null)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var messages = await conversationManager.GetMessages(conversationId, user.Id, count, olderThan);
            if (messages.Count == 0)
                return NotFound();
            return Ok(messages);
        }

        [Route("message")]
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> AddMessage(MessageDTO msg)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var added = await conversationManager.AddMessage(msg, user);
            if (added == null)
                return Forbid();
            await messageNotifier.SendMessage(added);
            return Ok(added);
        }
    }
}

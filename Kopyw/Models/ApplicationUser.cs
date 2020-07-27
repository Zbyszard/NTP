using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Comment> Comments { get; set; }
        public List<CommentVote> CommentVotes { get; set; }
        public List<Post> Posts { get; set; }
        public List<PostVote> PostVotes { get; set; }
        public List<Follow> FollowedBy { get; set; }
        public List<Follow> Follows { get; set; }
        public List<ConversationUser> ConversationParticipations { get; set; }
        public List<Message> Messages { get; set; }
    }
}

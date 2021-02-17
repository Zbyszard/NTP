using AutoMapper;
using Kopyw.Core.DTO;
using Kopyw.Core.Models;
using System.Linq;

namespace Kopyw.Infrastructure.Profiles
{
    public static class MappingConfigurationHolder
    {
        public static MapperConfiguration Configuration { get; set; }
        static MappingConfigurationHolder()
        {
            Configuration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
        }
    }
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.PostId, o => o.MapFrom(s => s.PostId))
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.PostTime, o => o.MapFrom(s => s.PostTime))
                .ForMember(d => d.Score, o => o.MapFrom(s =>
                    s.Votes.Where(v => v.Value > 0).Count() - s.Votes.Where(v => v.Value < 0).Count()))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text));
            CreateMap<CommentDTO, Comment>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.PostId, o => o.MapFrom(s => s.PostId))
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text));

            CreateMap<CommentVote, CommentVoteDTO>()
                .ForMember(d => d.CommentId, o => o.MapFrom(s => s.CommentId))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Value));
            CreateMap<CommentVoteDTO, CommentVote>()
                .ForMember(d => d.CommentId, o => o.MapFrom(s => s.CommentId))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Value));

            CreateMap<Post, PostDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.CommentCount, o => o.MapFrom(s => s.Comments.Count))
                .ForMember(d => d.PostTime, o => o.MapFrom(s => s.PostTime))
                .ForMember(d => d.Score, o => o.MapFrom(s => s.Votes.Count))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text));
            CreateMap<PostDTO, Post>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId));

            CreateMap<PostInfo, PostInfoDTO>()
                .ForMember(d => d.UserVote, o => o.MapFrom(s => s.UserVote != null));

            CreateMap<Follow, FollowDTO>()
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(d => d.ObserverId, o => o.MapFrom(s => s.ObserverId));
            CreateMap<FollowDTO, Follow>()
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(d => d.ObserverId, o => o.MapFrom(s => s.ObserverId));

            CreateMap<UserStats, UserStatsDTO>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
                .ForMember(d => d.IsFollowed, o => o.MapFrom(s => s.LoggedUserFollow != null));

            CreateMap<Conversation, ConversationDTO>()
                .ForMember(d => d.UserNames, o => o.MapFrom(s => s.Participations.Select(p => p.User.UserName).ToList()))
                .ForMember(d => d.Messages, o => o.MapFrom(s => s.Messages));
            CreateMap<ConversationDTO, Conversation>()
                .ForMember(d => d.Participations, o => o.MapFrom(s =>
                    s.UserNames.Select(str => new ConversationUser { User = new ApplicationUser { UserName = str } }).ToList()));

            CreateMap<Message, MessageDTO>()
                .ForMember(d => d.Sender, o => o.MapFrom(s => s.Sender.UserName));
            CreateMap<MessageDTO, Message>()
                .ForMember(d => d.Sender, o => o.MapFrom(s => new ApplicationUser { UserName = s.Sender }));
        }
    }
}

using AutoMapper;
using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.DTOs
{
    public class MappingProfile : Profile
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
        }
    }
}

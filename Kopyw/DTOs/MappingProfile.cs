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
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.PostTime, o => o.MapFrom(s => s.PostTime))
                .ForMember(d => d.Score, o => o.MapFrom(s =>
                    s.Votes.Where(v => v.Value > 0).Count() - s.Votes.Where(v => v.Value < 0).Count()))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text));
            CreateMap<CommentDTO, Comment>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.AuthorId))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text));
        }
    }
}

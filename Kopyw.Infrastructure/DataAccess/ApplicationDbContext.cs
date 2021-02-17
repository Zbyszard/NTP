using IdentityServer4.EntityFramework.Options;
using Kopyw.Core.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.DataAccess
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostVote> PostVotes { get; set; }
        public DbSet<ConversationUser> ConversationUsers { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ImageInfo> Images { get; set; }
        public DbSet<MessageImage> MessageImages { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public ApplicationDbContext( DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Author)
                .WithMany(u => u.FollowedBy)
                .HasForeignKey(f => f.AuthorId);
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Observer)
                .WithMany(u => u.Follows)
                .HasForeignKey(f => f.ObserverId);

            modelBuilder.Entity<CommentVote>()
                .HasOne(cv => cv.User)
                .WithMany(u => u.CommentVotes)
                .HasForeignKey(cv => cv.UserId);
            modelBuilder.Entity<CommentVote>()
                .HasOne(cv => cv.Comment)
                .WithMany(c => c.Votes)
                .HasForeignKey(cv => cv.CommentId);

            modelBuilder.Entity<PostVote>()
                .HasOne(pv => pv.User)
                .WithMany(u => u.PostVotes)
                .HasForeignKey(pv => pv.UserId);
            modelBuilder.Entity<PostVote>()
                .HasOne(pv => pv.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(pv => pv.PostId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Follows)
                .WithOne(f => f.Observer);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.FollowedBy)
                .WithOne(f => f.Author);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ConversationParticipations);

            modelBuilder.Entity<ConversationUser>()
                .HasKey(cu => new { cu.UserId, cu.ConversationId });

            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation);
            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Participations);

            modelBuilder.Entity<ImageInfo>()
                .Property(i => i.IsPrivate)
                .HasDefaultValue(false);
            modelBuilder.Entity<ImageInfo>()
                .HasOne(i => i.MessageImage)
                .WithOne(mi => mi.Image)
                .HasForeignKey<ImageInfo>(i => i.MessageImageId);
            modelBuilder.Entity<ImageInfo>()
                .HasOne(i => i.PostImage)
                .WithOne(pi => pi.Image)
                .HasForeignKey<ImageInfo>(i => i.PostImageId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages);
            modelBuilder.Entity<Message>()
                .HasMany(m => m.Images)
                .WithOne(i => i.Message);
            modelBuilder.Entity<Message>()
                .Property(m => m.SendTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Post);
            modelBuilder.Entity<Post>()
                .Property(p => p.PostTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Comment>()
                .Property(c => c.PostTime)
                .HasDefaultValueSql("getdate()");
        }
    }
}

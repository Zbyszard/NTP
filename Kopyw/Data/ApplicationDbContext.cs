using Kopyw.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostVote> PostVotes { get; set; }
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
                .WithMany(pv => pv.Votes)
                .HasForeignKey(pv => pv.PostId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Follows)
                .WithOne(f => f.Observer);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.FollowedBy)
                .WithOne(f => f.Author);

            modelBuilder.Entity<Post>()
                .Property(p => p.PostTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Comment>()
                .Property(c => c.PostTime)
                .HasDefaultValueSql("getdate()");
        }
    }
}

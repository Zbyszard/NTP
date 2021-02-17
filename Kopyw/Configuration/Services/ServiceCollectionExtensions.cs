using AutoMapper;
using Kopyw.Core.Notifications;
using Kopyw.Core.Repositiories;
using Kopyw.Core.Services;
using Kopyw.ImageProcessing;
using Kopyw.ImageProcessing.Interfaces;
using Kopyw.Infrastructure.Notifications;
using Kopyw.Infrastructure.Options;
using Kopyw.Infrastructure.Profiles;
using Kopyw.Infrastructure.Repositories;
using Kopyw.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kopyw.Configuration.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ImageManagerOptions>(config.GetSection(ImageManagerOptions.ImageManager));
            return services;
        }
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserFinder, UserFinder>();
            services.AddScoped<IPostDTOManager, PostDTOManager>();
            services.AddScoped<ICommentDTOManager, CommentDTOManager>();
            services.AddScoped<IFollowDTOManager, FollowDTOManager>();
            services.AddScoped<IUserStatsDTOManager, UserStatsDTOManager>();
            services.AddScoped<IConversationDTOManager, ConversationDTOManager>();
            services.AddScoped<IImageReader, ImageReader>();
            services.AddScoped<IImageProcessor, ImageProcessor>();
            services.AddScoped<IImageWriter, ImageWriter>();
            services.AddScoped<IFollowManager, FollowManager>();
            services.AddScoped<IPostManager, PostManager>();
            services.AddScoped<ICommentManager, CommentManager>();
            services.AddScoped<IUserStatsManager, UserStatsManager>();
            services.AddScoped<IPostNotifier, PostNotifier>();
            services.AddScoped<IConversationManager, ConversationManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<IMessageNotifier, MessageNotifier>();
            services.AddScoped<IFileSystemImageManager, FileSystemImageManager>();
            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = MappingConfigurationHolder.Configuration.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}

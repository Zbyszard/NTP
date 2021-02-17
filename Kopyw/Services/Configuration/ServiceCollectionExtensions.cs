using Kopyw.ImageProcessing;
using Kopyw.ImageProcessing.Interfaces;
using Kopyw.Services;
using Kopyw.Services.Configuration.Options;
using Kopyw.Services.DataAccess;
using Kopyw.Services.DataAccess.Interfaces;
using Kopyw.Services.DTOs;
using Kopyw.Services.DTOs.Interfaces;
using Kopyw.Services.FileProcessing;
using Kopyw.Services.FileProcessing.Interfaces;
using Kopyw.Services.Notifiers;
using Kopyw.Services.Notifiers.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ImageManagerOptions>(config.GetSection(ImageManagerOptions.ImageManager));
            return services;
        }
        public static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            services.AddScoped<IUserFinder, UserFinder>();
            services.AddScoped<IPostDTOManager, PostDTOManager>();
            services.AddScoped<ICommentDTOManager, CommentDTOManager>();
            services.AddScoped<IFollowDTOManager, FollowDTOManager>();
            services.AddScoped<IUserStatsDTOManager, UserStatsDTOManager>();
            services.AddScoped<IConversationDTOManager, ConversationDTOManager>();
            services.AddScoped<IFollowManager, FollowManager>();
            services.AddScoped<IPostManager, PostManager>();
            services.AddScoped<ICommentManager, CommentManager>();
            services.AddScoped<IUserStatsManager, UserStatsManager>();
            services.AddScoped<IPostNotifier, PostNotifier>();
            services.AddScoped<IConversationManager, ConversationManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<IMessageNotifier, MessageNotifier>();
            services.AddScoped<IImageReader, ImageReader>();
            services.AddScoped<IImageProcessor, ImageProcessor>();
            services.AddScoped<IImageWriter, ImageWriter>();
            services.AddScoped<IFileSystemImageManager, FileSystemImageManager>();
            return services;
        }
    }
}

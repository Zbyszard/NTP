using System.Threading.Tasks;
using Kopyw.Core.Repositiories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("img")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IFileSystemImageManager fsImageManager;
        private readonly IImageManager imageManager;
        private readonly IConversationManager conversationManager;
        public ImageController(IFileSystemImageManager fsImageManager,
            IImageManager imageManager)
        {
            this.fsImageManager = fsImageManager;
            this.imageManager = imageManager;
        }

        [Route("upload/message")]
        [HttpPost]
        public async Task<ActionResult<string>> UploadMessageImage(IFormFile file)
        {
            try
            {
                var info = await fsImageManager.SaveMessageImage(file);
                await imageManager.Add(info);
                return Ok(info.Id);
            }
            catch
            {
                return UnprocessableEntity();
            }
        }

        [Route("upload/post")]
        [HttpPost]
        public async Task<ActionResult<string>> UploadPostImage(IFormFile file)
        {
            try
            {
                var info = await fsImageManager.SavePostImage(file);
                await imageManager.Add(info);
                return Ok(info.Id);
            }
            catch
            {
                return UnprocessableEntity();
            }
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> Get(string id)
        {
            try
            {
                var info = await imageManager.Get(id);
                if (info.IsPrivate)
                {

                }
                var bytes = await fsImageManager.GetImageBytes(info.Path);
                return File(bytes, $"image/{info.Format}");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace JobExpressBack.Models
{
    public class PhotoService
    {
        private readonly Cloudinary cloudinary;

        public PhotoService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadPhotoAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult?.SecureUrl?.ToString(); // Retourne l'URL sécurisée de l'image
            }

            return null;
        }
    }
}


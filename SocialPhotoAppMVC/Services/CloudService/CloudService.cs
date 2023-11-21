﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using SocialPhotoAppMVC.Helpers;

namespace SocialPhotoAppMVC.Services
{
    public class CloudService : ICloudService
    {
        private readonly Cloudinary _cloudinary;

        public CloudService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );

            _cloudinary = new Cloudinary(acc);
        }

        public ImageUploadResult AddPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };
                uploadResult = _cloudinary.Upload(uploadParams);
            }
            return uploadResult;
        }

        public DeletionResult DeletePhoto(string publicUrl)
        {
            var publicId = publicUrl.Split('/').Last().Split('.')[0];
            var deleteParams = new DeletionParams(publicId);
            return _cloudinary.Destroy(deleteParams);
        }

    }
}

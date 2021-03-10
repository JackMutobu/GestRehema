using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GestRehema.Services
{
    public interface IFileService
    {
        string SaveImage(Bitmap bitmap, string fileName);
        string SaveImage(Bitmap bitmap, string destinationPath, string fileName);
    }

    public class FileService : IFileService
    {
        public string SaveImage(Bitmap bitmap, string fileName)
        {
            ImageCodecInfo myImageCodecInfo;
            EncoderParameters myEncoderParameters;
            GetImage(out myImageCodecInfo, out myEncoderParameters);
            bitmap.Save(@"Assets/" + fileName, myImageCodecInfo, myEncoderParameters);

            return $"/Assets/{fileName}";
        }

        public string SaveImage(Bitmap bitmap, string destinationPath, string fileName)
        {
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            ImageCodecInfo myImageCodecInfo;
            EncoderParameters myEncoderParameters;
            GetImage(out myImageCodecInfo, out myEncoderParameters);


            var imagePath = Path.Combine(destinationPath, fileName);

            bitmap.Save(imagePath, myImageCodecInfo, myEncoderParameters);

            return imagePath;
        }

        private static void GetImage(out ImageCodecInfo myImageCodecInfo, out EncoderParameters myEncoderParameters)
        {
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.

            // An EncoderParameters object has an array of EncoderParameter

            // objects. In this case, there is only one

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);


            // Save the bitmap as a JPEG file with quality level 75.
            myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            myEncoderParameters.Param[0] = myEncoderParameter;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null!;
        }
    }
}

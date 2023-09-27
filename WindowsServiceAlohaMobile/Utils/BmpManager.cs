using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Utils
{
    public static class BmpManager
    {


        public static byte[] ObtenerFotoSocio(String PathBmp)
        {

            var bmp = new Bitmap(PathBmp);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L); // Establecer la calidad de compresión JPEG al 50%

                var encoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(e => e.FormatID == ImageFormat.Jpeg.Guid);
                bmp.Save(stream, encoder, encoderParams);
                bytes = stream.ToArray();
            }

            bmp.Dispose();
            return bytes;

        }



    }
}

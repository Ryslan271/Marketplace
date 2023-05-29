using Microsoft.Win32;
using System.IO;

namespace KazanNewShop.Services
{
    public static class CommonMethods
    {
        #region Работа с изображениями 

        public static byte[] MainForProfileClientNullPhoto = null!;

        public static byte[] MainForProductNullPhoto = null!;

        public static byte[]? ConvertImage(string? filePath = null)
        {
            filePath ??= OpenImage();

            return File.ReadAllBytes(filePath!);
        }

        private static string? OpenImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Изображения|*.png;*.jpg",
                DefaultExt = "Изображения|*.png;*.jpg",
                CheckFileExists = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;

            return null;
        }
        #endregion
    }
}

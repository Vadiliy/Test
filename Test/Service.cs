using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using Test.Models;

namespace Test
{
    public static class Service
    {
        public static BaseModel GetFileInfoModel(string path)
        {
            List<string> imageFormats = new List<string>()
            {
                "jpg", "jpeg", "png", "bmp"
            };


            FileInfo fileInfo = new FileInfo(path);
            ShortFileInfo model = new ShortFileInfo(fileInfo);
            bool IsImage = imageFormats.Contains(model.Format.ToLower());
            if(IsImage)
            {
                ImageFileInfo imageModel = new ImageFileInfo(fileInfo);
                return imageModel;
            }
            return model;
        }

        public static List<string> GetFoldersAndFilesFromPath(string path)
        {
            List<string> folders = GetFolders(path);
            List<string> files = GetFiles(path);
            List<string> objects = folders.Concat(files).ToList();

            return objects;
        }

        public static bool IsDirectory(string path)
        { 
            FileAttributes attr = File.GetAttributes(path);

            bool isDirectory = false;
            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                isDirectory = true;

            return isDirectory;
        }

        public static List<string> GetFolders(string path)
        {
            List<string> folders = new List<string>();
            try
            {
                folders = Directory.GetDirectories(path).ToList();
            }
            catch
            {  
            }
            return folders;
        }

        public static List<string> GetFiles(string path)
        {
            List<string> files = new List<string>();
            try
            {
                files = Directory.GetFiles(path).ToList();
            }
            catch 
            {
            }
            finally
            {
                files.Sort();
            }
            return files;
        }

        public static List<string> GetNamesFromPaths(List<string> paths)
        {
            string toFind = "\\";
            List<string> newPaths = new List<string>();
            foreach (string path in paths)
            {
                int indexToDelete = path.LastIndexOf(toFind);
                string newPath = path.Remove(0, indexToDelete + toFind.Length);
                newPaths.Add(newPath);
            }
            return newPaths;
        }

        public static ImageSource GetImageByPath(string path)
        {
            try
            {
                Icon icon = Icon.ExtractAssociatedIcon(path);
                return ToImageSource(icon);
            }
            catch
            {
                return null;
            }
        }

        static ImageSource ToImageSource(Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());


            return wpfBitmap;
        }
    }
}

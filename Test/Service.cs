using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Test
{
    public static class Service
    {
        public static void GetObjectsFromDriver()
        {

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
            catch (Exception e)
            {
                folders.Add(e.Message);
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
            catch (Exception e)
            {
                files.Add(e.Message);
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

        public static Icon GetIconByPath(string path)
        {
            return Icon.ExtractAssociatedIcon(path);
        }

        public static ImageSource ToImageSource(Icon icon)
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

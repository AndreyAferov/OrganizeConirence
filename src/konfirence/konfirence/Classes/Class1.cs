using System;
using System.IO;
using System.Linq;
using System.Windows.Markup;

namespace konfirence.Classes
{
    public static class ImageToByte
    {
        public static void ImageConverter()
        {
            Console.WriteLine("ImageConverter started.");
            var users = Data.KonfirenceEntities.GetContext().Users.ToList();

            foreach (var user in users)
            {
                string imagePath = GetImagePath(user.Role, user.ImageName);
                Console.WriteLine($"Processing user: {user.Id}, imagePath: {imagePath}");

                if (File.Exists(imagePath))
                {
                    user.Image = File.ReadAllBytes(imagePath);
                    Console.WriteLine($"Image updated for user {user.Id}");
                }
                else
                {
                    Console.WriteLine($"Image file not found: {imagePath}");
                }
            }
            Data.KonfirenceEntities.GetContext().SaveChanges();
            Console.WriteLine("ImageConverter finished.");
        }

        private static string GetImagePath(int roleId, string photoName)
        {
            string roleFolder;

            if (roleId == 1)
            {
                roleFolder = "jury";
            }
            else if (roleId == 2)
            {
                roleFolder = "mod";
            }
            else if (roleId == 3)
            {
                roleFolder = "org";
            }
            else if (roleId == 4)
            {
                roleFolder = "users";
            }
            else
            {
                throw new ArgumentException("Неверный идентификатор роли!");
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"EventsImg\\{roleFolder}\\{photoName}");
        }
    }
}
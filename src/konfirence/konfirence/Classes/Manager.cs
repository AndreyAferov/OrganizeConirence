using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace konfirence.Classes
{
    internal class Manager
    {
        public static Frame MainFrame { get; set; }
        public static Data.Users CurrentUser { get; set; }
        public static void GetImageData()
        {
            try
            {
                var list = Data.KonfirenceEntities.GetContext().Users.ToList();
                foreach (var item in list)
                {
                    string path = Directory.GetCurrentDirectory() + @"\img\" + item.ImageName;
                    if (File.Exists(path))
                    {
                        item.Image = File.ReadAllBytes(path);
                    }

                }
                Data.KonfirenceEntities.GetContext().SaveChanges();
            }
            catch { }
        }
    }

}

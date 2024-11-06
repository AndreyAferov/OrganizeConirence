using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using konfirence.Data;
using konfirence.Classes;


namespace konfirence.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            var events = LoadEvents();
            Listing.ItemsSource = events;
        }
        public List<DisplayEvent> LoadEvents()
        {
            var displayEvents = new List<DisplayEvent>();
            string resourcePath = @"C:\Users\Андрей\Desktop\Организация_конференций_2023\konfirence\konfirence\Resources";

            // Получаем контекст базы данных
            var context = KonfirenceEntities.GetContext();

            // Выполняем запрос с объединением таблиц Events и EventTopics
            var query = from ev in context.Events
                        join topic in context.Event on ev.EventTopic equals topic.Id
                        select new
                        {
                            ev.Id,
                            EventTopicName = topic.Topic,  // Название темы из EventTopics
                            EventDirection = ev.Event,     // Название мероприятия
                            ev.Date
                        };

            foreach (var ev in query)
            {
                string imagePath = System.IO.Path.Combine(resourcePath, $"{ev.Id}.jpg");

                if (File.Exists(imagePath))
                {
                    displayEvents.Add(new DisplayEvent
                    {
                        ID = ev.Id,
                        EventTopic = ev.EventTopicName,     // Название темы мероприятия
                        EventDirection = ev.EventDirection, // Название мероприятия
                        Date = ev.Date,
                        ImagePath = imagePath
                    });
                }
            }

            return displayEvents;
        }


    }
}

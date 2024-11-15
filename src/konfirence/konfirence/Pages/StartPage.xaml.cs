using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using konfirence.Data;
using konfirence.Classes;

namespace konfirence.Pages
{
    public partial class StartPage : Page
    {
        // Список для хранения всех событий, загружаемых из базы данных
        private List<DisplayEvent> allEvents;

        // Конструктор для StartPage
        public StartPage()
        {
            InitializeComponent(); // Инициализация компонентов страницы из XAML

            allEvents = LoadEvents(); // Загружаем все события из базы данных и сохраняем для последующей фильтрации
            Listing.ItemsSource = allEvents; // Устанавливаем список событий как источник данных для ListView

            LoadDirections(); // Загружаем направления для ComboBox фильтра
        }

        // Метод для загрузки направлений (тем) из базы данных и добавления их в ComboBox
        private void LoadDirections()
        {
            var context = KonfirenceEntities.GetContext(); // Получаем объект контекста базы данных

            // Получаем все направления из базы данных и преобразуем в список
            var directions = context.Direction.ToList();
            directions.Insert(0, new Data.Direction { Id = 0, Name = "Все" }); // Добавляем элемент "Все" как первый для отображения всех направлений

            DirectionComboBox.ItemsSource = directions; // Устанавливаем направления как источник данных для ComboBox
            DirectionComboBox.SelectedIndex = 0; // Выбираем элемент "Все" как начальный
        }

        // Метод для загрузки событий из базы данных и подготовки их к отображению
        public List<DisplayEvent> LoadEvents()
        {
            var displayEvents = new List<DisplayEvent>(); // Создаем список для хранения всех событий
            string resourcePath = @"C:\Users\Андрей\Desktop\Организация_конференций_2023\Вариант 2\konfirence\konfirence\Resources\"; // Путь к папке с изображениями

            var context = KonfirenceEntities.GetContext(); // Получаем объект контекста базы данных

            // Запрос на объединение таблиц Events и Direction для получения информации о каждом событии
            var query = from ev in context.Events
                        join topic in context.Direction on ev.EventTopic equals topic.Id
                        select new
                        {
                            ev.Id, // ID события
                            EventTopicName = topic.Name, // Название темы (направления) мероприятия
                            EventDirection = ev.Event, // Название мероприятия
                            ev.Date // Дата проведения мероприятия
                        };

            // Обрабатываем результаты запроса, добавляя каждое событие в список
            foreach (var ev in query)
            {
                string imagePath = Path.Combine(resourcePath, $"{ev.Id}.jpg"); // Формируем путь к изображению на основе ID события

                // Если файл изображения существует, добавляем событие в список
                if (File.Exists(imagePath))
                {
                    displayEvents.Add(new DisplayEvent
                    {
                        ID = ev.Id, // ID события
                        EventTopic = ev.EventTopicName, // Название темы мероприятия
                        EventDirection = ev.EventDirection, // Название мероприятия
                        Date = ev.Date, // Дата мероприятия
                        ImagePath = imagePath // Путь к изображению
                    });
                }
            }
            return displayEvents; // Возвращаем список всех загруженных событий
        }

        // Метод для фильтрации событий по направлению и дате
        public List<DisplayEvent> ApplyFilters(List<DisplayEvent> events, Data.Direction directionFilter = null, DateTime? dateFilter = null)
        {
            var filteredEvents = events.AsEnumerable(); // Начинаем с полного списка событий

            // Если выбранное направление не "Все", фильтруем события по направлению
            if (directionFilter != null && directionFilter.Name != "Все")
            {
                filteredEvents = filteredEvents.Where(ev => ev.EventTopic == directionFilter.Name); // Фильтрация по имени направления
            }

            // Если указана дата, фильтруем события по дате
            if (dateFilter.HasValue)
            {
                filteredEvents = filteredEvents.Where(ev => ev.Date.Date == dateFilter.Value.Date); // Сравниваем даты без учета времени
            }

            return filteredEvents.ToList(); // Возвращаем отфильтрованный список событий
        }

        // Обработчик изменения выбора направления в ComboBox
        private void DirectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndUpdateList(); // Применяем фильтры и обновляем список событий
        }

        // Обработчик изменения выбранной даты в DatePicker
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndUpdateList(); // Применяем фильтры и обновляем список событий
        }

        // Метод для применения фильтров и обновления отображения списка событий
        private void ApplyFiltersAndUpdateList()
        {
            Data.Direction directionFilter = DirectionComboBox.SelectedItem as Data.Direction; // Получаем выбранное направление из ComboBox
            DateTime? dateFilter = DatePicker.SelectedDate; // Получаем выбранную дату из DatePicker

            var filteredEvents = ApplyFilters(allEvents, directionFilter, dateFilter); // Применяем фильтры к полному списку событий

            Listing.ItemsSource = filteredEvents; // Устанавливаем отфильтрованный список как источник данных для ListView
        }

        // Обработчик нажатия кнопки для перехода на страницу авторизации
        private void AutorizeButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AutorizePage()); // Переход на страницу авторизации
        }
    }
}

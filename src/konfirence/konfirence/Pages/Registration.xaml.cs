using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace konfirence.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Data.Users NewUser { get; set; }
        public Data.Users CurrentUser { get; set; }

        public Registration(Data.Users currentUser)
        {
            InitializeComponent();
            CurrentUser = currentUser;
            Init();
        }
        private void Init()
        {
            NewUser = new Data.Users();
            Role.ItemsSource = Data.KonfirenceEntities.GetContext().Roles.ToList();
            Gender.ItemsSource = Data.KonfirenceEntities.GetContext().Gender.ToList();
            var users = Data.KonfirenceEntities.GetContext().Users.ToList();
            if (users.Any())
            {
                IdNumber.Text = (users.Last().Id + 1).ToString();
            }
            else
            {
                IdNumber.Text = "1";
            }
            EventCB.IsEnabled = false;
            EventCB.ItemsSource = Data.KonfirenceEntities.GetContext().Event.ToList();
        }




        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var validationErrors = ValidateInputs();
                if (validationErrors.Length > 0)
                {
                    MessageBox.Show(validationErrors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                SaveUser();
                MessageBox.Show("Регистрация успешна", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                Classes.Manager.MainFrame.Navigate(new PageOrganize(CurrentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private StringBuilder ValidateInputs()
        {
            StringBuilder sb = new StringBuilder();
            string fio = Fio.Text;
            string email = Email.Text;
            string phone = NumberPhone.Text;
            string password = GetPassword();
            string confirmPassword = GetConfirmPassword();
            int selectedSex = Gender.SelectedIndex;
            int selectedRole = Role.SelectedIndex;
            string direction = Direction.Text;

            if (string.IsNullOrEmpty(fio)) sb.AppendLine("Заполните ФИО!");
            if (string.IsNullOrEmpty(email)) sb.AppendLine("Заполните Email!");
            if (string.IsNullOrEmpty(direction)) sb.AppendLine("Заполните направление!");
            if (selectedRole == 0) sb.AppendLine("Выберите должность!");
            if (selectedSex == 0) sb.AppendLine("Выберите пол!");
            ValidatePassword(password, confirmPassword, sb);
            ValidatePhone(phone, sb);
            ValidateFio(fio, sb);

            return sb;
        }
        private void ValidatePhone(string phone, StringBuilder sb)
        {
            if (CheckPhone(phone)) sb.AppendLine("Телефон не в формате +7(___)-___-__-__");
            else NewUser.PhoneNumber = phone;
        }
        private void ValidatePassword(string password, string confirmPassword, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(password))
            {
                sb.AppendLine("Введите пароль!");
            }
            else
            {
                if (password.Length < 6)
                    sb.AppendLine("Пароль должен содержать минимум 6 символов!");

                if (!password.Any(char.IsDigit))
                    sb.AppendLine("Пароль должен содержать хотя бы одну цифру!");

                if (!password.Any(c => !char.IsLetterOrDigit(c)))
                    sb.AppendLine("Пароль должен содержать хотя бы один спецсимвол!");

                if (!password.Any(char.IsLower))
                    sb.AppendLine("Пароль должен содержать хотя бы одну строчную букву!");

                if (!password.Any(char.IsUpper))
                    sb.AppendLine("Пароль должен содержать хотя бы одну заглавную букву!");

                if (password != confirmPassword)
                    sb.AppendLine("Пароли не совпадают!");
            }
        }
        private bool CheckPhone(string phone) => !Regex.IsMatch(phone, @"^\+7\(\d{3}\)-\d{3}-\d{2}-\d{2}$");




        private void ValidateFio(string fio, StringBuilder sb)
        {
            var allName = fio.Split(' ');
            if (allName.Length != 3) sb.AppendLine("ФИО введено неверно!");
            else
            {
                NewUser.Surname = allName[0];
                NewUser.Name = allName[1];
                NewUser.Patronomic = allName[2];
            }
        }

        private void SaveUser()
        {
            try
            {
                NewUser.Email = Email.Text;
                NewUser.DateBirth = null;
                NewUser.Country1 = null;
                NewUser.Password = GetPassword();
                NewUser.Gender = Gender.SelectedIndex;
                NewUser.Role = Role.SelectedIndex;

                var direction = Direction.Text;
                var directionList = Data.KonfirenceEntities.GetContext().Direction
                    .FirstOrDefault(i => i.Name == direction);
                if (directionList == null)
                {
                    var newDirection = new Data.Direction { Name = direction };
                    Data.KonfirenceEntities.GetContext().Direction.Add(newDirection);
                    Data.KonfirenceEntities.GetContext().SaveChanges();
                    NewUser.DirectionName = newDirection.Id;
                }
                else
                {
                    NewUser.DirectionName = directionList.Id;
                }

                Data.KonfirenceEntities.GetContext().Users.Add(NewUser);
                Data.KonfirenceEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetPassword() => ShowPassword.IsChecked == true ? PasswordTBShow.Text : PasswordTB.Password;
        private string GetConfirmPassword() => ShowPassword.IsChecked == true ? ConfirmPasswordTBShow.Text : ConfirmPasswordTB.Password;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Registration(CurrentUser));
        }

        private void RegisterImage_MouseDown(object sender, MouseButtonEventArgs e) => SetImage();

        private void SetImage()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg"
            };
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                RegisterImage.Source = new BitmapImage(new Uri(path));
                NewUser.Image = File.ReadAllBytes(path);
                NewUser.ImageName = System.IO.Path.GetFileName(path);
            }
        }

        private void AttachToEvent_Checked(object sender, RoutedEventArgs e) => EventCB.IsEnabled = true;
        private void AttachToEvent_Unchecked(object sender, RoutedEventArgs e) => EventCB.IsEnabled = false;

        private void DirectionTB_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => FollowVariantsLB.Visibility = Visibility.Visible;
        private void DirectionTB_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => FollowVariantsLB.Visibility = Visibility.Hidden;

        private void Direction_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Direction.Text.ToLower();
            FollowVariantsLB.ItemsSource = Data.KonfirenceEntities.GetContext().Direction
                .Where(d => d.Name.ToLower().Contains(text)).ToList();
        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTBShow.Text = PasswordTB.Password;
            ConfirmPasswordTBShow.Text = ConfirmPasswordTB.Password;
            TogglePasswordVisibility(true);
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordTB.Password = PasswordTBShow.Text;
            ConfirmPasswordTB.Password = ConfirmPasswordTBShow.Text;
            TogglePasswordVisibility(false);
        }

        private void TogglePasswordVisibility(bool isVisible)
        {
            PasswordTBShow.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            ConfirmPasswordTBShow.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            PasswordTB.Visibility = isVisible ? Visibility.Hidden : Visibility.Visible;
            ConfirmPasswordTB.Visibility = isVisible ? Visibility.Hidden : Visibility.Visible;
        }

        private void FollowVariantsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FollowVariantsLB.SelectedItem is Data.Direction selectedDirection)
            {
                Direction.Text = selectedDirection.Name;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new PageOrganize(CurrentUser));
        }
    }
}

using konfirence.Classes;
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

namespace konfirence.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutorizePage.xaml
    /// </summary>
    public  partial class AutorizePage : Page
    {
        public AutorizePage()
        {
            InitializeComponent();
        }
        private int failedAttempts = 0;
        private CaptchaGenerator captchaGenerator;
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if(String.IsNullOrEmpty(LoginText.Text))
            {
                errors.AppendLine("Введите логин");
            }
            if (String.IsNullOrEmpty(PasswordText.Password))
            {
                errors.AppendLine("Введите пароль");
            }
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

                if (failedAttempts == 1)
                {
                    LoadCaptcha();
                    errors.AppendLine("Введите капчу");

                }
                if (failedAttempts >= 2 && !!IsCaptchaCorrect())
                {
                    MessageBox.Show("Неправильные данные или CAPTCHA. Вход заблокирован на 10 секунд.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoginButton.IsEnabled = false;
                    LoadCaptcha();
                    await Task.Delay(10000);
                    LoginButton.IsEnabled = true;
                }
                failedAttempts++;
                return;
            }
            if (failedAttempts >= 2 && !!IsCaptchaCorrect())
            {
                MessageBox.Show("Неправильные данные или CAPTCHA. Вход заблокирован на 10 секунд.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                LoginButton.IsEnabled = false;
                LoadCaptcha();
                await Task.Delay(10000);
                LoginButton.IsEnabled = true;
                return;
            }
            if (Data.KonfirenceEntities.GetContext().Users.Any(d => d.Id.ToString() == LoginText.Text && d.Password == PasswordText.Password))
            {
                var user = Data.KonfirenceEntities.GetContext().Users.FirstOrDefault(d => d.Id.ToString() == LoginText.Text && d.Password == PasswordText.Password);

                Manager.CurrentUser = user;

                switch(user.Roles.Name)
                {
                    case "Жюри":
                        Manager.MainFrame.Navigate(new PageJury());
                        break;
                    case "Модераторы":
                        Manager.MainFrame.Navigate(new PageModarator());
                        break;
                    case "Организаторы":
                        Manager.MainFrame.Navigate(new PageOrganize());
                        break;
                    case "Участники":
                        Manager.MainFrame.Navigate(new PageUser());
                        break;
                }
            }
            else
            {
                failedAttempts++;
                if (failedAttempts == 1)
                {
                    MessageBox.Show("Некорректный логин или пароль.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoadCaptcha();
                }
                else
                {
                    MessageBox.Show("Некорректный логин или пароль.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private void LoadCaptcha()
        {
            captchaGenerator = new CaptchaGenerator();
            captchaGenerator.GenerateCaptcha(CaptchaCanvas);
            CaptchaCanvas.Visibility = Visibility.Visible;
            CaptchaTextBox.Visibility = Visibility.Visible;
        }

        private bool IsCaptchaCorrect()
        {
            return CaptchaTextBox.Text == captchaGenerator.CaptchaText;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new StartPage());
        }
    }
}

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
using System.Windows.Threading;

namespace Vasiyatullina41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private bool isBlocked = false;
        private string currentCaptcha = "";

        private void GenerateCaptcha()
        {
            currentCaptcha = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            CaptchaText.Text = $"{currentCaptcha[0]}   {currentCaptcha[1]}\n    {currentCaptcha[2]}   {currentCaptcha[3]}";
            CaptchaPanel.Visibility = Visibility.Visible;
            CaptchaTB.Text = "";
        }

        private async void LoginInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isBlocked) return;

            if (CaptchaPanel.Visibility == Visibility.Visible)
            {
                if (CaptchaTB.Text.ToUpper() != currentCaptcha)
                {
                    isBlocked = true;
                    LoginBtn.IsEnabled = false;

                    for (int i = 10; i > 0; i--)
                    {
                        ErrorTextBlock.Text = $"Заблокировано! Осталось: {i} сек.";
                        await Task.Delay(1000);
                    }
                    ErrorTextBlock.Text = $"Попробуйте ввести данные снова";
                    GenerateCaptcha();
                    isBlocked = false;
                    LoginBtn.IsEnabled = true;
                    return;
                }
            }

            string login = LoginTB.Text;
            string password = PassTB.Text;
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые строки");
                return;
            }

            
            User user = Vasiyatullina_41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
            }
            else
            {
                if (CaptchaPanel.Visibility == Visibility.Collapsed)
                {
                    MessageBox.Show("Неверный логин или пароль. Введите капчу!");
                    GenerateCaptcha();
}
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                    GenerateCaptcha();
                    return;
                    
                }
            }
        }
        
        private void LoginInGoustBtn_Click(object sender, RoutedEventArgs e)
        {
            User anonUser = new User();

            anonUser.UserSurname = "Гость";
            anonUser.UserName = "";
            anonUser.UserPatronymic = "";
            anonUser.UserRole = 4;

            Manager.MainFrame.Navigate(new ProductPage(anonUser));
        }

    }
}

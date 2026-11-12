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

namespace Vasiyatullina41
{
    public partial class ProductPage : Page
    {
        private List<Product> currentProducts; //приватный список на уровне класса, для работы со всеми "продуктами"

        public ProductPage(User user)
        {
            InitializeComponent();
            FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            switch (user.UserRole)
            {
                case 1:
                    RoleTB.Text = "Администратор";
                    break;
                case 2:
                    RoleTB.Text = "Клиент";
                    break;
                case 3:
                    RoleTB.Text = "Менеджер";
                    break;
                case 4:
                    RoleTB.Text = "Отсутствует";
                    break;
            }
            
            currentProducts =   Vasiyatullina_41Entities.GetContext().Product.ToList();

            UpdateList();
        }
        private void UpdateList()
        {
            if (currentProducts == null) return;

            var result = currentProducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string searchText = SearchTextBox.Text.ToLower();
                result = result.Where(p => p.ProductName != null && p.ProductName.ToLower().Contains(searchText));
            }

            if (FilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string filterText = selectedItem.Content.ToString();

                if (filterText == "0-9,99%")
                    result = result.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10);
                else if (filterText == "10-14,99%")
                    result = result.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15);
                else if (filterText == "15% и более")
                    result = result.Where(p => p.ProductDiscountAmount >= 15);
            }

            if (SortAscRadioButton.IsChecked == true)
                result = result.OrderBy(p => p.ProductCost);
            else if (SortDescRadioButton.IsChecked == true)
                result = result.OrderByDescending(p => p.ProductCost);

            var finalList = result.ToList();
            ProductListView.ItemsSource = finalList;

            CountTextBlock.Text = $"кол-во {finalList.Count} из {currentProducts.Count}";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void SortRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}





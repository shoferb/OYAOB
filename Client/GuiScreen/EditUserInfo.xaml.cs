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
using System.Windows.Shapes;

namespace TexasHoldem.GuiScreen
{
    /// <summary>
    /// Interaction logic for EditUserInfo.xaml
    /// </summary>
    public partial class EditUserInfo : Window
    {
        private ClientLogic cl;
        private Window parentScreen;
        private int currId;
        private int Id;
        private string name;
        private string username;
        private string email;
        private string firstPassword;
        private string secPassword;
        private int money;


        public EditUserInfo(Window parent, int id, ClientLogic cli)
        {
            InitializeComponent();
            parentScreen = parent;
            cl = cli;
            currId = id;
            cl.SetUserId(id);
        }

        private void currPasswordTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currPasswordTextBox.Text = "";
            currPasswordTextBox.Opacity = 100;

        }
        private void currPasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NewPasswordTextBox_OnPreviewMouseDownPasswordTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currPasswordTextBox.Text = "";
            currPasswordTextBox.Opacity = 100;
        }

      

      

        private void IDtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = IDtextBox.Text;
            bool isValid = int.TryParse(temp, out Id);
            if (!isValid)
            {
                MessageBox.Show("Invalid Id input");
            }

        }

        private void EditIdButton_Click(object sender, RoutedEventArgs e)
        {
            if (Id == null)
            {
                MessageBox.Show("Please enter new Id");
                return;
            }
            bool EditIdOk = cl.EditDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField.Id,
                Id.ToString());
            if (EditIdOk)
            {
                MessageBox.Show("User Id was sucssesful edit to: " +Id + "From: " + currId);
                currId = Id;
                cl.SetUserId(Id);
            }
            else
            {
                MessageBox.Show("User Id Edit - fail");
            }
        }

        private void EditNamebutton_Click(object sender, RoutedEventArgs e)
        {
            if (name.Equals(""))
            {
                MessageBox.Show("Please enter new name");
                return;
            }
            bool EditNameOk = cl.EditDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField.Name,
                name);
            if (EditNameOk)
            {
                MessageBox.Show("Name was sucssesful edit to: " + name);
            }
            else
            {
                MessageBox.Show("Name Edit - fail");
            }
        }

        private void EditUserName_Click(object sender, RoutedEventArgs e)
        {
            if (username.Equals(""))
            {
                MessageBox.Show("Please enter userName name");
                return;
            }
            bool EditUserNameOk = cl.EditDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField.UserName,
                username);
            if (EditUserNameOk)
            {
                MessageBox.Show("User Name was sucssesful edit to: " + username);
            }
            else
            {
                MessageBox.Show("userName Edit - failed");

            }
        }

        private void EditEmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (email.Equals(""))
            {
                MessageBox.Show("Please enter new email");
                return;
            }
            bool EditEmailOk = cl.EditDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField.Email,
                username);
            if (EditEmailOk)
            {
                MessageBox.Show("User Email was sucssesful edit to: " + email);
            }
            else
            {
                MessageBox.Show("Email Edit - failed");

            }
        }

        private void EditMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            if (money == null)
            {
                MessageBox.Show("Please enter new money");
                return;
            }
            bool EditMoneyOk = cl.EditDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField.Money,
                money.ToString());
            if (EditMoneyOk)
            {
                MessageBox.Show("User Money was sucssesful edit to: " + money);
            }
            else
            {
                MessageBox.Show("User money Edit - fail");
            }
        }

        private void EditPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (secPassword.Equals(""))
            {
                MessageBox.Show("Please enter new password");
                return;
            }
            if (firstPassword.Equals(""))
            {
                MessageBox.Show("Please enter current password");
                return;
            }
            if (firstPassword.Equals(secPassword))
            {
                bool EditPasswordOk = cl.EditDetails(TexasHoldemShared.CommMessages.ClientToServer.EditCommMessage.EditField.Password,
                    firstPassword);
                if (EditPasswordOk)
                {
                    MessageBox.Show("User Password was sucssesful edit!");
                }
                else
                {
                    MessageBox.Show("User password Edit - fail");
                }
            }
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            parentScreen.Show();
            this.Hide();
        }
    }
}

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

namespace Client.GuiScreen
{
    /// <summary>
    /// Interaction logic for SearchScreen.xaml
    /// </summary>
    public partial class SearchScreen : Window
    {
        public SearchScreen()
        {
            InitializeComponent();
            startList = _cusBl.GetAllClubMember();
            listView.ItemsSource = startList;



            listView.AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(HandleDoubleClick));
        }

        private int currclubId;
        private int field;
        private string toSearch;
        private List<ClubMember> result;
        private ClubMember_BL _cusBl = new ClubMember_BL();
        bool isValid = false;
        private List<ClubMember> startList;
        private int idSearch;
        private Window perent;
        private DateTime dateToSearch;
        private int memberIdSearch;
        private ProgressBar progressBar;

        public void SetPerent(Window w)
        {
            perent = w;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        public void toStartlist()
        {
            startList = _cusBl.GetAllClubMember();
        }

        public void SetStartList(List<ClubMember> newStartList)
        {
            this.startList = newStartList;
            listView.ItemsSource = startList;
        }

        private void HandleDoubleClick(object sender, RoutedEventArgs e)
        {
            ClubMember clubMember = (ClubMember) listView.SelectedItem;
            if (clubMember != null)
            {
                currclubId = clubMember.TeudatZehute;
                edit.setPerent(this, currclubId);
                edit.Show();
                this.Hide();
            }
        }

        private CustomerEditInfo edit = new CustomerEditInfo();

        private void Bremoedit_Click(object sender, RoutedEventArgs e)
        {
            edit.setPerent(this, currclubId);
            edit.Show();
            this.Hide();
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            perent.Show();
            this.Hide();
        }



        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            startList = _cusBl.GetAllClubMember();

            listView.ItemsSource = startList;
        }



        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbSearchBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void emptySearch()
        {
            MessageBox.Show("The search returned no results.");
        }

        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
            progressBar = new ProgressBar();
            progressBar.Run();
            if (field == 0) //First name
            {

                toSearch = searchBox.Text;
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                temp = _cusBl.GetByClubMemberFirstName(toSearch);
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else if (field == 1) //last name
            {

                toSearch = searchBox.Text;
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                temp = _cusBl.GetByClubMemberLastName(toSearch);
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else if (field == 2) //Gender
            {

                toSearch = searchBox.Text;
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                temp = _cusBl.GetByClubMemberGender(toSearch);
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
            else if (field == 3) //tz
            {
                toSearch = searchBox.Text;
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                isValid = int.TryParse(toSearch, out idSearch);
                if (isValid)
                {
                    temp = _cusBl.GeBytClubMemberTeudatZehute(idSearch);
                    result = temp;
                    if (result == null || !result.Any())
                    {
                        emptySearch();
                    }
                    else
                    {
                        listView.ItemsSource = result;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Input-TZ contains only numbers");
                }
            }
            else if (field == 4) //dob
            {
                toSearch = searchBox.Text;
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                isValid = DateTime.TryParse(toSearch, out dateToSearch);
                if (isValid)
                {
                    temp = _cusBl.GetByClubMemberDOB(dateToSearch);
                    result = temp;
                    if (result == null || !result.Any())
                    {
                        emptySearch();
                    }
                    else
                    {
                        listView.ItemsSource = result;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input-enter input in date form");
                }
            }
            else if (field == 5) //mmberid
            {
                toSearch = searchBox.Text;
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                isValid = int.TryParse(toSearch, out memberIdSearch);
                if (isValid)
                {
                    temp = _cusBl.GetByClubMemberID(memberIdSearch);
                    result = temp;
                    if (result == null || !result.Any())
                    {
                        emptySearch();
                    }
                    else
                    {
                        listView.ItemsSource = result;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Input-member Id contains only numbers");
                }
            }
            if (field == 6) //get all
            {
                _cusBl = new ClubMember_BL();
                List<ClubMember> temp = null;
                temp = _cusBl.GetAllClubMember();
                result = temp;
                if (result == null || !result.Any())
                {
                    emptySearch();
                }
                else
                {
                    listView.ItemsSource = result;
                }
            }
        }

        //FN
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            field = 0;
            searchBox.IsEnabled = true;
        }

        //Ln
        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            field = 1;
            searchBox.IsEnabled = true;
        }

        //gender
        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            field = 2;
            searchBox.IsEnabled = true;
        }

        //tz
        private void ComboBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {
            field = 3;
            searchBox.IsEnabled = true;
        }

        //DOB
        private void ComboBoxItem_Selected_4(object sender, RoutedEventArgs e)
        {
            field = 4;
            searchBox.IsEnabled = true;
        }

        //member id
        private void ComboBoxItem_Selected_5(object sender, RoutedEventArgs e)
        {
            field = 5;
            searchBox.IsEnabled = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected_6(object sender, RoutedEventArgs e)
        {
            field = 6;
            searchBox.IsEnabled = false;
        }

        private void ComboBoxItem_Selected_7(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBoxItem_Selected_8(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBoxItem_Selected_9(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBoxItem_Selected_10(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBoxItem_Selected_11(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBoxItem_Selected_12(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBoxItem_Selected_13(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

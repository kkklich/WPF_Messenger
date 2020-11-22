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
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Drawing;


namespace WPF_Messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        List<Class1> listClasa;
        readonly ClassMain classMain1;
        bool ifFirstBtn = true;
        int nrSearch = 0;
        List<Class1> listNr;


        public MainWindow()
        {
            InitializeComponent();

            listClasa = new List<Class1>();
            classMain1 = new ClassMain();

            

            cmbGroup.Items.Add("Użytkownik");
            cmbGroup.Items.Add("Dzień tygodnia");
            cmbGroup.Items.Add("Miesiąc");
            cmbGroup.Items.Add("Rok");
        }




        //open new file
        private void btn_file_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                listviewmessage.ItemsSource = null;
                listviewmessage.Items.Clear();


                Tuple<string, string, List<Class1>, List<ClassMain>> tup1;
                tup1 = classMain1.AllFunction();


                lbl_nameFile.Content = tup1.Item1;
                lbl_count.Content = tup1.Item2;
                listClasa = tup1.Item3;
                listviewmessage.ItemsSource = tup1.Item3;

                listViewCount.ItemsSource = tup1.Item4;


                //listviewmessage.ItemsSource = listClasa.ToList();
            }catch(IOException )
            {
                MessageBox.Show("Błąd pliku");
            }catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }
            


        }



        //function search  BUtton
       
        private void button_Click(object sender, RoutedEventArgs e)
        {

            SearchList();
        }


        //txt search
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ifFirstBtn = true;
            nrSearch = 0;
            btn_search.Content = "Wyszukaj";
        }




        //cmb group by
        private void cmbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int cmbNr = cmbGroup.SelectedIndex;

                Tuple<List<ClassMain>, string> tuple1 = classMain1.CmbLinq(cmbNr);

                listViewCount.ItemsSource = tuple1.Item1;
                gridName1.Header = tuple1.Item2;

            }catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }




        //Datapicker
        private void DataPick1_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {

                if (DataPick1.SelectedDate == null)
                {
                    MessageBox.Show("Nie wybrano daty", "Uwaga");
                }
                else
                {

                    DateTime datePic = (DateTime)DataPick1.SelectedDate;

                    List<Class1> listInt = classMain1.DataLinq(datePic);



                    if (listInt.Count > 0)
                    {
                        var meitem = listviewmessage.Items[listInt[0].Number];
                        listviewmessage.ScrollIntoView(meitem);

                    }
                    else
                    {
                        MessageBox.Show("W wybranej dacie nie odbyła konwersacja", "Uwaga");
                    }
                }

            }catch(FormatException)
            {
                MessageBox.Show("Zły format daty");

            }catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }


        //function search list
        void SearchList()
        {

            try
            {

                if (ifFirstBtn)
                {

                    string wordSearch = txtSearch.Text.ToLower();

                    var linqNr = from x in listClasa
                                 where x.Text.ToLower().Contains(wordSearch) || x.User.ToLower().Contains(wordSearch)
                                 select x;




                    listNr = new List<Class1>();
                    listNr = linqNr.ToList();

                    MessageBox.Show("Ilość znalezionych słów: " + listNr.Count.ToString());
                    ifFirstBtn = false;
                    btn_search.Content = "Kolejny";


                }



                if (listNr.Count > 0 & nrSearch <= listNr.Count - 1)
                {


                    var meitem = listviewmessage.Items[listNr[nrSearch].Number - 1];

                    listviewmessage.ScrollIntoView(meitem);
                    MessageBox.Show("Wyszukiwane słowo jest w " + listNr[nrSearch].Number.ToString() + " wierszu ", (nrSearch + 1).ToString() + "/" + listNr.Count.ToString());

                    nrSearch++;
                }


                if (nrSearch == listNr.Count)
                {
                    ifFirstBtn = true;
                    nrSearch = 0;
                    btn_search.Content = "Wyszukaj";
                }

            }catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }

        }


    }
}

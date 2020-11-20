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
        StreamReader streamread;
        List<Class1> listClasa;

        public MainWindow()
        {
            InitializeComponent();

;

            listClasa = new List<Class1>();

            cmbGroup.Items.Add("Użytkownik");
            cmbGroup.Items.Add("Dzień tygodnia");
            cmbGroup.Items.Add("Miesiąc");
            cmbGroup.Items.Add("Rok");
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }


        //open new file
        private void btn_file_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog open = new OpenFileDialog();

                open.Multiselect = true;
                open.Filter = "Pliki webowe (*.html)|*.html;";

                if (open.ShowDialog() == true)
                {


                    //open.OpenFiles;
                    Stream[] listStream = open.OpenFiles();
                    //listStream = open.OpenFiles();
                    string alltext = "";
                    string title = "title";


                    foreach (var x in listStream)
                    {
                        streamread = new StreamReader(x);
                        string tempText = streamread.ReadToEnd();
                        if (tempText.Contains($"<div class=\"_3b0c\"><div class=\"_3b0d\">"))
                        {
                            int positionTitle = tempText.IndexOf($"<div class=\"_3b0c\"><div class=\"_3b0d\">");
                            int positionEndTitle = tempText.IndexOf($"</div></div></div><div class=\"_4t5n\" role=\"main\">");
                            // MessageBox.Show(positionTitle.ToString()+"  "+ positionEndTitle.ToString()); 
                            if (positionTitle > 0 & positionEndTitle > 0)
                                title = tempText.Substring(positionTitle + 38, positionEndTitle - positionTitle - 38);


                            tempText = tempText.Remove(0, positionTitle);

                        }

                        alltext += tempText;
                        streamread.Close();

                        //string textFile = streamread.ReadToEnd();
                        // txtBlockFile.Text = File.ReadAllText(open.FileName);

                    }



                    openFile(alltext);
                    lbl_nameFile.Content = "  Nazwa konwersacji: " + title;


                    //streamread = new StreamReader(open.FileName);


                }

            }catch(IOException )
            {
                MessageBox.Show("Błą pliku");
            }catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        
        //function open new file
        private void openFile(string textFile)
        {
            try
            {    
                listviewmessage.ItemsSource = null;
                listviewmessage.Items.Clear();

                lbl_count.Content = countMessage(textFile);
                groupMessages();

            }
            catch (FileFormatException)
            {
                MessageBox.Show("Zły format pliku (HTML) ");
            }
            catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }
            
        }





        //function count and display message
        private string countMessage(string text)
        {

            string message = text;
            string code = "pam _3-95 _2pi0";// _2lej uiBoxWhite noborder"
            
            string codeName = "_3-96 _2pio _2lek _2lel";
            string codeText = $"</div><div class=\"_3-96 _2let";
            string codeTime = "_3-94 _2lem";         

            bool ifFirst = true;        
            int countword;
            int numberMessage = 0;

            listClasa = new List<Class1>();
            List<string> listMessage = message.Split(code).ToList();
            //listMessage = 

            //listviewmessage.ItemsSource = message.Split(code).ToList();


            countword = message.Split(code).Length-1;


            foreach (var mess in message.Split(code))
            {

                if (ifFirst)
                    ifFirst = false;
                else
                {

                    Class1 klasa1 = new Class1();
                    int positionDiv1 = mess.IndexOf(codeName)+25;
                    int positionDiv2 = mess.IndexOf(codeText);
                    int positionDiv3 = mess.IndexOf(codeTime);
                    int positionDiv4 = mess.LastIndexOf("</div></div><div class=");
                    // MessageBox.Show(positionDiv1.ToString());
                    // MessageBox.Show(mess.LastIndexOf("</div></div><div class=").ToString());


                    // MessageBox.Show(mess.Substring(positionDiv1, positionDiv2-positionDiv1));
                    // MessageBox.Show(mess.Substring(positionDiv2+52, positionDiv3 - positionDiv2 - 104));
                    //  MessageBox.Show(mess.Substring(positionDiv3 + 13,positionDiv4-positionDiv3-13 ));


                    if (positionDiv2 - positionDiv1 >= 0 & positionDiv3 - positionDiv2 - 104 >= 0 & positionDiv4 - positionDiv3 - 13 > 0)
                    {
                        numberMessage++;
                        klasa1.Number = numberMessage;
                       
                        klasa1.User = mess.Substring(positionDiv1, positionDiv2 - positionDiv1);


                        string textMessage= mess.Substring(positionDiv2 + 52, positionDiv3 - positionDiv2 - 104);

                       

                        if(textMessage.Contains("<a href") )
                        {
                            int positionAhref = textMessage.IndexOf("<a href")+9;
                         

                            string newLink= textMessage.Remove(0, positionAhref);
                            int positionT = newLink.IndexOf('"');
                            string link = newLink.Substring(0, positionT);
                         klasa1.Text = link;
                        }
                        else
                        {
                            klasa1.Text = textMessage;
                        }
                     

                       
                        string data= mess.Substring(positionDiv3 + 13, positionDiv4 - positionDiv3 - 13);


                        if (data.Contains("</div>"))
                        {
                            int pos = data.IndexOf("</div></div>");
                            data=data.Substring(0, pos);
                            
                        }

                        var timeDate = DateTime.Parse(data);

                        //MessageBox.Show(timeDate.Year.ToString());

                        klasa1.Time = timeDate.ToString("dddd, dd MMMM yyyy HH:mm:ss");

                        listClasa.Add(klasa1);
                    }
                    else {
                       // MessageBox.Show(mess+ "   positionDiv2 - positionDiv1: " + (positionDiv2 - positionDiv1) + "positionDiv3 - positionDiv2 - 104: " + (positionDiv3 - positionDiv2 - 104) + "positionDiv4 - positionDiv3 - 13: " + (positionDiv4 - positionDiv3 - 13 ));
                    }                    
                }
            }          
      

            listviewmessage.ItemsSource = listClasa.ToList();
            return numberMessage.ToString();

        }


        //function grouping message linq
        private void groupMessages()
        {

            var linqCountMessages = from x in listClasa
                                    group x by x.User into xgroup
                                    select new { User = xgroup.Key, CountText = xgroup.Count(), };


            
            var linqOrder = from x in linqCountMessages
                            orderby x.CountText descending
                            select x;


            listViewCount.ItemsSource = linqOrder.ToList();

            

        }




        //function search  BUtton
        bool ifFirstBtn = true;
        int nrSearch = 0;
        List<Class1> listNr;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (ifFirstBtn)
            {

                string wordSearch = txtSearch.Text.ToLower();

                var linqNr = from x in listClasa
                             where x.Text.ToLower().Contains(wordSearch) || x.User.ToLower().Contains(wordSearch)
                             select x;

               
           

                listNr = new List<Class1>();
                listNr = linqNr.ToList();

                MessageBox.Show("Ilość znalezionych słów: "+listNr.Count.ToString());

                if (listNr.Count > 0)
                {
                   // MessageBox.Show("Pier",listNr[0].Number.ToString());
                    MessageBox.Show("Wyszukiwane słowo jest w " + listNr[nrSearch].Number.ToString() + " wierszu ", (nrSearch + 1).ToString()+"/"+listNr.Count.ToString());



                    var meitem = listviewmessage.Items[listNr[0].Number - 1];

                    listviewmessage.ScrollIntoView(meitem);


                    ifFirstBtn = false;
                    btn_search.Content = "Kolejny";
                }

            }
            else
            {
                if (listNr.Count>0 & nrSearch < listNr.Count-1)
                {
                    nrSearch++;
                    MessageBox.Show("Wyszukiwane słowo jest w "+ listNr[nrSearch].Number.ToString() + " wierszu ",(nrSearch+1).ToString()+"/"+listNr.Count.ToString());

                    var meitem = listviewmessage.Items[listNr[nrSearch].Number-1];

                    listviewmessage.ScrollIntoView(meitem);
                    

                }

                if (nrSearch == listNr.Count - 1)
                {
                    ifFirstBtn = true;
                    nrSearch = 0;
                    btn_search.Content = "Wyszukaj";
                }
                
                

            }

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ifFirstBtn = true;
            nrSearch = 0;
        }


        //cmb group by
        private void cmbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            

            int cmbNr = cmbGroup.SelectedIndex;


            var linqCountMessages = from x in listClasa
                                    group x by x.User into xgroup
                                    select new { User = xgroup.Key, CountText = xgroup.Count(), };


            string thingGroupBy;

            if (cmbNr >= 0)
            {
                var groupThing = cmbGroup.SelectedItem;
              //  MessageBox.Show(groupThing.ToString() + "  " + cmbGroup.SelectedIndex.ToString());

                switch (cmbNr)
                {


                    case 0:
                        gridName1.Header = "Użytkownik";
                        linqCountMessages = from x in listClasa
                                            group x by x.User into xgroup
                                            select new { User = xgroup.Key, CountText = xgroup.Count(), };                       

                        break;


                    case 1:
                        gridName1.Header = "Dzień";

                        linqCountMessages = from x in listClasa
                                            group x by DateTime.Parse(x.Time).ToString("dddd") into xgroup
                                            select new { User = xgroup.Key, CountText = xgroup.Count(), };
                        break;



                    case 2:
                        gridName1.Header = "Miesiąc";
                        linqCountMessages = from x in listClasa
                                            group x by DateTime.Parse(x.Time).ToString("MMMM") into xgroup
                                            select new { User = xgroup.Key, CountText = xgroup.Count(), };
                        break;

                    case 3:
                        gridName1.Header = "Rok";
                        linqCountMessages = from x in listClasa
                                            group x by DateTime.Parse(x.Time).Year.ToString() into xgroup
                                            select new { User = xgroup.Key, CountText = xgroup.Count(), };
                        break;


                }
            }



            var linqOrder = from x in linqCountMessages
                            orderby x.CountText descending
                            select x;


            listViewCount.ItemsSource = linqOrder.ToList();
        }

        private void DataPick1_CalendarClosed(object sender, RoutedEventArgs e)
        {

            DateTime datePic = (DateTime)DataPick1.SelectedDate;

            

            var linqDate = from x in listClasa
                           where DateTime.Parse(x.Time).Year  == datePic.Year & DateTime.Parse(x.Time).Month==datePic.Month & DateTime.Parse(x.Time).Day==datePic.Day
                           select x;

            List<Class1> listInt = new List<Class1>();
           
            listInt = linqDate.ToList();
            
            if (listInt.Count > 0)
            {
                var meitem = listviewmessage.Items[listInt[0].Number];

                listviewmessage.ScrollIntoView(meitem);


            }
        }
    }
}

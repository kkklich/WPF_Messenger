using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WPF_Messenger
{
    class Class1
    {
        public int Number { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }

    }

    class ClassMain
    {

        public string Grouping { get; set; }
        public int CountText { get; set; }

        List<Class1> listClasa = new List<Class1>();



        public Tuple<string, string, List<Class1>, List<ClassMain>> AllFunction()
        {

            Tuple<string, string> tupleFile = FileDialogOpen();
            string title1 = tupleFile.Item1;
            string text = tupleFile.Item2;

            Tuple<string, List<Class1>> tupleCount = CountMessage(text);

            string numberMessage = tupleCount.Item1;
            List<Class1> listMessage = tupleCount.Item2;



            return Tuple.Create(title1, numberMessage, listMessage, GroupMessages());
        }

        private Tuple<string, string> FileDialogOpen()
        {

            StreamReader streamread;
            OpenFileDialog open = new OpenFileDialog();

            Stream[] listStream;
            //listStream = open.OpenFiles();
            string alltext = "";
            string title = "title";


            open.Multiselect = true;
            open.Filter = "Pliki webowe (*.html)|*.html;";

            if (open.ShowDialog() == true)
            {


                //open.OpenFiles;

                listStream = open.OpenFiles();

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



                //openFile(alltext);
                // string x=  countMessage(alltext).Item1;
                // groupMessages();

                //return "  Nazwa konwersacji: " + title;



                //streamread = new StreamReader(open.FileName);


            }


            return Tuple.Create(title, alltext);




        }



        //function grouping message linq
        List<ClassMain> GroupMessages()
        {

            var linqCountMessages = from x in listClasa
                                    group x by x.User into xgroup
                                    select new ClassMain() { Grouping = xgroup.Key, CountText = xgroup.Count(), };



            var linqOrder = from x in linqCountMessages
                            orderby x.CountText descending
                            select x;

            List<ClassMain> listMain = new List<ClassMain>();

            listMain = linqOrder.ToList();
            //var listview1 = linqOrder.ToList();

            // List<ClassMain> lista = (ClassMain)linqOrder.ToList();

            return listMain;
            // return linqOrder.ToList();

            // listViewCount.ItemsSource = linqOrder.ToList();

        }




        //function count and display message
        private Tuple<string, List<Class1>> CountMessage(string text)
        {

            string message = text;
            string code = "pam _3-95 _2pi0";// _2lej uiBoxWhite noborder"

            string codeName = "_3-96 _2pio _2lek _2lel";
            string codeText = $"</div><div class=\"_3-96 _2let";
            string codeTime = "_3-94 _2lem";

            bool ifFirst = true;
            // int countword;
            int numberMessage = 0;

            listClasa = new List<Class1>();
            //List<string> listMessage = message.Split(code).ToList();
            //listMessage = 

            //listviewmessage.ItemsSource = message.Split(code).ToList();


            //  countword = message.Split(code).Length - 1;


            foreach (var mess in message.Split(code))
            {

                if (ifFirst)
                    ifFirst = false;
                else
                {

                    Class1 klasa1 = new Class1();
                    int positionDiv1 = mess.IndexOf(codeName) + 25;
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
                        int a = positionDiv2 - positionDiv1;

                        klasa1.User = mess.Substring(positionDiv1, a);


                        string textMessage = mess.Substring(positionDiv2 + 52, positionDiv3 - positionDiv2 - 104);



                        if (textMessage.Contains("<a href"))
                        {
                            int positionAhref = textMessage.IndexOf("<a href") + 9;


                            string newLink = textMessage.Remove(0, positionAhref);
                            int positionT = newLink.IndexOf('"');
                            string link = newLink.Substring(0, positionT);
                            klasa1.Text = link;
                        }
                        else
                        {
                            klasa1.Text = textMessage;
                        }



                        string data = mess.Substring(positionDiv3 + 13, positionDiv4 - positionDiv3 - 13);


                        if (data.Contains("</div>"))
                        {
                            int pos = data.IndexOf("</div></div>");
                            data = data.Substring(0, pos);

                        }

                        var timeDate = DateTime.Parse(data);

                        //MessageBox.Show(timeDate.Year.ToString());

                        klasa1.Time = timeDate.ToString("dddd, dd MMMM yyyy HH:mm:ss");

                        listClasa.Add(klasa1);
                    }
                    else
                    {
                        // MessageBox.Show(mess+ "   positionDiv2 - positionDiv1: " + (positionDiv2 - positionDiv1) + "positionDiv3 - positionDiv2 - 104: " + (positionDiv3 - positionDiv2 - 104) + "positionDiv4 - positionDiv3 - 13: " + (positionDiv4 - positionDiv3 - 13 ));
                    }
                }
            }
            //listviewmessage.ItemsSource = listClasa.ToList();
            return Tuple.Create(numberMessage.ToString(), listClasa);
            //  numberMessage.ToString();

        }




        public List<Class1> DataLinq(DateTime datePic)
        {
            var linqDate = from x in this.listClasa
                           where DateTime.Parse(x.Time).Year == datePic.Year & DateTime.Parse(x.Time).Month == datePic.Month & DateTime.Parse(x.Time).Day == datePic.Day
                           select x;

            //List<Class1> listInt = new List<Class1>();

            //listInt = linqDate.ToList();

            return linqDate.ToList();
        }






        public Tuple<List<ClassMain>, string> CmbLinq(int cmbNr)
        {
            var linqCountMessages = from x in listClasa
                                    group x by x.User into xgroup
                                    select new ClassMain() { Grouping = xgroup.Key, CountText = xgroup.Count(), };
            string gridHeader = "Użytkownik";

            if (cmbNr >= 0)
            {

                switch (cmbNr)
                {


                    case 0:
                        gridHeader = "Użytkownik";
                        linqCountMessages = from x in listClasa
                                            group x by x.User into xgroup
                                            select new ClassMain() { Grouping = xgroup.Key, CountText = xgroup.Count(), };

                        break;


                    case 1:
                        gridHeader = "Dzień";

                        linqCountMessages = from x in listClasa
                                            group x by DateTime.Parse(x.Time).ToString("dddd") into xgroup
                                            select new ClassMain() { Grouping = xgroup.Key, CountText = xgroup.Count(), };
                        break;



                    case 2:
                        gridHeader = "Miesiąc";
                        linqCountMessages = from x in listClasa
                                            group x by DateTime.Parse(x.Time).ToString("MMMM") into xgroup
                                            select new ClassMain() { Grouping = xgroup.Key, CountText = xgroup.Count(), };
                        break;

                    case 3:
                        gridHeader = "Rok";
                        linqCountMessages = from x in listClasa
                                            group x by DateTime.Parse(x.Time).Year.ToString() into xgroup
                                            select new ClassMain() { Grouping = xgroup.Key, CountText = xgroup.Count(), };
                        break;


                }
            }



            var linqOrder = from x in linqCountMessages
                            orderby x.CountText descending
                            select x;

            return Tuple.Create(linqOrder.ToList(), gridHeader);

        }


    }

}

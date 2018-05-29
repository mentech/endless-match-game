using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
namespace MatchGame
{
    public partial class GoogleAnalyticsApi
    {

        /*
        *   Author: Tyler Hughes
        *   Credit for the Track function and the Enum HitType goes to 0liver (https://gist.github.com/0liver/11229128)
        *   Credit goes to spyriadis (http://www.spyriadis.net/2014/07/google-analytics-measurement-protocol-track-events-c/)
        *       for the idea of putting the values for each tracking method in its own function
        *
        *   Documentation of the Google Analytics Measurement Protocol can be found at:
        *   https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
        */
        static Random rnd = new Random();
        static int month = rnd.Next(44444, 99999);
        private static string endpoint = "http://www.google-analytics.com/collect";
        private static string googleVersion = "1";
        static string googleTrackingID = "UA-117508048-1";
        static string googleClientID = month.ToString();
        public static string scrtxt = "anasayfa";


        public static void TrackEvent()
        {
            var values = new Dictionary<string, string>
                {
                    { "v", "1" },
                   { "tid", "UA-117508048-1" },
                   { "cid",  googleClientID},
                   { "t", "event" },
                   { "ec", "butonaBasildi" },
                   { "ea", "buton"},
                   { "el", "label"},
                };
            Track(values);
        }

        public static void TrackPageview()
        {

            var values = new Dictionary<string, string>
                {
                    { "v", "1" },
                   { "tid", "UA-117508048-1" },
                   { "cid",  googleClientID},
                   { "t", "screenview" },
                   { "cd", scrtxt },
                   { "an", "mentechGame"},
                   { "aid", "applicationid"},
                   { "av", "versiyon02.34"},
                };



            Track(values);
        }

        private static void Track(Dictionary<string, string> values)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.KeepAlive = false;

            var postDataString = values
                .Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key,
                                                             HttpUtility.UrlEncode(next.Value)))
                .TrimEnd('&');

            // set the Content-Length header to the correct value
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

            // write the request body to the request
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postDataString);
            }

            try
            {
                // Send the response to the server
                var webResponse = (HttpWebResponse)request.GetResponse();

                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode, "Google Analytics tracking did not return OK 200");
                }

                webResponse.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }

    public partial class Form1 : Form
    {




        public int level =1;
        public Point emtyPoint;
        public int HamleSayisi =0;
        Button[] Buttons = new Button[16];
        Point[] Points = new Point[16];
        DateTime startTime = DateTime.Now;

        Button[] ButtonsAuto = new Button[16];
        Point[] PointsAuto = new Point[16];



        public object KeyCode { get; private set; }

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            startGame();

            startTime = DateTime.Now;
            timerSaat.Start();

            try
            {
                GoogleAnalyticsApi.TrackPageview();
            }
            catch (Exception)
            {

               
            }
            

        }


        public void startGame()
        {
            lbllevel.Text = level.ToString();
            TaniButonlari();
           
            ButonlarAuto();
            // karistir();
            OyunuLeveleGoreBaslat();
            
            try
            {
                GoogleAnalyticsApi.scrtxt = "level" + level;
                GoogleAnalyticsApi.TrackPageview();
            }
            catch (Exception)
            {

                
            }

        }


        void isFinish()
        {
            bool matched =true;
            for (int i = 0; i < 16; i++)
            {
                if (Buttons[i].Text == ButtonsAuto[i].Text && matched)
                {

                }
                else
                {
                    matched = !matched;
                    break;
                }
            }


            if (matched)
            {
                level++;
                lbllevel.Text = level.ToString();
                MessageBox.Show("Tebrikler Level " + level + "'e geçtiniz! ");
                startGame();
            }

           
        }


        public void OyunuLeveleGoreBaslat()
        {

            startTime = DateTime.Now;
            timerSaat.Start();


            int otoTiklananBtnID=0;


            for (int saydir = 0; saydir < level ; saydir++)
            {

            

                Random rnd = new Random();
                int[] intler = new int[16];
                int k = 0;
                int b ;
                bool exist;
                while (k != intler.Length)
                {
                    exist = false;
                    b = (rnd.Next(16) + 1);
                    for (int j = 0; j < intler.Length; j++)
                        if (intler[j] == b) { exist = true;  }
                    if (!exist) { intler[k++] = b; }
                }
              
               

                if (intler[saydir % 16] >15)
                {
                    otoTiklananBtnID = intler[saydir % 16] -2;
                }
                else
                {
                    otoTiklananBtnID = intler[saydir % 16];
                }

               
               

               Button     buton = new Button();
                buton=ButtonsAuto[otoTiklananBtnID];
                buton.Text = (Convert.ToInt32(buton.Text) + 1).ToString();
                if (Convert.ToInt32(buton.Text) > 3)
                {
                    buton.Text = "0";
                }

                for (int i = 0; i < 16; i++)
                {




                    // sol butonu arttırır
                    if (ButtonsAuto[i].Location.X + 100 == buton.Location.X && ButtonsAuto[i].Location.Y == buton.Location.Y && ButtonsAuto[i].Text != "0")
                    {
                        ButtonsAuto[i].Text = (Convert.ToInt32(ButtonsAuto[i].Text) + 1).ToString();
                        if (Convert.ToInt32(ButtonsAuto[i].Text) > 3)
                        {
                            ButtonsAuto[i].Text = "0";
                        }
                    }

                    // sağ butonu arttırır
                    if (ButtonsAuto[i].Location.X - 100 == buton.Location.X && ButtonsAuto[i].Location.Y == buton.Location.Y && ButtonsAuto[i].Text != "0")
                    {
                        ButtonsAuto[i].Text = (Convert.ToInt32(ButtonsAuto[i].Text) + 1).ToString();
                        if (Convert.ToInt32(ButtonsAuto[i].Text) > 3)
                        {
                            ButtonsAuto[i].Text = "0";
                        }
                    }

                    // üst butonu arttırır
                    if (ButtonsAuto[i].Location.X == buton.Location.X && ButtonsAuto[i].Location.Y == buton.Location.Y - 100 && ButtonsAuto[i].Text != "0")
                    {
                        ButtonsAuto[i].Text = (Convert.ToInt32(ButtonsAuto[i].Text) + 1).ToString();
                        if (Convert.ToInt32(ButtonsAuto[i].Text) > 3)
                        {
                            ButtonsAuto[i].Text = "0";
                        }
                    }

                    // alt butonu arttırır
                    if (ButtonsAuto[i].Location.X == buton.Location.X && ButtonsAuto[i].Location.Y == buton.Location.Y + 100 && ButtonsAuto[i].Text != "0")
                    {
                        ButtonsAuto[i].Text = (Convert.ToInt32(ButtonsAuto[i].Text) + 1).ToString();
                        if (Convert.ToInt32(ButtonsAuto[i].Text) > 3)
                        {
                            ButtonsAuto[i].Text = "0";
                        }
                    }


                    if (ButtonsAuto[i].Text=="1")
                    {
                        ButtonsAuto[i].BackColor = Color.LightGreen;
                        
                    }
                    if (ButtonsAuto[i].Text == "2")
                    {
                        ButtonsAuto[i].BackColor = Color.LightBlue;

                    }
                    if (ButtonsAuto[i].Text == "3")
                    {
                        ButtonsAuto[i].BackColor = Color.LightCoral;

                    }
                    if (ButtonsAuto[i].Text == "0")
                    {
                        ButtonsAuto[i].BackColor = Color.LightGray;

                    }

                    if (ButtonsAuto[i].Text == "0")
                    {
                        ButtonsAuto[i].ForeColor = Color.LightGray ;

                    }
                    else
                    {
                        ButtonsAuto[i].ForeColor = Color.Black;
                    }
                }


            }

            //Eğer dolu kutu sayısı az ve level yüksekse methodu tekrar çağırır

            if (level>19)
            {
                int kacKutuDolu = 0;
                for (int i = 0; i < 16; i++)
                {
                    if (ButtonsAuto[i].Text!="0")
                    {
                        kacKutuDolu++;
                    }
                }

               // MessageBox.Show("dolu kutu sayısı: " + kacKutuDolu);

                if (kacKutuDolu<8)
                {
                    OyunuLeveleGoreBaslat();
                }
            }

        }




    private void buttons_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;


            try
            {

                GoogleAnalyticsApi.TrackEvent();
            }
            catch (Exception)
            {

                
            }

            but.Text = (Convert.ToInt32(but.Text) + 1).ToString();
            if (Convert.ToInt32(but.Text) > 3)
            {
                but.Text = "0";
            }

            for (int i = 0; i < 16; i++)
            {
               
               // sol butonu arttırır
                if (Buttons[i].Location.X + 100 == but.Location.X && Buttons[i].Location.Y == but.Location.Y && Buttons[i].Text!="0")
                {
                    Buttons[i].Text = (Convert.ToInt32(Buttons[i].Text) + 1).ToString();
                    if (Convert.ToInt32(Buttons[i].Text) > 3)
                    {
                        Buttons[i].Text = "0";
                    }
                }

                // sağ butonu arttırır
                if (Buttons[i].Location.X - 100 == but.Location.X && Buttons[i].Location.Y == but.Location.Y && Buttons[i].Text != "0")
                {
                    Buttons[i].Text = (Convert.ToInt32(Buttons[i].Text) + 1).ToString();
                    if (Convert.ToInt32(Buttons[i].Text) > 3)
                    {
                        Buttons[i].Text = "0";
                    }
                }

                // üst butonu arttırır
                if (Buttons[i].Location.X  == but.Location.X && Buttons[i].Location.Y == but.Location.Y - 100 && Buttons[i].Text != "0")
                {
                    Buttons[i].Text = (Convert.ToInt32(Buttons[i].Text) + 1).ToString();
                    if (Convert.ToInt32(Buttons[i].Text) > 3)
                    {
                        Buttons[i].Text = "0";
                    }
                }

                // alt butonu arttırır
                if (Buttons[i].Location.X  == but.Location.X && Buttons[i].Location.Y == but.Location.Y + 100 && Buttons[i].Text != "0")
                {
                    Buttons[i].Text = (Convert.ToInt32(Buttons[i].Text) + 1).ToString();
                    if (Convert.ToInt32(Buttons[i].Text) > 3)
                    {
                        Buttons[i].Text = "0";
                    }
                }


                if (Buttons[i].Text == "1")
                {
                    Buttons[i].BackColor = Color.LightGreen;

                }
                if (Buttons[i].Text == "2")
                {
                    Buttons[i].BackColor = Color.LightBlue;

                }
                if (Buttons[i].Text == "3")
                {
                    Buttons[i].BackColor = Color.LightCoral;

                }
                if (Buttons[i].Text == "0")
                {
                    Buttons[i].BackColor = Color.LightGray;

                }


                if (Buttons[i].Text == "0")
                {
                    Buttons[i].ForeColor = Color.LightGray;

                }
                else
                {
                    Buttons[i].ForeColor = Color.Black;
                }


            }


            HamleSayisi++;
            lblHamleSayici.Text = HamleSayisi.ToString();
            isFinish();
          //  isfinish();
        }

        private void karıştırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //karistir();

            TaniButonlari();
            ButonlarAuto();
            OyunuLeveleGoreBaslat();
            HamleSayisi = 0;
            lblHamleSayici.Text = HamleSayisi.ToString();
        }


     

        public void timerSaat_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - startTime;
            lblTimer.Text = ((int)ts.TotalSeconds).ToString();


        }

        public void FinishControlMode()
        {

            
                    for (int i = 0; i < 15; i++)
                    {
                        Buttons[i].Location = Points[i];
                        Buttons[i].Text = (i + 1).ToString();
                        Buttons[i].Visible = true;
                        emtyPoint.X = 320;
                        emtyPoint.Y = 340;

                    }
                

        }


        public void ButonlarAuto()
        {
            ButtonsAuto[0] = button32;
            ButtonsAuto[1] = button31;
            ButtonsAuto[2] = button30;
            ButtonsAuto[3] = button29;
            ButtonsAuto[4] = button28;
            ButtonsAuto[5] = button27;
            ButtonsAuto[6] = button26;
            ButtonsAuto[7] = button25;
            ButtonsAuto[8] = button24;
            ButtonsAuto[9] = button23;
            ButtonsAuto[10] = button22;
            ButtonsAuto[11] = button21;
            ButtonsAuto[12] = button20;
            ButtonsAuto[13] = button19;
            ButtonsAuto[14] = button18;
            ButtonsAuto[15] = button17;

            for (int i = 0; i < 16; i++)
            {
                PointsAuto[i] = ButtonsAuto[i].Location;
                ButtonsAuto[i].BackColor = Color.LightGray;
                ButtonsAuto[i].Text = "0";   //(i+1).ToString();
                ButtonsAuto[i].Visible = true;

            }

        }

        public void TaniButonlari()
        {
            Buttons[0] = button1;
            Buttons[1] = button2;
            Buttons[2] = button3;
            Buttons[3] = button4;
            Buttons[4] = button5;
            Buttons[5] = button6;
            Buttons[6] = button7;
            Buttons[7] = button8;
            Buttons[8] = button9;
            Buttons[9] = button10;
            Buttons[10] = button11;
            Buttons[11] = button12;
            Buttons[12] = button13;
            Buttons[13] = button14;
            Buttons[14] = button15;
            Buttons[15] = button16;


            for (int i = 0; i < 16; i++)
            {
                Points[i] = Buttons[i].Location;
                Buttons[i].BackColor = Color.LightGray;
                Buttons[i].Text = "0";   //(i+1).ToString();
                Buttons[i].Visible = true;
               


                if (Buttons[i].Text == "0")
                {
                    Buttons[i].ForeColor = Color.LightGray;

                }
                else
                {
                    Buttons[i].ForeColor = Color.Black;
                }
            }

            


        }



        
        private void zorToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
           // karistir();
            for (int i = 0; i < 15; i++)
            {
               
                  Buttons[i].Text = Char.ConvertFromUtf32(65+i);
                  Buttons[i].Visible = true;
            }

            lokasyonGostergelerNormal();
            HamleSayisi = 0;
            lblHamleSayici.Text = HamleSayisi.ToString();
        }



        private void ortaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i < 15; i++)
            {
               
                Buttons[i].Text = (i + 1).ToString();
                Buttons[i].Visible = true;
               
            }
          //  karistir();

            lokasyonGostergelerNormal();
            HamleSayisi = 0;
            lblHamleSayici.Text = HamleSayisi.ToString();
        }

        private void kolayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }


        public void lokasyonGostergelerEasy()
        {
            lblHamleSayici.Location = new Point(lblHamleSayici.Location.X, 360);
            lblTimer.Location = new Point(lblTimer.Location.X, 360);
            label1.Location = new Point(label1.Location.X, 360);
            label2.Location = new Point(label2.Location.X, 360);

            this.Size = new Size(355, 430);
        }

      

        public void lokasyonGostergelerNormal()
        {
            lblHamleSayici.Location = new Point(lblHamleSayici.Location.X, 460);
            lblTimer.Location = new Point(lblTimer.Location.X, 460);
            label1.Location = new Point(label1.Location.X, 460);
            label2.Location = new Point(label2.Location.X,460);

            this.Size = new Size(455,530);
        }

        private void finishControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FinishControlMode();
            HamleSayisi = 0;
            lblHamleSayici.Text = HamleSayisi.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}

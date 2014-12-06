//https://github.com/NeilChainani

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WindowsFormsApplication1
{

   
    public partial class tinderapp : Form
    {
        public tinderapp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Tick += new EventHandler(timer_Tick);
            timer1.Start();
            getusers();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            getusers();
        }
        void login()
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["facebook_token"] = textBox3.Text;
                values["facebook_id"] = textBox2.Text;

                var response = client.UploadValues("https://api.gotinder.com/auth", values);
                var responseString = Encoding.Default.GetString(response);
               //textBox1.Text = responseString;
                var foodJsonObj = JsonConvert.DeserializeObject <RootObject1>(responseString);
                label16.Text = foodJsonObj.user.full_name;
                label18.Text = foodJsonObj.token;
                label20.Text = foodJsonObj.user._id;
                //will get later
            }
        }
        void like(string _id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.gotinder.com/like/" + _id);
            request.Headers.Add("X-Auth-Token", label18.Text);
            request.UserAgent = "User-Agent: Tinder/3.0.4 (iPhone; iOS 7.1; Scale/2.00)";
            request.ContentType = "application/json";
            var response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

           
            response.Close();
            readStream.Close();
            
        
        }

        void  getusers()
        {
            //like("53c0b8e196c9a58008a230d0");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.gotinder.com/recs");
            request.Headers.Add("X-Auth-Token", label18.Text);
            request.UserAgent = "User-Agent: Tinder/3.0.4 (iPhone; iOS 7.1; Scale/2.00)";
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "GET";
            request.UseDefaultCredentials = true;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {

                var foodJsonObj = JsonConvert.DeserializeObject <RootObject>(reader.ReadToEnd());
                if (foodJsonObj != null)
                {
                    
                    for (int i = 0; i < foodJsonObj.results.Count; i++)
                    {

                                label5.Text = foodJsonObj.results[i].common_friend_count.ToString();

                                label4.Text = foodJsonObj.results[i].name.ToString();
                                if (foodJsonObj.results[i].gender == 1)
                                {
                                    label7.Text = "female";
                                }
                                else
                                {
                                    label7.Text = "male";
                                }
                                label9.Text = foodJsonObj.results[i].distance_mi.ToString();
                                pictureBox1.ImageLocation = foodJsonObj.results[i].photos[0].url;
                                label2.Text = foodJsonObj.results[i]._id;
                                label11.Text = foodJsonObj.results[i].birth_date;
                                like(foodJsonObj.results[i]._id);
                                var t1 = new Timer { Enabled = true, Interval = 7000 };
                                t1.Tick += (o, a) => { t1.Stop(); };

                    }
                }
                
              
            }

        }
        public class ProcessedFile
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
        }

        public class Photo
        {
            public string url { get; set; }
            public List<ProcessedFile> processedFiles { get; set; }
            public string extension { get; set; }
            public string fileName { get; set; }
            public string crop { get; set; }
            public object main { get; set; }
            public string id { get; set; }
            public string shape { get; set; }
            public double? xdistance_percent { get; set; }
            public double? yoffset_percent { get; set; }
            public double? xoffset_percent { get; set; }
            public double? ydistance_percent { get; set; }
        }

        public class Result
        {
            public int distance_mi { get; set; }
            public int common_like_count { get; set; }
            public int common_friend_count { get; set; }
            public List<object> common_likes { get; set; }
            public List<object> common_friends { get; set; }
            public string _id { get; set; }
            public string bio { get; set; }
            public string birth_date { get; set; }
            public int gender { get; set; }
            public string name { get; set; }
            public string ping_time { get; set; }
            public List<Photo> photos { get; set; }
            public string birth_date_info { get; set; }
        }

        public class RootObject
        {
            public int status { get; set; }
            public List<Result> results { get; set; }
        }

        //here logined auth

        public class ProcessedFile1
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
        }

        public class Photo1
        {
            public double xdistance_percent { get; set; }
            public string id { get; set; }
            public double xoffset_percent { get; set; }
            public int yoffset_percent { get; set; }
            public double ydistance_percent { get; set; }
            public bool main { get; set; }
            public string fileName { get; set; }
            public object fbId { get; set; }
            public string extension { get; set; }
            public List<ProcessedFile> processedFiles { get; set; }
            public string url { get; set; }
        }

        public class User1
        {
            public string _id { get; set; }
            public string create_date { get; set; }
            public int age_filter_max { get; set; }
            public int age_filter_min { get; set; }
            public string api_token { get; set; }
            public string bio { get; set; }
            public string birth_date { get; set; }
            public int distance_filter { get; set; }
            public string full_name { get; set; }
            public int gender { get; set; }
            public int gender_filter { get; set; }
            public string name { get; set; }
            public string ping_time { get; set; }
            public bool discoverable { get; set; }
            public List<Photo> photos { get; set; }
        }

        public class Versions1
        {
            public string active_text { get; set; }
            public string age_filter { get; set; }
            public string matchmaker { get; set; }
            public string trending { get; set; }
            public string trending_active_text { get; set; }
        }

        public class Globals1
        {
            public string invite_type { get; set; }
            public int recs_interval { get; set; }
            public int updates_interval { get; set; }
            public int recs_size { get; set; }
            public string matchmaker_default_message { get; set; }
            public string share_default_text { get; set; }
            public int boost_decay { get; set; }
            public int boost_up { get; set; }
            public int boost_down { get; set; }
            public bool sparks { get; set; }
            public bool kontagent { get; set; }
            public bool sparks_enabled { get; set; }
            public bool kontagent_enabled { get; set; }
            public bool mqtt { get; set; }
            public bool tinder_sparks { get; set; }
            public int moments_interval { get; set; }
        }

        public class RootObject1
        {
            public string token { get; set; }
            public User1 user { get; set; }
            public Versions1 versions { get; set; }
            public Globals1 globals { get; set; }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}

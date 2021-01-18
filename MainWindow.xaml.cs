using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;

namespace baha_skip
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    /// 
  
    public partial class MainWindow : Window
    {
        List<TimeTableArray> timetable = new List<TimeTableArray>();
        public MainWindow()
        {
            InitializeComponent();
            load();
        }
        void load()
        {
            string test = "[{\"sn\":\"19635\",\"OP_Start\":\"122,20\",\"OP_End\":\"123,40\",\"ED_Start\":\"125,40\",\"ED_End\":\"150,50\"},{\"sn\":\"19636\",\"OP_Start\":\"122,20\",\"OP_End\":\"123,40\",\"ED_Start\":\"125,40\",\"ED_End\":\"150,50\"}]";
            string _path = "TimeTable/timetable.json";
            if (!File.Exists(_path))
            {
                string noSuchJson = "找不到json檔案";
                MessageBox.Show(noSuchJson);
                return;
            }
            
            string raw = "";
            using (StreamReader reader = new StreamReader(_path))
            {
                raw = reader.ReadToEnd();
            }
            timetable = JsonConvert.DeserializeObject<List<TimeTableArray>>(raw);
            refreshListView();
            
        }
        void refreshListView()
        {
            json_lv.ItemsSource = null;
            json_lv.ItemsSource = timetable;
        }
        public class TimeTableArray
        {
            public string sn { get; set; }
            public string OP_Start { get; set; }
            public string OP_End { get; set; }
            public string ED_Start { get; set; }
            public string ED_End { get; set; }
        }

        private void json_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (json_lv.SelectedIndex == -1)
            {
                return;
            }
            TimeTableArray snInfo = (TimeTableArray)json_lv.SelectedItem;

            opStart_tb.Text = TimeDecoder( snInfo.OP_Start);
            opEnd_tb.Text = TimeDecoder(snInfo.OP_End);

            edStart_tb.Text = TimeDecoder(snInfo.ED_Start);
            edEnd_tb.Text = TimeDecoder(snInfo.ED_End);

            sn_tb.Text = snInfo.sn;

          TimeEncoder(snInfo.OP_End);
        }
        string TimeEncoder(string time)
        {
            string[] temp = time.Split(' ');
            int h = 0, m = 0, s = 0;

            switch (temp.Length)
            {
                default:
                    throw new Exception("日期格式有誤");
                    break;
                case 1:
                    s = Convert.ToInt32(temp[1]);
                    return $"{(s).ToString()}";
                case 2:
                    
                    m = Convert.ToInt32(temp[0]);
                    s = Convert.ToInt32(temp[1]);
                    return $"{(m).ToString()} {(s).ToString()}";
                    
                case 3:
                    
                    h = Convert.ToInt32(temp[0]);
                    m = Convert.ToInt32(temp[1]);
                    s = Convert.ToInt32(temp[2]);
                    return $"{(h * 60 + m).ToString()} {(s).ToString()}"; 
                

            }

        }
        string TimeDecoder(string time)
        {
            string[] temp = time.Split(' ');
            int h = 0, m = 0, s = 0;
            switch (temp.Length)
            {
                default:
                    throw new Exception("日期格式有誤");
                    break;
                case 1:
                    //s = Convert.ToInt32(temp[0]);
                    if (s>60)
                    {
                        
                        m = s / 60;
                        s = s % 60;
                        if (m>60)
                        {
                            h = m / 60;
                            m = m % 60;

                            return $"{(h).ToString()} {(m).ToString()} {(s).ToString()}";
                        }
                        else
                        {
                            return $"{(m).ToString()} {(s).ToString()}";
                        }
                    }
                    else
                    {
                        return $"{(s).ToString()}";
                    }


                    
                case 2:

                    m = Convert.ToInt32(temp[0]);
                    s = Convert.ToInt32(temp[1]);

                    if (m>60)
                    {
                        h = m / 60;
                        m = m % 60;

                    }

                    return $"{(h).ToString()} {(m).ToString()} {(s).ToString()}";




            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            int selected = json_lv.SelectedIndex;
            if (selected != -1)
            {

                timetable[selected].ED_Start = TimeEncoder(edStart_tb.Text);
                timetable[selected].ED_End= TimeEncoder(edEnd_tb.Text);
                timetable[selected].OP_Start= TimeEncoder(opStart_tb.Text);
                timetable[selected].OP_End= TimeEncoder(opEnd_tb.Text);
                timetable[selected].sn= sn_tb.Text;
                save();
            }
         
        }
        private void save()
        {
            string raw=JsonConvert.SerializeObject(timetable);
            string _path = "TimeTable/timetable.json";
            using (StreamWriter streamWriter =new StreamWriter(_path))
            {
                streamWriter.Write(raw);
            }
            clearControls();
            load();
        }

        private void clearControls()
        {

            opStart_tb.Text = "";
            opEnd_tb.Text = "";


            edStart_tb.Text = "";
            edEnd_tb.Text = "";


            sn_tb.Text = "";
        }
    }
}

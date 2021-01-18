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
        }
        public class TimeTableArray
        {
            public string sn { get; set; }
            public string OP_Start { get; set; }
            public string OP_End { get; set; }
            public string ED_Start { get; set; }
            public string ED_End { get; set; }
        }
     
    }
}

using Baidu.Aip.Ocr;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace GeneralTextExtraction
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //Set APPID/AK/SK
        private const string APP_ID = "10690655";

        private const string API_KEY = "fTH0IrSpgBGZCgZFWmuFYKda";
        private const string SECRET_KEY = "1Z1Lw73kCHP4u5EKDgTqKKXX7U8C9Qoo";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Images(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == true)
                {
                    string fileName = openFileDialog.FileName;
                    tbx_filePath.Text = fileName;
                    ImageSource imgSource = new BitmapImage(new Uri(fileName));
                    imagePresentation.Source = imgSource;
                    // Text extraction from selected image
                    JObject result = ExtractTextFromImage(fileName);
                    string returnText = GetElementsFromJObject(result);
                    tbx_presentation.Text = returnText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private JObject ExtractTextFromImage(string filePath)
        {
            var client = new Ocr(API_KEY, SECRET_KEY);
            var image = File.ReadAllBytes(filePath);
            var options = new Dictionary<string, object> {
                {"language_type", "CHN_ENG" },
                {"detect_direction", "false" },
                {"detect_language", "true" },
                {"probability", "false" }
            };
            JObject TextObjet = client.GeneralBasic(image, options);
            return TextObjet;
        }

        private string GetElementsFromJObject(JObject jobject)
        {
            StringBuilder sb = new StringBuilder();
            JToken words_results = jobject["words_result"];
            foreach (JToken words_result in words_results)
            {
                foreach (JProperty property in words_result)
                {
                    sb.Append($"{Environment.NewLine}{property.Value}");
                }
            }
            return sb.ToString();
        }
    }
}
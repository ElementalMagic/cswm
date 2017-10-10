using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public double fontsize;
        
        public Window1()
        {           
            InitializeComponent();
            scrollBar.Maximum = 50;
            scrollBar.Minimum = 10;
             if (fontsize == 0) fontsize = 10;  
            label.Content = fontsize;
            MainWindow form1 = this.Owner as MainWindow;
           ipAdr.Text = MainWindow.host;
           port.Text = MainWindow.port.ToString();
            //MainWindow.port
        }
       
        
        public void scrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            MainWindow form1 = this.Owner as MainWindow;
            form1.text1.FontSize = scrollBar.Value;
            form1.text2.FontSize = scrollBar.Value;
            label.Content = Math.Round(scrollBar.Value)+" pt";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form1 = this.Owner as MainWindow;
           MainWindow.port = Int32.Parse(port.Text);
            MainWindow.host = ipAdr.Text;
            this.Close();
        }
       
    }
}

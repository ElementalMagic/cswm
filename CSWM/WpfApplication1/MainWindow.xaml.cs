using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool music = false;
        public static string userName;
        public static string host = "83.217.8.201";
        public static int port = 8886;
        private static TcpClient client;
        private static NetworkStream stream;
        private bool localNetWork = false;
        private string nonLockalHost;

      //  List<string> _IdList = new List<string>();
        List<Dialog> DialogList = new List<Dialog>();        
        public MainWindow()
        {
            InitializeComponent();
            if (music)
            {
                System.Media.SoundPlayer Audio;
                Audio = new System.Media.SoundPlayer(CSWM.Properties.Resources.hi1);
                Audio.Load();
                Audio.Play();
            }
            richTextBox.IsReadOnly = true;
            nonLockalHost = host;
            comboBox1.SelectedIndex = 0;
            DialogList.Add(new Dialog("All", "0"));
        }

        private void pusk_Click_1(object sender, RoutedEventArgs e)
        {
            Makeshifr(text1.Text);
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            MakeDeShifr(text1.Text);
        }

        private delegate void Delag(string s);

        public void MakeDeShifr(string text)
        {
            bool _IFind = false;
            int keyForShifr = 0;
            char[] alphabet = { 'а', 'б', 'в', 'г', 'д', 'е',
                'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р',
                'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ',
                'ы', 'ь', 'э', 'ю', 'я' };

            char[] keyChars = key.Text.ToCharArray();
            if (string.IsNullOrEmpty(key.Text)) { keyChars = new char[] { '1' }; }

            foreach (char ch in keyChars)
            {
                foreach (char alph in alphabet)
                {
                    if (ch == alph)
                    {
                        keyForShifr += Array.IndexOf(alphabet, alph);
                        keyChars[Array.IndexOf(keyChars, ch)] = '0';
                        _IFind = true;
                    }
                }
            }
            if (_IFind == true)
            {
                foreach (char ch in keyChars)
                {
                    try
                    {
                        if (ch != '0')
                        {
                            keyForShifr += Int32.Parse(keyChars[Array.IndexOf(keyChars, ch)].ToString());
                        }
                    }
                    catch { }
                }
            }
            else
            {
                keyForShifr = int.Parse(key.Text);
            }
            switch (comboBox1.SelectedIndex)
            {
                case (0):
                    {
                        Cezar(keyForShifr, -1, text);
                        break;
                    }
                case (1):
                    {
                        MyShifr1(keyForShifr, 1, text);
                        break;
                    }
                case (2):
                    {
                        MyShifr2(1, text);
                        break;
                    }
            }
        }

        public void Makeshifr(string text)
        {
            bool _IFind = false;
            int keyForShifr = 0;
            char[] alphabet = { 'а', 'б', 'в', 'г', 'д', 'е',
                'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р',
                'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ',
                'ы', 'ь', 'э', 'ю', 'я' };
            char[] keyChars = key.Text.ToCharArray();
            if (string.IsNullOrEmpty(key.Text)) { keyChars = new char[] { '1' }; key.Text = "1"; }
            foreach (char ch in keyChars)
            {
                foreach (char alph in alphabet)
                {
                    if (ch == alph)
                    {
                        keyForShifr += Array.IndexOf(alphabet, alph);
                        keyChars[Array.IndexOf(keyChars, ch)] = '0';
                        _IFind = true;
                    }
                }
            }
            if (_IFind == true)
            {
                foreach (char ch in keyChars)
                {
                    try
                    {
                        if (ch != '0')
                        {
                            keyForShifr += Int32.Parse(keyChars[Array.IndexOf(keyChars, ch)].ToString());
                        }
                    }
                    catch { }
                }
            }
            else
            {
                try
                { keyForShifr = int.Parse(key.Text); }
                catch { System.Windows.MessageBox.Show("Слишком большое число"); }
            }

            switch (comboBox1.SelectedIndex)
            {
                case (0):
                    {
                        Cezar(keyForShifr, 1, text);
                        break;
                    }
                case (1):
                    {
                        this.text2.Clear();
                        MyShifr1(keyForShifr, 0, text);
                        break;
                    }
                case (2):
                    {
                        this.text2.Clear();
                        MyShifr2(0, text);
                        break;
                    }
            }
        }

        private void Cezar(int keyShifr, int n, string text)
        {
            text2.Clear();
            char[] alphabet = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };
            char[] toShifr = text.ToCharArray();
            char[] shifred = toShifr;
            int key = keyShifr;
            key = key * (n);
            for (int j = 0; j < shifred.Length; j++)
            {
                int i = 0;
                int newkey = 0;
                foreach (char alphabetChar in alphabet)
                {
                    if (alphabetChar.ToString() == shifred[j].ToString().ToLower())
                    {
                        newkey = key - (key / 33 * 33) + i;
                        if (newkey < 0)
                        {
                            newkey = 33 + newkey;
                        }
                        if (newkey > 32)
                        {
                            newkey = newkey - 33;
                        }

                        shifred[j] = alphabet[newkey];
                        break;
                    }
                    i++;
                }
            }
            for (int i = 0; i < shifred.Length; i++)
            {
                text2.Text += shifred[i];
            }
        }

        private void MyShifr1(int key, int m, string text)
        {
            char[] alphabet = { 'а', 'б', 'в', 'г', 'д', 'е',
                'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р',
                'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ',
                'ы', 'ь', 'э', 'ю', 'я' };
            string[] shifr = new string[33];
            foreach (char ch in alphabet)
            {
                int index = Array.IndexOf(alphabet, ch) + 1;
                string one = (index * (key + 3)).ToString();
                string two = (index * (key + 1)).ToString();
                string three = (index * (key + 5)).ToString();
                string four = (index * (key + 2)).ToString();
                string complete = (Int64.Parse(one) * Int64.Parse(two) * Int64.Parse(three)).ToString() + four;
                shifr[index - 1] = (complete); //Int64.Parse(complete);
            }
            switch (m)
            {
                case (0):
                    {
                        string toShifr = text;
                        foreach (char Charkey in alphabet)
                        {
                            int index = Array.IndexOf(alphabet, Charkey);
                            string shifrReplace = shifr[index].ToString();
                            string ckey = Charkey.ToString();
                            Regex reg = new Regex(ckey, RegexOptions.IgnoreCase);
                            toShifr = reg.Replace(toShifr, shifrReplace);
                        }
                        text2.Text = toShifr;
                        break;
                    }
                case (1):
                    {
                        string toShifr = text;
                        foreach (string Charkey in shifr)
                        {
                            int index = Array.IndexOf(shifr, Charkey);
                            string alphabetReplace = alphabet[index].ToString();
                            string ckey = Charkey.ToString();
                            Regex reg = new Regex(ckey);
                            toShifr = reg.Replace(toShifr, alphabetReplace);
                        }
                        text2.Text = toShifr;
                        break;
                    }
            }
        }

        private void MyShifr2(int m, string text)
        {
            char[] engAlp = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
                'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] alphabet = { 'а', 'б', 'в', 'г', 'д', 'е',
                'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р',
                'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ',
                'ы', 'ь', 'э', 'ю', 'я' };
            string[] shifredAlp = new string[33];
            foreach (char shifrChar in alphabet)
            {
                int index = Array.IndexOf(alphabet, shifrChar) - 7;
                int index1 = Array.IndexOf(alphabet, shifrChar);
                int a = CAR(index - 7);
                int b = CAR(index + 13);
                int c = CAR(index - 18);
                int d = CAR(index + 10);
                int e = CAR(index - 8);
                int f = CAR(index + 4);
                string toShifr = engAlp[a].ToString() + engAlp[b].ToString() + engAlp[c].ToString() + engAlp[d].ToString() + engAlp[e].ToString() + engAlp[f].ToString();
                shifredAlp[index1] = toShifr;
            }
            switch (m)
            {
                case (0):
                    {
                        string toShifrText = text;
                        foreach (char Charkey in alphabet)
                        {
                            int index = Array.IndexOf(alphabet, Charkey);
                            string shifrReplace = shifredAlp[index] + "_";
                            string ckey = Charkey.ToString();
                            Regex reg = new Regex(ckey, RegexOptions.IgnoreCase);
                            toShifrText = reg.Replace(toShifrText, shifrReplace);
                        }
                        if (toShifrText.EndsWith("_"))
                        {
                            toShifrText = toShifrText.Remove(toShifrText.Length - 1);
                        }
                        text2.Text = toShifrText;
                        break;
                    }
                case (1):
                    {
                        string toShifrText = text;
                        foreach (string Charkey in shifredAlp)
                        {
                            int index = Array.IndexOf(shifredAlp, Charkey);
                            string alphabetReplace = alphabet[index].ToString();
                            string ckey = Charkey.ToString();
                            Regex reg = new Regex(ckey);
                            toShifrText = reg.Replace(toShifrText, alphabetReplace);
                        }
                        toShifrText = ConvertStringArrayToString(toShifrText.Split('_'));
                        text2.Text = toShifrText;
                        break;
                    }
            }
        }

        private static string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
            }
            return builder.ToString();
        }

        private int CAR(int index)
        {
            if (index < 0) { index = 33 + index; return index; }
            else {
                if (index > 32) { index = index - 33; return index; } else { return index; }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            text1.Clear();
            text2.Clear();
        }

        private void text2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (text2.Text != "")
            {
                System.Windows.Clipboard.SetText(text2.Text); text2.SelectAll();
                text2.Focus();
            }
        }

        private void text1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (text1.Text != "")
            {
                text1.SelectAll();
                text1.Clear();
            }
            text1.Text = System.Windows.Clipboard.GetText();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }

        private void label1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (music)
                {
                    System.Media.SoundPlayer Audio;
                    Audio = new System.Media.SoundPlayer(CSWM.Properties.Resources.goodbye);
                    Audio.Load();
                    Audio.Play();
                }
                Disconnect();
                receiveThread.Abort();
                Environment.Exit(0);
            }
            catch { }
            finally
            {
                if (music)
                {
                    System.Media.SoundPlayer Audio;
                    Audio = new System.Media.SoundPlayer(CSWM.Properties.Resources.goodbye);
                    Audio.Load();
                    Audio.Play();
                }
                Environment.Exit(0);
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedIndex == 2) key.IsEnabled = false; else key.IsEnabled = true;
        }

        public void updateFont(double size)
        {
            text1.FontSize = size;
        }

        private void label3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window1 form = new Window1();
            form.Owner = this;
            form.fontsize = text1.FontSize;
            form.ShowDialog();
            text1.FontSize = form.scrollBar.Value;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
        private void MessageSound()
        {

            System.Media.SoundPlayer Audio;
            Audio = new System.Media.SoundPlayer(CSWM.Properties.Resources.vk);
            Audio.Load();
            Audio.Play();
        }
        private void ChatEnable_Click(object sender, RoutedEventArgs e)
        {
            if (ChatEnable.IsChecked == false)
            {
                userName = userNameBlock.Text;
                StartWork();
                if (music)
                {
                    System.Media.SoundPlayer Audio;
                    Audio = new System.Media.SoundPlayer(CSWM.Properties.Resources.connect);
                    Audio.Load();
                    Audio.Play();
                }
                List<string> _nameList = new List<string>();
                _nameList.Add("All");
                ListOfUsers.ItemsSource = _nameList;
                ListOfUsers.SelectedIndex = 0;
            }
            else {
                Disconnect();
            }
        }

        private Thread receiveThread;

        public void StartWork()
        {
            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();
                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                //DialogList[0]._text +=(String.Format(Environment.NewLine + "Добро пожаловать, {0}", userName));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                ChatEnable.IsChecked = false;
                Disconnect();
            }
        }

        public void SendMessage()
        {
           
            if (string.IsNullOrEmpty(textBox.Text)) { System.Windows.MessageBox.Show("Сообщение пустое"); }
            else {
                try
                {
                    string message = textBox.Text;
                    Makeshifr(message);
                    message = text2.Text;
                    text2.Clear();
                    string ToAppend = Environment.NewLine + userName + ": " + textBox.Text;
                    if (ListOfUsers.SelectedIndex != 0)
                    {
                        try {
                            message = string.Format("/id:{0},{1}", DialogList[ListOfUsers.SelectedIndex]._UserId, message);
                            Dispatcher.BeginInvoke(new ThreadStart(delegate { DialogList[ListOfUsers.SelectedIndex]._text += ToAppend; }));
                        } catch { }
                    } else { Dispatcher.BeginInvoke(new ThreadStart(delegate { DialogList[0]._text += ToAppend; })); }
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    UpdateChat();
                }
                catch (Exception ex)
                {
                    richTextBox.AppendText(ex.Message + Environment.NewLine);
                }
            }
        }
        private void UpdateChat()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { richTextBox.Document.Blocks.Clear();
                richTextBox.Selection.Text = DialogList[ListOfUsers.SelectedIndex]._text; }));
        }
        private delegate void Del(string s);

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    string message = builder.ToString();
                    if (Regex.IsMatch(message, "/Clients:", RegexOptions.IgnoreCase))
                    {
                       // try { Dispatcher.BeginInvoke(new ThreadStart(delegate { _IdList.Clear();}));} catch { }
                        var _nameString = Regex.Replace(message, "/Clients:", "");
                        var _NameIdList = _nameString.Split(';');                       
                        List<string> _nameList = new List<string>();
                        List<string> _IdList = new List<string>();
                        _IdList.Add("0");
                        foreach(var item in _NameIdList)
                        {
                            try
                            {
                                var name = item.Split(',')[1];
                                string id = item.Split(',')[0];
                                _IdList.Add(id);
                                Dispatcher.BeginInvoke(new ThreadStart(delegate {
                                    bool found = false;
                                    foreach(Dialog _dialog in DialogList)
                                    {
                                        if (_dialog._UserId == id) found = true;
                                    }
                                    if(!found)
                                    DialogList.Add(new Dialog(name, id));
                                }));
                            }
                            catch { }
                        }
                        Dispatcher.BeginInvoke(new ThreadStart(delegate {
                            if (DialogList != null)
                            {
                                foreach (var _dialog in DialogList.ToArray())
                                {
                                    bool found = false;
                                    foreach (string _id in _IdList)
                                    {
                                        if (_id == _dialog._UserId) found = true;
                                    }
                                    if (!found) DialogList.Remove(_dialog); else _nameList.Add(_dialog._UserName);
                                }
                            }
                        }));
                       
                        Dispatcher.BeginInvoke(new ThreadStart(delegate { ListOfUsers.ItemsSource = _nameList; ListOfUsers.SelectedIndex = 0; }));
                    }
                    else {

                        if (Regex.IsMatch(message, "/id:"))
                        {
                            message = Regex.Replace(message, "/id:", "");
                            string id = message.Split(',')[0];
                            Dispatcher.BeginInvoke(new ThreadStart(delegate {
                                foreach (var _dialog in DialogList)
                                {
                                    if (_dialog._UserId == id)
                                    {
                                        message = Regex.Replace(message,message.Split(',')[0]+",","");
                                        Regex _NameRegex = new Regex(@"[\S\s]*" + ": ");
                                        string name = _NameRegex.Match(message).Value;
                                        message = _NameRegex.Replace(message, "");
                                        Dispatcher.Invoke(new Delag((s) => MakeDeShifr(s)), message);
                                        message = Environment.NewLine + string.Format("{0}{1}", name, text2.Text);
                                        Dispatcher.BeginInvoke(new ThreadStart(delegate { text2.Clear(); }));
                                        _dialog._text += message;
                                        UpdateChat();
                                        MessageSound();
                                    }
                                }
                            }));
                            
                        }
                        else
                        {
                            Regex _NameRegex = new Regex(@"[\S\s]*" + ": ");
                            string name = _NameRegex.Match(message).Value;
                            message = _NameRegex.Replace(message, "");
                            Dispatcher.Invoke(new Delag((s) => MakeDeShifr(s)), message);                            
                            Dispatcher.BeginInvoke(new ThreadStart(delegate {
                                message = Environment.NewLine + string.Format("{0}{1}", name, text2.Text);
                                text2.Clear();
                                DialogList[0]._text += message; }));
                            UpdateChat();
                            MessageSound();                     
                        }                       
                    }
                }
                catch { }
            }
        }

        public void Disconnect()
        {
            if (stream != null) 
                stream.Close();
            if (client != null)
                client.Close();
            try
            { receiveThread.Abort(); }
            catch { }
            Dispatcher.BeginInvoke(new ThreadStart(delegate { richTextBox.AppendText(Environment.NewLine + "Вы вышли из чата."); }));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Disconnect();
        }

        private void textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                textBox.Clear();
            }
        }

        private void local_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (localNetWork == false)
            {
                localNetWork = true;
                host = System.Net.Dns.GetHostName();
                System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
                host = ip.ToString();
                local.Foreground = new SolidColorBrush(Color.FromRgb(188, 208, 238));
            }
            else
            {
                localNetWork = false;
                host = nonLockalHost;
                local.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        private void ListOfUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
                try
                {
                    richTextBox.Document.Blocks.Clear();
                richTextBox.Selection.Text.Remove(0, richTextBox.Selection.Text.Length);
                    richTextBox.Selection.Text = DialogList[ListOfUsers.SelectedIndex]._text;                    
                }
                catch { }
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
                       
        }
    }
    class Dialog
    {
        public string _text;
        public string _UserId { get; set; }
        public string _UserName { get; set; }
        public Dialog(string userName,string id)
        {
            _UserName = userName;
            _UserId = id;
            _text = "";
        }
    }
}
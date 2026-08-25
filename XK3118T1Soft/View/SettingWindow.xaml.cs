using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace XK3118T1Soft.View
{
    /// <summary>
    /// Логика взаимодействия для SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private string _selectedPort;
        public string SelectedPort
        {
            get => _selectedPort;
            set
            {
                _selectedPort = value;
                OnPropertyChanged();
            }
        }

        private string _port; //наименование порта
        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();

            }
        }

        private int _settingBoudRate; //скорость передачи данных
        public int SettingBoudRate
        {
            get => _settingBoudRate;
            set
            {
                _settingBoudRate = value;
                OnPropertyChanged();
            }
        }

        private Parity _settingParity; //паритетность
        public Parity SettingParity
        {
            get => _settingParity;
            set
            {
                _settingParity = value;
                OnPropertyChanged();
            }
        }
        private int _settingDataBits;
        public int SettingDataBits  //данные биты
        {
            get => _settingDataBits;
            set
            {
                _settingDataBits = value;
                OnPropertyChanged();
            }
        }

        private StopBits _settingBits; //стоп бит
        public StopBits SettingStopBits
        {
            get => _settingBits;
            set
            {
                _settingBits = value;
                OnPropertyChanged();
            }
        }
        public SettingWindow ( )
        {
            InitializeComponent();
        }
         private void SaveButton_Click ( object sender, EventArgs e )
        {
            Port = (cmbPorts.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "COM1";
            SelectedPort = Port;

            // Скорость
            SettingBoudRate = int.TryParse((cmbBaudRate.SelectedItem as ComboBoxItem)?.Content.ToString(), out int baud)
                ? baud : 9600;

            // Биты данных
            SettingDataBits = int.TryParse((cmbDataBits.SelectedItem as ComboBoxItem)?.Content.ToString(), out int dataBits)
                ? dataBits : 8;

            // Стоп-биты
            string stopBitsValue = (cmbStopBits.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "1";
            SettingStopBits = stopBitsValue switch
            {
                "1" => StopBits.One,
                "1.5" => StopBits.OnePointFive,
                "2" => StopBits.Two,
                _ => StopBits.One
            };

            // Четность
            string parityValue = (cmbParity.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Нет";
            SettingParity = parityValue switch
            {
                "Нет" => Parity.None,
                "Нечет" => Parity.Odd,
                "Чет" => Parity.Even,
                "Марк" => Parity.Mark,
                "Спейс" => Parity.Space,
                _ => Parity.None
            };

            MessageBox.Show($"Настройки сохранены:\n" +
                     $"Порт: {Port}\n" +
                     $"Скорость: {SettingBoudRate}\n" +
                     $"Биты данных: {SettingDataBits}\n" +
                     $"Стоп-биты: {SettingStopBits}\n" +
                     $"Четность: {SettingParity}");

            this.DialogResult = true;
            this.Close();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged ( [CallerMemberName] string name = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

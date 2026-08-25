using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XK3118T1Soft.Model;
using XK3118T1Soft.View;

namespace XK3118T1Soft.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ASCIIprotocol aSCI;
        bool isRunning;
        private List<string> _comPorts;
        public List<string> ComPorts 
        {
            get => _comPorts;
            set
            {
                _comPorts = value;
                OnPropertyChanged();
            }
        }
        List<SavePort> SavePort = new List<SavePort>();


        //свойства
        private string _mode;
        public string Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        public string Status 
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

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
        private string _weigth; //свойство веса
        public string Weight 
        {
            get => _weigth;
            set
            {
                _weigth = value;
                System.Diagnostics.Debug.WriteLine($"🔄 Weight изменен на: '{value}'");
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

        private int _boudRate; //скорость передачи данных
        public int BoudRate 
        {
            get => _boudRate;
            set
            {
                _boudRate = value;
                OnPropertyChanged();
            }
        }

        private Parity _parity; //паритетность
        public Parity Parity 
        {
            get => _parity;
            set
            {
                _parity = value;
                OnPropertyChanged();
            }
        }
        private int _dataBits;
        public int DataBits  //данные биты
        {
            get => _dataBits;
            set
            {
                _dataBits = value;
                OnPropertyChanged();
            }
        }

        private StopBits _stopBits;
        public StopBits StopBits 
        {
            get => _stopBits;
            set
            {
                _stopBits = value;
                OnPropertyChanged();
            }
        }

        //кнопка
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand SettingCommand { get; }
        public ICommand SeePort { get; }

        public MainViewModel ( )
        {
            Weight = "0.01";
            Port = "COM9";
            BoudRate = 9600;
            Parity = Parity.None;
            DataBits = 8;
            StopBits = StopBits.One;

            System.Diagnostics.Debug.WriteLine("=== MainViewModel КОНСТРУКТОР ===");

            StartCommand = new RelayCommand(ExecuteStart, CanExecuteStart);
            StopCommand = new RelayCommand(ExecuteStop, CanExecuteStop);
            SettingCommand = new RelayCommand(ExecuteSetting, CanExecuteSetting);
            SeePort = new RelayCommand(ExecuteSeePort, CanExecuteSeePort);

            ComPorts = new List<string>()
            {
                "COM1",
                "COM2",
                "COM3",
                "COM4",
                "COM5",
                "COM6",
                "COM7",
                "COM8",
                "COM9"
            };

            // Создаем и подписываемся
            aSCI = new ASCIIprotocol();
            aSCI.DataReceived += OnDataReceived;

            Mode = "Неизвестно";
            Status = "⏸ Ожидание";

            aSCI.DataReceived += OnDataReceived;
            System.Diagnostics.Debug.WriteLine("Подписка на DataReceived выполнена");
        }

        private bool CanExecuteStart ( object parameter )
        {
            return true;
        }
        private bool CanExecuteStop ( object parameter )
        {
            return true;
        }

        private bool CanExecuteSetting ( object parameter )
        {
            return true;
        }
        
        private bool CanExecuteSeePort(object parameter )
        {
            return true;
        }

        private void ExecuteSeePort(object parameter)
        {
            string message = "📋 СОХРАНЕННЫЕ НАСТРОЙКИ:\n\n";
            message += string.Join("\n", SavePort.Select(( p, i ) => $"[{i + 1}] {p}"));

            MessageBox.Show(message, "Настройки",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteStop(object parameter )
        {
            aSCI.Stop();    
            isRunning = false;
            Mode = "Нет данных";
            Status = "⏹СТОП";
        }

        private void ExecuteSetting ( object param ) //открытие нового окна настроек
        {


            var settingWindow = new SettingWindow();
            if (settingWindow.ShowDialog() == true)
            {
                Port = settingWindow.Port;
                BoudRate = settingWindow.SettingBoudRate;
                DataBits = settingWindow.SettingDataBits;
                StopBits = settingWindow.SettingStopBits;
                Parity = settingWindow.SettingParity;
                SavePort.Add(new SavePort(Port, BoudRate, Parity, DataBits, StopBits));
            }
        }

        public void ExecuteStart ( object param )
        {
           

            try
            {
                if(isRunning)

                {
                    MessageBox.Show("Процесс уже запущен");
                    return;
                }
                if (aSCI == null)
                {
                    System.Diagnostics.Debug.WriteLine("❌ aSCI == null!");
                    return;
                }
                //Port = SelectedPort;
                aSCI.ASCIIchanel(Port, BoudRate, Parity, DataBits, StopBits);
                isRunning = true;
                Mode = "Получение данных";
                Status = "▶работа";
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка: {ex.Message}");
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

       

        private void OnDataReceived ( string data )
        {
            System.Diagnostics.Debug.WriteLine($"🔥🔥🔥 OnDataReceived ВЫЗВАН с данными: '{data}'");
            try
            {
                Application.Current.Dispatcher.Invoke(( ) =>
                {
                    System.Diagnostics.Debug.WriteLine($"🔄 Dispatcher.Invoke: '{data}'");
                    // ========== РЕЖИМ 3/4: =0000210(kg) ==========
                    if (data.StartsWith("=") && data.Contains("(kg)"))
                    {
                        // Извлекаем число между "=" и "(kg)"
                        var match = System.Text.RegularExpressions.Regex.Match(data, @"=([\d\-,.]+)\(kg\)");
                        if (match.Success)
                        {
                            string weightStr = match.Groups[1].Value;
                           

                        
                            weightStr = weightStr.Replace(',', '.');

                  
                            weightStr = weightStr.TrimStart('0');
                            if (string.IsNullOrEmpty(weightStr)) weightStr = "0";

                            if (double.TryParse(weightStr, System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture, out double weight))
                            {
                                
                                Weight = weight.ToString("0.000");
                                

                            }
                        }
                    }

                   
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка в OnDataReceived: {ex.Message}");
            }
        }

        //подпись на события
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged ( [CallerMemberName] string name = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

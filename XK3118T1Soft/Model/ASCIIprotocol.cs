using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XK3118T1Soft.Interface;
using System.IO.Ports;
using System.Threading;

namespace XK3118T1Soft.Model
{
    public class ASCIIprotocol: IASCII
    {
        public string port; //название порта
        public int boudRate; //скорость
        public Parity parity; //паритетность
        public int dataBits; //биты данных
        public StopBits stopBits; //стопбит
        public int readBits; //переменная времени чтения
        public int delayTime;//переменная паузы

        private SerialPort serialPort; // переменная порта
        private bool isStart = false; //переменная старта
        public event Action<string> DataReceived; //событие для передачи данных
        private Thread thread;


        public void ASCIIchanel (string port, int boudRate, Parity parity, int dataBits, StopBits stopBits ) //метод для получения данных
        {
            try
            {
                serialPort = new SerialPort(port, boudRate, parity, dataBits, stopBits);
                serialPort.ReadTimeout = readBits;
                serialPort.Encoding = Encoding.ASCII;

                serialPort.Open();
                isStart = true;
                thread = new Thread(StartLoop);
                thread.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void StartLoop()
        {
            while(isStart)
            {
                if(serialPort.BytesToRead > 0)
                {
                    var data = serialPort.ReadExisting();
                    if(!string.IsNullOrEmpty(data))
                    {
                        string processedData = ProcessData(data);
                        // Отправляем данные через событие
                        //System.Diagnostics.Debug.WriteLine($"📤 Отправляем DataReceived: '{processedData}'");
                        DataReceived?.Invoke(processedData);
                    }
                }
                else
                {
                    Thread.Sleep(delayTime);
                }
            }
        }

        public string ProcessData(string data)
        {
            var clearData = data.Replace("\r", "").Replace("\n", "").Trim();
            
            return clearData;
        }

        public void Stop ( )
        {
            isStart = false;
            thread?.Join(1000);

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort = null;
            }
        }
    }
}

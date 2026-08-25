using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XK3118T1Soft.Interface
{
    public interface IASCII
    {
        void ASCIIchanel (string port, int boudRate, Parity parity, int dataBits, StopBits stopBits ); //метод чтения
        void StartLoop ( ); //метод проверки
        string ProcessData ( string data); //метод вывода на экран
        void Stop ( ); // Добавьте метод остановки
        event Action<string> DataReceived;
    }
}

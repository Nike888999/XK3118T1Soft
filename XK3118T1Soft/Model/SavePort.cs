using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XK3118T1Soft.Interface;

namespace XK3118T1Soft.Model
{
    public class SavePort 
    {
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }

        public SavePort ( string port, int baudRate, Parity parity, int dataBits, StopBits stopBits )
        {
            Port = port;
            BaudRate = baudRate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = stopBits;
        }

        public override string ToString ( )
        {
            return $"{Port}, {BaudRate} бод, {DataBits} бит, {StopBits} стоп-бит, {Parity}";
        }

    }
}

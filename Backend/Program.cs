using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Testapplication1.Views;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Mqttlistener
{
    class Program
    {
        public static void Main(string[] args)
        {
            Mqttlistener.Client.client();
        }
    }
}
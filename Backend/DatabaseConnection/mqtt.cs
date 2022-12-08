using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Mqttlistener
{
    class Client
    {
        //MqttClient mqttClient;
        public static string ListenerDataPath = Path.GetFullPath(@"ListenerData.json");
        public static void client()
        {
            MqttClient mqttClient = new MqttClient("65.108.249.175");
            mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsPublishedReceived;
            mqttClient.Subscribe(new string[] { "chengeta/notifications" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            mqttClient.Connect("Listener", "chengeta2022", "chengetaALTENHR2022");
        }

        private static void MqttClient_MqttMsPublishedReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            //Mqttlistener.AddListenerInfoToDB.AddJsonToDB(); //Run when database is empty, this is cached sample data saved in ListenerData.json\
            Console.WriteLine(message);
            Mqttlistener.AddListenerInfoToDB.AddToDB(message);
        }

    }
}
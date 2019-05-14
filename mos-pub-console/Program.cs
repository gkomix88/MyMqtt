using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace mos_pub_console
{
    class Program
    {
        static MqttClient client;
        static string clientName;
         static  void Main(string[] args)
        {
            client = new MqttClient("iot.eclipse.org");
            Random a = new Random();
            clientName = "Client-" + a.Next(1000, 5000);
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.MqttMsgPublished += Client_MqttMsgPublished;
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            
            client.Subscribe(new String[] { "/c/" + clientName }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            WaitForItToWork().Wait();

        }

        

        static async Task WaitForItToWork()
        {
            bool succeeded = false;
            Random a = new Random();
            while (!succeeded)
            {
                // do work
                string strValue = Convert.ToString(a.Next(1, 100));
                string message = strValue + "," + clientName;
                var p = client.Publish("/master", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                //System.Console.ReadLine();
                await Task.Delay(500);
            }
           // return succeeded;
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // Handle message received
            var message = System.Text.Encoding.Default.GetString(e.Message);
            System.Console.WriteLine("Message received from master: " + message);
        }

        private static void Client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if(e.IsPublished)
                System.Console.WriteLine("Message published!");
            else
                System.Console.WriteLine("Message published failed!");
            //throw new NotImplementedException();
        }
    }
}

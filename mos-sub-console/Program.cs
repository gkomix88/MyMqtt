using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace mos_sub_console
{
    class Program
    {
        static MqttClient myClient;
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");
            // Create Client instance
            myClient = new MqttClient("iot.eclipse.org");
            

            // Register to message received
            myClient.MqttMsgPublishReceived += client_recievedMessage;
            myClient.MqttMsgPublished += MyClient_MqttMsgPublished;

            string clientId = Guid.NewGuid().ToString();
            myClient.Connect(clientId);
            
            // Subscribe to topic
            myClient.Subscribe(new String[] { "/master" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            System.Console.ReadLine();
        }

        private static void MyClient_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (e.IsPublished)
                System.Console.WriteLine("Message sent!");
            else
                System.Console.WriteLine("Message sent failed!");
        }

        static void client_recievedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            // Handle message received
            var message = System.Text.Encoding.Default.GetString(e.Message);
            System.Console.WriteLine("Message received: " + message);

            string[] strs = message.Split(',');
            //Console.WriteLine(message);
            Double c = Convert.ToDouble(strs[0]);
            string str;
            if (c < 50)
                str = strs[1] + " is OK";
            else
                str = strs[1] + " is too Hot!!";

            myClient.Publish("/c/" + strs[1], Encoding.UTF8.GetBytes(str), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        }
    }
}

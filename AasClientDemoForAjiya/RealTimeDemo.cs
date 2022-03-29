using System;
using AAS.Client;
using AAS.Common.Metadata;

namespace AasClientDemoForAjiya
{
    public class RealTimeDemo
    {
        public static void StartSignalR(AasClient aasClient)
        {
            //1. Listen to events first.
            aasClient.RealTime.OnConnected += OnConnected;
            aasClient.RealTime.OnSubscribed += OnSubscribed;
            aasClient.RealTime.OnNewInstanceData += OnNewInstanceData;
            aasClient.RealTime.OnUpdateInstanceData += OnUpdateInstanceData;

            //2. Connect.
            aasClient.RealTime.Connect();

            //3. Subscribe to your desired events.
            aasClient.RealTime.SubscribeInstance("Irai here.");
            Console.Read();
        }

        private static void OnUpdateInstanceData(RealTimeInstanceData obj)
        {
            //Process updated instance data here.
            Console.WriteLine(obj.InstanceData.Data.ToString());
        }

        private static void OnNewInstanceData(RealTimeInstanceData obj)
        {
            //Process newly added instance data here.
            Console.WriteLine(obj.InstanceData.Data.ToString());
        }

        private static void OnSubscribed(string obj)
        {
            Console.WriteLine(obj);
        }

        private static void OnConnected(string obj)
        {
            Console.WriteLine(obj);
        }
    }
}
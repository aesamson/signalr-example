using System;
using System.Threading.Tasks;
using App.Client.ChatClient.Contracts;
using App.Client.ChatClient.Models;

namespace App.Client
{
    internal sealed class Application
    {
        private readonly IChatHttpClient _httpClient;
        private readonly IChatSignalRClient _signalRClient;
        
        private static bool _keepRunning = true;
        
        public Application(IChatHttpClient httpClient, IChatSignalRClient signalRClient)
        {
            _httpClient = httpClient;
            _signalRClient = signalRClient;
        }

        public async Task Run()
        {
            Console.WriteLine("Application started. Press Ctrl+C to shut down.");

            while (string.IsNullOrWhiteSpace(ApplicationStorage.Nickname))
            {
                Console.WriteLine("Please, write your nickname:");
                ApplicationStorage.Nickname = Console.ReadLine();    
            }

            var initialState = await _httpClient.GetInitialState();
            var messageHistory = await _httpClient.GetHistory();

            foreach (var m in messageHistory)
                Console.WriteLine($"> {m.Nickname}: {m.Data}");
            
            _signalRClient.Subscribe<NewMessage>("message", ProcessNewMessage);
            _signalRClient.Subscribe<NewMember>("joined", ProcessNewMember);
            _signalRClient.Subscribe<LostMember>("lost", ProcessLostMember);
        
            Console.CancelKeyPress += ConsoleCancelKeyHandler;
        
            try
            {
                await _signalRClient.Connect();
        
                while (_keepRunning)
                {
                    var message = Console.ReadLine();
        
                    if (!string.IsNullOrWhiteSpace(message))
                        await _signalRClient.PostMessage(message, initialState.ChatId);
                }
            }
            finally
            {
                await _signalRClient.Disconnect();   
            }
        }
        
        private static void ConsoleCancelKeyHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Application is shutting down...");
            _keepRunning = false;
        }

        private static void ProcessNewMessage(NewMessage m)
        {
            Console.WriteLine($"> {m.Nick}: {m.Message}");
        }

        private static void ProcessNewMember(NewMember m)
        {
            Console.WriteLine($"# New member {m.Nick}");
        }

        private static void ProcessLostMember(LostMember m)
        {
            Console.WriteLine($"# Lost member {m.Nick}");
        }
    }
}
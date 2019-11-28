using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Rest;

namespace botbase
{
    class Program
    {
        //@"release/Info/UINFO.txt"
        public DiscordSocketClient Client;
        public CommandHandler Handler;
        public static string bottoken;
        public static bool InfoEntOpen = false;

        static void Main(string[] args)
        {
            CheckForInfo();

            
            new Program().Start().GetAwaiter().GetResult();
            //Console.WriteLine("checkpoint alpha");
        }

        public static void CheckForInfo()
        {
            String PlaceToCheck = @"Info/UINFO.txt";
            StreamReader sc = new StreamReader(PlaceToCheck);
            String a = sc.ReadLine();
            
            if(a == null)
            {
                sc.Close();
                if (!InfoEntOpen)
                {
                    Process.Start("DiscordBotSetup.exe");
                    InfoEntOpen = true;
                }

                InfoEntOpen = true;
                Console.WriteLine("Waiting for input in opened window...");
                
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine("Could not find info...checking again in 5 seconds");
                CheckForInfo();
            }
            else
            {
                String b = sc.ReadLine();
                //Console.WriteLine(a);
                //Console.WriteLine(b);
                Commands.ownerid = (ulong)Convert.ToInt64(a);
                bottoken = b;
                sc.Close();
                
            }
        }
        public async Task Start()
        {
            Client = new DiscordSocketClient();
            Handler = new CommandHandler();

            await Client.LoginAsync(Discord.TokenType.Bot, bottoken, true);
            await Client.StartAsync();


            await Handler.Install(Client);


            // subscribe to Client.Ready callback
            Client.Ready += Client_Ready;
            await Task.Delay(-1);
        }

        private async Task Client_Ready()
        {
            Console.WriteLine("Were Online");

            return;
        }

        public static int SetBotToken(string InputToken)
        {
            bottoken = InputToken;
            return 0;
        }

    }
}

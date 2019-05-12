using System;
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
        public DiscordSocketClient Client;
        public CommandHandler Handler;
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        public static string bottoken = "PUT YOUR BOT TOKEN HERE";


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

    }
}

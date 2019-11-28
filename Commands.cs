using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace botbase
{
    public class Commands : ModuleBase
    {
        public static SocketUser Owner;
        private static bool OwnerSet = false;
        private int servercheck;
        private string servercheckstring;
        private bool serverhasplace;
        public string currentline;
        private bool servercommandcheck;
        // don't forget to remove quotes \/
        public static ulong ownerid;


        [Command("SendOwnerMessage")]
        public async Task sendOwnerMessage()
        {
            if(Context.User.Id == ownerid)
            {
                if (!OwnerSet)
                {
                    await Context.Message.DeleteAsync();
                    //await ReplyAsync("using this for initialization");
                    CreateServerListDir();
                    Owner = (SocketUser)Context.User;
                    await Owner.GetOrCreateDMChannelAsync();
                    await Owner.SendMessageAsync("hello");
                    OwnerSet = true;
                }
            }

        }

        public void CreateServerListDir()
        {

            System.IO.Directory.CreateDirectory(@"Servers");
            var a = System.IO.File.Create(@"Servers/List.txt");
            a.Close();
            
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);
        private ISocketMessageChannel defaultChannel;

        private string RandomString(int Size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public void CheckIfVerified(int id)
        {
            StreamReader sc = new StreamReader(@"Servers/List.txt");
            servercommandcheck = false;
            while((currentline = sc.ReadLine()) != null)
            {
                if(currentline == id.ToString())
                {
                    servercommandcheck = true;
                }
            }
            sc.Close();
        }

        [Command("AllowCommandsOnServer")]
        public async Task AllowCommandsOnServer(string servername, string code = null)
        {
            await ReplyAsync("checking...");
            servercheck = (int)Context.Guild.Id;
            servercheckstring = servercheck.ToString();

            try
            {
                serverhasplace = true;
                StreamReader fc = new StreamReader($@"{servername}/{servername}.txt");
                fc.Close();
            }
            catch
            {
                serverhasplace = false;
                System.IO.Directory.CreateDirectory($@"{servername}");
                var b = System.IO.File.Create($@"{servername}/{servername}.txt");
                b.Close();
                var newserverloginpass = RandomString(16);
                StreamWriter fc = new StreamWriter($@"{servername}/{servername}.txt", false);
                fc.WriteLine(servercheck);
                fc.WriteLine(newserverloginpass);
                fc.Close();

                await Owner.SendMessageAsync($"The server {servername} with an id of {servercheckstring}, would like their password, it is {newserverloginpass}");
                await ReplyAsync($"a folder has been created for your server, please ask the owner {Owner.Mention} for a code to activate it");
                await ReplyAsync("Use this command with the same server name and add the code on the end of the command");
                
            }
            finally
            {
                if (serverhasplace)
                {
                    StreamReader fc = new StreamReader($@"{servername}/{servername}.txt");

                    if(fc.ReadLine() == servercheckstring)
                    {
                        var c = fc.ReadLine();
                        if(c == code)
                        {
                            servercheckstring = Environment.NewLine + servercheckstring;
                            await ReplyAsync("your server is now verified!");
                            File.AppendAllText(@"Servers/List.txt", servercheckstring);
                            //StreamWriter sc = new StreamWriter(@"Servers /List.txt");
                            //sc.WriteLine(servercheckstring);
                            //sc.Close();
                        }
                        else
                        {
                            await ReplyAsync("The code seems to be wrong, please check again!");
                        }
                        fc.Close();
                    }
                    else
                    {
                        await ReplyAsync("Please make sure you are using your servername and code!");
                    }
                }
                serverhasplace = true;
            }
        }


        [Command("ping")]
        public async Task Ping()
        {
            //var checksrv = (int)Context.Guild.Id;
            CheckIfVerified((int)Context.Guild.Id);


            if (servercommandcheck)
            {
                var msg = await ReplyAsync("pinging...");
                await ReplyAsync($"pong... ***{DateTime.Now.Millisecond - msg.Timestamp.Millisecond}***ms");

                await msg.DeleteAsync();

                //await ReplyAsync(serververified.ToString());
                //await ReplyAsync("You need to allow commands on server! to do this use the !AllowCommandsOnServer (servername) command!");
            }
            else
            {
                await ReplyAsync("You need to allow commands on this server! to do this, use the !AllowCommandsOnServer (servername) command!");
            }

        }

        [Command("kick")]
        public async Task Kick(SocketGuildUser mention)
        {

            //var checksrv = (int)Context.Guild.Id;
            CheckIfVerified((int)Context.Guild.Id);
            if (servercommandcheck)
            {
                //await ReplyAsync("Not allowed yet");

                var UserKicked = mention;
                bool abletokick = false;
                var User = (SocketGuildUser)Context.User;
                foreach (SocketRole role in User.Roles)
                {
                    if (role.Permissions.KickMembers)
                    {
                        abletokick = true;
                    }
                }
                if (abletokick)
                {
                    if (mention != null)
                    {
                        var channel = await mention.GetOrCreateDMChannelAsync();
                        await channel.SendMessageAsync($"You've been kicked from {Context.Guild.Name}");
                        await mention.KickAsync();

                        await ReplyAsync($"Removed {UserKicked.Username} from the server");
                        abletokick = false;
                    }
                    else
                    {
                        await ReplyAsync("dunno who that is");
                        abletokick = false;
                    }
                }
                else
                {
                    await ReplyAsync("Unable to preform that task for you.");
                }



            }
            else
            {
                await ReplyAsync("You need to allow commands on server! to do this use the !AllowCommandsOnServer (servername) command!");
            }

        }

        [Command("AddRole")]
        public async Task UpdatePlayerRole(SocketGuildUser user, SocketRole role)
        {

            //var checksrv = (int)Context.Guild.Id;
            CheckIfVerified((int)Context.Guild.Id);

            if (servercommandcheck)
            {
                var User = (SocketGuildUser)Context.User;
                bool canadjustroles = false;
                foreach (SocketRole userrole in User.Roles)
                {
                    if (userrole.Permissions.ManageRoles)
                    {
                        canadjustroles = true;
                    }
                }
                if (canadjustroles)
                {
                    //CommandHandler.updatedplayerrole = 0;
                    await user.AddRoleAsync(role);
                    await ReplyAsync($"{user.Username} succesfully assigned role: {role}");
                    canadjustroles = false;
                    //CommandHandler.updatedplayerrole = 1;
                }
                else
                {
                    await ReplyAsync("cannot do that for you");
                }
            }
            else
            {
                await ReplyAsync("You need to allow commands on server! to do this use the !AllowCommandsOnServer (servername) command!");
            }
        }

        /*
         * Cannot use without fucking over other servers
        [Command("ChangePrefix")]
        public async Task ChangePrefix(string newprefix)
        {
            //update with server check and privilege check
            CommandHandler.prefix = newprefix;
            await ReplyAsync($"New prefix set, remember to use {newprefix} from now on.");
            await ReplyAsync("You need to allow commands on server! to do this use the !AllowCommandsOnServer (servername) command!");

        }
        */

        
        [Command("SetDefaultChannel")]
        public async Task SetDefaultChannel()
        {
            //update to be able to add channels to listen on
            var User = (SocketGuildUser)Context.User;
            bool isadmin = false;
            foreach (SocketRole userrole in User.Roles)
            {
                if (userrole.Permissions.Administrator)
                {
                    isadmin = true;
                }
            }
            if (isadmin)
            {
                defaultChannel = (ISocketMessageChannel)Context.Channel;
                await defaultChannel.SendMessageAsync("This will be the new default channel");
            }
            else
            {
                await ReplyAsync("cannot do that for you");
            }

        }

        [Command("Help")]
        public async Task SendHelp()
        {
            //update with server check and privelege check as well as commands
            await ReplyAsync("My owner is lazy, this is all the commands:" + Environment.NewLine +
                "1. Ping: Kinda broken, but oh well." + Environment.NewLine +
                "2. Kick (user): Kicks a user, disabled until commands are restricted" + Environment.NewLine +
                "3. ChangePrefix (prefix): Changes what the bot uses to signal a command");
            
        }
    }
}

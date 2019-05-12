﻿using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using botbase;

public class CommandHandler
{
    private CommandService _cmds;
    private DiscordSocketClient _client;
    //public static SocketRole DefaultPlayerRole;
    public static string prefix = "?";
    public async Task Install(DiscordSocketClient c)
    {
        _client = c;
        _cmds = new CommandService();

        await _cmds.AddModulesAsync(Assembly.GetEntryAssembly(), null);

        _client.MessageReceived += HandleCommand;


        _client.UserJoined += AnnounceUserJoined;
        _client.UserLeft += AnnounceUserLeft;
    }

    public async Task AnnounceUserJoined(SocketGuildUser user)
    {
        //Console.WriteLine("someone joined");

        //await user.Guild.DefaultChannel.SendMessageAsync($"A warm welcome to {user.Mention}!);

        // if default channel set, use  commands.defaultchannel, else, use user.Guild.DefaultChannel

        //await Commands.defaultChannel.SendMessageAsync($"A warm welcome to {user.Mention}!");


        //await user.AddRoleAsync(Commands.thedefaultrole);
    }


    public async Task AnnounceUserLeft(SocketGuildUser user)
    {
        var person = user;
        //await Task.Delay(0);
        //await Commands.defaultChannel.SendMessageAsync($"sad to see {person.Mention} go...");

    }
    public void code()
    {

    }
    public async Task HandleCommand(SocketMessage s)
    {

        var msg = s as SocketUserMessage;
        if (msg == null) return;

        var context = new CommandContext(_client, msg);

        int argPos = 0;
        
        if (msg.HasStringPrefix(prefix, ref argPos))
        {


            var result = await _cmds.ExecuteAsync(context, argPos, null);

            if (!result.IsSuccess)
            {
                switch (result.ToString())
                {
                    default:

                        await s.Channel.SendMessageAsync($"An error occurred! Details: ```" + result.ToString() + "```");

                        break;
                    case "UnknownCommand: Unknown command.":

                        await msg.DeleteAsync();

                        await s.Channel.SendMessageAsync($"Command not found! Use the command {prefix}help for a list of commands.");
                        break;

                }
            }
        }
    }


}
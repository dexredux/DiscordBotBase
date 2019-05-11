# DiscordBotBase
I've created a botbase for people to learn basics off of

it contains a basic client file, commandhandler, and commands to get you started, as well as the ability to lock the bot to servers of your choosing, it shows off very basic skills that you will need to get further in bot development, the most complext part is the part that doesn't actually pertain to building the bot itself.

To use you need to do the following:

1. Add your own bottoken to the variable in Program.cs, this way it knows what bot to login as
2. In the commands.cs file there is a ulong you need to change to your discord id.  Remove quotes when you do this
3. Now you need to initialize bot, this is so it can send messages to the owner, such as server passwords for the bot
4. do the command !sendownermessage, this needs to be set every time bot is restarted IF you want new servers to be able to register with the bot, servers already registered will be able to use the bot as normal.  This is also used to send you passwords when servers attempt to register, you can remove this check but you will be going into the server check files to find the password for them.
5. for a server to register(you should be able to figure it out, but here): "!allowcommandsonserver servername" it will generate a password for the server, and tell them to ask the owner for the server password, which you will have.  then when you give them the password: "!allowcommandsonserver SAMESERVERNAME password"  this will verify them and they will have access to commands.
**I have set the person they are supposed to ask for password to Owner.Mention**

now the server is verified and you are good.

I know my code isn't the best, lot's of trash, and some security holes, also if a server does allowcommandsonserver with different names it will generate different passwords, they only need to verify 1, other will just be useless because no other server can use it.

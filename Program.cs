using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ConsoleApplication1
{
    internal class Program
    {
        private const String Token = "token";
    
        private Boolean _isEnabled = false;
        private DiscordSocketClient _client;
        
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private Task OnMessage(SocketMessage message)
        {
            SocketGuild guild = ((SocketGuildChannel) message.Channel).Guild;
            IEmote emote = guild.Emotes.First(e => e.Name == "danger");
            
            if (message.Author.IsBot)
            {
                return Task.CompletedTask;
            }

            if (message.Content == "!toggle")
            {
                _isEnabled = !_isEnabled;
                Console.WriteLine(_isEnabled ? "enabled" : "disabled");
            }
            
            if (_isEnabled) {
                message.AddReactionAsync(emote);   
            }

            return Task.CompletedTask;
        }
        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += OnMessage;

            await _client.LoginAsync(TokenType.Bot, Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}

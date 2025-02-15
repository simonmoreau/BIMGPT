using DevExpress.AIIntegration.Blazor.Chat;
using DevExpress.Utils.Design;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.AI;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using OpenAI.Chat;
using System.ComponentModel;
using System.ServiceModel.Channels;

namespace BIMGPT.Client.Pages
{
    public partial class Home
    {
        [Inject]
        public IChatClient _chatClient { get; set; }

        private readonly List<Microsoft.Extensions.AI.ChatMessage> _conversation = [];

        protected override async Task OnInitializedAsync()
        {
            _conversation.Add(
                new(ChatRole.System, "You are an assistant. ")
            );

        }
        async Task MessageSent(MessageSentEventArgs args)
        {
            _conversation.Add(new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, args.Content.ToString()));

            ChatOptions _chatOptions = new ChatOptions
            {
                Tools = [AIFunctionFactory.Create(GetCurrentTime)]
            };

            Microsoft.Extensions.AI.ChatCompletion response = await _chatClient.CompleteAsync(_conversation, _chatOptions);

            _conversation.Add(response.Message);

            await args.Chat.SendMessage(response.Message.Text, ChatRole.Assistant);
        }


        // 👇🏼 Define a time tool
        [Description("Get the current time for a city")]
        string GetCurrentTime(string city)
        {
            return $"It is {DateTime.Now.ToLongTimeString()} in {city}.";
        }
    }

}

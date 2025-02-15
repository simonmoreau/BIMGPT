using DevExpress.AIIntegration.Blazor.Chat;
using DevExpress.Utils.Design;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.AI;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using OpenAI.Chat;
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
                new(ChatRole.System, "You are a product review assistant. Your job is to help people write great product reviews. Keep asking questions on the person's experience with the product until you have enough information to write a review. Then write the review for them and ask if they are happy with it.")
            );

        }
        async Task MessageSent(MessageSentEventArgs args)
        {
            _conversation.Add(new Microsoft.Extensions.AI.ChatMessage(ChatRole.User, args.Content.ToString()));

            Microsoft.Extensions.AI.ChatCompletion response = await _chatClient.CompleteAsync(_conversation);

            _conversation.Add(response.Message);

            await args.Chat.SendMessage(response.Message.Text, ChatRole.Assistant);
        }
    }
}

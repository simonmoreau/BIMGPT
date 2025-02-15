using DevExpress.AIIntegration.Blazor.Chat;
using DevExpress.Utils.Design;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.AI;
using Microsoft.Identity.Client;
using OpenAI.Chat;

namespace BIMGPT.Client.Pages
{
    public partial class Home
    {
        [Inject]
        public IChatClient _chatClient { get; set; }

        async Task MessageSent(MessageSentEventArgs args)
        {
            Microsoft.Extensions.AI.ChatCompletion response = await _chatClient.CompleteAsync(args.Content.ToString());
            
            await args.Chat.SendMessage(response.Message.Text, ChatRole.Assistant);
        }
    }
}

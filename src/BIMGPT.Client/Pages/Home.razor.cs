using DevExpress.AIIntegration.Blazor.Chat;
using Microsoft.Extensions.AI;

namespace BIMGPT.Client.Pages
{
    public partial class Home
    {
        async Task MessageSent(MessageSentEventArgs args)
        {
            await args.Chat.SendMessage($"Processed: {args.Content}", ChatRole.Assistant);
        }
    }
}

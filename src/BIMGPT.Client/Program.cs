using Azure.Identity;
using DevExpress.Blazor;
using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BIMGPT.Client.Services.Configuration;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using System.ClientModel;

namespace BIMGPT.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            builder.Services.AddDevExpressBlazor(options =>
            {
                options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
            });

            builder.Services.AddAzureServices(builder);
            builder.Services.AddAIServices(builder);


            await builder.Build().RunAsync();
        }
    }

    public static class ConfigureAzureServices
    {
        public static IServiceCollection AddAzureServices(this IServiceCollection services, WebAssemblyHostBuilder builder)
        {
            string storageAccountName = builder.Configuration["StorageAccountName"];
            string tableEndpoint = $"https://{storageAccountName}.table.core.windows.net/";
            string keyVaultEndPoint = $"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/";

            DefaultAzureCredential credential = new DefaultAzureCredential();

            // builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndPoint), credential);

            return services;
        }

        public static IServiceCollection AddAIServices(this IServiceCollection services, WebAssemblyHostBuilder builder)
        {
            AzureOpenAI? azureOpenAISettigns = builder.Configuration.GetSection(nameof(AzureOpenAI)).Get<AzureOpenAI>();

            string deploymentName = azureOpenAISettigns.ModelId;
            Uri endpoint = new Uri(azureOpenAISettigns.Endpoint);
            ApiKeyCredential apiKey = new ApiKeyCredential(azureOpenAISettigns.Key);

            IChatClient chatClient = new AzureOpenAIClient(
                        endpoint,
                        apiKey)
                    .AsChatClient(deploymentName);

            services.AddChatClient(chatClient);
            services.AddDevExpressAI();

            return services;
        }
    }
}

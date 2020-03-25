using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Chat.BackgroundServices;
using Chat.Handlers;
using Chat.MessageHandlers;
using Chat.Producers;
using Chat.Repositories;
using WebSocketManager;

namespace Chat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //REPOSITORIES
            services.AddSingleton<ChatChannelRepository>();
            services.AddSingleton<ChatMessageRepository>();
            services.AddSingleton<ActiveChatChannelRepository>();

            //MESSAGE HANDLERS
            services.AddTransient<MessageSentMessageHandler>();            

            //KAFKA PRODUCERS
            services.AddSingleton<KafkaProducer>();

            //CONSUMERS
            services.AddSingleton<ChatMessageConsumer>();

            //BACKGROUND SERVICES
            services.AddHostedService<ChatMessageConsumerBackgroundService>();
            //services.AddHostedService<ChatChannelBackgroundService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWebSockets();
            app.MapWebSocketManager("/chat", serviceProvider.GetService<ChatHandler>());

        }
    }
}

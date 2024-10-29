using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using TreechChatLinkCreator;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

CreateHostBuilder(args).Build().Run();

return;

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("ChatLinkJob");
                q.AddJob<ChatLinkJob>(x => x.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("RevokeChatLink-Trigger")
                    .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever()));
            });



            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        });
using Quartz;

namespace TreechChatLinkCreator;

public class HelloWorld : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("wowowowowowoowowowo");
        return Task.CompletedTask;
    }
}
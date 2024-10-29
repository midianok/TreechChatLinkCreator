using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Quartz;
using Telegram.Bot;

namespace TreechChatLinkCreator;

public class ChatLinkJob : IJob
{
    private readonly ILogger<ChatLinkJob> _logger;
    private readonly Regex _chatLinkRegex;
    private readonly TelegramBotClient _telegramBotClient;
    private const long chatId = -1002073878391;

    public ChatLinkJob(ILogger<ChatLinkJob> logger)
    {
        _logger = logger;
        var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
        _chatLinkRegex = new Regex(@"https?:\/\/(?:www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b(?:[-a-zA-Z0-9()@:%_\+.~#?&\/=]*)");
        _telegramBotClient = new TelegramBotClient(botToken);
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("ChatInviteLink job started");
        var link = await _telegramBotClient.ExportChatInviteLinkAsync(chatId);

        _logger.LogInformation("New link created {0}", link);

        var chat = await _telegramBotClient.GetChatAsync(chatId);
        var oldLink = _chatLinkRegex.Match(chat.Description).Groups.Values.First().Value;

        var result = chat.Description.Replace(oldLink, link);
        await _telegramBotClient.SetChatDescriptionAsync(chatId, result);
        _logger.LogInformation("Description updated");

    }
}
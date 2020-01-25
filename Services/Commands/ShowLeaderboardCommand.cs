﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using StravaDiscordBot.Exceptions;
using StravaDiscordBot.Helpers;
using StravaDiscordBot.Models;
using StravaDiscordBot.Models.Strava;
using StravaDiscordBot.Storage;

namespace StravaDiscordBot.Services.Commands
{
    public partial class ShowLeaderboardCommand : CommandBase
    {
        private readonly ILogger<ShowLeaderboardCommand> _logger;
        private readonly ICommandCoreService _commandCoreService;

        public ShowLeaderboardCommand(AppOptions options, BotDbContext context, ILogger<ShowLeaderboardCommand> logger, ICommandCoreService commandCoreService) : base(options, context, logger)
        {
            _logger = logger;
            _commandCoreService = commandCoreService;
        }

        public override string CommandName => "leaderboard";
        public override string Descriptions => "Print current leaderboard";

        public override async Task Execute(SocketUserMessage message, int argPos)
        {
            if (!CanExecute(message, argPos))
                throw new InvalidCommandArgumentException($"Whoops, this seems wrong, the command should be in format of `{CommandName}`");

            _logger.LogInformation($"Executing 'leaderboard' command. Full: {message.Content} | Author: {message.Author}");

            var embeds = await _commandCoreService.GenerateLeaderboardCommandContent(message.Channel.Id);
            foreach(var embed in embeds)
            {
                await message.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
            }
        }
    }
}

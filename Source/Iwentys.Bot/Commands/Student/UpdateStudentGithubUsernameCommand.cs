﻿using System.Threading.Tasks;
using FluentResults;
using Iwentys.ApiClient.OpenAPIService;
using Iwentys.ClientBot.Tools;
using Iwentys.Models.Transferable.Students;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.Student
{
    public class UpdateStudentGithubUsernameCommand : IBotCommand
    {
        private readonly IwentysApiProvider _iwentysApi;
        private readonly UserIdentifier _userIdentifier;

        public UpdateStudentGithubUsernameCommand(IwentysApiProvider iwentysApi, UserIdentifier userIdentifier)
        {
            _iwentysApi = iwentysApi;
            _userIdentifier = userIdentifier;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            if (args.Arguments.Count != Args.Length)
                return Result.Fail("Wrong argument count");

            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            Client client = await _userIdentifier.GetProvider(args.Sender.UserSenderId, _iwentysApi).ConfigureAwait(false);
            StudentFullProfileDto profile = await client.ApiStudentPutAsync(new StudentUpdateDto { GithubUsername = args.Arguments[0]}).ConfigureAwait(false);
            return Result.Ok(profile.FormatFullInfo());
        }

        public string CommandName { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string Description { get; } = nameof(UpdateStudentGithubUsernameCommand);
        public string[] Args { get; } = {"New github username"};
    }
}
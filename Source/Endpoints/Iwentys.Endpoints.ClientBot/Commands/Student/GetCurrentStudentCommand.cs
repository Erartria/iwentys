﻿using System;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.Endpoints.ClientBot.Tools;
using Iwentys.Features.StudentFeature.Domain;
using Iwentys.Features.StudentFeature.Models;
using Iwentys.Features.StudentFeature.Services;
using Microsoft.Extensions.DependencyInjection;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.Endpoints.ClientBot.Commands.Student
{
    public class GetCurrentStudentCommand : IBotCommand
    {
        private readonly StudentService _studentService;
        private readonly UserIdentifier _userIdentifier;

        public GetCurrentStudentCommand(ServiceProvider serviceProvider, UserIdentifier userIdentifier)
        {
            _studentService = serviceProvider.GetService<StudentService>();
            _userIdentifier = userIdentifier;
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            if (currentUser is null)
                return Result.Fail("Current user is not set");

            return Result.Ok();
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            AuthorizedUser currentUser = _userIdentifier.GetUser(args.Sender.UserSenderId);
            StudentFullProfileDto profile = await _studentService.GetAsync(currentUser.Id);
            return Result.Ok(profile.FormatFullInfo());
        }

        public string CommandName => nameof(GetCurrentStudentCommand);
        public string Description => nameof(GetCurrentStudentCommand);
        public string[] Args => Array.Empty<string>();
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Enums;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildTestTaskService
    {
        private readonly AchievementProvider _achievementProvider;
        private readonly GithubIntegrationService _githubIntegrationService;

        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<GuildTestTaskSolution> _guildTestTaskSolutionRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<IwentysUser> _userRepository;


        public GuildTestTaskService(AchievementProvider achievementProvider, IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
        {
            _achievementProvider = achievementProvider;
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _guildTestTaskSolutionRepository = _unitOfWork.GetRepository<GuildTestTaskSolution>();
        }

        public Task<List<GuildTestTaskInfoResponse>> GetResponses(int guildId)
        {
            return _guildTestTaskSolutionRepository
                .Get()
                .Where(t => t.GuildId == guildId)
                .Select(GuildTestTaskInfoResponse.FromEntity)
                .ToListAsync();
        }

        public async Task<GuildTestTaskInfoResponse> Accept(AuthorizedUser user, int guildId)
        {
            Guild authorGuild = _guildMemberRepository.ReadForStudent(user.Id);
            if (authorGuild is null || authorGuild.Id != guildId)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, guildId);

            IwentysUser author = await _userRepository.FindByIdAsync(user.Id);

            GuildTestTaskSolution existedTestTaskSolution = await _guildTestTaskSolutionRepository
                .Get()
                .Where(GuildTestTaskSolution.IsNotCompleted)
                .FirstOrDefaultAsync(k =>
                    k.GuildId == authorGuild.Id &&
                    k.AuthorId == user.Id);

            if (existedTestTaskSolution is not null)
                InnerLogicException.GuildExceptions.ActiveTestExisted(user.Id, guildId);

            var testTaskSolution = GuildTestTaskSolution.Create(authorGuild, author);

            await _guildTestTaskSolutionRepository.InsertAsync(testTaskSolution);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTaskSolution);
        }

        public async Task<GuildTestTaskInfoResponse> Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId)
        {
            IwentysUser review = await _userRepository.FindByIdAsync(user.Id);
            await review.EnsureIsGuildMentor(_guildRepository, guildId);

            GuildTestTaskSolution testTask = await _guildTestTaskSolutionRepository
                .GetSingle(t => t.AuthorId == taskSolveOwnerId && t.GuildId == guildId);

            if (testTask.GetState() != GuildTestTaskState.Submitted)
                throw new InnerLogicException("Task must be submitted");

            testTask.SetCompleted(review);
            await _achievementProvider.Achieve(AchievementList.TestTaskDone, taskSolveOwnerId);

            _guildTestTaskSolutionRepository.Update(testTask);
            await _unitOfWork.CommitAsync();
            return GuildTestTaskInfoResponse.Wrap(testTask);
        }
    }
}
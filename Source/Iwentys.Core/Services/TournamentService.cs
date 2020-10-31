﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Context;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.Services
{
    public class TournamentService
    {
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IGithubApiAccessor _githubApi;
        private readonly GithubUserDataService _githubUserDataService;

        public TournamentService(DatabaseAccessor databaseAccessor, IGithubApiAccessor githubApi, GithubUserDataService githubUserDataService)
        {
            _githubApi = githubApi;
            _databaseAccessor = databaseAccessor;
            _githubUserDataService = githubUserDataService;
        }

        public TournamentInfoResponse[] Get()
        {
            return _databaseAccessor.Tournament
                .Read()
                .AsEnumerable()
                .Select(TournamentInfoResponse.Wrap)
                .ToArray();
        }

        public TournamentInfoResponse[] GetActive()
        {
            return _databaseAccessor.Tournament
                .Read()
                .Where(t => t.StartTime < DateTime.UtcNow && t.EndTime > DateTime.UtcNow)
                .AsEnumerable()
                .Select(TournamentInfoResponse.Wrap)
                .ToArray();
        }

        public async Task<TournamentInfoResponse> Get(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _databaseAccessor.Tournament.ReadById(tournamentId);
            return TournamentInfoResponse.Wrap(tournamentEntity);
        }

        public async Task<TournamentLeaderboardDto> GetLeaderboard(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _databaseAccessor.Tournament.ReadById(tournamentId);
            return tournamentEntity
                .WrapToDomain(_githubApi, _databaseAccessor, _githubUserDataService)
                .GetLeaderboard();
                
        }
    }
}
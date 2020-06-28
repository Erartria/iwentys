﻿using System;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public UserProfile[] Get()
        {
            return _userProfileRepository.Read();
        }

        public UserProfile Get(int profileId)
        {
            return _userProfileRepository.Get(profileId);
        }

        public UserProfile GetOrCreate(int profileId)
        {
            throw new NotImplementedException();
        }

        public UserProfile AddGithubUsername(int profileId, string githubUsername)
        {
            throw new NotImplementedException("Need to validate github credentials");
        }

        public UserProfile RemoveGithubUsername(int profileId, string githubUsername)
        {
            UserProfile user = _userProfileRepository.Get(profileId);
            user.GithubUsername = null;
            return _userProfileRepository.Update(user);
        }
    }
}
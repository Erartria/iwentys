﻿using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildPinnedProject
    {
        //FYI: It's always must be github repo id
        public long Id { get; init; }

        public virtual Guild Guild { get; init; }
        public int GuildId { get; init; }

        //TODO: here must be just libk to github project
        public long ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }
        
        public static GuildPinnedProject Create(int guildId, GithubRepositoryInfoDto repositoryInfoDto)
        {
            return new GuildPinnedProject
            {
                Id = repositoryInfoDto.Id,
                GuildId = guildId,
                ProjectId = repositoryInfoDto.Id,
            };
        }
    }
}
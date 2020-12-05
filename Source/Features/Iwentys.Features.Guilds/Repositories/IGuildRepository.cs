﻿using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildRepository : IGenericRepository<GuildEntity, int>
    {
        GuildEntity Create(StudentEntity creator, GuildCreateRequest arguments);
        GuildEntity[] ReadPending();
        GuildEntity ReadForStudent(int studentId);
        Task<GuildPinnedProjectEntity> PinProjectAsync(int guildId, GithubRepository repository);
        void UnpinProject(long pinnedProjectId);
    }
}
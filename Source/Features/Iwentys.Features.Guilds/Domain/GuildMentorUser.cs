﻿using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Domain
{
    public class GuildMentorUser
    {
        public GuildMentorUser(Student student, Guild guild)
        {
            Student = student;
            Guild = guild;
        }

        public Student Student { get; }
        public Guild Guild { get; }
    }

    public static class GuildMentorUserExtensions
    {
        public static async Task<GuildMentorUser> EnsureIsMentor(this Student student, IGenericRepository<Guild> guildRepository, int guildId)
        {
            Guild guild = await guildRepository.FindByIdAsync(guildId);
            GuildMember membership = guild.Members.First(m => m.MemberId == student.Id);
            if (!membership.MemberType.IsEditor())
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);

            return new GuildMentorUser(student, guild);
        }
    }
}
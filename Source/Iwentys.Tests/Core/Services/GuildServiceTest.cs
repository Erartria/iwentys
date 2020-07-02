﻿using System.Linq;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
{
    [TestFixture]
    public class GuildServiceTest
    {
        [Test]
        public void CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext
                .Case()
                .WithNewUser(out UserProfile user)
                .WithGuild(user, out GuildProfileDto guild);

            Assert.IsTrue(guild.Members.Any(m => m.Id == user.Id));
        }

        [Test]
        public void CreateGuild_GuildStateIsPending()
        {
            var context = TestCaseContext
                .Case()
                .WithNewUser(out UserProfile user)
                .WithGuild(user, out GuildProfileDto guild);

            var createdGuild = context.GuildRepository.Get(guild.Id);

            Assert.AreEqual(GuildType.Pending, createdGuild.GuildType);
        }

        [Test]
        public void ApproveCreatedRepo_GuildStateIsCreated()
        {
            var context = TestCaseContext
                .Case()
                .WithNewUser(out UserProfile user)
                .WithNewUser(out var admin, UserType.Admin)
                .WithGuild(user, out GuildProfileDto guild);

            context.GuildService.ApproveGuildCreating(admin.Id, guild.Id);
            var createdGuild = context.GuildRepository.Get(guild.Id);

            Assert.AreEqual(GuildType.Created, createdGuild.GuildType);
        }

        [Test]
        public void CreateTwoGuildForUser_ErrorUserAlreadyInGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewUser(out UserProfile user)
                .WithNewUser(out UserProfile _, UserType.Admin)
                .WithGuild(user, out GuildProfileDto _);

            Assert.Catch<InnerLogicException>(() => context.WithGuild(user, out GuildProfileDto _));
        }
    }
}
﻿using System;
using System.Collections.Generic;
using Iwentys.Core.DomainModel.Guilds;
using Iwentys.Core.GithubIntegration;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;
using Iwentys.Models.Types.Github;
using Iwentys.Models.Types.Guilds;
using Moq;
using NUnit.Framework;

namespace Iwentys.Tests.Core.DomainModels
{
    [TestFixture]
    public class UserCapabilityTest
    {
        private Guild _guild;
        private GuildDomain _guildDomain;

        private Student _student;

        private Mock<ITributeRepository> _tributeRepository;
        private Mock<IGuildRepository> _guildRepository;
        private Mock<IStudentRepository> _studentRepository;
        private Mock<IGithubApiAccessor> _githubApiAccessor;

        // User without guild
        //      is not in blocked list
        //      have not send request to this guild
        //      left guild later than 24 hours before try to enter
        //
        // Guild is open
        [SetUp]
        public void SetUp()
        { 
            _student = new Student()
            {
                Id = 1,
                LastOnlineTime = DateTime.MinValue
            };

            _guild = new Guild()
            {
                Id = 1,
                Members = new List<GuildMember>()
                {
                    new GuildMember()
                    {
                        MemberType = GuildMemberType.Creator,
                        Member = new Student()
                        {
                            GithubUsername = string.Empty
                        }
                    }
                },
                HiringPolicy = GuildHiringPolicy.Open,
                PinnedProjects = new List<GuildPinnedProject>(),
                
            };

            _tributeRepository = new Mock<ITributeRepository>();
            _tributeRepository
                .Setup(r => r.ReadStudentActiveTribute(It.IsAny<Int32>(), It.IsAny<Int32>()))
                .Returns(default(Tribute));

            _githubApiAccessor = new Mock<IGithubApiAccessor>();
            _githubApiAccessor
                .Setup(a => a.GetRepository(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(default(GithubRepository));
            _githubApiAccessor
                .Setup(a => a.GetUserActivity(It.IsAny<String>()))
                .Returns(new ContributionFullInfo() {PerMonthActivity = new List<ContributionsInfo>()});

            _guildRepository = new Mock<IGuildRepository>();
            _guildRepository
                .Setup(r => r.ReadForStudent(It.IsAny<Int32>()))
                .Returns(default(Guild));
            _guildRepository
                .Setup(r => r.IsStudentHaveRequest(It.IsAny<Int32>()))
                .Returns(false);

            _studentRepository = new Mock<IStudentRepository>();
            _studentRepository
                .Setup(r => r.ReadById(It.IsAny<Int32>()))
                .Returns(_student);

            _guildDomain = new GuildDomain(_guild, _tributeRepository.Object, _guildRepository.Object, _studentRepository.Object, _githubApiAccessor.Object);
        }

        [Test]
        public void GetGuild_ForUserWithNoGuildAndForOpenedGuild_UserCapabilityIsCanEnter()
        {
            Assert.That(_guildDomain.ToGuildProfileDto(_student.Id).UserCapability, Is.EqualTo(UserCapability.CanEnter));
        }

        [Test]
        public void GetGuild_ForUserWithNoGuildAndForClosedGuild_UserCapabilityIsCanRequest()
        {
            _guild.HiringPolicy = GuildHiringPolicy.Close;

            Assert.That(_guildDomain.ToGuildProfileDto(_student.Id).UserCapability, Is.EqualTo(UserCapability.CanRequest));
        }

        [Test]
        public void GetGuild_ForGuildMember_UserCapabilityIsEntered()
        {
            _guild.Members.Add(new GuildMember() {Guild = _guild, Member = _student, MemberType = GuildMemberType.Member});
            _guildRepository
                .Setup(r => r.ReadForStudent(_student.Id))
                .Returns(_guild);


            Assert.That(_guildDomain.ToGuildProfileDto(1).UserCapability, Is.EqualTo(UserCapability.Entered));
        }

        [Test]
        public void GetGuild_ForBlockedUser_UserCapabilityIsBlocked()
        {
            _guild.Members.Add(new GuildMember() {Guild = _guild, Member = _student, MemberType = GuildMemberType.Blocked});

            Assert.That(_guildDomain.ToGuildProfileDto(1).UserCapability, Is.EqualTo(UserCapability.Blocked));
        }

        [Test]
        public void GetGuild_ForUserInAnotherGuild_UserCapabilityIsBlocked()
        {
            _guildRepository
                .Setup(r => r.ReadForStudent(_student.Id))
                .Returns(new Guild() {Id = 2});

            Assert.That(_guildDomain.ToGuildProfileDto(1).UserCapability, Is.EqualTo(UserCapability.Blocked));
        }

        [Test]
        public void GetGuild_ForUserWithRequestToThisGuild_UserCapabilityIsRequested()
        {
            _guild.Members.Add(new GuildMember() {Guild = _guild, Member = _student, MemberType = GuildMemberType.Requested});
            _guildRepository
                .Setup(r => r.IsStudentHaveRequest(_student.Id))
                .Returns(true);

            Assert.That(_guildDomain.ToGuildProfileDto(1).UserCapability, Is.EqualTo(UserCapability.Requested));
        }

        [Test]
        public void GetGuild_ForUserWithRequestToAnotherGuild_UserCapabilityIsBlocked()
        {
            _guildRepository
                .Setup(r => r.IsStudentHaveRequest(_student.Id))
                .Returns(true);

            Assert.That(_guildDomain.ToGuildProfileDto(1).UserCapability, Is.EqualTo(UserCapability.Blocked));
        }

        [Test]
        public void GetGuild_ForUserWhichLeftGuild23hoursAgo_UserCapabilityIsBlocked()
        {
            _student.GuildLeftTime = DateTime.Now.AddHours(-23);

            Assert.That(_guildDomain.ToGuildProfileDto(1).UserCapability, Is.EqualTo(UserCapability.Blocked));
        }
    }
}
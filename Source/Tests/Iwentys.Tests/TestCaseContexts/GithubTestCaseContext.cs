﻿using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities;

namespace Iwentys.Tests.TestCaseContexts
{
    public class GithubTestCaseContext
    {
        private readonly TestCaseContext _context;

        public GithubTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public GithubProject WithStudentProject(AuthorizedUser userInfo)
        {
            IwentysUser student = _context.UnitOfWork.GetRepository<IwentysUser>().GetById(userInfo.Id).Result;
            GithubUser githubUser = _context.GithubIntegrationService.User.Get(userInfo.Id).Result;
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(student.GithubUsername);

            var githubProject = new GithubProject(githubUser, repositoryInfo);
            //FYI: force EF to generate unique id
            githubProject.Id = 0;

            _context.UnitOfWork.GetRepository<GithubProject>().Insert(githubProject);
            _context.UnitOfWork.CommitAsync().Wait();

            return githubProject;
        }

        public GithubUser WithGithubAccount(AuthorizedUser user)
        {
            IwentysUser iwentysUser = _context.UnitOfWork.GetRepository<IwentysUser>().GetById(user.Id).Result;
            var newGithubUser = new GithubUser
            {
                IwentysUserId = iwentysUser.Id,
                Username = iwentysUser.GithubUsername
            };
            _context.UnitOfWork.GetRepository<GithubUser>().Insert(newGithubUser);
            _context.UnitOfWork.CommitAsync().Wait();

            return newGithubUser;
        }
    }
}
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Students.Services
{
    public class StudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<StudentEntity> _studentRepository;
        //private readonly AchievementProvider _achievementProvider;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
        }

        public async Task<List<StudentInfoDto>> GetAsync()
        {
            List<StudentEntity> students = await _studentRepository
                .GetAsync()
                .ToListAsync();

            return students.SelectToList(s => new StudentInfoDto(s));
        }

        public async Task<StudentInfoDto> GetAsync(int id)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(id);
            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> GetOrCreateAsync(int id)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(id);
            if (student is null)
            {
                var newStudent = StudentEntity.CreateFromIsu(id, "userInfo.FirstName", "userInfo.MiddleName", "userInfo.SecondName");
                await _studentRepository.InsertAsync(newStudent);
                student = await _studentRepository.GetByIdAsync(newStudent.Id);
            }

            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> AddGithubUsernameAsync(int id, string githubUsername)
        {
            bool isUsernameUsed = await _studentRepository.GetAsync().AnyAsync(s => s.GithubUsername == githubUsername);
            if (isUsernameUsed)
                throw InnerLogicException.Student.GithubAlreadyUser(githubUsername);

            //TODO: implement github access validation
            //throw new NotImplementedException("Need to validate github credentials");
            Task ret;
            StudentEntity user = await _studentRepository.GetByIdAsync(id);
            user.GithubUsername = githubUsername;
            await _studentRepository.UpdateAsync(user);

            //TODO: implement getting achievements for adding github
            //_achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            //TODO: ensure we need to return this
            return new StudentInfoDto(await _studentRepository.GetByIdAsync(id));
        }

        public async Task<StudentInfoDto> RemoveGithubUsernameAsync(int id, string githubUsername)
        {
            Task ret;
            StudentEntity user = await _studentRepository.GetByIdAsync(id);
            user.GithubUsername = githubUsername;
            await _studentRepository.UpdateAsync(user);
            return new StudentInfoDto(await _studentRepository.GetByIdAsync(id));
        }
    }
}
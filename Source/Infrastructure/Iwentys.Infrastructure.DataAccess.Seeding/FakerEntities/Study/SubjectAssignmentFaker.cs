﻿using System;
using Bogus;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.Study.Models;

namespace Iwentys.Database.Seeding.FakerEntities.Study
{
    public class SubjectAssignmentFaker
    {
        public static readonly SubjectAssignmentFaker Instance = new SubjectAssignmentFaker();

        private readonly Faker _faker = new Faker();

        public SubjectAssignment Create(int subjectId, Assignment assignment)
        {
            return new SubjectAssignment
            {
                Id = _faker.IndexVariable++ + 1,
                AssignmentId = assignment.Id,
                SubjectId = subjectId
            };
        }

        public SubjectAssignmentSubmit CreateSubjectAssignmentSubmit(int subjectAssignmentId, int studentId)
        {
            return new SubjectAssignmentSubmit
            {
                Id = _faker.IndexVariable++ + 1,
                SubjectAssignmentId = subjectAssignmentId,
                StudentId = studentId,
                SubmitTimeUtc = DateTime.UtcNow
            };
        }

        public SubjectAssignmentSubmitFeedbackArguments CreateFeedback(int submitId, FeedbackType feedbackType = FeedbackType.Approve)
        {
            return new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = _faker.Lorem.Word(),
                FeedbackType = feedbackType,
                SubjectAssignmentSubmitId = submitId
            };
        }

        public SubjectAssignmentSubmitCreateArguments CreateSubjectAssignmentSubmitCreateArguments(int assignmentId)
        {
            return new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = _faker.Lorem.Word(),
                SubjectAssignmentId = assignmentId
            };
        }

        public SubjectAssignmentCreateArguments CreateSubjectAssignmentCreateArguments()
        {
            return new SubjectAssignmentCreateArguments
            {
                Title = new Faker().Lorem.Word(),
                Description = new Faker().Lorem.Word(),
                Link = new Faker().Lorem.Word(),
                DeadlineUtc = DateTime.UtcNow.AddDays(1)
            };
        }
    }
}
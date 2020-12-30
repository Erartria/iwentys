﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class QuestGenerator
    {
        private const int QuestCount = 10;
        
        public List<Quest> Quest { get; }
        public List<QuestResponse> QuestResponse { get; } = new List<QuestResponse>();

        public QuestGenerator(List<Student> students)
        {
            var faker = new Faker<Quest>();
            Student author = students.First();

            faker
                .RuleFor(q => q.Id, f => ++f.IndexVariable)
                .RuleFor(q => q.Title, f => f.Lorem.Slug())
                .RuleFor(q => q.Description, f => f.Lorem.Paragraph())
                .RuleFor(q => q.Price, 100)
                .RuleFor(q => q.CreationTime, DateTime.UtcNow)
                .RuleFor(q => q.Deadline, DateTime.UtcNow.AddDays(100))
                .RuleFor(q => q.State, QuestState.Active)
                .RuleFor(q => q.AuthorId, author.Id);

            Quest = faker.Generate(QuestCount);

            foreach (Quest quest in Quest)
            {
                foreach (Student student in students.Take(5))
                {
                    QuestResponse.Add(new QuestResponse()
                    {
                        QuestId = quest.Id,
                        StudentId = student.Id,
                        ResponseTime = DateTime.UtcNow.AddDays(1)
                    });
                }
            }
        }
    }
}
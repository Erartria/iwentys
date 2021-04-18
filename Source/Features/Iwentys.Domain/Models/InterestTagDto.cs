﻿using System;
using System.Linq.Expressions;
using Iwentys.Domain.Gamification;

namespace Iwentys.Domain.Models
{
    public class InterestTagDto
    {
        public InterestTagDto(InterestTag interestTag) : this()
        {
            Id = interestTag.Id;
            Title = interestTag.Title;
        }

        public InterestTagDto()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public static Expression<Func<InterestTag, InterestTagDto>> FromEntity =>
            entity => new InterestTagDto
            {
                Id = entity.Id,
                Title = entity.Title
            };
    }
}
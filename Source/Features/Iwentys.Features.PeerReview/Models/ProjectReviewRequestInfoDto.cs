﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Features.PeerReview.Entities;
using Iwentys.Features.PeerReview.Enums;

namespace Iwentys.Features.PeerReview.Models
{
    public class ProjectReviewRequestInfoDto
    {
        //TODO: need to add project info, title, tags
        public int Id { get; set; }
        public string Description { get; set; }
        public ProjectReviewState State { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        public long ProjectId { get; set; }
        public ICollection<ProjectReviewFeedbackInfoDto> ReviewFeedbacks { get; set; }

        public ProjectReviewRequestInfoDto(ProjectReviewRequest reviewRequest) : this()
        {
            Id = reviewRequest.Id;
            Description = reviewRequest.Description;
            State = reviewRequest.State;
            CreationTimeUtc = reviewRequest.CreationTimeUtc;
            ProjectId = reviewRequest.ProjectId;
            ReviewFeedbacks = reviewRequest.ReviewFeedbacks?.Select(rf => new ProjectReviewFeedbackInfoDto(rf)).ToList();
        }

        public ProjectReviewRequestInfoDto()
        {
        }

        public static Expression<Func<ProjectReviewRequest, ProjectReviewRequestInfoDto>> FromEntity =>
            entity => new ProjectReviewRequestInfoDto(entity);
    }
}
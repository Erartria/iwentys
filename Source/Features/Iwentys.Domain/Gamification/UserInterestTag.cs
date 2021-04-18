﻿using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Gamification
{
    public class UserInterestTag
    {
        public int InterestTagId { get; init; }
        public virtual InterestTag InterestTag { get; init; }

        public int UserId { get; init; }
        public virtual IwentysUser User { get; init; }
    }
}
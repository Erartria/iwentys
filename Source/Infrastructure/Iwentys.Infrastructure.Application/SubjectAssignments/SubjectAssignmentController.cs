﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study.Models;
using Iwentys.FeatureBase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Study.SubjectAssignments
{
    [Route("api/subject-assignment")]
    [ApiController]
    public class SubjectAssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectAssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetByGroupId))]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetByGroupId(int groupId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectAssignmentForGroup.Response response = await _mediator.Send(new GetSubjectAssignmentForGroup.Query(groupId));
            return Ok(response.SubjectAssignments);
        }

        [HttpGet(nameof(GetBySubjectId))]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetBySubjectId(int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectAssignmentForSubject.Response response = await _mediator.Send(new GetSubjectAssignmentForSubject.Query(subjectId));
            return Ok(response.SubjectAssignments);
        }
        
        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(AssignmentCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateSubjectAssignment.Response response = await _mediator.Send(new CreateSubjectAssignment.Query(arguments, authorizedUser));
            return Ok();
        }
    }
}
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.StudentPages
{
    public partial class StudentSubjectAssignmentPage
    {
        private ICollection<SubjectAssignmentJournalItemDto> _subjectAssignments;
        private ICollection<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignments = await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentsAsync();
            _subjectAssignmentSubmits = await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentSubmitsAsync(SubjectId);
        }

        private string LinkToCreateSubmit() => $"/subject/{SubjectId}/assignments/create-submit";
    }
}

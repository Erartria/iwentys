﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentResults;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Entities
{
    public class GroupSubjectEntity
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public SubjectEntity Subject { get; set; }

        public int StudyGroupId { get; set; }
        public StudyGroupEntity StudyGroup { get; set; }

        public int LectorTeacherId { get; set; }
        public TeacherEntity LectorTeacher { get; set; }

        public int PracticeTeacherId { get; set; }
        public TeacherEntity PracticeTeacher { get; set; }

        public string SerializedGoogleTableConfig { get; set; }
        public StudySemester StudySemester { get; set; }

        public Result<GoogleTableData> TryGetGoogleTableDataConfig()
        {
            if (SerializedGoogleTableConfig is null)
                return Result.Fail<GoogleTableData>("Value is not set");

            try
            {
                var googleTableData = JsonSerializer.Deserialize<GoogleTableData>(SerializedGoogleTableConfig);
                return Result.Ok(googleTableData);
            }
            catch (Exception e)
            {
                return Result.Fail<GoogleTableData>(new Error("Data parse failed").CausedBy(e));
            }
        }
    }
}
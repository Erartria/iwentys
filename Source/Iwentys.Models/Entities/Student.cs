﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public string Group { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }

        public DateTime GuildLeftTime { get; set; }

        public static Student CreateFromIsu(int id, string firstName, string middleName, string secondName, string group)
        {
            return new Student
            {
                Id = id,
                FirstName = firstName,
                MiddleName = middleName,
                SecondName = secondName,
                Role = UserType.Common,
                Group = group,
                CreationTime = DateTime.UtcNow,
                LastOnlineTime = DateTime.UtcNow,
                GuildLeftTime = DateTime.MinValue
            };
        }
    }
}
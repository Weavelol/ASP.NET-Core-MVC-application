﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task9.Models.TaskModels;

namespace Task9.Data
{
    public class Task9Context : DbContext
    {
        public Task9Context (DbContextOptions<Task9Context> options)
            : base(options)
        {
        }

        public DbSet<Task9.Models.TaskModels.Course> Course { get; set; }

        public DbSet<Task9.Models.TaskModels.Group> Group { get; set; }

        public DbSet<Task9.Models.TaskModels.Student> Student { get; set; }
    }
}

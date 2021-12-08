﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public sealed class StudentRepository : AbstractRepository<Student> {
        private StudentRepository(Task9Context context) : base(context) { }
        public static StudentRepository GetStudentData(Task9Context context) {
            return context is null ? null : new StudentRepository(context);
        }

        public override async Task<IEnumerable<Student>> GetEntityListAsync() {
            return await _context.Students.Include(x => x.Group).AsNoTracking().ToListAsync();
        }

        public override async Task<Student> GetEntityAsync(int id) {
            if (id < 0) {
                return null;
            }
            var student = await _context.Students
                .Include(x => x.Group)
                .Include(x => x.Group.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student is null) {
                throw new NoEntityException();
            }
            return student;
        }

        public override async Task DeleteAsync(int id) {
            var student = GetEntityAsync(id).Result;
            _context.Students.Remove(student);
            await SaveAsync();
        }

        public bool StudentExists(int id) {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
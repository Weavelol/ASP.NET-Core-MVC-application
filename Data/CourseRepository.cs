﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data {
    public sealed class CourseRepository : AbstractRepository<Course> {
        private CourseRepository(Task9Context context) : base(context) { }
        public static CourseRepository GetCourseRepository(Task9Context context) {
            return context is null ? null : new CourseRepository(context);
        }

        public override IEnumerable<Course> GetEntityList(string searchString) {
            var courses = GetEntityList();
            if (!string.IsNullOrEmpty(searchString)) {
                courses = courses.Where(
                    s => s.CourseName!.Contains(searchString) 
                 || s.CourseDescription!.Contains(searchString));
            }
            return courses;
        }

        public override IEnumerable<Course> GetEntityList() {
            var courses = from c in _context.Course select c;
            return courses.ToListAsync().Result;
        }

        public override Course GetEntity(int id) {
            if (id < 0) {
                return null;
            }
            var course = _context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            return course.Result;
        }

        public override void Delete(int id) {
            var course = GetEntity(id);
            if (_context.Group.Any(x => x.CourseId == course.Id)) {
                throw new Exception();
            }
            _context.Course.Remove(course);
            Save();
        }

        public bool CourseExists(int id) {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Interfaces;
using Services.ModelsDTO;

namespace Services.Services {
    public class StudentService : AbstractService<Student, StudentDto> {

        public StudentService(IRepository<Student> repository, IMapper mapper) : base(repository, mapper) { }
        protected override async Task<List<Student>> GetFilteredItemsAsync(string searchString = null, int? filter = null) {
            var students = await Repository.GetEntityListAsync();

            if (filter > 0) {
                students = students.Where(x => x.Group.Id == filter);
            }

            if (!string.IsNullOrEmpty(searchString)) {
                students = students.Where(
                    x => x.FirstName.Contains(searchString)
                         || x.LastName.Contains(searchString)
                         || x.Group.GroupName.Contains(searchString));
            }
            return students.ToList();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Data;
using Services.ModelsDTO;
using ServicesInterfaces;

namespace Business {
    public class GroupPresentation : IPresentationItem<GroupDTO> {
        private readonly GroupRepository _groupRepository;
        private readonly CourseRepository _courseRepository;
        private readonly StudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GroupPresentation(Task9Context context, IMapper mapper) {
            _groupRepository = GroupRepository.GetGroupRepository(context);
            _courseRepository = CourseRepository.GetCourseRepository(context);
            _studentRepository = StudentRepository.GetStudentData(context);
            _mapper = mapper;
        }

        public async Task<List<GroupDTO>> GetAllItemsAsync(string searchString = null, string groupCourse = null) {
            var groups = await _groupRepository.GetEntityListAsync();

            if (!string.IsNullOrEmpty(groupCourse)) {
                groups = groups.Where(x => x.Course.CourseName.Contains(groupCourse));
            }

            if (!string.IsNullOrEmpty(searchString)) {
                groups = groups.Where(
                    x => x.GroupName.Contains(searchString) 
                         || x.Course.CourseName.Contains(searchString));
            }

            return groups.Select(x => _mapper.Map<GroupDTO>(x)).ToList();
        }

        public async Task<GroupDTO> GetItemAsync(int? id) {
            if (id is null) {
                throw new Exception();
            }
            var group = await _groupRepository.GetEntityAsync((int)id);
            if (group is null) {
                throw new Exception();
            }

            return _mapper.Map<GroupDTO>(group);
        }

        public async Task CreateItemAsync(GroupDTO item) {
            var course = await _courseRepository.GetEntityAsync(item.CourseId);
            var group = new Group {
                Id = item.Id,
                GroupName = item.GroupName,
                CourseId = course.Id

            };
            await _groupRepository.CreateAsync(group);
        }

        public async Task UpdateItemAsync(GroupDTO item) {
            var course = await _courseRepository.GetEntityAsync(item.CourseId);
            var group = new Group {
                Id = item.Id,
                GroupName = item.GroupName,
                Course = course,
                CourseId = course.Id

            };
            await _groupRepository.UpdateAsync(group);
        }

        public async Task DeleteItemAsync(int id) {
            var students = await _studentRepository.GetEntityListAsync();
            if (students.Any(x => x.GroupId == id)) {
                throw new Exception();
            }
            await _groupRepository.DeleteAsync(id);
        }

        public bool ItemExists(int id) {
            return _groupRepository.GroupExists(id);
        }

        public async Task<IEnumerable<Course>> GetCourses() {
            return await _courseRepository.GetEntityListAsync();
        }
    }
}
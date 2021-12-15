﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.ModelsDTO;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly IService<StudentDto> _studentService;
        private readonly IService<GroupDto> _groupService;
        private readonly IService<CourseDto> _courseService;

        public StudentsController(IService<StudentDto> studentService, IService<GroupDto> groupService, IService<CourseDto> courseService) {
            _studentService = studentService;
            _groupService = groupService;
            _courseService = courseService;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? selectedCourseId, int? selectedGroupId, string searchString) {
            await PopulateCoursesDropDownList(selectedCourseId);
            await PopulateGroupsDropDownList(selectedGroupId, selectedCourseId);

            var filter = new Filter(searchString, selectedGroupId, selectedCourseId);
            var students = await _studentService.GetAllItemsAsync(filter);

            var studentViewModel = new StudentsViewModel {
                SelectedCourseId = selectedCourseId,
                SelectedGroupId = selectedGroupId,
                SearchString = searchString,
                FilteredStudents = students.ToList()
            };

            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            var studentDto = await _studentService.GetAsync(id);
            return View(studentDto);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create(int? selectedGroupId, int? selectedCourseId = null) {
            var student = new StudentDto();
            if (selectedGroupId > 0) {
                student.GroupId = (int)selectedGroupId;
            }
            await PopulateGroupsDropDownList(selectedGroupId, selectedCourseId);
            return View(student);
        }

        // POST: Students/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentDto studentDto) {
            if (!ModelState.IsValid) {
                await PopulateGroupsDropDownList();
                return View(studentDto);
            }
            var student = await _studentService.CreateAsync(studentDto);
            return RedirectToAction("Index", new { selectedGroup = student.GroupId});
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var studentDto = await _studentService.GetAsync(id);
            await PopulateGroupsDropDownList();
            return View(studentDto);
        }

        // POST: Students/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentDto studentDto) {
            if (!ModelState.IsValid) {
                return View(studentDto);
            }
            await _studentService.UpdateAsync(studentDto);
            return RedirectToAction("Index", new { selectedGroup = studentDto.GroupId });
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var studentDto = await _studentService.GetAsync(id);
            return View(studentDto);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var studentDto = await _studentService.GetAsync(id);
            await _studentService.DeleteAsync(id);
            return RedirectToAction("Index", new {selectedGroup = studentDto.GroupId});
        }

        private async Task PopulateGroupsDropDownList(object selectedGroup = null, int? selectedCourse = null) {
            var filter = new Filter(courseFilter:selectedCourse);
            var groups = await _groupService.GetAllItemsAsync(filter);
            ViewBag.Groups = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
        private async Task PopulateCoursesDropDownList(object selectedCourse = null) {
            var coursesDto = await _courseService.GetAllItemsAsync(null);
            ViewBag.Courses = new SelectList(coursesDto, "Id", "CourseName", selectedCourse);
        }
    }
}

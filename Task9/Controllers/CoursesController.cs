﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.ModelsDTO;
using ServicesInterfaces;

namespace Task9.Controllers {
    public class CoursesController : Controller {
        private readonly IService<CourseDTO> _courseService;

        public CoursesController(IService<CourseDTO> service) {
            _courseService = service;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString) {
            var courseDtos = await _courseService.GetAllItemsAsync(searchString);
            return View(courseDtos);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id) {
            var courseDtos = await _courseService.GetAsync(id);
            return View(courseDtos);
        }

        // GET: Courses/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Courses/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseDTO courseDtos) {
            if (!ModelState.IsValid) {
                return View(courseDtos);
            }
            await _courseService.CreateAsync(courseDtos);
            return RedirectToAction("Index");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var courseDtos = await _courseService.GetAsync(id);
            return View(courseDtos);
        }

        // POST: Courses/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseDTO courseDtos) {
            if (!ModelState.IsValid) {
                return View(courseDtos);
            }
            await _courseService.UpdateAsync(courseDtos);
            return RedirectToAction("Index");
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var courseDtos = await _courseService.GetAsync(id);
            return View(courseDtos);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _courseService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult ViewGroups(string courseName) {
            return RedirectToAction("Index", "Groups", new { groupCourse = courseName });
        }
        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }
    }
}

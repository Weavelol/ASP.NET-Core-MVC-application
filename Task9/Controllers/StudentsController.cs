﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.ModelsDTO;
using Services.Presentations;
using Task9.TaskViewModels;

namespace Task9.Controllers {
    public class StudentsController : Controller {
        private readonly StudentPresentation _studentPresentation;

        public StudentsController(Task9Context context, IMapper mapper) {
            _studentPresentation = new StudentPresentation(context, mapper);
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentGroup, string searchString) {
            var studentDTOs = await _studentPresentation.GetAllItemsAsync(searchString, studentGroup);
            var groupsNames = await _studentPresentation.GetGroupsNames();


            var studentViewModel = new StudentsViewModel {
                Groups = new SelectList(groupsNames),
                Students = studentDTOs.ToList()
            };
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id) {
            var studentDTO = await _studentPresentation.GetItemAsync(id);
            return View(studentDTO);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create() {
            await PopulateGroupsDropDownList();
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentDTO studentDTO) {
            if (!ModelState.IsValid) {
                await PopulateGroupsDropDownList();
                return View(studentDTO);
            }
            await _studentPresentation.CreateItemAsync(studentDTO);
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            var studentDTO = await _studentPresentation.GetItemAsync(id);
            await PopulateGroupsDropDownList(studentDTO.GroupId);
            return View(studentDTO);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentDTO studentDTO) {
            if (!ModelState.IsValid) {
                return View(studentDTO);
            }
            await _studentPresentation.UpdateItemAsync(studentDTO);
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            var studentDTO = await _studentPresentation.GetItemAsync(id);
            return View(studentDTO);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _studentPresentation.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult ClearFilter() {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task PopulateGroupsDropDownList(object selectedGroup = null) {
            var groups = await _studentPresentation.GetGroups();
            ViewBag.GroupId = new SelectList(groups, "Id", "GroupName", selectedGroup);
        }
    }
}

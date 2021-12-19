#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeMVC.Data;
using EmployeeMVC.Models;
using EmployeeMVC.Repository;
using EmployeeMVC.Models.DTOs;

namespace EmployeeMVC.Controllers.Views
{
    public class LeavesController : BaseController
    {
        public LeavesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: Leaves
        public async Task<IActionResult> Index()
        {
            try
            {
                var Leavelist = await _repositoryWrapper.Leave.LoadLeaveList();
                return View(Leavelist);
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get Leavelist fail", ex.Message);
                return BadRequest();
            }
        }

        // GET: Leaves/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var leave = await _repositoryWrapper.Leave.LoadLeaveDetails(id);

            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // GET: Leaves/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,Date,Reason")] CreateLeaveDTO leaveDTO)
        {
            if (ModelState.IsValid)
            {
                var Employee = await _repositoryWrapper.Employee.FindByIDAsync(leaveDTO.EmployeeId);

                if (Employee == null)
                {
                    ModelState.AddModelError("EmployeeId", "Employee does not exist");
                    return View(leaveDTO);
                }
                Leave leave = new()
                {
                    EmployeeId = leaveDTO.EmployeeId,
                    Reason = leaveDTO.Reason,
                    Date = leaveDTO.Date,
                    CreatedTime = DateTime.Now
                };
                try
                {
                    await _repositoryWrapper.Leave.CreateAsync(leave);
                    await _repositoryWrapper.Eventlog.Insert(leave);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    if (await LeaveExists(leave.Id))
                    {
                        return Conflict();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    await _repositoryWrapper.Eventlog.Error("create fail", ex.Message);
                    return BadRequest();
                }
            }
            return View(leaveDTO);
        }

        // GET: Leaves/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var leave = await _repositoryWrapper.Leave.FindByIDAsync(id);

            if (leave == null)
            {
                return NotFound();
            }

            return View(leave.AsDTO());
        }

        // POST: Leaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,Date,Reason,CreatedTime,ModifiedTime")] LeaveDTO leaveDTO)
        {
            if (id != leaveDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var Employee = await _repositoryWrapper.Employee.FindByIDAsync(leaveDTO.EmployeeId);

                if (Employee == null)
                {
                    ModelState.AddModelError("EmployeeId", "Employee does not exist");
                    return View(leaveDTO);
                }
                Leave leave = new()
                {
                    Id = leaveDTO.Id,
                    EmployeeId = leaveDTO.EmployeeId,
                    Reason = leaveDTO.Reason,
                    Date = leaveDTO.Date,
                    CreatedTime = leaveDTO.CreatedTime,
                    ModifiedTime = DateTime.Now
                };
                try
                {
                    await _repositoryWrapper.Leave.UpdateAsync(leave);
                    await _repositoryWrapper.Eventlog.Update(leave);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await LeaveExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    await _repositoryWrapper.Eventlog.Error("update fail", ex.Message);
                    return BadRequest();
                }            }
            return View(leaveDTO);
        }

        // GET: Leaves/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _repositoryWrapper.Leave.LoadLeaveDetails(id);

            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var Leave = await _repositoryWrapper.Leave.FindByIDAsync(id);
                if (Leave == null)
                {
                    return NotFound();
                }

                await _repositoryWrapper.Leave.DeleteAsync(Leave);
                await _repositoryWrapper.Eventlog.Delete(Leave);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest();
            }
        }

        private async Task<bool> LeaveExists(int id)
        {
            return await _repositoryWrapper.Leave.IsExist(id);
        }
    }
}

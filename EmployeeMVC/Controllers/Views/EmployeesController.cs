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
    public class EmployeesController : BaseController
    {
        public EmployeesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            try
            {
                var Employeelist = (await _repositoryWrapper.Employee.FindAllAsync()).Select(x => x.AsDTO());
                return View(Employeelist);
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("Employee List View fail", ex.Message);
                return NotFound();
            }
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var Employee = await _repositoryWrapper.Employee.FindByIDAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee.AsDTO());
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Address,Phone,Email")] CreateEmployeeDTO employeeDTO)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new()
                {
                    Name = employeeDTO.Name,
                    DateOfBirth = employeeDTO.DateOfBirth,
                    Address = employeeDTO.Address,
                    Phone = employeeDTO.Phone,
                    Email = employeeDTO.Email,
                    CreatedTime = DateTime.Now
                };
                try
                { 
                    await _repositoryWrapper.Employee.CreateAsync(employee);
                    await _repositoryWrapper.Eventlog.Insert(employee);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    if (await EmployeeExists(employee.Id))
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
            return View(employeeDTO);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var Employee = await _repositoryWrapper.Employee.FindByIDAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee.AsDTO());
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Address,Phone,Email,CreatedTime,ModifiedTime")] EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Employee employee = new()
                {
                    Id = employeeDTO.Id,
                    Name = employeeDTO.Name,
                    DateOfBirth = employeeDTO.DateOfBirth,
                    Address = employeeDTO.Address,
                    Phone = employeeDTO.Phone,
                    Email = employeeDTO.Email,
                    CreatedTime = employeeDTO.CreatedTime,
                    ModifiedTime = DateTime.Now
                };
                try
                {
                    await _repositoryWrapper.Employee.UpdateAsync(employee);
                    await _repositoryWrapper.Eventlog.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeDTO);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var Employee = await _repositoryWrapper.Employee.FindByIDAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee.AsDTO());
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var Employee = await _repositoryWrapper.Employee.FindByIDAsync(id);
                if (Employee == null)
                {
                    return NotFound();
                }

                await _repositoryWrapper.Employee.DeleteAsync(Employee);
                await _repositoryWrapper.Eventlog.Delete(Employee);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest();
            }
        }

        private async Task<bool> EmployeeExists(int id)
        {
            return await _repositoryWrapper.Employee.IsExist(id);
        }
    }
}

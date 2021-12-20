using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EmployeeMVC.Models;
using EmployeeMVC.Repository;
using EmployeeMVC.Controllers;

namespace EmployeeMVC.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : BaseAPIController
    {
        public EmployeesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetEmployees()
        {
            try
            {
                var Employeelist = await _repositoryWrapper.Employee.FindAllAsync();
                return Ok(new { status = "success", data = Employeelist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Employeelist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var Employee = await _repositoryWrapper.Employee.FindByIDAsync(id);

            if (Employee == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = Employee.AsDTO() });
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee Employee)
        {
            if (id != Employee.Id)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Employee.UpdateAsync(Employee);
                await _repositoryWrapper.Eventlog.Update(Employee);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EmployeeExists(id))
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }
                else
                {
                    return BadRequest(new { status = "fail", data = "Bad Parameters."});
                }
            }
            catch(Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("update fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee Employee)
        { 
            try
            {
                await _repositoryWrapper.Employee.CreateAsync(Employee);
                await _repositoryWrapper.Eventlog.Insert(Employee);

                return CreatedAtAction(nameof(GetEmployee), new { id = Employee.Id }, new { status = "success", data = Employee });
            }
            catch (DbUpdateException)
            {
                if (await EmployeeExists(Employee.Id))
                {
                    return Conflict(new { status = "fail", data = "Data Already Exist."});
                }
                else
                {
                    return BadRequest(new { status = "fail", data = "Bad Parameters."});
                }
            }
            catch(Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("create fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var Employee = await _repositoryWrapper.Employee.FindByIDAsync(id);
                if (Employee == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Employee.DeleteAsync(Employee);
                await _repositoryWrapper.Eventlog.Delete(Employee);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> EmployeeExists(int id)
        {
            return await _repositoryWrapper.Employee.IsExist(id);
        }
    }
}

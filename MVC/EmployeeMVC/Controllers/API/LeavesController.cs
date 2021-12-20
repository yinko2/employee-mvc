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
    public class LeavesController : BaseAPIController
    {
        public LeavesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Leave
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetLeaves()
        {
            try
            {
                var Leavelist = await _repositoryWrapper.Leave.FindAllAsync();
                return Ok(new { status = "success", data = Leavelist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Leavelist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Leave/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leave>> GetLeave(int id)
        {
            var Leave = await _repositoryWrapper.Leave.FindByIDAsync(id);

            if (Leave == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = Leave });
        }

        // PUT: api/Leave/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeave(int id, Leave Leave)
        {
            if (id != Leave.Id)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Leave.UpdateAsync(Leave);
                await _repositoryWrapper.Eventlog.Update(Leave);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LeaveExists(id))
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

        // POST: api/Leave
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Leave>> PostLeave(Leave Leave)
        { 
            try
            {
                await _repositoryWrapper.Leave.CreateAsync(Leave);
                await _repositoryWrapper.Eventlog.Insert(Leave);

                return CreatedAtAction(nameof(GetLeave), new { id = Leave.Id }, new { status = "success", data = Leave });
            }
            catch (DbUpdateException)
            {
                if (await LeaveExists(Leave.Id))
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

        // DELETE: api/Leave/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            try
            {
                var Leave = await _repositoryWrapper.Leave.FindByIDAsync(id);
                if (Leave == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Leave.DeleteAsync(Leave);
                await _repositoryWrapper.Eventlog.Delete(Leave);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> LeaveExists(int id)
        {
            return await _repositoryWrapper.Leave.IsExist(id);
        }
    }
}

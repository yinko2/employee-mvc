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
    public class HolidaysController : BaseAPIController
    {
        public HolidaysController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/Holiday
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetHolidays()
        {
            try
            {
                var Holidaylist = await _repositoryWrapper.Holiday.FindAllAsync();
                return Ok(new { status = "success", data = Holidaylist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Holidaylist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/Holiday/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Holiday>> GetHoliday(int id)
        {
            var Holiday = await _repositoryWrapper.Holiday.FindByIDAsync(id);

            if (Holiday == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = Holiday });
        }

        // PUT: api/Holiday/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoliday(int id, Holiday Holiday)
        {
            if (id != Holiday.Id)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.Holiday.UpdateAsync(Holiday);
                await _repositoryWrapper.Eventlog.Update(Holiday);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HolidayExists(id))
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

        // POST: api/Holiday
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Holiday>> PostHoliday(Holiday Holiday)
        { 
            try
            {
                await _repositoryWrapper.Holiday.CreateAsync(Holiday);
                await _repositoryWrapper.Eventlog.Insert(Holiday);

                return CreatedAtAction(nameof(GetHoliday), new { Holiday.Id }, new { status = "success", data = Holiday });
            }
            catch (DbUpdateException)
            {
                if (await HolidayExists(Holiday.Id))
                {
                    return Conflict(new { status = "fail", data = "Data already exist."});
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

        // DELETE: api/Holiday/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            try
            {
                var Holiday = await _repositoryWrapper.Holiday.FindByIDAsync(id);
                if (Holiday == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.Holiday.DeleteAsync(Holiday);
                await _repositoryWrapper.Eventlog.Delete(Holiday);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> HolidayExists(int id)
        {
            return await _repositoryWrapper.Holiday.IsExist(id);
        }
    }
}

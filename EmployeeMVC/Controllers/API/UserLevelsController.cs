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
    public class UserLevelsController : BaseAPIController
    {
        public UserLevelsController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/UserLevel
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetUserLevels()
        {
            try
            {
                var UserLevellist = await _repositoryWrapper.UserLevel.FindAllAsync();
                return Ok(new { status = "success", data = UserLevellist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get UserLevellist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/UserLevel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Userlevel>> GetUserLevel(int id)
        {
            var UserLevel = await _repositoryWrapper.UserLevel.FindByIDAsync(id);

            if (UserLevel == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = UserLevel });
        }

        // PUT: api/UserLevel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLevel(int id, Userlevel UserLevel)
        {
            if (id != UserLevel.Id)
            {
                return BadRequest(new { status = "fail", data = "Bad Parameters." });
            }

            try
            {
                await _repositoryWrapper.UserLevel.UpdateAsync(UserLevel);
                await _repositoryWrapper.Eventlog.Update(UserLevel);

                return Ok(new { status = "success", data = "Updated" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserLevelExists(id))
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

        // POST: api/UserLevel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Userlevel>> PostUserLevel(Userlevel UserLevel)
        { 
            try
            {
                await _repositoryWrapper.UserLevel.CreateAsync(UserLevel);
                await _repositoryWrapper.Eventlog.Insert(UserLevel);

                return CreatedAtAction(nameof(GetUserLevel), new { id = UserLevel.Id }, new { status = "success", data = UserLevel });
            }
            catch (DbUpdateException)
            {
                if (await UserLevelExists(UserLevel.Id))
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

        // DELETE: api/UserLevel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLevel(int id)
        {
            try
            {
                var UserLevel = await _repositoryWrapper.UserLevel.FindByIDAsync(id);
                if (UserLevel == null)
                {
                    return NotFound(new { status = "fail", data = "Data Not Found."});
                }

                await _repositoryWrapper.UserLevel.DeleteAsync(UserLevel);
                await _repositoryWrapper.Eventlog.Delete(UserLevel);

                return Ok(new { status = "success", data = "Deleted"});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        private async Task<bool> UserLevelExists(int id)
        {
            return await _repositoryWrapper.UserLevel.IsExist(id);
        }
    }
}

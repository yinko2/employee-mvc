using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeMVC.Models;
using EmployeeMVC.Repository;
using EmployeeMVC.Controllers;

namespace EmployeeMVC.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseAPIController
    {
        public UsersController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetUsers()
        {
            try
            {
                var Userlist = (await _repositoryWrapper.User.FindAllAsync()).Select(x => x.AsDTO());
                return Ok(new { status = "success", data = Userlist });
            }
            catch (Exception ex) 
            {
                await _repositoryWrapper.Eventlog.Error("get Userlist fail", ex.Message);
                return BadRequest( new { status = "fail", data = "Something went wrong." });
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var User = await _repositoryWrapper.User.FindByIDAsync(id);

            if (User == null)
            {
                return NotFound(new { status = "fail", data = "Data Not Found." });
            }

            return Ok( new { status = "success", data = User.AsDTO() });
        }

        // // PUT: api/User/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutUser(int id, User User)
        // {
        //     if (id != User.Id)
        //     {
        //         return BadRequest(new { status = "fail", data = "Bad Parameters." });
        //     }

        //     try
        //     {
        //         await _repositoryWrapper.User.UpdateAsync(User);
        //         await _repositoryWrapper.Eventlog.Update(User);

        //         return Ok(new { status = "success", data = "Updated" });
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!await UserExists(id))
        //         {
        //             return NotFound(new { status = "fail", data = "Data Not Found."});
        //         }
        //         else
        //         {
        //             return BadRequest(new { status = "fail", data = "Bad Parameters."});
        //         }
        //     }
        //     catch(Exception ex)
        //     {
        //         await _repositoryWrapper.Eventlog.Error("update fail", ex.Message);
        //         return BadRequest( new { status = "fail", data = "Something went wrong." });
        //     }
        // }

        [HttpPost("Registration")]
        public async Task<ActionResult<dynamic>> RegisterCashier(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                //search user for duplicate username
                bool check = await _repositoryWrapper.User.AnyByConditionAsync(u => u.UserName == userRegisterDTO.UserName);
                if (check)
                {
                    await _repositoryWrapper.Eventlog.Warning("User with UserName: " + userRegisterDTO.UserName + " already exists");
                    return StatusCode(StatusCodes.Status409Conflict, new { status = "fail", data = "Cashier with UserName: " + userRegisterDTO.UserName + " already exists" });
                }

                //create new cashier
                string salt = Util.SaltedHash.GenerateSalt();
                User newObj = new()
                {
                    UserLevelId = userRegisterDTO.UserLevelId,
                    UserName = userRegisterDTO.UserName,
                    CreatedTime = DateTime.UtcNow,
                    PasswordSalt = salt,
                    Password = Util.SaltedHash.ComputeHash(salt, userRegisterDTO.Password)
                };

                await _repositoryWrapper.User.CreateAsync(newObj, true); //add user to database
                await _repositoryWrapper.Eventlog.Insert(newObj);

                return StatusCode(StatusCodes.Status201Created, new { status = "success", data = new { newObj.Id, newObj.UserName }});
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("Registration Failed", ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new { error = "Something went wrong" });
            }
        }

        // // DELETE: api/User/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteUser(int id)
        // {
        //     try
        //     {
        //         var User = await _repositoryWrapper.User.FindByIDAsync(id);
        //         if (User == null)
        //         {
        //             return NotFound(new { status = "fail", data = "Data Not Found."});
        //         }

        //         await _repositoryWrapper.User.DeleteAsync(User);
        //         await _repositoryWrapper.Eventlog.Delete(User);

        //         return Ok(new { status = "success", data = "Deleted"});
        //     }
        //     catch (Exception ex)
        //     {
        //         await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
        //         return BadRequest( new { status = "fail", data = "Something went wrong." });
        //     }
        // }

        //private async Task<bool> UserExists(int id)
        //{
        //    return await _repositoryWrapper.User.IsExist(id);
        //}
    }
}

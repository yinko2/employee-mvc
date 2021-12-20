#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeMVC.Data;
using EmployeeMVC.Models;
using EmployeeMVC.Repository;
using EmployeeMVC.Models.DTOs;
using System.Data;
using ClosedXML.Excel;
using Aspose.Pdf;

namespace EmployeeMVC.Controllers
{
    public class EmployeesController : BaseController
    {
        public EmployeesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: Employees
        public async Task<IActionResult> Index(string Eid, string Name, DateTime BDate, string Address, string Phone, string Email)
        {
            try
            {
                var Employeelist = (await _repositoryWrapper.Employee.FindAllAsync()).Select(x => x.AsDTO());
                if (!string.IsNullOrEmpty(Eid) && int.TryParse(Eid, out int o))
                {
                    Employeelist = Employeelist.Where(s => s.Id == o);
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Employeelist = Employeelist.Where(s => s.Name!.Contains(Name));
                }
                if (BDate != DateTime.MinValue)
                {
                    Employeelist = Employeelist.Where(s => s.DateOfBirth == BDate);
                }
                if (!string.IsNullOrEmpty(Address))
                {
                    Employeelist = Employeelist.Where(s => s.Address!.Contains(Address));
                }
                if (!string.IsNullOrEmpty(Phone))
                {
                    Employeelist = Employeelist.Where(s => s.Phone!.Contains(Phone));
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    Employeelist = Employeelist.Where(s => s.Email!.Contains(Email));
                }
                return View(Employeelist);
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("Employee List View fail", ex.Message);
                return NotFound();
            }
        }

        public async Task<ActionResult> ExportToExcel()
        {
            DataTable dtProduct = await GetEmployeesListAsync();

            using (XLWorkbook woekBook = new XLWorkbook())
            {
                woekBook.Worksheets.Add(dtProduct);
                using (MemoryStream stream = new MemoryStream())
                {
                    woekBook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeesList.xlsx");
                }
            }
        }

        public async Task<ActionResult> ExportToPdf()
        {
            DataTable dtProduct = await GetEmployeesListAsync();

            if (dtProduct.Rows.Count > 0)
            {
                var document = new Document
                {
                    PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
                };
                var pdfPage = document.Pages.Add();
                Table table = new Table
                {
                    ColumnWidths = "12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%",
                    DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                    Border = new BorderInfo(BorderSide.All, .5f, Color.Black),
                    DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Color.Black)
                };

                table.ImportDataTable(dtProduct, true, 0, 0);
                document.Pages[1].Paragraphs.Add(table);

                using (var stream = new MemoryStream())
                {
                    document.Save(stream);
                    return new FileContentResult(stream.ToArray(), "application/pdf")
                    {
                        FileDownloadName = "EmployeesList.pdf"
                    };
                }
            }
            return View();
        }

        private async Task<DataTable> GetEmployeesListAsync()
        {
            var Employeelist = (await _repositoryWrapper.Employee.FindAllAsync()).Select(x => x.AsDTO());

            DataTable dtEmployee = new DataTable("EmployeesList");
            dtEmployee.Columns.AddRange(new DataColumn[8] 
            { 
                new DataColumn("ID"),
                new DataColumn("Name"),
                new DataColumn("Date of Birth"),
                new DataColumn("Address"),
                new DataColumn("Phone"),
                new DataColumn("Email"),
                new DataColumn("Created Time"),
                new DataColumn("Modified Time")
            });
            foreach (var item in Employeelist)
            {
                dtEmployee.Rows.Add(item.Id, item.Name, item.DateOfBirth, item.Address, item.Phone, item.Email, item.CreatedTime, item.ModifiedTime);
            }

            return dtEmployee;
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

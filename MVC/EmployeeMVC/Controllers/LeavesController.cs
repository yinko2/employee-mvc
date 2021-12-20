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
using System.Data;
using ClosedXML.Excel;
using Aspose.Pdf;

namespace EmployeeMVC.Controllers
{
    public class LeavesController : BaseController
    {
        public LeavesController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: Leaves
        public async Task<IActionResult> Index(string Eid, string EName, DateTime Date, string Reason)
        {
            try
            {
                var Leavelist = await _repositoryWrapper.Leave.LoadLeaveList(DateTime.MinValue, DateTime.MinValue);
                if (!string.IsNullOrEmpty(Eid) && int.TryParse(Eid, out int o))
                {
                    Leavelist = Leavelist.Where(s => s.EmployeeId == o);
                }
                if (!string.IsNullOrEmpty(EName))
                {
                    Leavelist = Leavelist.Where(s => s.EmployeeName!.Contains(EName));
                }
                if (Date != DateTime.MinValue)
                {
                    Leavelist = Leavelist.Where(s => s.Date == Date);
                }
                if (!string.IsNullOrEmpty(Reason))
                {
                    Leavelist = Leavelist.Where(s => s.Reason!.Contains(Reason));
                }
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

                int HolidayExistingId = await _repositoryWrapper.Holiday.FindExistingDate(leaveDTO.Date);

                if (HolidayExistingId > 0)
                {
                    ModelState.AddModelError("Date", "Do not apply leave on public holiday");
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

        public async Task<ActionResult> ExportToExcel()
        {
            DataTable dtLeave = await GetLeavesListAsync();

            using (XLWorkbook woekBook = new XLWorkbook())
            {
                woekBook.Worksheets.Add(dtLeave);
                using (MemoryStream stream = new MemoryStream())
                {
                    woekBook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LeavesList.xlsx");
                }
            }
        }

        public async Task<ActionResult> ExportToPdf()
        {
            DataTable dtLeave = await GetLeavesListAsync();

            if (dtLeave.Rows.Count > 0)
            {
                var document = new Document
                {
                    PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
                };
                var pdfPage = document.Pages.Add();
                Table table = new Table
                {
                    ColumnWidths = "14.5% 14.5% 14.5% 14.5% 14.5% 14.5% 14.5%",
                    DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                    Border = new BorderInfo(BorderSide.All, .5f, Color.Black),
                    DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Color.Black)
                };

                table.ImportDataTable(dtLeave, true, 0, 0);
                document.Pages[1].Paragraphs.Add(table);

                using (var stream = new MemoryStream())
                {
                    document.Save(stream);
                    return new FileContentResult(stream.ToArray(), "application/pdf")
                    {
                        FileDownloadName = "LeavesList.pdf"
                    };
                }
            }
            return View();
        }

        private async Task<DataTable> GetLeavesListAsync()
        {
            var Leavelist = await _repositoryWrapper.Leave.LoadLeaveList(DateTime.MinValue, DateTime.MinValue);

            DataTable dtLeave = new DataTable("LeavesList");
            dtLeave.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("ID"),
                new DataColumn("Employee ID"),
                new DataColumn("Employee Name"),
                new DataColumn("Date"),
                new DataColumn("Reason"),
                new DataColumn("Created Time"),
                new DataColumn("Modified Time")
            });
            foreach (var item in Leavelist)
            {
                dtLeave.Rows.Add(item.Id, item.EmployeeId, item.EmployeeName, item.Date, item.Reason, item.CreatedTime, item.ModifiedTime);
            }

            return dtLeave;
        }
    }
}

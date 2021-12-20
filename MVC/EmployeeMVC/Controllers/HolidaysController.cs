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
using System.Data;
using ClosedXML.Excel;
using Aspose.Pdf;

namespace EmployeeMVC.Controllers
{
    public class HolidaysController : BaseController
    {

        public HolidaysController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        // GET: Holidays
        public async Task<IActionResult> Index(DateTime Date, string Name)
        {
            
            try
            {
                var Holidaylist = await _repositoryWrapper.Holiday.FindAllAsync();
                if (Date != DateTime.MinValue)
                {
                    Holidaylist = Holidaylist.Where(s => s.Date == Date);
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    Holidaylist = Holidaylist.Where(s => s.Name!.Contains(Name));
                }
                return View(Holidaylist);
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("get Holidaylist fail", ex.Message);
                return BadRequest();
            }
        }

        // GET: Holidays/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var holiday = await _repositoryWrapper.Holiday.FindByIDAsync(id);

            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        // GET: Holidays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Holidays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Name")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _repositoryWrapper.Holiday.AnyByConditionAsync(x => x.Date == holiday.Date))
                    {
                        ModelState.AddModelError("Date", "Holiday with provided date already exists");
                        return View(holiday);
                    }
                    await _repositoryWrapper.Holiday.CreateAsync(holiday);
                    await _repositoryWrapper.Eventlog.Insert(holiday);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    if (await HolidayExists(holiday.Id))
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
            return View(holiday);
        }

        // GET: Holidays/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var holiday = await _repositoryWrapper.Holiday.FindByIDAsync(id);

            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        // POST: Holidays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name")] Holiday holiday)
        {
            if (id != holiday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int existingid = await _repositoryWrapper.Holiday.FindExistingDate(holiday.Date);
                    if (existingid > 0 &&  existingid != holiday.Id)
                    {
                        ModelState.AddModelError("Date", "Holiday with provided date already exists");
                        return View(holiday);
                    }
                    await _repositoryWrapper.Holiday.UpdateAsync(holiday);
                    await _repositoryWrapper.Eventlog.Update(holiday);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await HolidayExists(holiday.Id))
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
            return View(holiday);
        }

        // GET: Holidays/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var holiday = await _repositoryWrapper.Holiday.FindByIDAsync(id);

            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        // POST: Holidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var Holiday = await _repositoryWrapper.Holiday.FindByIDAsync(id);
                if (Holiday == null)
                {
                    return NotFound();
                }

                await _repositoryWrapper.Holiday.DeleteAsync(Holiday);
                await _repositoryWrapper.Eventlog.Delete(Holiday);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _repositoryWrapper.Eventlog.Error("delete fail", ex.Message);
                return BadRequest();
            }
        }

        public async Task<ActionResult> ExportToExcel()
        {
            DataTable dtHoliday = await GetHolidaysListAsync();

            using (XLWorkbook woekBook = new XLWorkbook())
            {
                woekBook.Worksheets.Add(dtHoliday);
                using (MemoryStream stream = new MemoryStream())
                {
                    woekBook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HolidaysList.xlsx");
                }
            }
        }

        public async Task<ActionResult> ExportToPdf()
        {
            DataTable dtHoliday = await GetHolidaysListAsync();

            if (dtHoliday.Rows.Count > 0)
            {
                var document = new Document
                {
                    PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
                };
                var pdfPage = document.Pages.Add();
                Table table = new Table
                {
                    ColumnWidths = "33% 33% 33%",
                    DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                    Border = new BorderInfo(BorderSide.All, .5f, Color.Black),
                    DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Color.Black)
                };

                table.ImportDataTable(dtHoliday, true, 0, 0);
                document.Pages[1].Paragraphs.Add(table);

                using (var stream = new MemoryStream())
                {
                    document.Save(stream);
                    return new FileContentResult(stream.ToArray(), "application/pdf")
                    {
                        FileDownloadName = "HolidaysList.pdf"
                    };
                }
            }
            return View();
        }

        private async Task<DataTable> GetHolidaysListAsync()
        {
            var Holidaylist = await _repositoryWrapper.Holiday.FindAllAsync();

            DataTable dtHoliday = new DataTable("HolidaysList");
            dtHoliday.Columns.AddRange(new DataColumn[3]
            {
                new DataColumn("ID"),
                new DataColumn("Date"),
                new DataColumn("Name")
            });
            foreach (var item in Holidaylist)
            {
                dtHoliday.Rows.Add(item.Id, item.Name, item.Date.ToShortDateString());
            }

            return dtHoliday;
        }

        private async Task<bool> HolidayExists(int id)
        {
            return await _repositoryWrapper.Holiday.IsExist(id);
        }
    }
}

using Batch_input_API.Data;
using Batch_input_API.DTOs;
using Batch_input_API.Models;
using Batch_input_API.ResponseDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace Batch_input_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BatchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/Create")]
        public async Task<IActionResult> Create_BatchInput(CreateBatchInputDto dto)
        {
            var history = new BatchInputHistory {
                TerminalID = dto.TerminalID,
                Machine = dto.Machine,
                Msnv = dto.msnv,
                Shift = dto.Shift,
                //isComplate = dto.isComplate,
                start = dto.Start
            };

            foreach (var po in dto.ListPO)
            {
                history.ListPOs.Add(new ListPO { Po = po });
            }

            _context.BatchInputHistories.Add(history);

            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpGet("/notComplate")]
        public async Task<IActionResult> Get_noteComplate_BatchInput()
        {
            var Data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .Where(x => x.isComplate == false)
                .Select(x => new BatchInputResponseDto
                {
                    Id = x.ID,
                    Machine = x.Machine,
                    TerminalID = x.TerminalID,
                    Msnv = x.Msnv,
                    Shift = x.Shift,
                    Start = x.start,
                    isComplate = x.isComplate,
                    DateCreated = x.DateCreated,
                    ListPO = x.ListPOs.Select(p=> new PODto
                    {
                        ID = p.ID,
                        IDGroup = p.IDGroup,
                        Po = p.Po,
                    }).ToList()
                }).ToListAsync();

            return Ok(Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll_BatchInput()
        {
            var Data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .Select(x => new BatchInputResponseDto
                {
                    Id = x.ID,
                    Machine = x.Machine,
                    TerminalID = x.TerminalID,
                    Msnv = x.Msnv,
                    Shift = x.Shift,
                    Start = x.start,
                    isComplate = x.isComplate,
                    DateCreated = x.DateCreated,
                    ListPO = x.ListPOs.Select(p => new PODto
                    {
                        ID = p.ID,
                        IDGroup = p.IDGroup,
                        Po = p.Po,
                    }).ToList()
                }).ToListAsync();

            return Ok(Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Update_BatchInput(int id)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .Where(x => x.ID == id)
                .Select(x => new BatchInputResponseDto {
                    Id = x.ID,
                    Machine = x.Machine,
                    TerminalID = x.TerminalID,
                    Msnv = x.Msnv,
                    Shift = x.Shift,
                    Start = x.start,
                    isComplate = x.isComplate,
                    ListPO = x.ListPOs.Select(p => new PODto
                    {
                        ID = p.ID,
                        IDGroup = p.IDGroup,
                        Po = p.Po,
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (data == null) return NotFound("Không tìm thấy dữ liệu");

            return Ok(data);
        }

        [HttpGet("GetCount")]
        public async Task<IActionResult> GetCountWeek()
        {
            var today = DateTime.Today;

            var startDate = today.AddDays(-6);
            var endDate = today.AddDays(1);

            var result = await (
                from bh in _context.BatchInputHistories
                join lp in _context.ListPOs
                    on bh.ID equals lp.IDGroup
                    into poGroup

                from lp in poGroup.DefaultIfEmpty()

                where bh.DateCreated >= startDate
                   && bh.DateCreated < endDate

                group lp by bh.DateCreated!.Value.Date
                into g

                orderby g.Key

                select new GetCountWeekDto
                {
                    RunDate = g.Key,
                    Count = g.Count(x => x != null)
                }
            ).ToListAsync();

            return Ok(result);

        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete_BatchInput(int id)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync(X => X.ID == id);
            if (data == null) return NotFound("khong tim thay du lieu");

            _context.BatchInputHistories.Remove(data);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Delete succses"});
                
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update_BatchInput(int id, bool isComplate, bool isStart)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm thấy dữ liệu");

            data.isComplate = isComplate;
            data.start = isStart;
            await _context.SaveChangesAsync();
            return Ok(new {message = "cap nhat thanh cong"});
        }

        
    }
}

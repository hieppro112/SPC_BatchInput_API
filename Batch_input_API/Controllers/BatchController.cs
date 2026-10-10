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

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
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

            var response = new BatchInputResponseDto
            {
                Id = history.ID,
                TerminalID = history.TerminalID,
                Machine = history.Machine,
                Start = history.start,
                Msnv = history.Msnv,
                Shift = history.Shift,
                isComplate = history.isComplate,
                DateCreated = history.DateCreated,

                ListPO = history.ListPOs
                .Select(x => new PODto
                {
                    ID = x.ID,
                    Po = x.Po,
                    IDGroup = x.IDGroup
                })
                .ToList()
            };

            return Ok(response);
        }


        /// <summary>
        /// GET DATA
        /// </summary>
        /// <returns></returns>
        [HttpGet("/notComplate")]
        public async Task<IActionResult> Get_noteComplate_BatchInput()
        {
            var delayTime = DateTime.Now.AddMinutes(-20);
            var Data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .Where(x => x.isComplate == false && x.isDelete == false && (x.numRetry==0|| x.RUNTIME <= delayTime)&& x.numRetry<=10)
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
                    description = x.description,
                    isDelete = x.isDelete,
                    numRetry = x.numRetry,
                    RunTime = x.RUNTIME,
                    ListPO = x.ListPOs.Select(p=> new PODto
                    {
                        ID = p.ID,
                        IDGroup = p.IDGroup,
                        Po = p.Po,
                        isComplate=p.isComplate??false,
                    }).ToList()
                }).ToListAsync();

            return Ok(Data);
        }


        /// <summary>
        /// GET ALL (phân trang)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll_BatchInput([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var query = _context.BatchInputHistories
                .AsNoTracking()
                .Where(x => !x.isDelete)
                .OrderByDescending(x => x.ID);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BatchInputResponseDto
                {
                    Id = x.ID,
                    Machine = x.Machine ?? "",
                    TerminalID = x.TerminalID ?? "",
                    Msnv = x.Msnv ?? "",
                    Shift = x.Shift,
                    Start = x.start,
                    isComplate = x.isComplate,
                    DateCreated = x.DateCreated,
                    description = x.description,
                    isDelete = x.isDelete,
                    numRetry = x.numRetry,
                    RunTime = x.RUNTIME,
                    ListPO = x.ListPOs.Select(p => new PODto
                    {
                        ID = p.ID,
                        IDGroup = p.IDGroup,
                        Po = p.Po ?? "",
                        isComplate = p.isComplate ?? false,
                        DesPO = p.DescriptionPO

                    }).ToList(),
                })
                .ToListAsync();

            var result = new PagedResultDto<BatchInputResponseDto>
            {
                Items = data,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Update_BatchInput(int id)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .Where(x => x.ID == id)
                .Select(x => new BatchInputResponseDto {
                    Id = x.ID,
                    Machine = x.Machine ?? "",
                    TerminalID = x.TerminalID ?? "",
                    Msnv = x.Msnv ?? "",
                    Shift = x.Shift,
                    Start = x.start,
                    isComplate = x.isComplate,
                    DateCreated=x.DateCreated,
                    description=x.description,
                    isDelete=x.isDelete,
                    numRetry=x.numRetry,
                    ListPO = x.ListPOs.Select(p => new PODto
                    {
                        ID = p.ID,
                        IDGroup = p.IDGroup,
                        Po = p.Po ?? "",
                        isComplate= p.isComplate??false,
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (data == null) return NotFound("Không tìm thấy dữ liệu");

            return Ok(data);
        }

        /// <summary>
        /// UPDATE
        /// </summary>
        /// 
        [HttpPut("Update/RunTime/{id}")]
        public async Task<IActionResult> Update_runtime(int id)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm thấy dữ liệu");
            data.RUNTIME = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        [HttpPut("Update/isComplate/{id}")]
        public async Task<IActionResult> Update_BatchInput_isComplate(int id, bool isComplate)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm thấy dữ liệu");

            data.isComplate = isComplate;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        [HttpPut("Update/isStart/{id}")]
        public async Task<IActionResult> Update_BatchInput_isStart(int id,bool isStart)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm th?y d? li?u");

            data.start = isStart;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        //cap nhat descriptin Batch
        [HttpPut("Update/Batch_descriptions/{id}")]
        public async Task<IActionResult> Update_Des_batch(int id, string description)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm th?y d? li?u");
            data.description = description;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        //cap nhat isErr Batch
        [HttpPut("Update/Item_Soft_Delete/{id}")]
        public async Task<IActionResult> Update_err_batch(int id, bool isErr)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm th?y d? li?u");
            data.isDelete = isErr;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        //cap nhat so lan retry Batch
        [HttpPut("Update/Batch_numTry/{id}")]
        public async Task<IActionResult> Update_retry_batch(int id)
        {
            var data = await _context.BatchInputHistories
                .Include(x => x.ListPOs)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm th?y d? li?u");
            data.numRetry += 1;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        //cap nhat trang thai PO
        [HttpPut("UpdatePO/{id}")]
        public async Task<IActionResult> Update_PO(int id, bool isComplate)
        {
            var data = await _context.ListPOs
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm th?y d? li?u");
            data.isComplate = isComplate;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        //cap nhat trang thai PO
        [HttpPut("UpdatePO/Description/{id}")]
        public async Task<IActionResult> Update_PODescrip(int id, string desPo)
        {
            var data = await _context.ListPOs
                .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm thấy dữ liệu");
            data.DescriptionPO =desPo ;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }


        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete_BatchInput(int id)
        {
            //var data = await _context.BatchInputHistories
            //    .Include(x => x.ListPOs)
            //    .Where(x => x.ID == id)
            //    .FirstOrDefaultAsync(X => X.ID == id);
            //if (data == null) return NotFound("khong tim thay du lieu");

            //_context.BatchInputHistories.Remove(data);
            //await _context.SaveChangesAsync();
            //return Ok(new {message = "Delete succses"});

            var data = await _context.BatchInputHistories
    .Include(x => x.ListPOs)
    .FirstOrDefaultAsync(x => x.ID == id);
            if (data == null) return NotFound("Không tìm th?y d? li?u");
            data.isDelete = true;
            await _context.SaveChangesAsync();
            return Ok(new { message = "cap nhat thanh cong" });
        }

        
    }
}

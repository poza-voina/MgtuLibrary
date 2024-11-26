using MgtuLibrary.Entities;
using MgtuLibrary.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace MgtuLibrary.Controllers;

public class ReaderGetBookDto
{
    public long? ReaderId { get; set; }
    public long? BookId { get; set; }
}

public class ReportDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public int NumberOfBooks { get; set; }
    public int NumberOfOverdueBooks { get; set; }
}

public class ReportResponseDto
{
    public IEnumerable<ReportDto> Reports { get; set; }
}

public class GiveAwayDto
{
    public long BookId { get; set; }
    public long ReaderId { get; set; }
}

[ApiController]
[Route("api/v1/reader")]
public class ReaderController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    public ReaderController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpPost("/readerGetBook")]
    public async Task<ActionResult> ReaderGetBook([FromBody] ReaderGetBookDto dto)
    {
        var reader = await _dbContext.Readers.FindAsync(dto.ReaderId);
        var book = await _dbContext.Books.FindAsync(dto.BookId);
        var loan = _dbContext.Set<LoanOfBook>().Where(x => x.Reader == reader && x.Book == book && x.DateReturn == null).FirstOrDefault();

        if (loan is { })
        {
            return Conflict();
        }

        if (reader is null)
        {
            return NotFound();
        }

        if (book is null)
        {
            return NotFound();
        }

        var loanOfBook = new LoanOfBook()
        {
            DateLoan = DateTime.UtcNow,
            Tenure = 10,
            Book = book,
            Reader = reader
        };

        _dbContext.Add(loanOfBook);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("/getReport")]
    public async Task<ActionResult> GetReport()
    {
        var loans = await _dbContext.Set<LoanOfBook>().Where(x => x.DateReturn == null).ToListAsync();
        loans.ForEach(x => x.CurrentTenure = (DateTime.UtcNow - x.DateLoan).Days);
        await _dbContext.SaveChangesAsync();

        var readers = await _dbContext.Set<Reader>()
            .Include(r => r.LoanOfBooks).ToListAsync();

        readers.ForEach(reader => reader.LoanOfBooks = reader.LoanOfBooks
                .Where(book => book.DateReturn == null)
                .ToList());

        var response = new ReportResponseDto
        {
            Reports = readers.Select(r => new ReportDto
            {
                Name = r.Name,
                LastName = r.LastName,
                NumberOfBooks = r.LoanOfBooks.Count,
                NumberOfOverdueBooks = r.LoanOfBooks.Count(book => book.CurrentTenure > book.Tenure)
            }).ToList()
        };

        return Ok(response);
    }

    [HttpPost("/giveAway")]
    public async Task<ActionResult> GiveAway([FromBody] GiveAwayDto dto)
    {
        var reader = await _dbContext.Set<Reader>().FirstOrDefaultAsync(x => x.Id == dto.ReaderId);
        var book = await _dbContext.Set<Book>().FirstOrDefaultAsync(x => x.Id == dto.BookId);

        var loan = await _dbContext.Set<LoanOfBook>().FirstOrDefaultAsync(x => x.Book == book && x.Reader == reader && x.DateReturn == null);

        if (loan is null)
        {
            return NotFound();
        }

        loan.DateReturn = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}

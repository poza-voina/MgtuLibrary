namespace MgtuLibrary.Entities;

public class Book
{
	public long Id { get; set; }

	public string Author { get; set; }
	public string NameBook { get; set; }
	public string Town { get; set; }

	public string Publisher { get; set; }

	public virtual ICollection<LoanOfBook> LoanOfBooks { get; set; } = new List<LoanOfBook>();
}

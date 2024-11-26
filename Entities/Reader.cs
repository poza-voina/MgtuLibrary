namespace MgtuLibrary.Entities;

public class Reader
{
	public long Id { get; set; }
	public string Name { get; set; }

	public string LastName { get; set; }

	public string Gender { get; set; }

	public string Group { get; set; }
	public virtual ICollection<LoanOfBook> LoanOfBooks { get; set; } = new List<LoanOfBook>();
}

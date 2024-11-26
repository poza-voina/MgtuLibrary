namespace MgtuLibrary.Entities;

public class LoanOfBook
{
	public int Id { get; set; }
	public DateTime DateLoan { get; set; }

	public DateTime? DateReturn { get; set; }

	public int Tenure { get; set; }
	public int? CurrentTenure { get; set; }
	public virtual Book Book { get; set; }
	public virtual Reader Reader { get; set; }

}

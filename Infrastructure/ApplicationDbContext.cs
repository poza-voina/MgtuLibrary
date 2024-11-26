using System.Text.Json;
using MgtuLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MgtuLibrary.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Reader> Readers => Set<Reader>();
    public DbSet<LoanOfBook> LoanOfBooks => Set<LoanOfBook>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<LoanOfBook>().HasOne(x => x.Book).WithMany(y => y.LoanOfBooks);

        modelBuilder
            .Entity<LoanOfBook>().HasOne(x => x.Reader).WithMany(y => y.LoanOfBooks);


        List<Reader> readers =
            [
                new Reader()
                {
                    Id = 1,
                    Name = "Валера",
                    LastName = "Петров",
                    Gender = "муж",
                    Group = "АВБ-21-2",
                },
                new Reader()
                {
                    Id = 2,
                    Name = "Семен",
                    LastName = "Сидоров",
                    Gender = "жен",
                    Group = "АВБ-21-1",
                },
                new Reader()
                {
                    Id = 3,
                    Name = "Валерия",
                    LastName = "Столешникова",
                    Gender = "жен",
                    Group = "АВб-20-2",
                },
            ];

        modelBuilder.Entity<Reader>().HasData(readers);


        List<Book> books =
            [
                new Book() 
                {
                    Id = 1,
                    Author = "А.С. Пушкин",
                    NameBook = "Капитанская дочка",
                    Town = "Москва",
                    Publisher =  "Publisher1"
                },
                new Book()
                {
                    Id = 2,
                    Author = "Есенин С.А.",
                    NameBook = "Черный человек",
                    Town = "Москва",
                    Publisher =  "Publisher1"
                },
                new Book()
                {
                    Id = 3,
                    Author = "Иван Тургенев",
                    NameBook = "Муму",
                    Town = "Орел",
                    Publisher =  "Publisher1"
                },
            ];

        modelBuilder.Entity<Book>().HasData(books);

    }
}
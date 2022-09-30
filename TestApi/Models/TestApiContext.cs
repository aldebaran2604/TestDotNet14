using Microsoft.EntityFrameworkCore;

namespace TestApi.Models;

public class TestApiContext : DbContext
{
    #region DbSet Properties

    internal DbSet<Humano>? humanos { get; set; }

    #endregion DbSet Properties

    #region Constructors

    public TestApiContext(DbContextOptions<TestApiContext> options) : base(options)
    {

    }

    #endregion Constructors
}
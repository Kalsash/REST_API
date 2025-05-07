using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace REST_API.Models.Contexts;

public partial class MoviesContext : DbContext
{
    public MoviesContext()
    { }

    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    { }


    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Keyword> Keywords { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LanguageRole> LanguageRoles { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieCast> MovieCasts { get; set; }

    public virtual DbSet<MovieCompany> MovieCompanies { get; set; }

    public virtual DbSet<MovieCrew> MovieCrews { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<MovieKeyword> MovieKeywords { get; set; }

    public virtual DbSet<MovieLanguage> MovieLanguages { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<ProductionCompany> ProductionCompanies { get; set; }

    public virtual DbSet<ProductionCountry> ProductionCountries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("DataSource=Database\\\\movies.db;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("country");

            entity.Property(e => e.CountryId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("country_id");
            entity.Property(e => e.CountryIsoCode).HasColumnName("country_iso_code");
            entity.Property(e => e.CountryName).HasColumnName("country_name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("department");
            entity.HasKey(e => e.DepartmentId);

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("department_id");
            entity.Property(e => e.DepartmentName).HasColumnName("department_name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.ToTable("gender");

            entity.Property(e => e.GenderId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("gender_id");
            entity.Property(e => e.Gender1).HasColumnName("gender");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("genre");

            entity.Property(e => e.GenreId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("genre_id");
            entity.Property(e => e.GenreName).HasColumnName("genre_name");
        });

        modelBuilder.Entity<Keyword>(entity =>
        {
            entity.ToTable("keyword");

            entity.Property(e => e.KeywordId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("keyword_id");
            entity.Property(e => e.KeywordName)
                .HasColumnType("varchar(100)")
                .HasColumnName("keyword_name");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("language");

            entity.Property(e => e.LanguageId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("language_id");
            entity.Property(e => e.LanguageCode).HasColumnName("language_code");
            entity.Property(e => e.LanguageName).HasColumnName("language_name");
        });

        modelBuilder.Entity<LanguageRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("language_role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("role_id");
            entity.Property(e => e.LanguageRole1).HasColumnName("language_role");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("movie");
            entity.HasKey(e => e.MovieId);

            entity.Property(e => e.MovieId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("movie_id");
            entity.Property(e => e.Budget)
                .HasColumnType("INT")
                .HasColumnName("budget");
            entity.Property(e => e.Homepage).HasColumnName("homepage");
            entity.Property(e => e.MovieStatus).HasColumnName("movie_status");
            entity.Property(e => e.Overview).HasColumnName("overview");
            entity.Property(e => e.Popularity).HasColumnName("popularity");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("DATE")
                .HasColumnName("release_date");
            entity.Property(e => e.Revenue)
                .HasColumnType("INT")
                .HasColumnName("revenue");
            entity.Property(e => e.Runtime)
                .HasColumnType("INT")
                .HasColumnName("runtime");
            entity.Property(e => e.Tagline).HasColumnName("tagline");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.VoteAverage).HasColumnName("vote_average");
            entity.Property(e => e.VoteCount)
                .HasColumnType("INT")
                .HasColumnName("vote_count");
        });

        modelBuilder.Entity<MovieCast>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("movie_cast");

            entity.Property(e => e.CastOrder)
                .HasColumnType("INT")
                .HasColumnName("cast_order");
            entity.Property(e => e.CharacterName).HasColumnName("character_name");
            entity.Property(e => e.GenderId)
                .HasColumnType("INT")
                .HasColumnName("gender_id");
            entity.Property(e => e.MovieId)
                .HasColumnType("INT")
                .HasColumnName("movie_id");
            entity.Property(e => e.PersonId)
                .HasColumnType("INT")
                .HasColumnName("person_id");

            entity.HasOne(d => d.Gender).WithMany().HasForeignKey(d => d.GenderId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);

            entity.HasOne(d => d.Person).WithMany().HasForeignKey(d => d.PersonId);
        });

        modelBuilder.Entity<MovieCompany>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("movie_company");

            entity.Property(e => e.CompanyId)
                .HasColumnType("INT")
                .HasColumnName("company_id");
            entity.Property(e => e.MovieId)
                .HasColumnType("INT")
                .HasColumnName("movie_id");

            entity.HasOne(d => d.Company).WithMany().HasForeignKey(d => d.CompanyId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);
        });

        modelBuilder.Entity<MovieCrew>(entity =>
        {
            entity.ToTable("movie_crew");
            entity.HasKey(e => new
            {
                e.MovieId,
                e.PersonId,
                e.DepartmentId
            });

            entity.Property(e => e.DepartmentId)
                .HasColumnType("INT")
                .HasColumnName("department_id");
            entity.Property(e => e.Job).HasColumnName("job");
            entity.Property(e => e.MovieId)
                .HasColumnType("INT")
                .HasColumnName("movie_id");
            entity.Property(e => e.PersonId)
                .HasColumnType("INT")
                .HasColumnName("person_id");

            entity.HasOne(d => d.Department).WithMany().HasForeignKey(d => d.DepartmentId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);

            entity.HasOne(d => d.Person).WithMany().HasForeignKey(d => d.PersonId);
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("movie_genres");

            entity.Property(e => e.GenreId)
                .HasColumnType("INT")
                .HasColumnName("genre_id");
            entity.Property(e => e.MovieId)
                .HasColumnType("INT")
                .HasColumnName("movie_id");

            entity.HasOne(d => d.Genre).WithMany().HasForeignKey(d => d.GenreId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);
        });

        modelBuilder.Entity<MovieKeyword>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("movie_keywords");

            entity.Property(e => e.KeywordId)
                .HasColumnType("INT")
                .HasColumnName("keyword_id");
            entity.Property(e => e.MovieId)
                .HasColumnType("INT")
                .HasColumnName("movie_id");

            entity.HasOne(d => d.Keyword).WithMany().HasForeignKey(d => d.KeywordId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);
        });

        modelBuilder.Entity<MovieLanguage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("movie_languages");

            entity.Property(e => e.LanguageId)
                .HasColumnType("INT")
                .HasColumnName("language_id");
            entity.Property(e => e.LanguageRoleId)
                .HasColumnType("INT")
                .HasColumnName("language_role_id");
            entity.Property(e => e.MovieId)
                .HasColumnType("INT")
                .HasColumnName("movie_id");

            entity.HasOne(d => d.Language).WithMany().HasForeignKey(d => d.LanguageId);

            entity.HasOne(d => d.LanguageRole).WithMany().HasForeignKey(d => d.LanguageRoleId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("person");
            entity.HasKey(e => e.PersonId);
            entity.Property(e => e.PersonId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("person_id");
            entity.Property(e => e.PersonName).HasColumnName("person_name");
        });

        modelBuilder.Entity<ProductionCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId);

            entity.ToTable("production_company");

            entity.Property(e => e.CompanyId)
                .ValueGeneratedNever()
                .HasColumnType("INT")
                .HasColumnName("company_id");
            entity.Property(e => e.CompanyName).HasColumnName("company_name");
        });

        modelBuilder.Entity<ProductionCountry>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("production_country");

            entity.Property(e => e.CountryId)
                .HasColumnType("INT")
                .HasColumnName("country_id");
            entity.Property(e => e.MovieId)
                .HasColumnType("INT")
                .HasColumnName("movie_id");

            entity.HasOne(d => d.Country).WithMany().HasForeignKey(d => d.CountryId);

            entity.HasOne(d => d.Movie).WithMany().HasForeignKey(d => d.MovieId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

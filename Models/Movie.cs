using System;
using System.Collections.Generic;

namespace REST_API.Models;

public partial class Movie
{
    public long MovieId { get; set; }

    public string? Title { get; set; }

    public long? Budget { get; set; }

    public string? Homepage { get; set; }

    public string? Overview { get; set; }

    public double? Popularity { get; set; }

    public string? ReleaseDate { get; set; }

    public long? Revenue { get; set; }

    public long? Runtime { get; set; }

    public string? MovieStatus { get; set; }

    public string? Tagline { get; set; }

    public double? VoteAverage { get; set; }

    public long? VoteCount { get; set; }
}

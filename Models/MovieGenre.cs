using System;
using System.Collections.Generic;

namespace REST_API.Models;

public partial class MovieGenre
{
    public long? MovieId { get; set; }

    public long? GenreId { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Movie? Movie { get; set; }
}

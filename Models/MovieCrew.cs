using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace REST_API.Models;


public partial class MovieCrew
{
    
    public long? MovieId { get; set; }
    
    public long? PersonId { get; set; }
    
    public long? DepartmentId { get; set; }

    public string? Job { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual Person? Person { get; set; }
}

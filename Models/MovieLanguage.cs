using System;
using System.Collections.Generic;

namespace REST_API.Models;

public partial class MovieLanguage
{
    public long? MovieId { get; set; }

    public long? LanguageId { get; set; }

    public long? LanguageRoleId { get; set; }

    public virtual Language? Language { get; set; }

    public virtual LanguageRole? LanguageRole { get; set; }

    public virtual Movie? Movie { get; set; }
}

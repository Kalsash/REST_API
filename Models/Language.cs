﻿using System;
using System.Collections.Generic;

namespace REST_API.Models;

public partial class Language
{
    public long LanguageId { get; set; }

    public string? LanguageCode { get; set; }

    public string? LanguageName { get; set; }
}

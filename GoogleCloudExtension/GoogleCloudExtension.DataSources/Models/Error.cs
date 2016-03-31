﻿// Copyright 2015 Google Inc. All Rights Reserved.
// Licensed under the Apache License Version 2.0.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleCloudExtension.DataSources.Models
{
    public class Error
    {
        [JsonProperty("errors")]
        public IList<ErrorEntry> Errors { get; set; }
    }
}
﻿// Copyright 2015 Google Inc. All Rights Reserved.
// Licensed under the Apache License Version 2.0.

using AspnetTool.Commands;
using CommandLine;
using CommandLine.Text;

namespace AspnetTool
{
    internal class Options
    {
        [VerbOption("deploy", HelpText = "Deploys the app")]
        public DeployCmd.Options Deploy { get; set; } = new DeployCmd.Options();

        [VerbOption("list", HelpText = "List the ASP.NET instances.")]
        public ListCmd.Options List { get; set; } = new ListCmd.Options();

        [VerbOption("generate", HelpText = "Generate a .publishsettings file for the given instance.")]
        public GenerateCmd.Options Generate { get; set; } = new GenerateCmd.Options();

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}
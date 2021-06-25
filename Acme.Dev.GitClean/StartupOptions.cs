// -----------------------------------------------------------------------
//  <copyright file="StartupOptions.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Dev.GitClean
{
    using System;
    using System.Linq;

    using CommandLine;

    public class StartupOptions
    {
        /// <summary>
        /// Gets or sets the MainBranchName.
        /// </summary>
        /// <value>The MainBranchName.</value>
        [Option('m', "main", Required = false, HelpText = "The name of the main branch.")]
        public string MainBranchName { get; set; } = "main";

        /// <summary>
        /// Gets or sets the DevelopBranchName.
        /// </summary>
        /// <value>The DevelopBranchName.</value>
        [Option('d', "develop", Required = false, HelpText = "The name of the develop branch.")]
        public string DevelopBranchName { get; set; } = "develop";
    }
}
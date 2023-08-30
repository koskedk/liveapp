// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Reflection;
using SqlMigrationAssembly = LiveApp.Identity.Admin.EntityFramework.SqlServer.Helpers.MigrationAssembly;

namespace LiveApp.Identity.Admin.Configuration.Database
{
    public static class MigrationAssemblyConfiguration
    {
        public static string GetMigrationAssemblyByProvider()
        {
            return typeof(SqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name;
        }
    }
}








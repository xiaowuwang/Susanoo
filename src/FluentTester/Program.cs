﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Susanoo;

namespace FluentTester
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandManager.RegisterDatabaseManagerFactory((connectionStringName) =>
                new DatabaseManager(System.Data.SqlClient.SqlClientFactory.Instance, connectionStringName));

            var command = CommandManager.DefineCommand("SELECT TOP 1 * FROM SimpleTable;", System.Data.CommandType.Text)
                .DefineResults<SimpleTable>()
                .ForResults((mapping) =>
                {
                    mapping.ForProperty(result => result.Id, prop => prop.UseAlias("id"));
                    mapping.ForProperty(result => result.Data, prop => prop.UseAlias("data"));
                    mapping.ForProperty(result => result.Date, prop => prop.UseAlias("date"));
                }).Finalize();

            CommandManager.BuildDatabaseManager("test").Execute(command);

            Console.ReadLine();
        }
    }
}
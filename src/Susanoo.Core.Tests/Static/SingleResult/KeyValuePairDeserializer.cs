﻿#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;
using Susanoo.Pipeline.Command;
using Susanoo.Pipeline.Command.ResultSets.Processing;

#endregion

namespace Susanoo.Tests.Static.SingleResult
{
    [Category("Type Resolution")]
    [TestFixture]
    public class KeyValuePairTests
    {
        private readonly DatabaseManager _databaseManager = Setup.DatabaseManager;

        [Test(Description = "Tests that results correctly map data to CLR types.")]
        public void KeyValuePairMap()
        {
            var results = CommandManager.DefineCommand("SELECT  Int, String FROM #DataTypeTable;", CommandType.Text)
                .DefineResults<KeyValuePair<int, string>>()
                .ForResults(expression =>
                {
                    expression.ForProperty(pair => pair.Key, configuration => configuration.UseAlias("Int"));
                    expression.ForProperty(pair => pair.Value, configuration => configuration.UseAlias("String"));
                })
                .Realize()
                .Execute(_databaseManager);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count(), 1);

            var first = results.First();

            Assert.AreEqual(first.Key, 1);
            Assert.AreEqual(first.Value, "varchar");
        }

        [Test(Description = "Tests that results correctly map data to CLR types.")]
        public void KeyValuePairMapReverse()
        {
            var results = CommandManager.DefineCommand("SELECT  String, Int FROM #DataTypeTable;", CommandType.Text)
                .DefineResults<KeyValuePair<int, string>>()
                .ForResults(expression =>
                {
                    expression.ForProperty(pair => pair.Key, configuration => configuration.UseAlias("Int"));
                    expression.ForProperty(pair => pair.Value, configuration => configuration.UseAlias("String"));
                })
                .Realize()
                .Execute(_databaseManager);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count(), 1);

            var first = results.First();

            Assert.AreEqual(first.Key, 1);
            Assert.AreEqual(first.Value, "varchar");
        }

        [Test(Description = "Tests that results correctly map data to CLR types.")]
        public void KeyValuePairMapStringCoercion()
        {
            var results = CommandManager.DefineCommand("SELECT Int, String FROM #DataTypeTable;", CommandType.Text)
                .DefineResults<KeyValuePair<string, string>>()
                .ForResults(expression =>
                {
                    expression.ForProperty(pair => pair.Key, configuration => configuration.UseAlias("Int"));
                    expression.ForProperty(pair => pair.Value, configuration => configuration.UseAlias("String"));
                })
                .Realize()
                .Execute(_databaseManager);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count(), 1);

            var first = results.First();

            Assert.AreEqual(first.Key, "1");
            Assert.AreEqual(first.Value, "varchar");
        }

        [Test(Description = "Tests that results correctly map data to CLR types.")]
        public void KeyValueWithWhereFilter()
        {
            var results = CommandManager.DefineCommand<KeyValuePair<string, string>>("SELECT Int, String FROM #DataTypeTable", CommandType.Text)
                .IncludeProperty(o => o.Key, parameter => parameter.ParameterName = "Int")
                .IncludeProperty(o => o.Value, parameter => parameter.ParameterName = "String")
                .SendNullValues(NullValueMode.FilterOnlyFull)
                .DefineResults<KeyValuePair<string, string>>()
                .ForResults(expression =>
                {
                    expression.ForProperty(pair => pair.Key, 
                        configuration => configuration.UseAlias("Int"));
                    expression.ForProperty(pair => pair.Value,
                        configuration => configuration.UseAlias("String"));
                })
                .BuildWhereFilter(new
                {
                    Key = Comparison.Ignore//,
                    //Value = Comparison.Override("({0} IS NULL OR UPPER({1}) LIKE '%' + UPPER({0}))")
                })
                .Realize()
                .Execute(_databaseManager, new KeyValuePair<string, string>(null, "C"));

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var first = results.First();

            Assert.AreEqual(first.Key, "1");
            Assert.AreEqual(first.Value, "varchar");
        }
    }
}
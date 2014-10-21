﻿#region

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Susanoo
{
    /// <summary>
    ///     Shared members for all command processors.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    public interface ICommandProcessorInterop<TFilter> : IFluentPipelineFragment
    {
        /// <summary>
        ///     Gets the command expression.
        /// </summary>
        /// <value>The command expression.</value>
        ICommandExpression<TFilter> CommandExpression { get; }
    }

    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with no mapping expressions and a filter
    ///     parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    public interface ICommandProcessor<TFilter> : ICommandProcessorInterop<TFilter>
#if !NETFX40
        , ICommandProcessorAsync<TFilter>
#endif
    {
        /// <summary>
        ///     Executes the command and retrieves a single value.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>TReturn.</returns>
        TReturn ExecuteScalar<TReturn>(IDatabaseManager databaseManager, TFilter filter,
            params DbParameter[] explicitParameters);

        /// <summary>
        ///     Executes the command and retrieves a single value.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>TReturn.</returns>
        TReturn ExecuteScalar<TReturn>(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Executes the command and returns a return code.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>System.Int32.</returns>
        int ExecuteNonQuery(IDatabaseManager databaseManager, TFilter filter, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Executes the command and returns a return code.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>System.Int32.</returns>
        int ExecuteNonQuery(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }

#if !NETFX40
    public interface ICommandProcessorAsync<TFilter>
    {
        /// <summary>
        ///     Executes the scalar action asynchronously.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;TReturn&gt;.</returns>
        Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            TFilter filter, CancellationToken cancellationToken, params DbParameter[] explicitParameters);
    }

    public interface ICommandProcessorAsync<TFilter, TResult> where TResult : new()
    {
        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;IEnumerable&lt;TResult&gt;&gt;.</returns>
        Task<IEnumerable<TResult>> ExecuteAsync(IDatabaseManager databaseManager, CancellationToken cancellationToken,
            params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;IEnumerable&lt;TResult&gt;&gt;.</returns>
        Task<IEnumerable<TResult>> ExecuteAsync(IDatabaseManager databaseManager,
            params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;IEnumerable&lt;TResult&gt;&gt;.</returns>
        Task<IEnumerable<TResult>> ExecuteAsync(IDatabaseManager databaseManager,
            TFilter filter, CancellationToken cancellationToken, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;IEnumerable&lt;TResult&gt;&gt;.</returns>
        Task<IEnumerable<TResult>> ExecuteAsync(IDatabaseManager databaseManager,
            TFilter filter, params DbParameter[] explicitParameters);
    }
#endif
    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled
    ///     and a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled at the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult>
        : ICommandProcessorInterop<TFilter>
#if !NETFX40
        , ICommandProcessorAsync<TFilter, TResult>
#endif
        where TResult : new()
    {
        /// <summary>
        ///     Gets the command result expression.
        /// </summary>
        /// <value>The command result expression.</value>
        ICommandResultExpression<TFilter, TResult> CommandResultExpression { get; }

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        IEnumerable<TResult> Execute(IDatabaseManager databaseManager, TFilter filter,
            params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        IEnumerable<TResult> Execute(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }

    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled
    ///     and a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled at the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult1, TResult2>
        : ICommandProcessorInterop<TFilter>
        where TResult1 : new()
        where TResult2 : new()
    {
        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;
        ///     &gt;.
        /// </returns>
        Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>>
            Execute(IDatabaseManager databaseManager, TFilter filter, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;
        ///     &gt;.
        /// </returns>
        Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>>
            Execute(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }

    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled
    ///     and a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled at the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult1, TResult2, TResult3>
        : ICommandProcessorInterop<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
    {
        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;
        ///     &gt;.
        /// </returns>
        Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>>
            Execute(IDatabaseManager databaseManager, TFilter filter, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;
        ///     &gt;.
        /// </returns>
        Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>>
            Execute(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }

    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled
    ///     and a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled at the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4>
        : ICommandProcessorInterop<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
    {
        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;
        ///     &gt;.
        /// </returns>
        Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>>
            Execute(IDatabaseManager databaseManager, TFilter filter, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;
        ///     &gt;.
        /// </returns>
        Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>>
            Execute(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }

    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled
    ///     and a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    /// <typeparam name="TResult5">The type of the 5th result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled at the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5>
        : ICommandProcessorInterop<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
        where TResult5 : new()
    {
        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;,
        ///     IEnumerable&lt;TResult5&gt;
        ///     &gt;.
        /// </returns>
        Tuple
            <IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>,
                IEnumerable<TResult5>>
            Execute(IDatabaseManager databaseManager, TFilter filter, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;,
        ///     IEnumerable&lt;TResult5&gt;
        ///     &gt;.
        /// </returns>
        Tuple
            <IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>,
                IEnumerable<TResult5>>
            Execute(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }

    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled
    ///     and a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    /// <typeparam name="TResult5">The type of the 5th result.</typeparam>
    /// <typeparam name="TResult6">The type of the 6th result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled at the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>
        : ICommandProcessorInterop<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
        where TResult5 : new()
        where TResult6 : new()
    {
        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;,
        ///     IEnumerable&lt;TResult5&gt;,
        ///     IEnumerable&lt;TResult6&gt;
        ///     &gt;.
        /// </returns>
        Tuple
            <IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>,
                IEnumerable<TResult5>, IEnumerable<TResult6>>
            Execute(IDatabaseManager databaseManager, TFilter filter, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;,
        ///     IEnumerable&lt;TResult5&gt;,
        ///     IEnumerable&lt;TResult6&gt;
        ///     &gt;.
        /// </returns>
        Tuple
            <IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>,
                IEnumerable<TResult5>, IEnumerable<TResult6>>
            Execute(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }

    /// <summary>
    ///     Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled
    ///     and a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult1">The type of the 1st result.</typeparam>
    /// <typeparam name="TResult2">The type of the 2nd result.</typeparam>
    /// <typeparam name="TResult3">The type of the 3rd result.</typeparam>
    /// <typeparam name="TResult4">The type of the 4th result.</typeparam>
    /// <typeparam name="TResult5">The type of the 5th result.</typeparam>
    /// <typeparam name="TResult6">The type of the 6th result.</typeparam>
    /// <typeparam name="TResult7">The type of the 7th result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled at the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
        : ICommandProcessorInterop<TFilter>
        where TResult1 : new()
        where TResult2 : new()
        where TResult3 : new()
        where TResult4 : new()
        where TResult5 : new()
        where TResult6 : new()
        where TResult7 : new()
    {
        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;,
        ///     IEnumerable&lt;TResult5&gt;,
        ///     IEnumerable&lt;TResult6&gt;,
        ///     IEnumerable&lt;TResult7&gt;
        ///     &gt;.
        /// </returns>
        Tuple
            <IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>,
                IEnumerable<TResult5>, IEnumerable<TResult6>, IEnumerable<TResult7>>
            Execute(IDatabaseManager databaseManager, TFilter filter, params DbParameter[] explicitParameters);

        /// <summary>
        ///     Assembles a data command for an ADO.NET provider,
        ///     executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>
        ///     Tuple&lt;
        ///     IEnumerable&lt;TResult1&gt;,
        ///     IEnumerable&lt;TResult2&gt;,
        ///     IEnumerable&lt;TResult3&gt;,
        ///     IEnumerable&lt;TResult4&gt;,
        ///     IEnumerable&lt;TResult5&gt;,
        ///     IEnumerable&lt;TResult6&gt;,
        ///     IEnumerable&lt;TResult7&gt;
        ///     &gt;.
        /// </returns>
        Tuple
            <IEnumerable<TResult1>, IEnumerable<TResult2>, IEnumerable<TResult3>, IEnumerable<TResult4>,
                IEnumerable<TResult5>, IEnumerable<TResult6>, IEnumerable<TResult7>>
            Execute(IDatabaseManager databaseManager, params DbParameter[] explicitParameters);
    }
}
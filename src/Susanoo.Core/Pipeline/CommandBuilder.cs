﻿#region

using System.Data;
using Susanoo.Command;

#endregion

namespace Susanoo.Pipeline
{
    /// <summary>
    /// Provides an entry point to defining commands and therein entering the Susanoo command Fluent API.
    /// </summary>
    public class CommandBuilder : ICommandBuilder
    {
        /// <summary>
        /// Begins the CommandBuilder definition process using a Fluent API implementation, move to next step with DefineResults on
        /// the result of this call.
        /// </summary>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="commandText">The CommandBuilder text.</param>
        /// <param name="commandType">Type of the CommandBuilder.</param>
        /// <returns>ICommandExpression&lt;TFilter, TResult&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">commandText</exception>
        /// <exception cref="System.ArgumentException">No CommandBuilder text provided.;commandText
        /// or
        /// TableDirect is not supported.;commandType</exception>
        public virtual ICommandExpression<TFilter> DefineCommand<TFilter>(string commandText, CommandType commandType)
        {
            return new CommandExpression<TFilter>(commandText, commandType);
        }

        /// <summary>
        /// Begins the CommandBuilder definition process using a Fluent API implementation, move to next step with DefineResults on
        /// the result of this call.
        /// </summary>
        /// <param name="commandText">The CommandBuilder text.</param>
        /// <param name="commandType">Type of the CommandBuilder.</param>
        /// <returns>ICommandExpression&lt;TFilter, TResult&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">commandText</exception>
        /// <exception cref="System.ArgumentException">No CommandBuilder text provided.;commandText
        /// or
        /// TableDirect is not supported.;commandType</exception>
        public virtual ICommandExpression<dynamic> DefineCommand(string commandText, CommandType commandType)
        {
            return new CommandExpression<dynamic>(commandText, commandType);
        }
    }
}
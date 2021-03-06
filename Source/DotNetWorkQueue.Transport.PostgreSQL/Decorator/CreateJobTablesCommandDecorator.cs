﻿using DotNetWorkQueue.Exceptions;
using DotNetWorkQueue.Transport.RelationalDatabase;
using DotNetWorkQueue.Transport.RelationalDatabase.Basic.Command;
using DotNetWorkQueue.Validation;
using Npgsql;

namespace DotNetWorkQueue.Transport.PostgreSQL.Decorator
{
    internal class CreateJobTablesCommandDecorator : ICommandHandlerWithOutput<CreateJobTablesCommand<ITable>, QueueCreationResult>
    {
        private readonly ICommandHandlerWithOutput<CreateJobTablesCommand<ITable>, QueueCreationResult> _decorated;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateJobTablesCommandDecorator"/> class.
        /// </summary>
        /// <param name="decorated">The decorated.</param>
        public CreateJobTablesCommandDecorator(ICommandHandlerWithOutput<CreateJobTablesCommand<ITable>, QueueCreationResult> decorated)
        {
            Guard.NotNull(() => decorated, decorated);
            _decorated = decorated;
        }
        public QueueCreationResult Handle(CreateJobTablesCommand<ITable> command)
        {
            try
            {
                return _decorated.Handle(command);
            }
            //if the queue already exists, return that status; otherwise, bubble the error
            catch (PostgresException error)
            {
                if (error.SqlState == "42P07" || error.SqlState == "42710")
                {
                    return new QueueCreationResult(QueueCreationStatus.AttemptedToCreateAlreadyExists);
                }
                throw new DotNetWorkQueueException($"Failed to create job table(s). SQL script was {error.Statement}",
                    error);
            }
        }
    }
}

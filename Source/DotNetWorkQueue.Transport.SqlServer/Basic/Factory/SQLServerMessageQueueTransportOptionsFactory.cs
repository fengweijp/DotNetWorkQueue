﻿using DotNetWorkQueue.Transport.RelationalDatabase;
using DotNetWorkQueue.Transport.RelationalDatabase.Basic.Query;
using DotNetWorkQueue.Validation;

namespace DotNetWorkQueue.Transport.SqlServer.Basic.Factory
{
    /// <summary>
    /// Creates new instance of the options classes
    /// </summary>
    internal class SqlServerMessageQueueTransportOptionsFactory : ISqlServerMessageQueueTransportOptionsFactory
    {
        private readonly IQueryHandler<GetQueueOptionsQuery<SqlServerMessageQueueTransportOptions>, SqlServerMessageQueueTransportOptions> _queryOptions;
        private readonly IConnectionInformation _connectionInformation;
        private readonly object _creator = new object();
        private SqlServerMessageQueueTransportOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerMessageQueueTransportOptionsFactory"/> class.
        /// </summary>
        /// <param name="connectionInformation">The connection information.</param>
        /// <param name="queryOptions">The query options.</param>
        public SqlServerMessageQueueTransportOptionsFactory(IConnectionInformation connectionInformation,
            IQueryHandler<GetQueueOptionsQuery<SqlServerMessageQueueTransportOptions>, SqlServerMessageQueueTransportOptions> queryOptions)
        {
            Guard.NotNull(() => queryOptions, queryOptions);
            Guard.NotNull(() => connectionInformation, connectionInformation);

            _queryOptions = queryOptions;
            _connectionInformation = connectionInformation;
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <returns></returns>
        public SqlServerMessageQueueTransportOptions Create()
        {
            if (string.IsNullOrEmpty(_connectionInformation.ConnectionString))
            {
                return new SqlServerMessageQueueTransportOptions();
            }

            if (_options != null) return _options;
            lock (_creator)
            {
                if (_options == null)
                {
                    _options = _queryOptions.Handle(new GetQueueOptionsQuery<SqlServerMessageQueueTransportOptions>());
                }
                if (_options == null) //does not exist in DB; return a new copy
                {
                    _options = new SqlServerMessageQueueTransportOptions();
                }
            }
            return _options;
        }
    }
}

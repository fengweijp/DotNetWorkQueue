﻿using DotNetWorkQueue.Transport.RelationalDatabase.Basic.Query;
using Xunit;

namespace DotNetWorkQueue.Transport.RelationalDatabase.Tests.Basic.Query
{
    public class GetErrorRetryCountQueryTests
    {
        [Fact]
        public void Create_Default()
        {
            var test = new GetErrorRetryCountQuery("test", 100);
            Assert.Equal("test", test.ExceptionType);
            Assert.Equal(100, test.QueueId);
        }
    }
}

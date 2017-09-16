﻿// ---------------------------------------------------------------------
//This file is part of DotNetWorkQueue
//Copyright © 2017 Brian Lehnen
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// ---------------------------------------------------------------------

using DotNetWorkQueue.Configuration;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Xunit;

namespace DotNetWorkQueue.Tests.Configuration
{
    public class QueueConsumerConfigurationTests
    {
        [Fact]
        public void Test_DefaultNotReadOnly()
        {
            var configuration = GetConfiguration();
            Assert.False(configuration.IsReadOnly);
        }
        [Fact]
        public void Set_Readonly()
        {
            var configuration = GetConfiguration();
            configuration.SetReadOnly();
            Assert.True(configuration.IsReadOnly);
        }
        [Fact]
        public void Set_Readonly_SetsMessageExpiration()
        {
            var configuration = GetConfiguration();
            configuration.SetReadOnly();

            configuration.MessageExpiration.Received(1).SetReadOnly();
        }
        [Fact]
        public void Set_Readonly_SetsHeartBeat()
        {
            var configuration = GetConfiguration();
            configuration.SetReadOnly();

            configuration.HeartBeat.Received(1).SetReadOnly();
        }
        [Fact]
        public void Set_Readonly_SetsWorker()
        {
            var configuration = GetConfiguration();
            configuration.SetReadOnly();

            configuration.Worker.Received(1).SetReadOnly();
        }
        [Fact]
        public void Set_Readonly_SetsTransportConfiguration()
        {
            var configuration = GetConfiguration();
            configuration.SetReadOnly();
            Assert.True(configuration.TransportConfiguration.IsReadOnly);
        }
        private QueueConsumerConfiguration GetConfiguration()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            return fixture.Create<QueueConsumerConfiguration>();
        }
    }
}

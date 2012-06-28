//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Security.Principal;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;

namespace Tests.IctCommonRemoting.Server
{
    /// sample implementation
    public class TClientDomainManager : TClientDomainManagerBase
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TClientDomainManager(String AClientID,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            ISystemDefaultsCache ASystemDefaultsCacheRef,
            ICacheableTablesManager ACacheableTablesManagerRef,
            IPrincipal AUserInfo) :
            base(AClientID,
                AClientServerConnectionType, AClientManagerRef,
                "007")
        {
        }

        /// empty implementation
        public void EstablishDBConnection()
        {
        }

        /// empty implementation
        public void CloseDBConnection()
        {
        }
    }
}
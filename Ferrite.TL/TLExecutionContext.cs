﻿/*
 *   Project Ferrite is an Implementation Telegram Server API
 *   Copyright 2022 Aykut Alparslan KOC <aykutalparslan@msn.com>
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Affero General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Affero General Public License for more details.
 *
 *   You should have received a copy of the GNU Affero General Public License
 *   along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
namespace Ferrite.TL
{
    public struct TLExecutionContext
    {
        private readonly Dictionary<string, object> _sessionData;
        public Dictionary<string, object> SessionData => _sessionData;
        public long Salt { get; set; }
        public long SessionId { get; set; }
        public long MessageId { get; set; }
        public int SequenceNo { get; set; }
        public TLExecutionContext(Dictionary<string, object> sessionData)
        {
            _sessionData = sessionData;
            Salt = 0;
            SessionId = 0;
            MessageId = 0;
            SequenceNo = 0;
        }
    }
}


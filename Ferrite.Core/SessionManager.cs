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
using System.Collections.Concurrent;
using Ferrite.Data;
using MessagePack;

namespace Ferrite.Core;

public class SessionManager : ISessionManager
{
    public Guid NodeId { get; private set; }
    private readonly ConcurrentDictionary<long, MTPtotoSession> _localSessions = new();
    private readonly IDistributedStore _store;
    private Guid GetNodeId()
    {
        if (File.Exists("node.guid"))
        {
            var bytes = File.ReadAllBytes("node.guid");
            return new Guid(bytes);
        }
        else
        {
            var guid = Guid.NewGuid();
            File.WriteAllBytes("node.guid", guid.ToByteArray());
            return guid;
        }
    }
    public SessionManager(IDistributedStore store)
    {
        NodeId = GetNodeId();
        _store = store;
    }
    public async Task<bool> AddSessionAsync(SessionState state, MTPtotoSession session)
    {
        state.NodeId = NodeId;
        var remoteAdd = await _store.PutSessionAsync(state.SessionId, MessagePackSerializer.Serialize(state));
        return remoteAdd && _localSessions.TryAdd(state.SessionId, session);
    }
    public async Task<SessionState?> GetSessionStateAsync(long sessionId)
    {
        var rawSession = await _store.GetSessionAsync(sessionId);
        if (rawSession != null)
        {
            var state = MessagePackSerializer.Deserialize<SessionState>(rawSession);
            if (state.ServerSalt.ValidSince <= (DateTimeOffset.Now.ToUnixTimeSeconds() - 1800))
            {
                state.ServerSaltOld = state.ServerSalt;
                var salt = new ServerSalt();
                state.ServerSalt = salt;
                await _store.PutSessionAsync(state.SessionId, MessagePackSerializer.Serialize(state));
            }
            return state;
        }
        return null;
    }
    public bool RemoveSession(long sessionId)
    {
        _store.RemoveSessionAsync(sessionId);
        return _localSessions.TryRemove(sessionId, out var session);
    }
    public bool LocalSessionExists(long sessionId)
    {
        return _localSessions.ContainsKey(sessionId);
    }
    public bool TryGetLocalSession(long sessionId, out MTPtotoSession session)
    {
        return _localSessions.TryGetValue(sessionId, out session);
    }
}


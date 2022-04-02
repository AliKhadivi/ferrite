/*
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
using System.Buffers;
using DotNext.Buffers;
using DotNext.IO;
using Ferrite.Utils;

namespace Ferrite.TL.layer139.messages;
public class PeerDialogsImpl : PeerDialogs
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public PeerDialogsImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => 863093588;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write(_dialogs.TLBytes, false);
            writer.Write(_messages.TLBytes, false);
            writer.Write(_chats.TLBytes, false);
            writer.Write(_users.TLBytes, false);
            writer.Write(_state.TLBytes, false);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private Vector<Dialog> _dialogs;
    public Vector<Dialog> Dialogs
    {
        get => _dialogs;
        set
        {
            serialized = false;
            _dialogs = value;
        }
    }

    private Vector<Message> _messages;
    public Vector<Message> Messages
    {
        get => _messages;
        set
        {
            serialized = false;
            _messages = value;
        }
    }

    private Vector<Chat> _chats;
    public Vector<Chat> Chats
    {
        get => _chats;
        set
        {
            serialized = false;
            _chats = value;
        }
    }

    private Vector<User> _users;
    public Vector<User> Users
    {
        get => _users;
        set
        {
            serialized = false;
            _users = value;
        }
    }

    private updates.State _state;
    public updates.State State
    {
        get => _state;
        set
        {
            serialized = false;
            _state = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        buff.Skip(4); _dialogs  =  factory . Read < Vector < Dialog > > ( ref  buff ) ; 
        buff.Skip(4); _messages  =  factory . Read < Vector < Message > > ( ref  buff ) ; 
        buff.Skip(4); _chats  =  factory . Read < Vector < Chat > > ( ref  buff ) ; 
        buff.Skip(4); _users  =  factory . Read < Vector < User > > ( ref  buff ) ; 
        _state = (updates.State)factory.Read(buff.ReadInt32(true), ref buff);
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
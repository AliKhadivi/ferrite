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

namespace Ferrite.TL.layer139.updates;
public class DifferenceSliceImpl : Difference
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public DifferenceSliceImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => -1459938943;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write(_newMessages.TLBytes, false);
            writer.Write(_newEncryptedMessages.TLBytes, false);
            writer.Write(_otherUpdates.TLBytes, false);
            writer.Write(_chats.TLBytes, false);
            writer.Write(_users.TLBytes, false);
            writer.Write(_intermediateState.TLBytes, false);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private Vector<Message> _newMessages;
    public Vector<Message> NewMessages
    {
        get => _newMessages;
        set
        {
            serialized = false;
            _newMessages = value;
        }
    }

    private Vector<EncryptedMessage> _newEncryptedMessages;
    public Vector<EncryptedMessage> NewEncryptedMessages
    {
        get => _newEncryptedMessages;
        set
        {
            serialized = false;
            _newEncryptedMessages = value;
        }
    }

    private Vector<Update> _otherUpdates;
    public Vector<Update> OtherUpdates
    {
        get => _otherUpdates;
        set
        {
            serialized = false;
            _otherUpdates = value;
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

    private updates.State _intermediateState;
    public updates.State IntermediateState
    {
        get => _intermediateState;
        set
        {
            serialized = false;
            _intermediateState = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        buff.Skip(4); _newMessages  =  factory . Read < Vector < Message > > ( ref  buff ) ; 
        buff.Skip(4); _newEncryptedMessages  =  factory . Read < Vector < EncryptedMessage > > ( ref  buff ) ; 
        buff.Skip(4); _otherUpdates  =  factory . Read < Vector < Update > > ( ref  buff ) ; 
        buff.Skip(4); _chats  =  factory . Read < Vector < Chat > > ( ref  buff ) ; 
        buff.Skip(4); _users  =  factory . Read < Vector < User > > ( ref  buff ) ; 
        buff.Skip(4); _intermediateState  =  factory . Read < updates . State > ( ref  buff ) ; 
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
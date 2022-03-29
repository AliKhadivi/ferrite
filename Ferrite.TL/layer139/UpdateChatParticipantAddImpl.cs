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

namespace Ferrite.TL.layer139;
public class UpdateChatParticipantAddImpl : Update
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public UpdateChatParticipantAddImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => 1037718609;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.WriteInt64(_chatId, true);
            writer.WriteInt64(_userId, true);
            writer.WriteInt64(_inviterId, true);
            writer.WriteInt32(_date, true);
            writer.WriteInt32(_version, true);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private long _chatId;
    public long ChatId
    {
        get => _chatId;
        set
        {
            serialized = false;
            _chatId = value;
        }
    }

    private long _userId;
    public long UserId
    {
        get => _userId;
        set
        {
            serialized = false;
            _userId = value;
        }
    }

    private long _inviterId;
    public long InviterId
    {
        get => _inviterId;
        set
        {
            serialized = false;
            _inviterId = value;
        }
    }

    private int _date;
    public int Date
    {
        get => _date;
        set
        {
            serialized = false;
            _date = value;
        }
    }

    private int _version;
    public int Version
    {
        get => _version;
        set
        {
            serialized = false;
            _version = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _chatId = buff.ReadInt64(true);
        _userId = buff.ReadInt64(true);
        _inviterId = buff.ReadInt64(true);
        _date = buff.ReadInt32(true);
        _version = buff.ReadInt32(true);
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
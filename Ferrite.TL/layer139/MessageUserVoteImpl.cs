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
public class MessageUserVoteImpl : MessageUserVote
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public MessageUserVoteImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => 886196148;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.WriteInt64(_userId, true);
            writer.WriteTLBytes(_option);
            writer.WriteInt32(_date, true);
            serialized = true;
            return writer.ToReadOnlySequence();
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

    private byte[] _option;
    public byte[] Option
    {
        get => _option;
        set
        {
            serialized = false;
            _option = value;
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

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _userId = buff.ReadInt64(true);
        _option = buff.ReadTLBytes().ToArray();
        _date = buff.ReadInt32(true);
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
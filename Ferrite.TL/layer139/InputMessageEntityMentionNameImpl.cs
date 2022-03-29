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
public class InputMessageEntityMentionNameImpl : MessageEntity
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public InputMessageEntityMentionNameImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => 546203849;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.WriteInt32(_offset, true);
            writer.WriteInt32(_length, true);
            writer.Write(_userId.TLBytes, false);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private int _offset;
    public int Offset
    {
        get => _offset;
        set
        {
            serialized = false;
            _offset = value;
        }
    }

    private int _length;
    public int Length
    {
        get => _length;
        set
        {
            serialized = false;
            _length = value;
        }
    }

    private InputUser _userId;
    public InputUser UserId
    {
        get => _userId;
        set
        {
            serialized = false;
            _userId = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _offset = buff.ReadInt32(true);
        _length = buff.ReadInt32(true);
        buff.Skip(4); _userId  =  factory . Read < InputUser > ( ref  buff ) ; 
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
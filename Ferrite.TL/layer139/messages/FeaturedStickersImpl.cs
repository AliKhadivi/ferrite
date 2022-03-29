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
public class FeaturedStickersImpl : FeaturedStickers
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public FeaturedStickersImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => -2067782896;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.WriteInt64(_hash, true);
            writer.WriteInt32(_count, true);
            writer.Write(_sets.TLBytes, false);
            writer.Write(_unread.TLBytes, false);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private long _hash;
    public long Hash
    {
        get => _hash;
        set
        {
            serialized = false;
            _hash = value;
        }
    }

    private int _count;
    public int Count
    {
        get => _count;
        set
        {
            serialized = false;
            _count = value;
        }
    }

    private Vector<StickerSetCovered> _sets;
    public Vector<StickerSetCovered> Sets
    {
        get => _sets;
        set
        {
            serialized = false;
            _sets = value;
        }
    }

    private VectorOfLong _unread;
    public VectorOfLong Unread
    {
        get => _unread;
        set
        {
            serialized = false;
            _unread = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _hash = buff.ReadInt64(true);
        _count = buff.ReadInt32(true);
        buff.Skip(4); _sets  =  factory . Read < Vector < StickerSetCovered > > ( ref  buff ) ; 
        buff.Skip(4); _unread  =  factory . Read < VectorOfLong > ( ref  buff ) ; 
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
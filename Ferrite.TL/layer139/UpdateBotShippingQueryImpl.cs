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
public class UpdateBotShippingQueryImpl : Update
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public UpdateBotShippingQueryImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => -1246823043;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.WriteInt64(_queryId, true);
            writer.WriteInt64(_userId, true);
            writer.WriteTLBytes(_payload);
            writer.Write(_shippingAddress.TLBytes, false);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private long _queryId;
    public long QueryId
    {
        get => _queryId;
        set
        {
            serialized = false;
            _queryId = value;
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

    private byte[] _payload;
    public byte[] Payload
    {
        get => _payload;
        set
        {
            serialized = false;
            _payload = value;
        }
    }

    private PostAddress _shippingAddress;
    public PostAddress ShippingAddress
    {
        get => _shippingAddress;
        set
        {
            serialized = false;
            _shippingAddress = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _queryId = buff.ReadInt64(true);
        _userId = buff.ReadInt64(true);
        _payload = buff.ReadTLBytes().ToArray();
        _shippingAddress = (PostAddress)factory.Read(buff.ReadInt32(true), ref buff);
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
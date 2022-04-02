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
public class RequestEncryption : ITLObject, ITLMethod
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public RequestEncryption(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public int Constructor => -162681021;
    public ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write(_userId.TLBytes, false);
            writer.WriteInt32(_randomId, true);
            writer.WriteTLBytes(_gA);
            serialized = true;
            return writer.ToReadOnlySequence();
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

    private int _randomId;
    public int RandomId
    {
        get => _randomId;
        set
        {
            serialized = false;
            _randomId = value;
        }
    }

    private byte[] _gA;
    public byte[] GA
    {
        get => _gA;
        set
        {
            serialized = false;
            _gA = value;
        }
    }

    public async Task<ITLObject> ExecuteAsync(TLExecutionContext ctx)
    {
        throw new NotImplementedException();
    }

    public void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _userId = (InputUser)factory.Read(buff.ReadInt32(true), ref buff);
        _randomId = buff.ReadInt32(true);
        _gA = buff.ReadTLBytes().ToArray();
    }

    public void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
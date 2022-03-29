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
public class InputEncryptedFileUploadedImpl : InputEncryptedFile
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public InputEncryptedFileUploadedImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => 1690108678;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.WriteInt64(_id, true);
            writer.WriteInt32(_parts, true);
            writer.WriteTLString(_md5Checksum);
            writer.WriteInt32(_keyFingerprint, true);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private long _id;
    public long Id
    {
        get => _id;
        set
        {
            serialized = false;
            _id = value;
        }
    }

    private int _parts;
    public int Parts
    {
        get => _parts;
        set
        {
            serialized = false;
            _parts = value;
        }
    }

    private string _md5Checksum;
    public string Md5Checksum
    {
        get => _md5Checksum;
        set
        {
            serialized = false;
            _md5Checksum = value;
        }
    }

    private int _keyFingerprint;
    public int KeyFingerprint
    {
        get => _keyFingerprint;
        set
        {
            serialized = false;
            _keyFingerprint = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _id = buff.ReadInt64(true);
        _parts = buff.ReadInt32(true);
        _md5Checksum = buff.ReadTLString();
        _keyFingerprint = buff.ReadInt32(true);
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
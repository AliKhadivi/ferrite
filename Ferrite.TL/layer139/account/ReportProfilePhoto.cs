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

namespace Ferrite.TL.layer139.account;
public class ReportProfilePhoto : ITLObject, ITLMethod
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public ReportProfilePhoto(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public int Constructor => -91437323;
    public ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write(_peer.TLBytes, false);
            writer.Write(_photoId.TLBytes, false);
            writer.Write(_reason.TLBytes, false);
            writer.WriteTLString(_message);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private InputPeer _peer;
    public InputPeer Peer
    {
        get => _peer;
        set
        {
            serialized = false;
            _peer = value;
        }
    }

    private InputPhoto _photoId;
    public InputPhoto PhotoId
    {
        get => _photoId;
        set
        {
            serialized = false;
            _photoId = value;
        }
    }

    private ReportReason _reason;
    public ReportReason Reason
    {
        get => _reason;
        set
        {
            serialized = false;
            _reason = value;
        }
    }

    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            serialized = false;
            _message = value;
        }
    }

    public async Task<ITLObject> ExecuteAsync(TLExecutionContext ctx)
    {
        throw new NotImplementedException();
    }

    public void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _peer = (InputPeer)factory.Read(buff.ReadInt32(true), ref buff);
        _photoId = (InputPhoto)factory.Read(buff.ReadInt32(true), ref buff);
        _reason = (ReportReason)factory.Read(buff.ReadInt32(true), ref buff);
        _message = buff.ReadTLString();
    }

    public void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
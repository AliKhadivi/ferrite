/*
 *    Copyright 2022 Aykut Alparslan KOC <aykutalparslan@msn.com>
 *    This file is a part of Project Ferrite
 *
 *    Proprietary and confidential.
 *    Copying without express written permission is strictly prohibited.
 */

using System;
using System.Buffers;
using DotNext.Buffers;
using DotNext.IO;
using Ferrite.Utils;

namespace Ferrite.TL.mtproto;
public class DhGenRetry : ITLObject
{
    private SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private ITLObjectFactory factory;
    private bool serialized = false;
    public DhGenRetry(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public int Constructor => 1188831161;
    public ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write(nonce.TLBytes, false);
            writer.Write(serverNonce.TLBytes, false);
            writer.Write(newNonceHash2.TLBytes, false);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private Int128 nonce;
    public Int128 Nonce
    {
        get => nonce;
        set
        {
            serialized = false;
            nonce = value;
        }
    }

    private Int128 serverNonce;
    public Int128 ServerNonce
    {
        get => serverNonce;
        set
        {
            serialized = false;
            serverNonce = value;
        }
    }

    private Int128 newNonceHash2;
    public Int128 NewNonceHash2
    {
        get => newNonceHash2;
        set
        {
            serialized = false;
            newNonceHash2 = value;
        }
    }

    public bool IsMethod => false;
    public ITLObject Execute(TLExecutionContext ctx)
    {
        throw new NotImplementedException();
    }

    public void Parse(ref SequenceReader buff)
    {
        serialized = false;
        nonce = factory.Read<Int128>(ref buff);
        serverNonce = factory.Read<Int128>(ref buff);
        newNonceHash2 = factory.Read<Int128>(ref buff);
    }

    public void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
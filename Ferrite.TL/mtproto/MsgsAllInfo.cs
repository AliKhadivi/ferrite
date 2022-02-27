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
public class MsgsAllInfo : ITLObject
{
    private SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private ITLObjectFactory factory;
    private bool serialized = false;
    public MsgsAllInfo(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public int Constructor => -1933520591;
    public ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write(msgIds.TLBytes, false);
            writer.WriteTLBytes(info);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private VectorOfLong msgIds;
    public VectorOfLong MsgIds
    {
        get => msgIds;
        set
        {
            serialized = false;
            msgIds = value;
        }
    }

    private byte[] info;
    public byte[] Info
    {
        get => info;
        set
        {
            serialized = false;
            info = value;
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
        buff.Skip(4); msgIds  =  factory . Read < VectorOfLong > ( ref  buff ) ; 
        info = buff.ReadTLBytes().ToArray();
    }

    public void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
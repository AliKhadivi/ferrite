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
public class MessageMediaInvoiceImpl : MessageMedia
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public MessageMediaInvoiceImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => -2074799289;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write<Flags>(_flags);
            writer.WriteTLString(_title);
            writer.WriteTLString(_description);
            if (_flags[0])
            {
                writer.Write(_photo.TLBytes, false);
            }

            if (_flags[2])
            {
                writer.WriteInt32(_receiptMsgId, true);
            }

            writer.WriteTLString(_currency);
            writer.WriteInt64(_totalAmount, true);
            writer.WriteTLString(_startParam);
            serialized = true;
            return writer.ToReadOnlySequence();
        }
    }

    private Flags _flags;
    public Flags Flags
    {
        get => _flags;
        set
        {
            serialized = false;
            _flags = value;
        }
    }

    public bool ShippingAddressRequested
    {
        get => _flags[1];
        set
        {
            serialized = false;
            _flags[1] = value;
        }
    }

    public bool Test
    {
        get => _flags[3];
        set
        {
            serialized = false;
            _flags[3] = value;
        }
    }

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            serialized = false;
            _title = value;
        }
    }

    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            serialized = false;
            _description = value;
        }
    }

    private WebDocument _photo;
    public WebDocument Photo
    {
        get => _photo;
        set
        {
            serialized = false;
            _flags[0] = true;
            _photo = value;
        }
    }

    private int _receiptMsgId;
    public int ReceiptMsgId
    {
        get => _receiptMsgId;
        set
        {
            serialized = false;
            _flags[2] = true;
            _receiptMsgId = value;
        }
    }

    private string _currency;
    public string Currency
    {
        get => _currency;
        set
        {
            serialized = false;
            _currency = value;
        }
    }

    private long _totalAmount;
    public long TotalAmount
    {
        get => _totalAmount;
        set
        {
            serialized = false;
            _totalAmount = value;
        }
    }

    private string _startParam;
    public string StartParam
    {
        get => _startParam;
        set
        {
            serialized = false;
            _startParam = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _flags = buff.Read<Flags>();
        _title = buff.ReadTLString();
        _description = buff.ReadTLString();
        if (_flags[0])
        {
            buff.Skip(4);
            _photo = factory.Read<WebDocument>(ref buff);
        }

        if (_flags[2])
        {
            _receiptMsgId = buff.ReadInt32(true);
        }

        _currency = buff.ReadTLString();
        _totalAmount = buff.ReadInt64(true);
        _startParam = buff.ReadTLString();
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
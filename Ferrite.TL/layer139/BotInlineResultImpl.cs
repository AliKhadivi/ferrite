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
public class BotInlineResultImpl : BotInlineResult
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public BotInlineResultImpl(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public override int Constructor => 295067450;
    public override ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write<Flags>(_flags);
            writer.WriteTLString(_id);
            writer.WriteTLString(_type);
            if (_flags[1])
            {
                writer.WriteTLString(_title);
            }

            if (_flags[2])
            {
                writer.WriteTLString(_description);
            }

            if (_flags[3])
            {
                writer.WriteTLString(_url);
            }

            if (_flags[4])
            {
                writer.Write(_thumb.TLBytes, false);
            }

            if (_flags[5])
            {
                writer.Write(_content.TLBytes, false);
            }

            writer.Write(_sendMessage.TLBytes, false);
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

    private string _id;
    public string Id
    {
        get => _id;
        set
        {
            serialized = false;
            _id = value;
        }
    }

    private string _type;
    public string Type
    {
        get => _type;
        set
        {
            serialized = false;
            _type = value;
        }
    }

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            serialized = false;
            _flags[1] = true;
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
            _flags[2] = true;
            _description = value;
        }
    }

    private string _url;
    public string Url
    {
        get => _url;
        set
        {
            serialized = false;
            _flags[3] = true;
            _url = value;
        }
    }

    private WebDocument _thumb;
    public WebDocument Thumb
    {
        get => _thumb;
        set
        {
            serialized = false;
            _flags[4] = true;
            _thumb = value;
        }
    }

    private WebDocument _content;
    public WebDocument Content
    {
        get => _content;
        set
        {
            serialized = false;
            _flags[5] = true;
            _content = value;
        }
    }

    private BotInlineMessage _sendMessage;
    public BotInlineMessage SendMessage
    {
        get => _sendMessage;
        set
        {
            serialized = false;
            _sendMessage = value;
        }
    }

    public override void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _flags = buff.Read<Flags>();
        _id = buff.ReadTLString();
        _type = buff.ReadTLString();
        if (_flags[1])
        {
            _title = buff.ReadTLString();
        }

        if (_flags[2])
        {
            _description = buff.ReadTLString();
        }

        if (_flags[3])
        {
            _url = buff.ReadTLString();
        }

        if (_flags[4])
        {
            buff.Skip(4);
            _thumb = factory.Read<WebDocument>(ref buff);
        }

        if (_flags[5])
        {
            buff.Skip(4);
            _content = factory.Read<WebDocument>(ref buff);
        }

        buff.Skip(4); _sendMessage  =  factory . Read < BotInlineMessage > ( ref  buff ) ; 
    }

    public override void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
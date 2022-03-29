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
public class EditMessage : ITLObject, ITLMethod
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public EditMessage(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public int Constructor => 1224152952;
    public ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write<Flags>(_flags);
            writer.Write(_peer.TLBytes, false);
            writer.WriteInt32(_id, true);
            if (_flags[11])
            {
                writer.WriteTLString(_message);
            }

            if (_flags[14])
            {
                writer.Write(_media.TLBytes, false);
            }

            if (_flags[2])
            {
                writer.Write(_replyMarkup.TLBytes, false);
            }

            if (_flags[3])
            {
                writer.Write(_entities.TLBytes, false);
            }

            if (_flags[15])
            {
                writer.WriteInt32(_scheduleDate, true);
            }

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

    public bool NoWebpage
    {
        get => _flags[1];
        set
        {
            serialized = false;
            _flags[1] = value;
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

    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            serialized = false;
            _id = value;
        }
    }

    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            serialized = false;
            _flags[11] = true;
            _message = value;
        }
    }

    private InputMedia _media;
    public InputMedia Media
    {
        get => _media;
        set
        {
            serialized = false;
            _flags[14] = true;
            _media = value;
        }
    }

    private ReplyMarkup _replyMarkup;
    public ReplyMarkup ReplyMarkup
    {
        get => _replyMarkup;
        set
        {
            serialized = false;
            _flags[2] = true;
            _replyMarkup = value;
        }
    }

    private Vector<MessageEntity> _entities;
    public Vector<MessageEntity> Entities
    {
        get => _entities;
        set
        {
            serialized = false;
            _flags[3] = true;
            _entities = value;
        }
    }

    private int _scheduleDate;
    public int ScheduleDate
    {
        get => _scheduleDate;
        set
        {
            serialized = false;
            _flags[15] = true;
            _scheduleDate = value;
        }
    }

    public async Task<ITLObject> ExecuteAsync(TLExecutionContext ctx)
    {
        throw new NotImplementedException();
    }

    public void Parse(ref SequenceReader buff)
    {
        serialized = false;
        _flags = buff.Read<Flags>();
        buff.Skip(4); _peer  =  factory . Read < InputPeer > ( ref  buff ) ; 
        _id = buff.ReadInt32(true);
        if (_flags[11])
        {
            _message = buff.ReadTLString();
        }

        if (_flags[14])
        {
            buff.Skip(4);
            _media = factory.Read<InputMedia>(ref buff);
        }

        if (_flags[2])
        {
            buff.Skip(4);
            _replyMarkup = factory.Read<ReplyMarkup>(ref buff);
        }

        if (_flags[3])
        {
            buff.Skip(4);
            _entities = factory.Read<Vector<MessageEntity>>(ref buff);
        }

        if (_flags[15])
        {
            _scheduleDate = buff.ReadInt32(true);
        }
    }

    public void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
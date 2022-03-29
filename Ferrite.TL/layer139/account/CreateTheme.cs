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
public class CreateTheme : ITLObject, ITLMethod
{
    private readonly SparseBufferWriter<byte> writer = new SparseBufferWriter<byte>(UnmanagedMemoryPool<byte>.Shared);
    private readonly ITLObjectFactory factory;
    private bool serialized = false;
    public CreateTheme(ITLObjectFactory objectFactory)
    {
        factory = objectFactory;
    }

    public int Constructor => 1697530880;
    public ReadOnlySequence<byte> TLBytes
    {
        get
        {
            if (serialized)
                return writer.ToReadOnlySequence();
            writer.Clear();
            writer.WriteInt32(Constructor, true);
            writer.Write<Flags>(_flags);
            writer.WriteTLString(_slug);
            writer.WriteTLString(_title);
            if (_flags[2])
            {
                writer.Write(_document.TLBytes, false);
            }

            if (_flags[3])
            {
                writer.Write(_settings.TLBytes, false);
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

    private string _slug;
    public string Slug
    {
        get => _slug;
        set
        {
            serialized = false;
            _slug = value;
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

    private InputDocument _document;
    public InputDocument Document
    {
        get => _document;
        set
        {
            serialized = false;
            _flags[2] = true;
            _document = value;
        }
    }

    private Vector<InputThemeSettings> _settings;
    public Vector<InputThemeSettings> Settings
    {
        get => _settings;
        set
        {
            serialized = false;
            _flags[3] = true;
            _settings = value;
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
        _slug = buff.ReadTLString();
        _title = buff.ReadTLString();
        if (_flags[2])
        {
            buff.Skip(4);
            _document = factory.Read<InputDocument>(ref buff);
        }

        if (_flags[3])
        {
            buff.Skip(4);
            _settings = factory.Read<Vector<InputThemeSettings>>(ref buff);
        }
    }

    public void WriteTo(Span<byte> buff)
    {
        TLBytes.CopyTo(buff);
    }
}
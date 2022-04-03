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
public abstract class DcOption : ITLObject
{
    public virtual int Constructor => throw new NotImplementedException() ; public virtual ReadOnlySequence<byte> TLBytes => throw new NotImplementedException() ; public virtual void Parse(ref SequenceReader buff)
    {
        throw new NotImplementedException();
    }

    public virtual void WriteTo(Span<byte> buff)
    {
        throw new NotImplementedException();
    }
    private static Vector<DcOption> _options;
    public static async Task<Vector<DcOption>> GetDefaultDcOptionsAsync(ITLObjectFactory factory)
    {
        if(_options != null)
        {
            return _options;
        }
        //TODO: Populate from the data store
        var dcOption = factory.Resolve<DcOptionImpl>();
        dcOption.Id = 1;
        dcOption.IpAddress = "127.0.0.1";
        dcOption.Port = 5222;
        Vector<DcOption> options = factory.Resolve<Vector<DcOption>>();
        options.Add(dcOption);
        if (_options == null)
        {
            _options = options;
        }
        return _options;
    }
}
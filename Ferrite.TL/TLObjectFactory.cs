/*
 *    Copyright 2022 Aykut Alparslan KOC <aykutalparslan@msn.com>
 *    This file is a part of Project Ferrite
 *
 *    Proprietary and confidential.
 *    Copying without express written permission is strictly prohibited.
 */
using System;
using DotNext.IO;
using Ferrite.TL.Exceptions;
using Ferrite.TL.mtproto;
using Autofac;
using Ferrite.Crypto;
using Ferrite.Utils;

namespace Ferrite.TL;
public class TLObjectFactory : ITLObjectFactory
{
    private ILifetimeScope Container { get; set; }

    public TLObjectFactory(ILifetimeScope container)
    {
        Container = container;
    }

    public ITLObject Read(int constructor, ref SequenceReader buff) => constructor switch
    {
        85337187 => Read<Respq>(ref buff),
        -1443537003 => Read<PQInnerDataDc>(ref buff),
        1459478408 => Read<PQInnerDataTempDc>(ref buff),
        -790100132 => Read<ServerDhParamsOk>(ref buff),
        -1249309254 => Read<ServerDhInnerData>(ref buff),
        1715713620 => Read<ClientDhInnerData>(ref buff),
        1003222836 => Read<DhGenOk>(ref buff),
        1188831161 => Read<DhGenRetry>(ref buff),
        -1499615742 => Read<DhGenFail>(ref buff),
        1973679973 => Read<BindAuthKeyInner>(ref buff),
        -212046591 => Read<RpcResult>(ref buff),
        558156313 => Read<RpcError>(ref buff),
        1579864942 => Read<RpcAnswerUnknown>(ref buff),
        -847714938 => Read<RpcAnswerDroppedRunning>(ref buff),
        -1539647305 => Read<RpcAnswerDropped>(ref buff),
        155834844 => Read<FutureSalt>(ref buff),
        -1370486635 => Read<FutureSalts>(ref buff),
        880243653 => Read<Pong>(ref buff),
        -501201412 => Read<DestroySessionOk>(ref buff),
        1658015945 => Read<DestroySessionNone>(ref buff),
        -1631450872 => Read<NewSessionCreated>(ref buff),
        1945237724 => Read<MsgContainer>(ref buff),
        1538843921 => Read<Message>(ref buff),
        -530561358 => Read<MsgCopy>(ref buff),
        812830625 => Read<GzipPacked>(ref buff),
        1658238041 => Read<MsgsAck>(ref buff),
        -1477445615 => Read<BadMsgNotification>(ref buff),
        -307542917 => Read<BadServerSalt>(ref buff),
        2105940488 => Read<MsgResendReq>(ref buff),
        -630588590 => Read<MsgsStateReq>(ref buff),
        81704317 => Read<MsgsStateInfo>(ref buff),
        -1933520591 => Read<MsgsAllInfo>(ref buff),
        661470918 => Read<MsgDetailedInfo>(ref buff),
        -2137147681 => Read<MsgNewDetailedInfo>(ref buff),
        -161422892 => Read<DestroyAuthKeyOk>(ref buff),
        178201177 => Read<DestroyAuthKeyNone>(ref buff),
        -368010477 => Read<DestroyAuthKeyFail>(ref buff),
        -1099002127 => Read<ReqPqMulti>(ref buff),
        -686627650 => Read<ReqDhParams>(ref buff),
        -184262881 => Read<SetClientDhParams>(ref buff),
        1491380032 => Read<RpcDropAnswer>(ref buff),
        -1188971260 => Read<GetFutureSalts>(ref buff),
        2059302892 => Read<Ping>(ref buff),
        -213746804 => Read<PingDelayDisconnect>(ref buff),
        -414113498 => Read<DestroySession>(ref buff),
        -1835453025 => Read<HttpWait>(ref buff),
        -784117408 => Read<DestroyAuthKey>(ref buff),
        _ => throw new DeserializationException("Constructor " + string.Format("0x{ 0:X}", constructor) + " not found.")};
    public T Read<T>(ref SequenceReader buff)
        where T : ITLObject
    {
        var obj = Container.Resolve<T>();
        obj.Parse(ref buff);
        return obj;
    }

    public T Resolve<T>()
        where T : ITLObject
    {
        return Container.Resolve<T>();
    }
}
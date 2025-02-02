// 
// Project Ferrite is an Implementation of the Telegram Server API
// Copyright 2022 Aykut Alparslan KOC <aykutalparslan@msn.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// 

namespace Ferrite.Data.Repositories;

public interface IUnitOfWork
{
    IAuthKeyRepository AuthKeyRepository { get; }
    ITempAuthKeyRepository TempAuthKeyRepository { get; }
    IBoundAuthKeyRepository BoundAuthKeyRepository { get; }
    IAuthorizationRepository AuthorizationRepository { get; }
    IServerSaltRepository ServerSaltRepository { get; }
    IMessageRepository MessageRepository { get; }
    IUserStatusRepository UserStatusRepository { get; }
    ISessionRepository SessionRepository { get; }
    IAuthSessionRepository AuthSessionRepository { get; }
    IPhoneCodeRepository PhoneCodeRepository { get; }
    ISignInRepository SignInRepository { get; }
    ILoginTokenRepository LoginTokenRepository { get; }
    IDeviceLockedRepository DeviceLockedRepository { get; }
    IUserRepository UserRepository { get; }
    IAppInfoRepository AppInfoRepository { get; }
    IDeviceInfoRepository DeviceInfoRepository { get; }
    INotifySettingsRepository NotifySettingsRepository { get; }
    IReportReasonRepository ReportReasonRepository { get; }
    IPrivacyRulesRepository PrivacyRulesRepository { get; }
    IChatRepository ChatRepository { get; }
    IContactsRepository ContactsRepository { get; }
    IBlockedPeersRepository BlockedPeersRepository { get; }
    ISignUpNotificationRepository SignUpNotificationRepository { get; }
    IFileInfoRepository FileInfoRepository { get; }
    IPhotoRepository PhotoRepository { get; }
    ILangPackRepository LangPackRepository { get; }
    public bool Save();
    public ValueTask<bool> SaveAsync();
}
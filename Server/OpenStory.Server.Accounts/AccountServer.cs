﻿using System;
using System.Collections.Generic;
using OpenStory.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;
using NodaTime;

namespace OpenStory.Server.Accounts
{
    /// <summary>
    /// Represents an in-memory account activity server.
    /// </summary>
    public class AccountServer : GameServerBase, IAccountService
    {
        private readonly IClock clock;

        private readonly Dictionary<int, ActiveAccount> activeAccounts;
        private readonly AtomicInteger currentSessionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountServer"/> class.
        /// </summary>
        public AccountServer(IClock clock)
        {
            this.clock = clock;

            this.activeAccounts = new Dictionary<int, ActiveAccount>(256);
            this.currentSessionId = new AtomicInteger(0);
        }

        #region IAccountService Members

        /// <inheritdoc />
        public bool TryRegisterSession(int accountId, out int sessionId)
        {
            if (this.activeAccounts.ContainsKey(accountId))
            {
                sessionId = 0;
                return false;
            }
            else
            {
                sessionId = this.currentSessionId.Increment();

                var account = new ActiveAccount(accountId, sessionId);
                account.KeepAlive(this.clock.Now);

                this.activeAccounts.Add(accountId, account);
                return true;
            }
        }

        /// <inheritdoc />
        public bool TryRegisterCharacter(int accountId, int characterId)
        {
            ActiveAccount account;
            if (!this.activeAccounts.TryGetValue(accountId, out account))
            {
                return false;
            }
            else
            {
                if (!account.CharacterId.HasValue)
                {
                    account.RegisterCharacter(characterId);
                    return true;
                }

                return false;
            }
        }

        /// <inheritdoc />
        public bool TryUnregisterSession(int accountId)
        {
            ActiveAccount account;
            if (!this.activeAccounts.TryGetValue(accountId, out account))
            {
                return false;
            }
            else
            {
                this.activeAccounts.Remove(accountId);
                if (account.CharacterId.HasValue)
                {
                    account.UnregisterCharacter();
                }

                return true;
            }
        }

        /// <inheritdoc />
        public bool TryKeepAlive(int accountId, out TimeSpan lag)
        {
            ActiveAccount account;
            if (!this.activeAccounts.TryGetValue(accountId, out account))
            {
                lag = default(TimeSpan);
                return false;
            }
            else
            {
                lag = account.KeepAlive(this.clock.Now).ToTimeSpan();
                return true;
            }
        }

        #endregion
    }
}

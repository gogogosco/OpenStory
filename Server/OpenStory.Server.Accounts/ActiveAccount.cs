﻿using System;
using NodaTime;

namespace OpenStory.Server.Accounts
{
    /// <summary>
    /// Represents an active account session.
    /// </summary>
    internal sealed class ActiveAccount
    {
        /// <summary>
        /// Gets the identifier of the active account.
        /// </summary>
        public int AccountId { get; private set; }

        /// <summary>
        /// Gets the identifier of the active session.
        /// </summary>
        public int SessionId { get; private set; }

        /// <summary>
        /// Gets the identifier of the active character.
        /// </summary>
        public int? CharacterId { get; private set; }

        /// <summary>
        /// Gets the last "keep-alive" timestamp for this active session.
        /// </summary>
        public Instant LastKeepAlive { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveAccount"/> class.
        /// </summary>
        /// <param name="accountId">The ID of the active account.</param>
        /// <param name="sessionId">The ID of the active session.</param>
        public ActiveAccount(int accountId, int sessionId)
        {
            AccountId = accountId;
            SessionId = sessionId;
        }

        /// <summary>
        /// Registers a character ID for this active account.
        /// </summary>
        /// <param name="characterId">The character ID for this active account.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="CharacterId"/> is already assigned a value.</exception>
        public void RegisterCharacter(int characterId)
        {
            if (CharacterId.HasValue)
            {
                throw new InvalidOperationException(AccountServiceStrings.SessionAlreadyHasCharacter);
            }

            CharacterId = characterId;
        }

        /// <summary>
        /// Unregisters the active character.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="CharacterId"/> has no value assigned.</exception>
        public void UnregisterCharacter()
        {
            if (!CharacterId.HasValue)
            {
                throw new InvalidOperationException(AccountServiceStrings.SessionHasNoCharacter);
            }

            CharacterId = null;
        }

        public Duration KeepAlive(Instant now)
        {
            var newTimestamp = now;
            var oldTimestamp = LastKeepAlive;

            LastKeepAlive = newTimestamp;

            var lag = newTimestamp - oldTimestamp;
            return lag;
        }
    }
}

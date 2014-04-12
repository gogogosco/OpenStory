using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Framework.Model.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Provides authentication methods.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Attempts to authenticate the given account credentials.
        /// </summary>
        /// <param name="credentialsReader">An <see cref="IUnsafePacketReader"/> object from which to read the credential information.</param>
        /// <param name="session">An <see cref="IAccountSession"/> variable to hold the resulting session.</param>
        /// <param name="account">An <see cref="Account"/> variable to hold the loaded account data.</param>
        /// <returns>an <see cref="AuthenticationResult"/> for the operation.</returns>
        AuthenticationResult Authenticate(IUnsafePacketReader credentialsReader, out IAccountSession session, out Account account);
    }
}

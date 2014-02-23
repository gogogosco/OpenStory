using System.Net;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents a set of server configuration settings.
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// Gets the game version for the server.
        /// </summary>
        public ushort? Header { get; private set; }

        /// <summary>
        /// Gets the game version for the server.
        /// </summary>
        public ushort Version { get; private set; }

        /// <summary>
        /// Gets the game sub-version for the server.
        /// </summary>
        public string PatchLocation { get; private set; }

        /// <summary>
        /// Gets the locale ID for the server.
        /// </summary>
        public byte LocaleId { get; private set; }

        /// <summary>
        /// Gets the entry point definition for the server.
        /// </summary>
        public IPEndPoint Endpoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The object containing the configuration values.</param>
        public ServerConfiguration(OsServiceConfiguration configuration)
        {
            this.Endpoint = configuration.Get<IPEndPoint>("Endpoint", true);

            this.Header = configuration.GetValue<ushort>("Header");
            this.Version = configuration.Get<ushort>("Version", true);
            this.PatchLocation = configuration.Get<string>("PatchLocation", true);
            this.LocaleId = configuration.Get<byte>("LocaleId", true);
        }
    }
}
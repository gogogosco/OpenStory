using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    /// <summary>
    /// Represents a client for a generic game service.
    /// </summary>
    public sealed class RegisteredServiceClient : DuplexClientBase<IRegisteredService>, IRegisteredService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredServiceClient"/> class with the specified endpoint address.
        /// </summary>
        /// <param name="endpoint">The service endpoint information.</param>
        /// <param name="stateChangedHandler">The handler for the service state changes.</param>
        public RegisteredServiceClient(ServiceEndpoint endpoint, IServiceStateChanged stateChangedHandler)
            : base(stateChangedHandler, endpoint)
        {
        }

        #region Implementation of IRegisteredService

        /// <inheritdoc />
        public ServiceOperationResult Initialize()
        {
            var result = this.Call(() => this.Channel.Initialize());
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult Start()
        {
            var result = this.Call(() => this.Channel.Start());
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult Stop()
        {
            var result = this.Call(() => this.Channel.Stop());
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult Ping()
        {
            var result = this.Call(() => this.Channel.Ping());
            return result;
        }

        #endregion
    }
}

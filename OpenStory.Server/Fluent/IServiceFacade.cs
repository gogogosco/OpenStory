using System.ComponentModel;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for managing the OpenStory services.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceFacade : IFluentInterface
    {
        /// <summary>
        /// The entry point for managing the Account service.
        /// </summary>
        IAccountServiceFacade Accounts();

        /// <summary>
        /// The entry point for managing the Authentication service.
        /// </summary>
        IAuthServiceFacade Auth();

        /// <summary>
        /// The entry point for managing the Channel services.
        /// </summary>
        IChannelsServiceFacade Channels();

        /// <summary>
        /// The entry point for managing the current Channel service.
        /// </summary>
        IChannelServiceFacade Channel();

        /// <summary>
        /// The entry point for managing a specific Channel service.
        /// </summary>
        /// <param name="id">The identifier of the channel.</param>
        IChannelServiceFacade Channel(int id);

        /// <summary>
        /// The entry point for managing the current World service.
        /// </summary>
        IWorldServiceFacade World();
    }
}
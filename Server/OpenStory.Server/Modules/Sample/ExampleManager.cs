using System.ComponentModel;

namespace OpenStory.Server.Modules.Sample
{
    /// <summary>
    /// An example of how a manager is implemented.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ExampleManager : ManagerBase<ExampleManager>
    {
        /// <summary>
        /// The name of the "Service" component.
        /// </summary>
        public const string ServiceKey = "ServiceName";

        /// <summary>
        /// The name of the "InternalService" component.
        /// </summary>
        internal const string InternalServiceKey = "InternalServiceName";

        /// <summary>
        /// Gets the registered IExampleService instance.
        /// </summary>
        public IExampleService Service { get; private set; }

        /// <summary>
        /// Gets the registered IExampleInternalService instance.
        /// </summary>
        internal IExampleInternalService InternalService { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ExampleManager"/>.
        /// </summary>
        public ExampleManager()
        {
            base.RequireComponent<IExampleService>(ServiceKey);
            base.AllowComponent<IExampleInternalService>(InternalServiceKey);
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitializing()
        {
            if (!base.CheckComponent(InternalServiceKey))
            {
                // No component was registered for this key. We register the default one.
                base.RegisterComponent(InternalServiceKey, DefaultExampleInternalService.Instance);
            }

            base.OnInitializing();
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Service = base.GetComponent<IExampleService>(ServiceKey);
            this.InternalService = base.GetComponent<IExampleInternalService>(InternalServiceKey);
        }
    }
}
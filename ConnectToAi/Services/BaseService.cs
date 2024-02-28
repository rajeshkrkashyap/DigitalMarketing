using Core.Shared;

namespace ConnectToAi.Services
{
    public class BaseService : IDisposable
    {    // Flag to indicate whether the object has been disposed
        private bool disposed = false;
        protected readonly ConfigService _configService;
        public BaseService(ConfigService configService)
        {
            _configService = configService;
        }
        public string ApiBaseURL { get { return _configService.AppSettings.ApiBaseUrl; } }

        // Method to perform the actual cleanup
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed resources here (if any)
                }

                // Release unmanaged resources here
                // For example, if you have an IntPtr, use Marshal.Release or similar methods

                // Set the flag to indicate that the object has been disposed
                disposed = true;
            }
        }

        // Public method to allow users to explicitly call Dispose
        public void Dispose()
        {
            // Call the Dispose method, indicating that it's being called by user code
            Dispose(true);

            // Suppress finalization to prevent the object from being disposed again by the finalizer
            GC.SuppressFinalize(this);
        }

        // Finalizer (destructor) to ensure that resources are released if Dispose is not called
        ~BaseService()
        {
            // Call the Dispose method, indicating that it's being called by the finalizer
            Dispose(false);
        }
    }
}

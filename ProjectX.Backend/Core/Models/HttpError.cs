namespace ProjectX.Backend.Core.Models
{
    /// <summary>
    /// Represents an http error object.
    /// </summary>
    public class HttpError
    { 
        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// The service the error was thrown in.
        /// </summary>
        public string Service { get; init; }

        /// <summary>
        /// The error status code.
        /// </summary>
        public int StatusCode { get; init; }

        /// <summary>
        /// The error internal code.
        /// </summary>
        public int InternalCode { get; init; }
    }
}
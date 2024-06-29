using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CustomerTestApp.Service.Interceptors
{
    /// <summary>
    /// The logging interceptor for logging gRPC calls.
    /// </summary>
    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Starting call. Method: {Method}. Request: {Request}", context.Method, request);
            var response = await continuation(request, context);
            _logger.LogInformation("Finished call. Method: {Method}. Response: {Response}", context.Method, response);
            return response;
        }

        public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
            TRequest request,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Starting call. Method: {Method}. Request: {Request}", context.Method, request);
            await continuation(request, responseStream, context);
            _logger.LogInformation("Finished call. Method: {Method}", context.Method);
        }
    }
}

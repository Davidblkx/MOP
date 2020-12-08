namespace MOP.Core.Domain.RIP.Messaging
{
    public class Response : IResponse
    {
        public bool Valid { get; set; }

        public (int code, string message)? Error { get; set; }

        public object? Body { get; set; }

        public static Response Success(object? body = default)
            => new Response { Valid = true, Body = body };

        public static Response Success<T>(T body)
            => new Response { Valid = true, Body = body };

        public static Response Fail(int code, string message)
            => new Response { Valid = false, Error = (code, message) };

        public static Response Fail((int, string) error)
            => new Response { Valid = false, Error = error };
    }
}

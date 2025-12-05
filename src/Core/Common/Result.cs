using Core.Enums;

namespace Core.Common;

public class Result<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public string? Message { get; }
    public EResult ResultType { get; }
    public IReadOnlyDictionary<string, string[]>? Errors { get; }
    public byte[]? Bytes { get; }

    private Result(bool success, EResult resultType, T? data = default, string? message = null, IReadOnlyDictionary<string, string[]>? errors = null, byte[]? bytes = null)
    {
        Success = success;
        ResultType = resultType;
        Data = data;
        Message = message;
        Errors = errors;
        Bytes = bytes;
    }

    public static Result<T> Ok(T? data = default, string? message = null)
        => new Result<T>(true, EResult.Ok, data, message);

    public static Result<T> Created(T data, string? message = null)
        => new Result<T>(true, EResult.Created, data, message);

    public static Result<T> NotFound(string message)
        => new Result<T>(false, EResult.NotFound, default, message);

    public static Result<T> Unauthorized(string? message = null)
        => new Result<T>(false, EResult.Unauthorized, default, message);

    public static Result<T> BadRequest(IReadOnlyDictionary<string, string[]>? errors = null, string? message = null)
        => new Result<T>(false, EResult.BadRequest, default, message, errors);

    public static Result<T> File(byte[]? bytes, string? message = null)
        => new Result<T>(true, EResult.File, default, message, null, bytes);
}

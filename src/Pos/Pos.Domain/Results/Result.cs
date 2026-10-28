using Pos.Domain.Results.Errors;

namespace Pos.Domain.Results;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("A successful result cannot contain an error.");
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("A failed result must contain an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public sealed class Result<TValue> : Result
{
    private Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    public TValue Value => IsSuccess
        ? field! : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

    public static implicit operator Result<TValue>(TValue value) => new(value, true, Error.None);
    public static implicit operator Result<TValue>(Error error) => new(default, false, error);
}

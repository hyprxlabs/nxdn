// ReSharper disable ParameterHidesMember
namespace Hyprx.Results;

public readonly struct ValueResult<TValue, TError> : IValueResult<TValue, TError>,
    IEquatable<ValueResult<TValue, TError>>
    where TValue : notnull
    where TError : notnull
{
    private readonly TValue? value;

    private readonly TError? error;

    public ValueResult()
    {
        if (typeof(TValue).IsValueType && !typeof(TValue).IsGenericType)
        {
            this.value = default(TValue);
            this.IsOk = true;
            this.error = default;
            return;
        }

        this.value = (TValue)Activator.CreateInstance(typeof(TValue)) !;
        this.IsOk = this.value is not null;
        this.error = default;
    }

    internal ValueResult(TValue? value, TError? error, bool isOk)
    {
        this.value = value;
        this.error = error;
        this.IsOk = isOk;
        if (isOk && value is null)
            throw new ArgumentNullException(nameof(value), "Value cannot be null when result is ok.");
    }

    public bool IsOk { get; }

    public bool IsError => !this.IsOk;

    public TValue Value
    {
        get
        {
            if (this.IsOk)
                return this.value!;

            throw new ResultException("No value present");
        }
    }

    public TError Error
    {
        get
        {
            if (!this.IsOk)
                return this.error!;

            throw new ResultException("No error present");
        }
    }

    object IValueResult.Value => this.Value;

    object IResult.Error => this.Error;

    public static implicit operator Task<ValueResult<TValue, TError>>(ValueResult<TValue, TError> result)
        => Task.FromResult(result);

    public static implicit operator ValueTask<ValueResult<TValue, TError>>(ValueResult<TValue, TError> result)
        => new(result);

    public static bool operator ==(ValueResult<TValue, TError> left, ValueResult<TValue, TError> right)
        => left.Equals(right);

    public static bool operator !=(ValueResult<TValue, TError> left, ValueResult<TValue, TError> right)
        => !left.Equals(right);

    public static ValueResult<TValue, TError> Ok(TValue value)
        => new(value, default, true);

    public static ValueResult<TValue, TError> Fail(TError error)
        => new(default, error, false);

    public override int GetHashCode()
        => HashCode.Combine(this.IsOk, this.value, this.error);

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return !this.IsOk;

        if (obj is Result<TValue, TError> other2)
            return this.Equals(other2);

        if (obj is IValueResult<TValue, TError> other)
            return this.Equals(other);

        return false;
    }

    public bool Equals(IValueResult<TValue, TError>? other)
    {
        if (other is null)
            return !this.IsOk;

        if (!this.IsOk)
            return !other.IsOk;

        return this.value!.Equals(other.Value);
    }

    public bool Equals(ValueResult<TValue, TError> other)
    {
        if (this.IsOk != other.IsOk)
            return false;

        if (this.IsOk)
            return this.value!.Equals(other.value);

        return this.error!.Equals(other.error);
    }

    public ValueResult<TValue, TError> Inspect(Action<TValue> action)
    {
        if (this.IsOk)
            action(this.value!);

        return this;
    }

    public ValueResult<TValue, TError> InspectError(Action<TError> action)
    {
        if (!this.IsOk)
            action(this.error!);

        return this;
    }

    public bool IsOkAnd(Func<TValue, bool> predicate)
        => this.IsOk && predicate(this.value!);

    public bool IsErrorAnd(Func<TError, bool> predicate)
        => this.IsError && predicate(this.error!);

    public ValueResult<TOther, TError> Map<TOther>(Func<TValue, TOther> map)
        where TOther : notnull
        => this.IsOk ?
            new(map(this.value!), default, true) :
            new(default, this.error!, false);

    public ValueResult<TOther, TOtherError> Map<TOther, TOtherError>(
        Func<TValue, TOther> map,
        Func<TError, TOtherError> mapError)
        where TOther : notnull
        where TOtherError : notnull
        => this.IsOk ?
            new(map(this.value!), default, true) :
            new(default, mapError(this.error!), false);

    public ValueResult<TValue, TError> Or(TValue other)
        => this.IsOk ? this : Ok(other);

    public ValueResult<TValue, TError> Or(Func<TValue> other)
        => this.IsOk ? this : Ok(other());

    public TValue OrDefault(TValue other)
        => this.IsOk ?
            this.value! :
            other;

    public TValue OrDefault(Func<TValue> other)
        => this.IsOk ?
            this.value! :
            other();

    public ValueResult<TValue, TError> OrError(TError error)
        => this.IsError ? this : new(default, error, false);

    public ValueResult<TValue, TError> OrError(Func<TError> error)
        => this.IsError ? this : new(default, error(), false);

    public TError OrErrorDefault(TError other)
        => this.IsError ?
            this.error! :
            other;

    public TError OrErrorDefault(Func<TError> other)
        => this.IsError ?
            this.error! :
            other();

    bool IValueResult.TryGetValue(out object? value)
    {
        var res = this.TryGetValue(out var v);
        value = v;
        return res;
    }

    bool IResult.TryGetError(out object? error)
    {
        var res = this.TryGetError(out var e);
        error = e;
        return res;
    }

    public bool TryGetValue(out TValue? value)
    {
        value = this.value;
        return this.IsOk;
    }

    public bool TryGetError(out TError? error)
    {
        error = this.error;
        return !this.IsOk;
    }
}
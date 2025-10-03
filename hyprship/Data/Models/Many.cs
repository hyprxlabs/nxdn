using System.Collections;

namespace Hyprship.Data.Models;

public class Many<TOwner, TRelated, TValue> : IReadOnlyCollection<TRelated>
{
    private readonly TOwner owner;

    private readonly Func<TValue, TRelated> getter;

    private readonly ICollection<TValue> values;

    private readonly Func<TOwner, TRelated, TValue> factory;

    private readonly List<TValue> related = new();

    public Many(
        TOwner owner,
        ICollection<TValue> values,
        Func<TValue, TRelated> getter,
        Func<TOwner, TRelated, TValue> factory)
    {
        this.owner = owner;
        this.values = values;
        this.getter = getter;
        this.factory = factory;
    }

    public int Count => this.values.Count;

    public virtual bool Add(TRelated value)
    {
        foreach (var v in this.values)
        {
            var related = this.getter(v);
            if (related?.Equals(value) == true)
                return false;
        }

        var newValue = this.factory(this.owner, value);
        this.values.Add(newValue);
        return true;
    }

    public virtual bool AddRange(IEnumerable<TRelated> values)
    {
        var added = false;
        foreach (var value in values)
        {
            if (this.Add(value))
                added = true;
        }

        return added;
    }

    public void Clear()
    {
        this.values.Clear();
    }

    public virtual bool Remove(TRelated value)
    {
        foreach (var v in this.values)
        {
            var related = this.getter(v);
            if (related?.Equals(value) == true)
            {
                this.values.Remove(v);
                return true;
            }
        }

        return false;
    }

    public virtual bool Contains(TRelated value)
    {
        foreach (var v in this.values)
        {
            var related = this.getter(v);
            if (related?.Equals(value) == true)
                return true;
        }

        return false;
    }

    public virtual List<TRelated> ToList()
    {
        var list = new List<TRelated>();
        foreach (var v in this.values)
        {
            var related = this.getter(v);
            if (related != null)
                list.Add(related);
        }

        return list;
    }

    public IEnumerator<TRelated> GetEnumerator()
    {
        foreach (var v in this.values)
        {
            var related = this.getter(v);
            if (related != null)
                yield return related;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
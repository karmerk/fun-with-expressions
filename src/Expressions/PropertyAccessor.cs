using System.Linq.Expressions;
using System.Reflection;

namespace Expressions;

public sealed class PropertyAccessor
{
    public static PropertyAccessor<T, TProperty> From<T, TProperty>(Expression<Func<T, TProperty>> selector)
    {
        var property = (PropertyInfo)((MemberExpression)selector.Body).Member; // Some error handling might be nice

        return new PropertyAccessor<T, TProperty>(property);
    }
}

// TODO really dont like the name.. 
public sealed class PropertyAccessor<T, TProperty>
{
    private readonly PropertyInfo _property;

    private readonly Func<T, TProperty>? _getValue;
    private readonly Action<T, TProperty>? _setValue;

    public PropertyAccessor(PropertyInfo property)
    {
        _property = property;

        _getValue = CreateGetValue();
        _setValue = CreateSetValue();
    }

    public TProperty GetValue(T obj)
    {
        if (_getValue == null)
        {
            throw new NotSupportedException($"{typeof(T)}.{_property.Name} does not support get");
        }

        return _getValue(obj);
    }

    public void SetValue(T obj, TProperty value)
    {
        if (_setValue == null)
        {
            throw new NotSupportedException($"{typeof(T)}.{_property.Name} does not support set");
        }

        _setValue(obj, value);
    }

    private Func<T, TProperty>? CreateGetValue()
    {
        if (_property.CanRead)
        {
            // TODO Make this return null if no get is available (why should it not be?)

            var get = _property.GetGetMethod() ?? throw new InvalidOperationException("Noooooo!");

            var variable = Expression.Variable(typeof(T));
            var call = Expression.Call(variable, get);

            return Expression.Lambda<Func<T, TProperty>>(call, variable).Compile();
        }

        return null;
    }

    private Action<T, TProperty>? CreateSetValue()
    {
        // TODO now this is way way way more exciting

        // record classes are special
        // Is it possible to make a "with { property = value }" expression..
        // If so the "SetValue" must also return the modified instance of T

        // record classes should not at all be accessed this way, instead this should properly
        // be some other class thats capable of handling immutable types

        if (_property.CanWrite)
        {
            var set = _property.GetSetMethod() ?? throw new InvalidOperationException("Noooooo!");

            var variable = Expression.Variable(typeof(T));
            var parameter = Expression.Variable(typeof(TProperty));
            var call = Expression.Call(variable, set, parameter);

            return Expression.Lambda<Action<T, TProperty>>(call, variable, parameter).Compile();
        }

        return null;
    }
}
using System;
using System.Reflection;
using FormsApi.Recalculate;

namespace Sample;

public class TestService
{
    public PostRecalculateEvent? SetNumericValue(TestModel model)
    {
        model.NumericField = 12345;
        return null;
    }

    public PostRecalculateEvent? ResetForm(TestModel model)
    {
        CopyMembers(new TestModel(), model);
        return null;
    }

    private static void CopyMembers<T>(T source, T target)
    {
        Type type = typeof(T);

        foreach (PropertyInfo prop in type.GetProperties())
        {
            if (!prop.CanRead || !prop.CanWrite) continue;

            object? value = prop.GetValue(source);
            prop.SetValue(target, value);
        }

        foreach (FieldInfo field in type.GetFields())
        {
            object? value = field.GetValue(source);
            field.SetValue(target, value);
        }
    }

    public PostRecalculateEvent? Reserialize(TestModel model)
    {
        return null;
    }
}

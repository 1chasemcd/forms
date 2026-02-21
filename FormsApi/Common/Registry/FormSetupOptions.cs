using System;
using FormsApi.Builder;

namespace FormsApi.Common.Registry;

public interface IFormSetupOptions
{
    IFormSetupOptions Add(string path, FormBuilder<object> builder);
}
internal class FormSetupOptions : IFormSetupOptions
{
    private readonly ICollection<KeyValuePair<string, FormBuilder<object>>> _builders = [];
    public IFormSetupOptions Add(string path, FormBuilder<object> builder)
    {
        _builders.Add(new KeyValuePair<string, FormBuilder<object>>(path, builder));
        return this;
    }
    public void Configure(FormRegistry registry)
    {
        foreach (KeyValuePair<string, FormBuilder<object>> builder in _builders)
        {
            registry.Add(builder.Key, builder.Value);
        }
    }
}

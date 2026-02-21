using System.Collections.Concurrent;
using FormsApi.Builder;
using FormsApi.Form;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;


public class FormRegistry(ILogger<FormRegistry> logger)
{
    private readonly ConcurrentDictionary<string, FormModel> _forms = [];
    internal void Add(string path, FormBuilder<object> builder)
    {
        FormModel form = builder.Build();
        if (!_forms.TryAdd(path, form))
            logger.LogError("Already had form registered at path {path}", path);
    }

    internal FormModel? Get(string path)
    {
        _forms.TryGetValue(path, out FormModel? form);
        return form;
    }
}

using FormsApi.Form;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;


public class FormRegistry(ILogger<FormRegistry> logger) : BaseRegistry<string, FormModel>(logger);

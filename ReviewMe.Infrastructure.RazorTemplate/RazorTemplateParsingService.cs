using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.Extensions.Logging;
using ReviewMe.Core.Exceptions;
using ReviewMe.Core.Infrastructures;
using System.Diagnostics;
using System.Reflection;
using System.Text.Encodings.Web;

namespace ReviewMe.Infrastructure.RazorTemplates;

public class RazorTemplateParsingService : IRazorTemplateParsingService
{
    private readonly ILogger _logger;

    public RazorTemplateParsingService(ILogger<RazorTemplateParsingService> logger)
    {
        _logger = logger;
    }

    public async Task<string> RenderAsync<TModel>(string templatePath, TModel model)
    {
        var viewAssembly = typeof(IRazorTemplateParsingAssembly).Assembly;
        var relatedAssembly = RelatedAssemblyAttribute.GetRelatedAssemblies(viewAssembly, false).SingleOrDefault();

        if (relatedAssembly != null)
        {
            viewAssembly = relatedAssembly;
        }

        return await RenderInternalAsync(viewAssembly, templatePath, model);
    }

    public async Task<string> GetEmbeddedStaticTemplate(string templateName, string folderName)
    {
        if (string.IsNullOrEmpty(templateName))
        {
            throw new ArgumentNullException(nameof(templateName));
        }

        var type = typeof(IRazorTemplateParsingAssembly);
        var assembly = type.GetTypeInfo().Assembly;

        var embeddedResourceName = string.IsNullOrEmpty(folderName)
            ? string.Join(".", type.Namespace, templateName)
            : string.Join(".", type.Namespace, folderName, templateName);

        Stream resource = assembly.GetManifestResourceStream(embeddedResourceName)!;

        if (resource == null)
        {
            throw new ErrorTypeException(ErrorType.GenericEmailError, $"Unable to find the requested resource '{embeddedResourceName}'.");
        }

        using var streamReader = new StreamReader(resource);
        return await streamReader.ReadToEndAsync();
    }

    private async Task<string> RenderInternalAsync<TModel>(Assembly viewAssembly, string key, TModel model)
    {
        var razorCompiledItem = new RazorCompiledItemLoader().LoadItems(viewAssembly)
            .FirstOrDefault(item => item.Identifier == key);

        if (razorCompiledItem == null)
        {
            _logger.LogError(
                $"Unable to find template in {nameof(viewAssembly)}='{viewAssembly.FullName}' for the {nameof(key)}='{key}'");

            throw new ErrorTypeException(ErrorType.GenericEmailError,
                $"Unable to find template in {nameof(viewAssembly)}='{viewAssembly.FullName}' for the {nameof(key)}='{key}'");
        }

        return await GetOutput(viewAssembly, razorCompiledItem.Type.FullName!, model);
    }

    private async Task<string> GetOutput<TModel>(Assembly assembly, string razorCompiledItemTypeFullName,
        TModel model)
    {
        using (var output = new StringWriter())
        {
            var compiledTemplate = assembly.GetType(razorCompiledItemTypeFullName);

            if (compiledTemplate == null)
            {
                throw new ApplicationException("There was a problem to get assembly type for: " + razorCompiledItemTypeFullName);
            }

            var razorPage = (RazorPage)Activator.CreateInstance(compiledTemplate)!;

            if (razorPage == null)
            {
                throw new ApplicationException("Unable to activate razor page: " + razorCompiledItemTypeFullName);
            }

            if (razorPage is RazorPage<TModel> page)
            {
                AddViewData(page, model);
            }

            razorPage.ViewContext = new ViewContext
            {
                Writer = output
            };

            razorPage.DiagnosticSource = new DiagnosticListener("GetOutput");
            razorPage.HtmlEncoder = HtmlEncoder.Default;

            await razorPage.ExecuteAsync();

            return output.ToString();
        }

    }

    private static void AddViewData<TModel>(RazorPage<TModel> razorPage, TModel model)
    {
        razorPage.ViewData = new ViewDataDictionary<TModel>(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary())
        {
            Model = model
        };
    }
}
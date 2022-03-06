namespace ReviewMe.Core.Infrastructures;

public interface IRazorTemplateParsingService
{
    /// <summary>
    /// Renders a final HTML string from prerendered Razor *.cshtml template
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="templatePath">e.g. /EmailTemplates/EmailExample.cshtml</param>
    /// <param name="model">A model object used for razor template</param>
    /// <returns></returns>
    Task<string> RenderAsync<TModel>(string templatePath, TModel model);

    Task<string> GetEmbeddedStaticTemplate(string templateName, string folderName);
}
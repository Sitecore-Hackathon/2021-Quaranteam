using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hackathon.Foundation.Reflector
{
    public class ContentReflector
    {
        public static string VARIANTS_TEMPLATE_ID = "{FB3E3034-33F8-4CE8-BE98-DD05010F4C22}";
        public static string VARIANT_DEFINTION_TEMPLATE_ID = "{FB3E3034-33F8-4CE8-BE98-DD05010F4C22}";
        public static string SCRIBAN_TEMPLATE_ID = "{8FCD3CFE-8B3B-423E-8176-6A7C72CB43FC}";
        public static string TEMPLATE_ID = "{AB86861A-6030-46C5-B394-E8F99E8B87DB}";
        public static string TEMPLATE_SECTION_ID = "{E269FBB5-3750-427A-9149-7AA950B49301}";
        public static string TEMPLATE_FIELD_ID = "{455A3E98-A627-4B40-8035-E683A0331AC7}";
        public static string PAGE_CONTENT_RENDERING_ID = "{8B0393E9-EE25-4F0B-A5FD-777B3AD2C737}";
        public static string PAGE_LIST_RENDERING_ID = "{2E991AA4-4B08-43D2-AD2B-17C11E69A499}";

        public static bool BuildRenderings(string renderingFolderId)
        {
            var success = false;

            try
            {
                using (new SecurityDisabler())
                {
                    //First get the template folder item from the master database
                    Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                    Item renderingFolder = masterDb.GetItem(new ID(renderingFolderId));

                    // Copy page content to rendering folder
                    Item pageContentRendering = masterDb.GetItem(new ID(PAGE_CONTENT_RENDERING_ID));
                    pageContentRendering.CopyTo(renderingFolder, "Page Content");

                    // Copy page list to rendering folder
                    Item pageListRendering = masterDb.GetItem(new ID(PAGE_LIST_RENDERING_ID));
                    pageListRendering.CopyTo(renderingFolder, "Page List");

                    success = true;
                }
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception creatign renderings.", ex);
            }

            return success;
        }

        /// <summary>
        /// Create our template
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="parentItemId"></param>
        /// <param name="baseTemplates"></param>
        /// <param name="templateFields"></param>
        /// <returns></returns>
        public static bool BuildSitecoreTemplate(string templateName, string parentTemplateFolderId, IEnumerable<Item> baseTemplates, IDictionary<string,string> templateFields)
        {
            var success = false;

            try
            {
                using (new SecurityDisabler())
                {
                    //First get the template folder item from the master database
                    Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                    Item templateFolder = masterDb.GetItem(new ID(parentTemplateFolderId));

                    // Get the standard template items
                    TemplateItem standardTemplate = masterDb.GetItem(TEMPLATE_ID);
                    TemplateItem templateSection = masterDb.GetItem(TEMPLATE_SECTION_ID);
                    TemplateItem templateField = masterDb.GetItem(TEMPLATE_FIELD_ID);

                    // Insert our template
                    Item newTemplate = templateFolder.Add(templateName, standardTemplate);

                    // Adding field to template
                    newTemplate.Editing.BeginEdit();

                    // Add our base templates
                    StringBuilder baseTemplatesStringBuilder = new StringBuilder();
                    baseTemplatesStringBuilder.Append(standardTemplate.ID.ToString());
                    if (baseTemplates != null)
                    {
                        foreach (var baseTemplate in baseTemplates)
                        {
                            baseTemplatesStringBuilder.Append("|" + baseTemplate.ID.ToString());
                        }
                    }
                    newTemplate["__Base template"] = baseTemplatesStringBuilder.ToString();

                    // Add our section
                    Item newSection = newTemplate.Add("Data", templateSection);

                    // Add our template fields
                    foreach(var field in templateFields)
                    {
                        Item fieldItem = newSection.Add(field.Key, templateField);
                        fieldItem.Fields["Type"].Value = field.Value;
                    }

                    newTemplate.Editing.EndEdit();

                    success = true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception creating Sitecore template.", ex);
            }
            return success;
        }

        /// <summary>
        /// Builds a scriban template for our Sitecore template
        /// </summary>
        /// <param name="renderingName"></param>
        /// <param name="parentItemId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static bool BuildScribanRendering(string renderingName, string parentItemId, string templateId)
        {
            var success = false;

            try
            {
                //Again we need to handle security
                //In this example we just disable it
                using (new SecurityDisabler())
                {
                    //First get the rendering variants item from the master database
                    Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                    Item renderingVariantsItem = masterDb.GetItem(new ID(parentItemId));

                    // Get our variants templates
                    TemplateItem variantsTemplate = masterDb.GetTemplate(VARIANTS_TEMPLATE_ID);
                    TemplateItem variantDefinitionTemplate = masterDb.GetTemplate(VARIANT_DEFINTION_TEMPLATE_ID);
                    TemplateItem scribanTemplate = masterDb.GetTemplate(SCRIBAN_TEMPLATE_ID);

                    // Add our variants
                    Item variantsItem = renderingVariantsItem.Add(renderingName, variantsTemplate);

                    // Add our variant definition
                    Item variantDefintionItem = variantsItem.Add(renderingName, variantDefinitionTemplate);

                    // Add our variant definition
                    Item scribanTemplateItem = variantDefintionItem.Add(renderingName, scribanTemplate);

                    // Fill in the scriban template
                    scribanTemplateItem.Fields["Template"].Value = BuildScribanRenderingString(new ID(templateId));

                    success = true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception creating scriban rendering.", ex);
            }

            return success;
        }

        /// <summary>
        /// Iterates over fields in a template and builds a scriban rendering for the template.
        /// </summary>
        /// <param name="TemplateId">ID of the template we're rendering.</param>
        /// <returns>Scriban template string.</returns>
        public static string BuildScribanRenderingString(ID TemplateId)
        {
            StringBuilder scribanBuilder = new StringBuilder();

            try
            {
                // Retrieve our template
                Template template = TemplateManager.GetTemplate(TemplateId, Sitecore.Context.Database);
                TemplateField[] allFields = template.GetFields(true);

                // Add our template name
                scribanBuilder.Append($"<!-- {template.Name} -->");
                scribanBuilder.Append("<div>");


                // Iterate over the template and add renderers for each of the fields
                foreach (var templateField in allFields)
                {
                    scribanBuilder.Append($"<!-- {templateField.Name} -->");
                    scribanBuilder.Append("<div>");
                    scribanBuilder.Append($"{{{{ sc_field i_item '{templateField.Name}' }}}}");
                    scribanBuilder.Append("</div>");
                }
                scribanBuilder.Append("</div>");
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception building scriban template.", ex);
            }

            return scribanBuilder.ToString();

        }
    }
}
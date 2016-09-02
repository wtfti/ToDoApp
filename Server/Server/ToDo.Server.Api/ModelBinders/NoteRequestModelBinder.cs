namespace ToDo.Api.ModelBinders
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using Models.Note;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using Server.Common.Constants;

    public class NoteRequestModelBinder : IModelBinder
    {
        private readonly JsonSchemaGenerator jsonSchemaGenerator;
        private readonly IsoDateTimeConverter dateTimeConverter;
        private readonly JsonSchema schema;

        public NoteRequestModelBinder()
        {
            this.jsonSchemaGenerator = new JsonSchemaGenerator();
            this.dateTimeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = ValidationConstants.ExpireDateFormat
            };
            var type = typeof(NoteRequestModel);
            var schemaJson = this.jsonSchemaGenerator.Generate(type);
            schemaJson.Title = type.Name;

            this.schema = JsonSchema.Parse(schemaJson.ToString());
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(NoteRequestModel))
            {
                return false;
            }

            Task<string> content = actionContext.Request.Content.ReadAsStringAsync();
            string body = content.Result;

            JObject note = JObject.Parse(body);

            bool valid = note.IsValid(this.schema);
            if (!valid)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, 
                    MessageConstants.InvalidJsonFormat);
                return false;
            }

            try
            {
                NoteRequestModel jsonObj = JsonConvert.DeserializeObject<NoteRequestModel>(body, this.dateTimeConverter);
                
                if (!this.ValidateProperties(bindingContext, jsonObj))
                {
                    return false;
                }

                bindingContext.Model = jsonObj;
                return true;
            }
            catch (Exception)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, MessageConstants.InvalidDate);
                return false;
            }
        }

        /// <summary>
        /// Validate requested model using reflection</summary>
        /// <remarks>
        /// If there is any attribute which is not recognised
        /// by reflection, it will make ModelState invalid
        /// and throw a BadRequest</remarks>
        private bool ValidateProperties(ModelBindingContext bindingContext, NoteRequestModel jsonObj)
        {
            PropertyInfo[] props = jsonObj.GetType().GetProperties();

            foreach (var property in props)
            {
                var attributes = property.GetCustomAttributes(true);

                foreach (var attribute in attributes)
                {
                    var jsonPropertyAttribute = attribute as JsonPropertyAttribute;
                    var minAttribute = attribute as MinLengthAttribute;
                    var maxAttribute = attribute as MaxLengthAttribute;
                    bool isValid = true;
                    string errorMessage = string.Empty;
                    if (minAttribute != null)
                    {
                        isValid = minAttribute.IsValid(jsonObj.GetType().GetProperty(property.Name).GetValue(jsonObj, null));
                        if (!isValid)
                        {
                            errorMessage = minAttribute.FormatErrorMessage(property.Name);
                        }
                    }
                    else if (maxAttribute != null)
                    {
                        isValid = maxAttribute.IsValid(jsonObj.GetType().GetProperty(property.Name).GetValue(jsonObj, null));
                        if (!isValid)
                        {
                            errorMessage = maxAttribute.FormatErrorMessage(property.Name);
                        }
                    }
                    else if (jsonPropertyAttribute == null)
                    {
                        bindingContext.ModelState.AddModelError(
                            bindingContext.ModelName,
                            MessageConstants.UnknownAttribute);
                        return false;
                    }

                    if (!isValid)
                    {
                        bindingContext.ModelState.AddModelError(
                            bindingContext.ModelName,
                            errorMessage);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
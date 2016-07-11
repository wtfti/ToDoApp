namespace ToDo.Api.ModelBinders
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ValueProviders;
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
        private readonly JsonSchema jsonSchema;
        private readonly JsonSchema schema;

        public NoteRequestModelBinder()
        {
            this.jsonSchemaGenerator = new JsonSchemaGenerator();
            this.dateTimeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = ValidationConstants.ExpireDateFormat
            };
            var myType = typeof(NoteRequestModel);
            this.jsonSchema = this.jsonSchemaGenerator.Generate(myType);
            var schemaJson = this.jsonSchema;
            schemaJson.Title = myType.Name;

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

            JObject person = JObject.Parse(body);

            bool valid = person.IsValid(this.schema);
            if (!valid)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, MessageConstants.InvalidJsonFormat);
                return false;
            }

            try
            {
                NoteRequestModel jsonObj = JsonConvert.DeserializeObject<NoteRequestModel>(body, this.dateTimeConverter);

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
    }
}
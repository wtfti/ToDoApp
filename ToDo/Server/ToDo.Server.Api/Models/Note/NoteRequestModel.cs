namespace ToDo.Api.Models.Note
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Server.Common.Constants;

    public class NoteRequestModel
    {
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        [MinLength(ValidationConstants.TitleMinLenght)]
        [MaxLength(ValidationConstants.TitleMaxLenght)]
        public string Title { get; set; }

        [JsonProperty(Required = Required.Always)]
        [MinLength(ValidationConstants.ContentMinLenght)]
        [MaxLength(ValidationConstants.ContentMaxLenght)]
        public string Content { get; set; }

        [JsonProperty(Required = Required.Default)]
        public DateTime? ExpiredOn { get; set; }

        public string[] SharedWith { get; set; }
    }
}
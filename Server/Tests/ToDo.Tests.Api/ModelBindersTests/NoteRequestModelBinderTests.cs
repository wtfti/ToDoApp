namespace ToDo.Tests.Api.ModelBindersTests
{
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Metadata;
    using System.Web.Http.Metadata.Providers;
    using System.Web.Http.ModelBinding;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ToDo.Api.ModelBinders;
    using ToDo.Api.Models.Note;

    [TestClass]
    public class NoteRequestModelBinderTests
    {
        private NoteRequestModelBinder binder;
        private HttpControllerContext httpControllerContext;
        private EmptyModelMetadataProvider provider;
        private HttpActionContext httpActionContext;

        [TestInitialize]
        public void Init()
        {
            this.binder = new NoteRequestModelBinder();
            this.httpControllerContext = new HttpControllerContext();
            this.httpControllerContext.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/someUri");
            this.httpActionContext = new HttpActionContext();
            this.httpActionContext.ControllerContext = this.httpControllerContext;
            this.provider = new EmptyModelMetadataProvider();
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldPass()
        {
            this.httpControllerContext.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/someUri");
            this.httpControllerContext.Request.Content = new StringContent("{\"Title\":\"unit test\",\"Content\":\"test content\",\"ExpiredOn\":\"28/07/2016 03:30\",\"SharedWith\":[\"testUser\"]}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldPassOption2()
        {
            this.httpControllerContext.Request.Content = new StringContent("{\"Id\":\"30\",\"Title\":\"unit test\",\"Content\":\"test content\",\"ExpiredOn\":\"28/07/2016 03:30\",\"SharedWith\":[\"testUser\"]}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldPassOption3()
        {
            this.httpControllerContext.Request.Content = new StringContent("{\"Id\":\"636085272491684492\",\"Title\":\"test\",\"Content\":\"test c\",\"ExpiredOn\":\"\",\"SharedWith\":[\"testUser\"]}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldPassOption4()
        {
            this.httpControllerContext.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/someUri");
            this.httpControllerContext.Request.Content = new StringContent("{\"Title\":\"unit test\",\"Content\":\"test content\",\"ExpiredOn\":\"\",\"SharedWith\":[\"testUser\"]}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldReturnFalse()
        {
            // JSON is not valid because title is missing
            this.httpControllerContext.Request.Content = new StringContent("{\"Content\":\"test content\",\"ExpiredOn\":\"28/07/2016 03:30\"}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldReturnFalseOption2()
        {
            // JSON is not valid because content is missing
            this.httpControllerContext.Request.Content = new StringContent("{\"Title\":\"unit test\",\"ExpiredOn\":\"28/07/2016 03:30\"}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldReturnFalseOption3()
        {
            // JSON is not valid because date format is not valid
            this.httpControllerContext.Request.Content = new StringContent("{\"Title\":\"unit test\",\"Content\":\"test content\",\"ExpiredOn\":\"28.07.2016 03:30\"}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NoteRequestModelBinderShouldReturnFalseOption4()
        {
            // JSON is not valid because date format is not valid
            this.httpControllerContext.Request.Content = new StringContent("{\"Title\":\"unit test\",\"Content\":\"test content\",\"ExpiredOn\":\"28/07/2016\"}");
            var bindingContext = new ModelBindingContext();
            var metaData = new ModelMetadata(this.provider, null, null, typeof(NoteRequestModel), null);
            bindingContext.ModelMetadata = metaData;

            bool result = this.binder.BindModel(this.httpActionContext, bindingContext);

            Assert.IsFalse(result);
        }
    }
}

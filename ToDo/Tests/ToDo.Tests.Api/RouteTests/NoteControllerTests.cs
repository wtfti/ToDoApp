namespace ToDo.Tests.Api.RouteTests
{
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
    using ToDo.Api;
    using ToDo.Api.Controllers;
    using ToDo.Api.Models.Note;

    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {
            MyWebApi.IsRegisteredWith(WebApiConfig.Register)
                .WithBaseAddress("http://baseurl.net");
        }

        [TestMethod]
        public void AddNoteRoute()
        {
            // TODO test modelbinder and fix this method
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/Addnote")
                .WithHttpMethod(HttpMethod.Post)
                .WithJsonContent(@"{""Title"": ""expire"", ""Content"": ""create content""}")
                .To<NoteController>(a => a.AddNote(new NoteRequestModel()
                {
                    Title = "expire",
                    Content = "create content"
                }));
        }

        [TestMethod]
        public void GetNotesRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetNotes?page=5")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetNotes(5));
        }

        [TestMethod]
        public void GetNotesWithExpirationDateRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetNotesWithExpirateDate?page=5")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetNotesWithExpirateDate(5));
        }

        [TestMethod]
        public void RemoveNoteByIdRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/RemoveNoteById?id=5")
                .WithHttpMethod(HttpMethod.Delete)
                .To<NoteController>(a => a.RemoveNoteById(5));
        }

        [TestMethod]
        public void ChangeNoteTitleRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/ChangeNoteTitle?id=5&&newValue=test")
                .WithHttpMethod(HttpMethod.Put)
                .To<NoteController>(a => a.ChangeNoteTitle(5, "test"));
        }

        [TestMethod]
        public void ChangeNoteContentRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/ChangeNoteContent?id=5&&newValue=test")
                .WithHttpMethod(HttpMethod.Put)
                .To<NoteController>(a => a.ChangeNoteContent(5, "test"));
        }

        [TestMethod]
        public void SetNoteExpireDateRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/SetNoteExpireDate?id=5&&date=01 02 2016")
                .WithHttpMethod(HttpMethod.Put)
                .To<NoteController>(a => a.SetNoteExpireDate(5, "01 02 2016"));
        }

        [TestMethod]
        public void ChangeExpireDateRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/ChangeExpireDate?id=5&&date=01 02 2016")
                .WithHttpMethod(HttpMethod.Put)
                .To<NoteController>(a => a.ChangeExpireDate(5, "01 02 2016"));
        }
    }
}

namespace ToDo.Tests.Api.RouteTests
{
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
    using ToDo.Api;
    using ToDo.Api.Controllers;

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
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/Addnote")
                .WithHttpMethod(HttpMethod.Post)
                .WithJsonContent(@"{""Title"": ""expire"", ""Content"": ""create content""}")
                .To<NoteController>(a => a.AddNote(null));
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
                .ShouldMap("api/Note/GetNotesWithExpirationDate?page=5")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetNotesWithExpirationDate(5));
        }

        [TestMethod]
        public void RemoveNoteByIdRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/RemoveNoteById?id=5")
                .WithHttpMethod(HttpMethod.Delete)
                .To<NoteController>(a => a.RemoveNoteById("5"));
        }

        [TestMethod]
        public void GetCompletedNotesRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetCompletedNotes?page=1")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetCompletedNotes(1));
        }

        [TestMethod]
        public void SetCompleteRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/SetComplete?id=5")
                .WithHttpMethod(HttpMethod.Put)
                .To<NoteController>(a => a.SetComplete("5"));
        }

        [TestMethod]
        public void GetNotesFromTodayRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetNotesFromToday?page=1")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetNotesFromToday(1));
        }

        [TestMethod]
        public void ChangeNoteRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/ChangeNote")
                .WithHttpMethod(HttpMethod.Put)
                .WithJsonContent(@"{""Title"": ""expire"", ""Content"": ""create content""}")
                .To<NoteController>(a => a.ChangeNote(null));
        }

        [TestMethod]
        public void GetSharedNotesCountRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetSharedNotesCount")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetSharedNotesCount());
        }

        [TestMethod]
        public void GetNotesFromTodayCountRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetNotesFromTodayCount")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetNotesFromTodayCount());
        }

        [TestMethod]
        public void GetCompletedNotesCountRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetCompletedNotesCount")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetCompletedNotesCount());
        }

        [TestMethod]
        public void GetNotesCountRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetNotesCount")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetNotesCount());
        }

        [TestMethod]
        public void GetNotesWithExparationDateCountRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Note/GetNotesWithExparationDateCount")
                .WithHttpMethod(HttpMethod.Get)
                .To<NoteController>(a => a.GetNotesWithExparationDateCount());
        }
    }
}

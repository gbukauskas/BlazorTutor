using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBtransitions.ClassLibraryDatabase.DBContext;
using WEBtransitions.Components.Pages;
using WEBtransitions.Services;

namespace TestBusinessPart
{
    [Collection("DatabaseCollection")]
    public class UnitTestAppState : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private const string UserName = "Gediminas";
        private const string AppNameTest = "WEBtransitions";

        public UnitTestAppState(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// This test Writes state into the table "[dbo].[AppStates]".
        /// </summary>
        [Fact]
        public async Task Test1()
        {
            NorthwindContext ctxTest = _fixture.DbContext;
            var svc = new StateSvc(null);
            svc.SetDbContext(ctxTest);

            AppState state = new()
            {
                AppName = AppNameTest,
                UserId = UserName,
                ComponentName = "Test",

                SortState = "a_LastName",
                FilterFieldName = "FirstName",
                FilterFieldValue = "Nancy",
                FilterFieldMaxValue = "",
                FilterIsDateValue = 0,

                PagerButtonCount = 5,
                PagerRowCount = 100,
                PagerPageCount = 5,
                PagerPageNumber = 5,
                PagerPageSize = 20,

                PagerBaseUrl = "Customers",
                IsDeleted = 0
            };

            var key = new AppStateKey(state.AppName, state.UserId, state.ComponentName);
            var count1 = await svc.CountRecords(key);
            await svc.CreateEntity(state);
            var count2 = await svc.CountRecords(key);

            Assert.True(count1 < count2);
        }

        [Fact]
        public async Task Test2()
        {
            NorthwindContext ctxTest = _fixture.DbContext;
            var svc = new StateSvc(null);
            svc.SetDbContext(ctxTest);
            var key = new AppStateKey(AppNameTest, UserName, "Test");

            await Test1();
            var appState = await svc.GetEntityByIdAsync(key);
            Assert.True(appState != null && 
                        appState.AppName == AppNameTest && appState.UserId == UserName && appState.ComponentName == "Test");
        }
    }
}

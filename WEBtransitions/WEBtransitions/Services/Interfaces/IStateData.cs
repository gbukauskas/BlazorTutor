using WEBtransitions.ClassLibraryDatabase.CustomPager;

namespace WEBtransitions.Services.Interfaces
{
    public interface IStateData
    {
        StateForComponent GetState(string index, int buttonCount = 10, int pageSize = 9);
        void SetPagerState(string index, PgPostData currentPagerState);
    }
}

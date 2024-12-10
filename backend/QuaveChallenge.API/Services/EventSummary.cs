namespace QuaveChallenge.API.Services
{
    public class EventSummary
    {
        public object TotalPeople { get; internal set; }
        public int CheckedInPeople { get; internal set; }

        public int CheckedOutPeople { get; internal set; }

        public int NoCheckedInPeople { get; internal set; }
    }
}
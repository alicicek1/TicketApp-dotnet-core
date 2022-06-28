namespace TicketApp.Core.Util.Filter
{
    public class DataFilter
    {
        public string Filter { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortingProperty { get; set; }
        public string SortingDirection { get; set; }
    }
}
namespace App.Repository.Models.Messages
{
    public class ListRequest<T> : RequestBase
    {
        public ListRequest()
        {
            CurrentPage = 1;
            ItemsPerPage = int.MaxValue;
            NameOfSet = "list";
        }

        public string OrderBy { get; set; }
        public bool Ascending { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public T Value { get; set; }
        public string NameOfSet { get; set; }
    }
}
namespace HPSportsPlus.Models.AdvancedRetreival
{
    public class QueryParameters
    {
        const int _maxSize = 100; //maximum size which cannot be changed ie. const
        private int _size = 50; //defualt size
        public int Page { get; set; } = 1; // defualt page of 1 ie if no page is specified, then no items would be skiped
        public int Size
        {
            get { return _size; }

            set
            {
                _size = Math.Min(_maxSize, value); // return the minimum number between the max size and the size entered.

            }
        }

        //Properties for sorting
        public string Sortby { get; set; } = "Id";
        private string _sortOrder = "asc";

        public string SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if (value == "asc" || value == "desc")
                {
                    _sortOrder = value;
                }
            }
        }
    }
}

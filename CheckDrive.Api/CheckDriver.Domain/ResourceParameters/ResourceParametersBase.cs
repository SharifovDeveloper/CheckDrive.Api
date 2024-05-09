namespace CheckDrive.Domain.ResourceParameters
{
    public abstract class ResourceParametersBase
    {
        protected virtual int MaxPageSize { get; set; } = 25;

        public virtual string? SearchString { get; set; }
        public abstract string OrderBy { get; set; }

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 15;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}

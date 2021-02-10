using System;
using System.Configuration;

namespace SGE.Servicio.Vista.Shared
{
    public interface IPager
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
        int TotalPages { get; set; }
        int Skip { get; set; }
        string ActionName { get; set; }
        string FormName { get; set; }
        bool IsPreviousPage { get; }
        bool IsNextPage { get; }

    }

    [Serializable]
    public class Pager : IPager
    {
        private int _totalCount;
        private int _pageNumber;
        private int _pageSize;

        public Pager()
        {

        }

        public Pager(int totalCount, string formName, string actionToGo)
        {
            TotalCount = totalCount;
            FormName = formName;
            ActionName = actionToGo;
        }

        public Pager(int totalCount, int pageSize, string formName, string actionToGo)
        {
            PageSize = pageSize;
            TotalCount = totalCount;
            FormName = formName;
            ActionName = actionToGo;
        }

        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                   
                        _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount"));
                        if (_pageSize == 0)
                        {
                            _pageSize = 10;
                        }
                   


                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }
        public int PageNumber
        {
            get
            {
                if (_pageNumber == 0)
                {
                    _pageNumber = 1;
                }
                return _pageNumber;
            }
            set
            {
                _pageNumber = value == 0 ? 1 : value;
                Skip = (_pageNumber - 1) * PageSize;
            }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                TotalPages = SetPages(TotalCount, PageSize);
            }
        }
        public int TotalPages { get; set; }
        public int Skip { get; set; }
        public string ActionName { get; set; }
        public string FormName { get; set; }
        public bool IsPreviousPage
        {
            get { return (_pageNumber > 1); }
        }

        public bool IsNextPage
        {
            get { return _pageNumber < TotalPages; }
        }

        private static int SetPages(int pRows, int pPageSize)
        {
            int ret;
            
           
                ret = Convert.ToInt32(Math.Ceiling((decimal)pRows / pPageSize));
           
            
            return ret;
        }
    }
}

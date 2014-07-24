using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace COM.CF.Web
{
    public class FenPage
    {
        private string _nextString;

        private readonly int _pagecount;        

        public string _strsql;

        private readonly int _total;

        public readonly int CurPage;

        public readonly bool HaveNextPage;

        public string link;

        public readonly int maxI;

        private const int MaxPageNumber = 10;

        public readonly int minI;

        public readonly int PageCount;

        public readonly int PageNumber;

        public int NextPage
        {
            get
            {
                return HaveNextPage ? CurPage+1 : -1;
            }
        }

        public SqlConnection NotOpenedConnection
        {
            get;
            set;
        }

        private DataRowCollection _rows;
        public DataRowCollection Rows
        {
            get
            {
                if (_rows == null )
                {
                    if( _strsql != null)
                    {
                        if (NotOpenedConnection == null)
                        {
                            NotOpenedConnection = CFConfig.GetNotOpenConnection();
                        }
                        _rows = PubFunc.GetSQLRows(_strsql, NotOpenedConnection, minI, PageCount);
                    }
                    else
                    {
                        throw new CFException(enErrType.DevelopError, "分页集合未初始化！");
                    }
                }
                return _rows;
            }
        }

        public FenPage(int total,NameValueCollection form)
            :this(total,form,30)
        {
            
        }

        public FenPage(int total,NameValueCollection form,int defaultCount)
        {
            _nextString="";
            PageCount=PubFunc.GetDefaultInt(form["count"],defaultCount);
            _total=total;
            _pagecount=PageCount;
            PageNumber=Convert.ToInt32(Math.Ceiling((double)total/(double)PageCount));
            if(form["page"]!="end")
            {
                CurPage=PubFunc.GetInt(form["page"]);
            }
            else
            {
                CurPage=(PageNumber==0)?0:PageNumber-1;
            }
            minI=CurPage * PageCount;
            maxI=Math.Min(minI+PageCount-1,total-1);
            HaveNextPage=CurPage<PageNumber-1;
        }

        public FenPage(string strsql, int total, NameValueCollection form, string nextstring, int defaultPageCount)
        {
            _nextString = "";
            PageCount = PubFunc.GetDefaultInt(form["count"], defaultPageCount);
            _total = total;
            _pagecount = PageCount;
            _nextString = nextstring;
            _strsql = strsql;
            PageNumber = Convert.ToInt32(Math.Ceiling((double)total / (double)PageCount));
            if (form["page"] != "end")
            {
                CurPage = PubFunc.GetInt(form["page"]);
            }
            else
            {
                CurPage = (PageNumber == 0) ? 0 : PageNumber - 1;
            }
            minI = CurPage * PageCount;
            maxI = Math.Min(minI + PageCount - 1, total - 1);
            HaveNextPage = CurPage < PageNumber - 1;
        }


        public string GetFullPageList(string link)
        {
            string str;
            if (!HaveNextPage)
            {
                str = string.Concat(GetPageList(link), "");
            }
            else
            {
                string[] pageList = new string[7];
                pageList[0] = GetPageList(link);
                pageList[1] = "<a href=";
                pageList[2] = link;
                pageList[3] = _nextString;
                pageList[4] = "&page=";
                pageList[5] = Convert   .ToString(NextPage);
                pageList[6] = ">下一页>></a>";
                str = string.Concat(pageList);
            }
            return str;
        }

        public string GetFullPageList()
        {
            return GetFullPageList(link);
        }

        public string GetLastLink()
        {
            return string.Concat(link, _nextString, "&page=", Convert.ToString(PageNumber - 1));
        }

        public string GetPageList(string link)
        {
            link = string.Concat("<a href=", link, _nextString, "&page=");
            StringBuilder stringBuilder = new StringBuilder();
            int curPage = CurPage - CurPage % 10;
            int pageNumber = curPage + 10 - 1;

            if (curPage > 0)
            {
                stringBuilder.Append(link).Append(curPage - 1).Append("><<上十页</a> ");
            }
            if (pageNumber > PageNumber - 1)
            {
                pageNumber = PageNumber - 1;
            }
            if (curPage == 0)
            {
                curPage = 1;
            }
            int num1 = curPage;
            while (true)
            {
                if (num1 > pageNumber)
                {
                    break;
                }
                if (num1!=CurPage)
                {
                    stringBuilder.Append(link).Append(num1).Append(">").Append(num1 + 1).Append("</a> ");
                }
                else
                {
                    stringBuilder.Append(num1 + 1).Append(32);
                }
                num1++;
            }
            if ( pageNumber != PageNumber - 1)
            {
                stringBuilder.Append(link).Append(pageNumber + 1).Append(">下十页>></a> ");
            }
            return stringBuilder.ToString();
        }
    }
}

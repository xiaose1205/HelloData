using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Data
{
  public   class SQLliteHelper : DataBase
    {
        public SQLliteHelper() : base() { }
        public SQLliteHelper(string conn)
            : base(conn)
        {
            base.IsOpenTrans = false;
        }
        public override string ProviderName
        {
            get
            {
                return "System.Data.SQLite";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL.Interface
{
    public interface ILOADFILEDAL : IDAL
    {
        DataTable getDataTableSchema(string strKey);
        string genId(TFRegLibrary.Utility.RefRunningUtility.RefType objRefType, string strTblName, string strPrefix, string strYear);
        bool deleteOldData(string strDate);
        List<string> getListSqlInsert(DataTable dt);
        bool executSql(string strSql);
        bool executeImportTable(string strKey, string strDate);
    }
}

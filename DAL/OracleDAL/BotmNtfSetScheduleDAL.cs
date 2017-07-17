using COMMON;
using COMMON.Model;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TFLibrary.DBManager;
using TFRegLibrary.Utility;

namespace DAL.OracleDAL
{
    public class BotmNtfSetScheduleDAL : BaseDLL
    {
        public override DataTable getById(string strKey1, string strKey2)
        {
            string strSql = string.Format(RSSQL.SQL_GET_SCHEDULED, strOwnerInf, strKey1, strKey2);
            LogFileUtility.Sql(strProgram, strSql);
            return OracleManager.ExecuteQuery(strConStringInf, strSql);
        }

    }
}

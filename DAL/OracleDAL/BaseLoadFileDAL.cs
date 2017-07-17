// <copyright file="BaseLoadFileDAL.cs" company="Apar">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>CHIRAYUT WONGDANG</author>
// <date>19/05/2017 11:00:00 </date>
// <summary>Base class loadfile dataaccess doses not edit this internal business except found bug if you want to add more work you can override in your class.</summary>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TFRegLibrary.Utility;

namespace DAL.OracleDAL
{
    public abstract class BaseLoadFileDAL : BaseDLL
    {
        public virtual DataTable getDataTableSchema(string strKey)
        {
            TFRegEntity.Table.BOTM_SYS_VALUE entBotmSysValue = new TFRegEntity.Table.BOTM_SYS_VALUE(strConStringInf, strOwnerInf);
            DataTable dtGetSql = entBotmSysValue.Select(string.Format("  strsyskey = '{0}' ", strKey.Replace("'", "''")));
            DataTable dtStructure = new DataTable();

            if (dtGetSql != null && dtGetSql.Rows.Count > 0)
            {
                string strSql = dtGetSql.Rows[0]["STRSYSVAL"].ToString().Replace("{DB}", strOwnerInf);
                LogFileUtility.Sql(strProgram, strSql);
                dtStructure = TFLibrary.DBManager.OracleManager.ExecuteQuery(strConStringInf, strSql);
            }

            return dtStructure = dtStructure == null ? new DataTable() : dtStructure;
        }

        public virtual string genId(TFRegLibrary.Utility.RefRunningUtility.RefType objRefType, string strTblName, string strPrefix, string strYear)
        {
            return new TFRegLibrary.Utility.RefRunningUtility(strConStringInf, strOwnerInf, strTblName).GetRefRunning(objRefType, strPrefix, strYear);
        }
        public virtual bool executSql(string strSql)
        {
            TFRegLibrary.Utility.LogFileUtility.Sql(strProgram, strSql);
            return TFLibrary.DBManager.OracleManager.ExecuteNonQuery(strConStringInf, strSql) >= 0;
        }


        public virtual bool executeImportTable(string strKey, string strDate)
        {
            bool isComplete = false;
            TFRegEntity.Table.BOTM_SYS_VALUE entBotmSysValue = new TFRegEntity.Table.BOTM_SYS_VALUE(strConStringInf, strOwnerInf);
            DataTable dtGetSql = entBotmSysValue.Select(string.Format("  strsyskey = '{0}' ", strKey.Replace("'", "''")));
            DataTable dtStructor = new DataTable();

            if (dtGetSql != null && dtGetSql.Rows.Count > 0)
            {
                string strSql = dtGetSql.Rows[0]["STRSYSVAL"].ToString().Replace("{DB}", strOwnerInf);
                strSql = strSql.Replace("{0}", strDate);
                LogFileUtility.Sql(strProgram, strSql);
                isComplete = TFLibrary.DBManager.OracleManager.ExecuteNonQuery(strConStringInf, strSql) >= 0;
            }

            return isComplete;
        }
    }
}

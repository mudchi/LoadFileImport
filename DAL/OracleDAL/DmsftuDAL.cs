using COMMON;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL.OracleDAL
{
    public class DmsftuDAL : BaseLoadFileDAL, ILOADFILEDAL
    {

        TFRegEntity.Table.BOTT_NTF_DMSFTU entBottNtfDms = null;
        TFRegEntity.Table.BOTT_NTF_FTU entBottNtfFtu = null;

        public DmsftuDAL()
        {
            entBottNtfDms = new TFRegEntity.Table.BOTT_NTF_DMSFTU(strConStringInf, strOwnerInf);
            entBottNtfFtu = new TFRegEntity.Table.BOTT_NTF_FTU(strConStringInf, strOwnerInf);
        }
        public bool deleteOldData(string strDate)
        {
            return entBottNtfDms.Delete(string.Format(" FTTRDT ='{0}' ", strDate)) >= 0 && entBottNtfFtu.Delete(string.Format(" FTTRDT ='{0}' ", strDate)) >= 0;
        }
        public List<string> getListSqlInsert(DataTable dt)
        {
            return entBottNtfDms.GetInsertStatement(dt);
        }


    }
}

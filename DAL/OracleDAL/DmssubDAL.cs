using COMMON;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL.OracleDAL
{
    public class DmssubDAL : BaseLoadFileDAL, ILOADFILEDAL
    {

        TFRegEntity.Table.BOTT_NTF_DMSSUB entBottNtfDms = null;
        TFRegEntity.Table.BOTT_NTF_SUBREPEATING entBottNtfSub = null;

        public DmssubDAL()
        {
            entBottNtfDms = new TFRegEntity.Table.BOTT_NTF_DMSSUB(strConStringInf, strOwnerInf);
            entBottNtfSub = new TFRegEntity.Table.BOTT_NTF_SUBREPEATING(strConStringInf, strOwnerInf);
        }
        public bool deleteOldData(string strDate)
        {
            return entBottNtfDms.Delete(string.Format(" FTTRDT ='{0}' ", strDate)) >= 0 && entBottNtfSub.Delete(string.Format(" FTTRDT ='{0}' ", strDate)) >= 0; 
        }

        public List<string> getListSqlInsert(DataTable dt)
        {
            return entBottNtfDms.GetInsertStatement(dt);
        }

       
    }
}

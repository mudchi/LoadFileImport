using COMMON;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL.OracleDAL
{
    public class DmsftxDAL : BaseLoadFileDAL, ILOADFILEDAL
    {

        TFRegEntity.Table.BOTT_NTF_DMSFTX entBottNtfDms = null;
        TFRegEntity.Table.BOTT_NTF_FTX entBottNtfFtx = null;

        public DmsftxDAL()
        {
            entBottNtfDms = new TFRegEntity.Table.BOTT_NTF_DMSFTX(strConStringInf, strOwnerInf);
            entBottNtfFtx = new TFRegEntity.Table.BOTT_NTF_FTX(strConStringInf, strOwnerInf);
        }
        public bool deleteOldData(string strDate)
        {
            return entBottNtfDms.Delete(string.Format(" FTTRDT ='{0}' ", strDate)) >= 0 && entBottNtfFtx.Delete(string.Format(" FTTRDT ='{0}' ", strDate)) >= 0;
        }

        public List<string> getListSqlInsert(DataTable dt)
        {
            return entBottNtfDms.GetInsertStatement(dt);
        }

       
    }
}

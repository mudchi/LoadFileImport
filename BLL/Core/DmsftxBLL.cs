// <copyright file="DmsftxBLL.cs" company="Apar">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>CHIRAYUT WONGDANG</author>
// <date>19/05/2017 13:28:04 </date>
// <summary>Loadfile FTX</summary>
using BLL.Interface;
using COMMON;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFRegLibrary.Utility;
using System.Data;

namespace BLL.Core
{
    public class DmsftxBLL : BaseLoadfileBLL, ILOADFILEBLL
    {
        public DmsftxBLL() : base()
        {
            _LoadFileDAL = (ILOADFILEDAL)_applicationContext["DmsftxDAL"];

        }
        public override void setProgram(string strProgram)
        {
            strProgramLog = strProgram;
            _LoadFileDAL.setProgram(strProgramLog);
            _BottLogHeaderDAL.setProgram(strProgramLog);
            _BottLogDetailDAL.setProgram(strProgramLog);
            objRefType = RefRunningUtility.RefType.Transaction;
            strGenIdTblName = AppConfig.GenIdFtxTblName;
            strGenIdPrefixName = AppConfig.GenIdFtxPreFixName;
            strPrefixSysKeyImport = AppConfig.SYSKEY_IMPORT_FTX;
        }
     
    }
}

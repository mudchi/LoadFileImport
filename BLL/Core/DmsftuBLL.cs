// <copyright file="DmsftuBLL.cs" company="Apar">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>CHIRAYUT WONGDANG</author>
// <date>19/05/2017 13:28:34 </date>
// <summary>Loadfile FTU</summary>
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
    public class DmsftuBLL : BaseLoadfileBLL, ILOADFILEBLL
    {
        public DmsftuBLL() : base()
        {
            _LoadFileDAL = (ILOADFILEDAL)_applicationContext["DmsftuDAL"];

        }
        public override void setProgram(string strProgram)
        {
            strProgramLog = strProgram;
            _LoadFileDAL.setProgram(strProgramLog);
            _BottLogHeaderDAL.setProgram(strProgramLog);
            _BottLogDetailDAL.setProgram(strProgramLog);
            objRefType = RefRunningUtility.RefType.Transaction;
            strGenIdPrefixName = AppConfig.GenIdFtuPreFixName;
            strPrefixSysKeyImport = AppConfig.SYSKEY_IMPORT_FTU;
        }

    }
}

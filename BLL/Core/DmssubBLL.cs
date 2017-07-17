// <copyright file="DmssubBLL.cs" company="Apar">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>CHIRAYUT WONGDANG</author>
// <date>19/05/2017 13:28:00 </date>
// <summary>Loadfile Subrepeating</summary>
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
    public class DmssubBLL : BaseLoadfileBLL, ILOADFILEBLL
    {
        public DmssubBLL() : base()
        {
            _LoadFileDAL = (ILOADFILEDAL)_applicationContext["DmssubDAL"];

        }
        public override void setProgram(string strProgram)
        {
            strProgramLog = strProgram;
            _LoadFileDAL.setProgram(strProgramLog);
            _BottLogHeaderDAL.setProgram(strProgramLog);
            _BottLogDetailDAL.setProgram(strProgramLog);
            objRefType = RefRunningUtility.RefType.SubRepeating;
            strGenIdPrefixName = AppConfig.GenIdSubPreFixName;
            strPrefixSysKeyImport = AppConfig.SYSKEY_IMPORT_SUB;
        }

    }
}

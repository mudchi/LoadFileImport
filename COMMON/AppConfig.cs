using COMMON.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace COMMON
{
    public static class AppConfig
    {
        public static List<SCHEDULED_CONTROL> LIST_SCHEDULED;
        public static string ORACLE_INF
        {
            get { return ConfigurationManager.AppSettings["ORACLE_INF"].ToString().Trim(); }
        }
        public static string ORACLE_TN
        {
            get { return ConfigurationManager.AppSettings["ORACLE_TN"].ToString().Trim(); }
        }

        public static string ORACLE_DWH
        {
            get { return ConfigurationManager.AppSettings["ORACLE_DWH"].ToString().Trim(); }
        }

        public static string InfDB
        {
            get { return ConfigurationManager.AppSettings["InfDB"].ToString().Trim(); }
        }
        public static string InfOwner
        {
            get { return ConfigurationManager.AppSettings["InfOwner"].ToString().Trim(); }
        }

        public static string DwhDB
        {
            get { return ConfigurationManager.AppSettings["DWHDB"].ToString().Trim(); }
        }
        public static string DwhOwner
        {
            get { return ConfigurationManager.AppSettings["DWHOwner"].ToString().Trim(); }
        }

        public static string TNDB
        {
            get { return ConfigurationManager.AppSettings["TNDB"].ToString().Trim(); }
        }
        public static string TNOwner
        {
            get { return ConfigurationManager.AppSettings["TNOwner"].ToString().Trim(); }
        }
        public static int TIME_RERUN
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["TimeRerun"].ToString().Trim()); }
        }

        public static string DEFAULTCULTURE
        {
            get { return ConfigurationManager.AppSettings["DefaultCulture"].ToString().Trim(); }
        }
        public static string TYPE_WEBCALL
        {
            get { return ConfigurationManager.AppSettings["TYPE_WEBCALL"].ToString().Trim(); }
        }
        public static string TYPE_AUTO
        {
            get { return ConfigurationManager.AppSettings["TYPE_AUTO"].ToString().Trim(); }
        }
        public static string PROGRAMNAME_DEFAULT
        {
            get { return ConfigurationManager.AppSettings["ProjectName"].ToString().Trim(); }
        }

        public static string ProjectName
        {
            get { return ConfigurationManager.AppSettings["ProjectName"].ToString().Trim(); }
        }
        public static string Description
        {
            get { return ConfigurationManager.AppSettings["Description"].ToString().Trim(); }
        }

        public static string ProcessedFolder
        {
            get { return ConfigurationManager.AppSettings["ProcessedFolder"].ToString().Trim(); }
        }
        public static string TimeOutText
        {
            get { return ConfigurationManager.AppSettings["TimeOutText"].ToString().Trim(); }
        }
        public static string DefaultFileType
        {
            get { return ConfigurationManager.AppSettings["DefaultFileType"].ToString().Trim(); }
        }
        public static string DefaultFormatDate
        {
            get { return ConfigurationManager.AppSettings["DefaultFormatDate"].ToString().Trim(); }
        }

        public static string PREFIX_SYSKEY_SQL
        {
            get { return ConfigurationManager.AppSettings["PREFIX_SYSKEY_SQL"].ToString().Trim(); }
        }
        public static string PREFIX_SYSKEY_IMPORT
        {
            get { return ConfigurationManager.AppSettings["PREFIX_SYSKEY_IMPORT"].ToString().Trim(); }
        }
        public static string SYSKEY_IMPORT_FTX
        {
            get { return ConfigurationManager.AppSettings["SYSKEY_IMPORT_FTX"].ToString().Trim(); }
        }

        public static string SYSKEY_IMPORT_FTU
        {
            get { return ConfigurationManager.AppSettings["SYSKEY_IMPORT_FTU"].ToString().Trim(); }
        }
        public static string SYSKEY_IMPORT_SUB
        {
            get { return ConfigurationManager.AppSettings["SYSKEY_IMPORT_SUB"].ToString().Trim(); }
        }

        public static string GlobalFieldID
        {
            get { return ConfigurationManager.AppSettings["GLOBAL_FIELD_ID"].ToString().Trim(); }
        }

        public static string GlobalFieldSource
        {
            get { return ConfigurationManager.AppSettings["GLOBAL_FIELD_SOURCE"].ToString().Trim(); }
        }
        public static string GlobalTblRunningRef
        {
            get { return ConfigurationManager.AppSettings["GLOBAL_TABLE_RUNNING_REF"].ToString().Trim(); }
        }

        
        public static string GlobalFieldFile
        {
            get { return ConfigurationManager.AppSettings["GLOBAL_FIELD_FILE"].ToString().Trim(); }
        }
        public static string GlobalFieldSourceValue
        {
            get { return ConfigurationManager.AppSettings["GLOBAL_FIELD_SOURCE_VALUE"].ToString().Trim(); }
        }
        public static string GenIdFtxTblName
        {
            get { return ConfigurationManager.AppSettings["GEN_ID_FTX_TBL_NAME"].ToString().Trim(); }
        }
        public static string GenIdFtxPreFixName
        {
            get { return ConfigurationManager.AppSettings["GEN_ID_FTX_PREFIX"].ToString().Trim(); }
        }

        public static string GenIdFtuPreFixName
        {
            get { return ConfigurationManager.AppSettings["GEN_ID_FTU_PREFIX"].ToString().Trim(); }
        }

        public static string GenIdSubPreFixName
        {
            get { return ConfigurationManager.AppSettings["GEN_ID_SUB_PREFIX"].ToString().Trim(); }
        }
        

    }
}

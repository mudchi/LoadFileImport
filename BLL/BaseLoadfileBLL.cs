// <copyright file="BaseLoadfileBLL.cs" company="Apar">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>CHIRAYUT WONGDANG</author>
// <date>19/05/2017 13:28:00 </date>
// <summary>Base class loadfile doses not edit this internal business except found bug if you want to add more work you can override in your class.</summary>

using COMMON;
using DAL.Interface;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TFRegLibrary.Utility;

namespace BLL
{
    public abstract class BaseLoadfileBLL : BaseBLL
    {
        #region Setting config program can change default by method setprogram
        /// <summary>
        /// Setting default format date for get filename ex.YYYYMMDD
        /// </summary>
        protected string strFormatDateFile = AppConfig.DefaultFormatDate;
        /// <summary>
        /// Setting for default type file import ex.txt
        /// </summary>
        protected string strTypeFile = AppConfig.DefaultFileType;
        /// <summary>
        /// Setting catch column name for generate id  ex.FTTXNID
        /// </summary>
        protected string strFieldId = AppConfig.GlobalFieldID;
        /// <summary>
        /// Setting catch column name for save filename ex.FTFILE
        /// </summary>
        protected string strFieldFile = AppConfig.GlobalFieldFile;
        /// <summary>
        /// Setting catch column name for save source data ex.FTSOURCE
        /// </summary>
        protected string strFieldSource = AppConfig.GlobalFieldSource;
        /// <summary>
        /// Setting for input value to column name strFieldSource ex.BLANKTRADE
        /// </summary>
        protected string strFieldSourceVal = AppConfig.GlobalFieldSourceValue;
        /// <summary>
        /// Setting table name running id ex.BOTT_NTF_RUNNING_REF
        /// </summary>
        protected string strGenIdTblName = AppConfig.GlobalTblRunningRef;
        /// <summary>
        /// Setting PrefixName for gentid ex.FTX,FTU
        /// </summary>
        protected string strGenIdPrefixName = "";
        /// <summary>
        /// Setting PrefixSysKey for get sql Schema ex.SQL_IM_BT_DMSFTX
        /// </summary>
        protected string strPrefixSysKey = "";
        /// <summary>
        /// Setting PrefixSysKeyImport for get sql to importdat ex.SQL_IM_BT_DMSFTU
        /// </summary>
        protected string strPrefixSysKeyImport = "";
        protected bool IsAuto = false;
        protected List<int> lstLineErrFill = new List<int>();
        protected List<int> lstLineErrSave = new List<int>();
        protected ILOADFILEDAL _LoadFileDAL = null;
        protected int intTotalrows = 0;
        protected int intTotalSub = 0;
        protected int intStep = 1;
        protected string strCurrentStep = "";
        /// <summary>
        /// Setting Reftype for gen id ex.RefType.Transaction
        /// </summary>
        protected TFRegLibrary.Utility.RefRunningUtility.RefType objRefType { get; set; }
        #endregion

        #region Abstract method
        protected virtual DataTable getSchema(string strKey)
        {
            return _LoadFileDAL.getDataTableSchema(strKey);
        }
        protected virtual string genId(TFRegLibrary.Utility.RefRunningUtility.RefType refType, string strTblName, string strPrefix, string strYear)
        {
            return _LoadFileDAL.genId(refType, strTblName, strPrefix, strYear);
        }
        protected virtual bool saveData(DataTable dtSchema, DateTime dteRunDate)
        {
            bool isComplete = false;
            LogFileUtility.Info(strProgramLog, "\t- Delete Old data");
            if (!_LoadFileDAL.deleteOldData(dteRunDate.ToString(strFormatDateFile))) return isComplete;
            LogFileUtility.Info(strProgramLog, "\t\t- Complete Delete Old data");

            LogFileUtility.Info(strProgramLog, "\t- Gen sql from datatable");
            List<string> lstSql = _LoadFileDAL.getListSqlInsert(dtSchema);
            LogFileUtility.Info(strProgramLog, "\t\t- Complete gen sql from datatable");

            LogFileUtility.Info(strProgramLog, "\t- Insert data to table");
            for (int i = 0; i < lstSql.Count; i++)
            {
                try
                {
                    if (!_LoadFileDAL.executSql(lstSql[i])) putListErrorSaveData(i);
                    else intTotalrows++;
                }
                catch (Exception ex)
                {
                    putListErrorSaveData(i);
                    LogFileUtility.Fatal(ex);
                }
            }
            LogFileUtility.Info(strProgramLog, "\t\t- Complete insert data to table");

            LogFileUtility.Info(strProgramLog, "\t- Import to table");
            if (!_LoadFileDAL.executeImportTable(strPrefixSysKeyImport, dteRunDate.ToString(strFormatDateFile))) return isComplete;
            LogFileUtility.Info(strProgramLog, "\t\t- Complete import to table");

            isComplete = true;
            return isComplete;
        }
        #endregion

        #region main method
        public virtual void start(string strRunby, DateTime dteRunDate, string strProgram, string strPathShared, string strFileName)
        {
            DirectoryInfo objDir = null;
            string strBackUpfile = "";

            try
            {
                strPrefixSysKey = AppConfig.PREFIX_SYSKEY_SQL + strProgram;
                strPrefixSysKeyImport = AppConfig.PREFIX_SYSKEY_IMPORT + strProgram;
                // set instance
                setProgram(strProgram);


                LogFileUtility.Info(strProgramLog, "*******************************************************************************");
                LogFileUtility.Info(strProgramLog, "Program Start On " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                LogFileUtility.Info(strProgramLog, "Default culture " + Thread.CurrentThread.CurrentCulture);
                LogFileUtility.Info(strProgramLog, "*******************************************************************************");


                LogFileUtility.Info(strProgramLog, string.Format("{0}.Insert LogHeader Runby : {1} ,Programname : {2} ,RunDate : {3},PathFile : {4}, FileName : {5}", (intStep++), strRunby.ToUpper(), strProgram, dteRunDate.ToString("dd/MM/yyyy"), strPathShared, strFileName));
                insertLogHeader(strRunby, dteRunDate, strProgram);


                LogFileUtility.Info(strProgramLog, strCurrentStep = ((intStep++) + ".Set program Mode : " + strRunby.ToUpper() + " and change end string of path file to \\"));
                if (strRunby.ToUpper() == AppConfig.TYPE_AUTO) IsAuto = true;
                if (!strPathShared.EndsWith("\\")) strPathShared += "\\";

                LogFileUtility.Info(strProgramLog, strCurrentStep = ((intStep++) + ".Get file"));
                LogFileUtility.Info(strProgram, "\t- Begin get Folder Name : " + strPathShared);
                objDir = new DirectoryInfo(strPathShared);
                LogFileUtility.Info(strProgram, "\t- End get Folder Name : " + strPathShared);

                LogFileUtility.Info(strProgram, "\t- Get file name : " + string.Format(strFileName, dteRunDate.ToString(strFormatDateFile)));
                FileInfo Files = getFileRun(objDir, string.Format(strFileName, dteRunDate.ToString(strFormatDateFile)));

                if (!IsAuto && Files == null)
                {
                    LogFileUtility.Info(strProgramLog, "\t\t- File not found.");
                    LogFileUtility.Info(strProgramLog, "\t- Webcall mode will copy file from backup file to rerun : Start");
                    if (rerunFileBackUp(dteRunDate.ToString(strFormatDateFile), strPathShared, strFileName))
                    {
                        LogFileUtility.Info(strProgramLog, "\t\t- Copy File from backup file to rerun : Successful.");
                        LogFileUtility.Info(strProgramLog, "\t\t- Start try get file again. ");
                        Files = getFileRun(objDir, string.Format(strFileName, dteRunDate.ToString(strFormatDateFile)));
                    }
                    else LogFileUtility.Info(strProgramLog, "\t\t- Copy File from backup file to rerun : Error.");
                }

                // final check file if not found write log error and close program
                if (Files == null)
                {
                    LogFileUtility.Info(strProgramLog, "\t\t- File not found.Stop Program error step getfile");
                    LogFileUtility.Info(strProgramLog, string.Format("File {0} not found", string.Format(strFileName, dteRunDate.ToString(strFormatDateFile))));
                    updateLogHeader("E", "1");
                    return;
                }
                LogFileUtility.Info(strProgramLog, string.Format("\t- Get file {0} complete", Files.FullName));


                LogFileUtility.Info(strProgramLog, strCurrentStep = ((intStep++) + ".Start backup file"));
                if (!string.IsNullOrEmpty(strBackUpfile = createBackUp(Files, strPathShared))) LogFileUtility.Info(strProgramLog, string.Format("\t- backup file complete", strBackUpfile));
                else LogFileUtility.Info(strProgramLog, string.Format("\t- Error backup file"));

                LogFileUtility.Info(strProgramLog, strCurrentStep = ((intStep++) + ".Prepare data from text file"));
                LogFileUtility.Info(strProgramLog, "\t- Spit line from textfile ");
                string[] arrDataInfile = getDataFromFile(Files);
                LogFileUtility.Info(strProgramLog, "\t- Data in file " + arrDataInfile.Length.ToString() + "Line");

                intTotalSub = arrDataInfile.Length >= 0 ? arrDataInfile.Length - 2 : arrDataInfile.Length;

                LogFileUtility.Info(strProgramLog, "\t- Get Schema database from key " + strPrefixSysKey);
                DataTable dtSchema = getSchema(strPrefixSysKey);
                LogFileUtility.Info(strProgramLog, "\t- GetTableName " + dtSchema.TableName + " Total Column :" + dtSchema.Columns.Count.ToString());

                LogFileUtility.Info(strProgramLog, "\t- fill data to datatable ");
                if (fillDataTable(dtSchema, arrDataInfile, Files.Name, dteRunDate.ToString("yy")))
                {
                    LogFileUtility.Info(strProgramLog, "\t- Complete fill data " + dtSchema.Rows.Count + " Rows");
                    insertLogDetail("", string.Format("Total fill datatable from file : {0}/{1} records", dtSchema.Rows.Count, intTotalSub), "", "FILL DataTable", "C");
                }
                else {
                    LogFileUtility.Info(strProgramLog, "\t- Error fill data");
                }




                LogFileUtility.Info(strProgramLog, strCurrentStep = ((intStep++) + ".Save data to database "));
                if (saveData(dtSchema, dteRunDate))
                {
                    LogFileUtility.Info(strProgramLog, "\t- Complete save data to database");
                }
                else {
                    LogFileUtility.Info(strProgramLog, "\t- Error save data to database");
                }


                LogFileUtility.Info(strProgramLog, strCurrentStep = ((intStep++) + ".Move file to backup folder."));
                LogFileUtility.Info(strProgramLog, "\t- Delete File : " + Files.Name);
                if (!deleteFile(Files)) LogFileUtility.Info(strProgramLog, "\t- Error Delete File");


                LogFileUtility.Info(strProgramLog, "\t- Move backup File : " + strBackUpfile);
                if (!string.IsNullOrEmpty(strBackUpfile) && moveFileToProcessed(strPathShared, strBackUpfile)) LogFileUtility.Info(strProgramLog, "\t- Move file complete.");
                else LogFileUtility.Info(strProgramLog, "\t- Error Move File");



                if (!lstLineErrFill.Any() && !lstLineErrSave.Any())
                {
                    insertLogDetail("", string.Format("Total Rows Insert : {0}/{1} records", intTotalrows, intTotalSub), "", "Insert Data", "C");
                    updateLogHeader("C", "0");
                }
                else
                {
                    if (lstLineErrSave.Any()) insertLogDetail("", " Error save data Line : " + string.Join(",", lstLineErrSave) + string.Format(" ( Total Rows Insert : {0}/{1} records. )", intTotalrows, intTotalSub), "", "Insert Data", "W");
                    if (lstLineErrFill.Any()) insertLogDetail("", " Error fill data Line : " + string.Join(",", lstLineErrFill) + string.Format(" ( Total Rows Insert : {0}/{1} records. )", intTotalrows, intTotalSub), "", "Insert Data", "W");
                    updateLogHeader("W", "0");
                }

            }
            catch (Exception ex)
            {
                // exception write to log file and database.
                LogFileUtility.Fatal(strProgramLog, ex);
                if (lstLineErrFill.Any()) insertLogDetail("", " Error fill data Line : " + string.Join(",", lstLineErrFill) + string.Format(" ( Total Rows Insert : {0}/{1} records. )", intTotalrows, intTotalSub), strCurrentStep, "", "E");
                if (lstLineErrSave.Any()) insertLogDetail("", " Error save data Line : " + string.Join(",", lstLineErrSave) + string.Format(" ( Total Rows Insert : {0}/{1} records. )", intTotalrows, intTotalSub), strCurrentStep, "", "E");
                insertLogDetail("", ex.ToString(), strCurrentStep, "", "E");
                updateLogHeader("E", "1");

            }
        }
        #endregion

        #region sub method
        protected virtual bool fillDataTable(DataTable dt, string[] arrData, string strFileName, string strYear)
        {
            bool isComplete = false;

            for (int i = 0; i < arrData.Length; i++)
            {
                try
                {
                    string[] arrItem = arrData[i].Split('|');
                    if (arrItem.Length <= 1)
                    {
                        lstLineErrFill.Add((i + 1));
                    }

                    if (arrItem[0].Trim() == "0") continue;
                    else if (arrItem[0].Trim() == "9") break;
                    else if (arrItem[0].Trim() != "1")
                    {
                        lstLineErrFill.Add((i + 1));
                        LogFileUtility.Info(strProgramLog, "Read Line " + (i + 1) + " : Error - Unknown record type (" + arrItem[0] + ")");
                        continue;
                    }
                    else if (dt.Columns.Count - 2 != arrItem.Length)
                    {
                        lstLineErrFill.Add((i + 1));
                        LogFileUtility.Info(strProgramLog, "Read Line " + (i + 1) + " : not equal");
                        continue;
                    }


                    DataRow drTemp = dt.NewRow();

                    int intflagskip = 0;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        try
                        {


                            Type conversionType = dt.Columns[j].DataType;
                            if (dt.Columns[j].ColumnName == strFieldId)
                            {
                                drTemp[j] = genId(objRefType, strGenIdTblName, strGenIdPrefixName, strYear); intflagskip++;
                            }
                            else if (dt.Columns[j].ColumnName == strFieldFile)
                            {
                                drTemp[j] = strFileName; intflagskip++;
                            }
                            else if (dt.Columns[j].ColumnName == strFieldSource)
                            {
                                drTemp[j] = strFieldSourceVal; intflagskip++;
                            }
                            else {

                                if (string.IsNullOrEmpty(arrItem[j + 1 - intflagskip]))
                                {
                                    drTemp[j] = DBNull.Value;
                                }
                                else {
                                    drTemp[j] = Convert.ChangeType(arrItem[j + 1 - intflagskip], conversionType);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogFileUtility.Info(strProgramLog, "\t- Error Line " + (i + 1).ToString() + " Column " + (j + 1).ToString() + " " + ex.Message);
                            LogFileUtility.Fatal(ex);
                            throw ex;
                        }

                    }

                    dt.Rows.Add(drTemp);
                }
                catch (Exception ex)
                {
                    lstLineErrFill.Add((i + 1));
                    LogFileUtility.Fatal(ex);
                    continue;
                }
            }
            isComplete = true;

            return isComplete;
        }

        protected virtual void putListErrorSaveData(int idx)
        {
            idx = idx + 2;
            lstLineErrSave.Add((lstLineErrFill.Where(w => w < idx).Count() + idx));
        }

        protected virtual string[] getDataFromFile(FileInfo objFile)
        {
            string[] arrData = null;
            try
            {
                string strData = File.ReadAllText(objFile.FullName, Encoding.Default).Trim();
                arrData = strData.Split(new[] { Constants.vbNewLine }, StringSplitOptions.None);

                if (arrData.Length < 2) arrData = strData.Split(new[] { Constants.vbCrLf }, StringSplitOptions.None);
                if (arrData.Length < 2) arrData = strData.Split(new[] { Constants.vbCr }, StringSplitOptions.None);
                if (arrData.Length < 2) arrData = strData.Split(new[] { Constants.vbLf }, StringSplitOptions.None);

            }
            catch (Exception ex)
            {
                LogFileUtility.Fatal(strProgramLog, ex);
            }

            return arrData;
        }
        protected virtual bool moveFileToProcessed(string strPathShared, string strBackUpfile)
        {
            bool isComplete = false;

            try
            {
                string strSourceFile = strPathShared + strBackUpfile;
                string strDesFile = strPathShared + AppConfig.ProcessedFolder + "\\" + strBackUpfile;

                DirectoryInfo dirInfo = new DirectoryInfo(strPathShared + AppConfig.ProcessedFolder);
                if (!dirInfo.Exists)
                {
                    LogFileUtility.Info(strProgramLog, "Start create folder " + strPathShared + AppConfig.ProcessedFolder);
                    dirInfo.Create();
                }
                File.Move(strSourceFile, strDesFile);
                isComplete = true;
            }
            catch (Exception ex)
            {
                LogFileUtility.Fatal(strProgramLog, ex);
            }
            return isComplete;
        }
        protected virtual FileInfo getFileRun(DirectoryInfo objDir, string strFileName)
        {
            FileInfo[] Files = null;
            try
            {
                Files = objDir.GetFiles(strFileName);
                if (Files.Length <= 0) return null;
            }
            catch (Exception ex)
            {
                LogFileUtility.Error(strProgramLog, ex);
            }
            // return one file newer 
            return Files == null ? null : Files.OrderByDescending(or => or.FullName).First();
        }
        protected virtual bool deleteFile(FileInfo objFile)
        {
            bool isComplete = false;

            try
            {
                if (objFile.Exists)
                {
                    LogFileUtility.Info(strProgramLog, "\t- Delete File " + objFile.Name + " was successful.");
                    objFile.Delete();
                    isComplete = true;
                }

            }
            catch (Exception ex)
            {
                LogFileUtility.Fatal(strProgramLog, ex);
            }
            return isComplete;
        }
        protected virtual string createBackUp(FileInfo objFile, string strPath)
        {
            string strBackUpFile = "";

            try
            {
                LogFileUtility.Info(strProgramLog, "\t- Begin Create Backup File : " + objFile.Name);
                strBackUpFile = "BACK_" + objFile.Name.Replace("." + strTypeFile, "") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + strTypeFile;
                objFile.CopyTo(strPath + strBackUpFile);
                LogFileUtility.Info(strProgramLog, "\t- Create Backup File " + objFile.Name + " as " + strBackUpFile + " was successful.");
            }
            catch (Exception ex)
            {
                strBackUpFile = "";
                LogFileUtility.Fatal(strProgramLog, ex);
            }
            return strBackUpFile;
        }
        protected virtual bool rerunFileBackUp(string strDateRun, string strPathShared, string strFileName)
        {
            bool isComplete = false;

            try
            {
                LogFileUtility.Info(strProgramLog, "\t\t- RerunFile BackUp " + strDateRun);

                DirectoryInfo dirInfo = new DirectoryInfo(strPathShared + AppConfig.ProcessedFolder);

                string[] strTimeFile = null;
                string strFilePathMax = string.Empty;

                LogFileUtility.Info("\t\t- Search last file in backup folder " + strPathShared + AppConfig.ProcessedFolder + string.Format("BACK_{0}_*.{1}", string.Format(strFileName.Replace("." + strTypeFile, ""), strDateRun), strTypeFile));

                FileInfo[] fileRerun = dirInfo.GetFiles(string.Format("BACK_{0}_*.{1}", string.Format(strFileName.Replace("." + strTypeFile, ""), strDateRun), strTypeFile));

                LogFileUtility.Info(strProgramLog, "\t\t- Get " + fileRerun.Length + " Files");
                string strMaxTime = "";
                for (int i = 0; i <= fileRerun.Length - 1; i++)
                {
                    strTimeFile = fileRerun[i].Name.Split('_');
                    if (string.IsNullOrEmpty(strMaxTime))
                    {
                        strMaxTime = strTimeFile.Last().Replace("." + strTypeFile, "").Trim();
                        strFilePathMax = fileRerun[i].FullName;
                    }
                    else
                    {
                        if (Convert.ToDecimal(strTimeFile.Last().Replace("." + strTypeFile, "").Trim()) > Convert.ToDecimal(strMaxTime))
                        {
                            strMaxTime = strTimeFile.Last().Replace("." + strTypeFile, "").Trim();
                            strFilePathMax = fileRerun[i].FullName;
                        }
                    }
                }

                LogFileUtility.Info(strProgramLog, "\t\t- Copy File " + strFilePathMax + " from folder Retrieved to share folder ");
                FileInfo strFileinFolder = new FileInfo(strFilePathMax);
                if (strFileinFolder.Length > 0)
                {
                    string strSourceFile = strFilePathMax;
                    string strDesFile = strPathShared;
                    strDesFile += strFileinFolder.Name.Replace("BACK_", "").Replace("_" + strFileinFolder.Name.Split(new char[] { '_' }).Last(), "." + strTypeFile);
                    LogFileUtility.Info(strProgramLog, "\t\t- Copy file from  " + strSourceFile + " to " + strDesFile);
                    File.Copy(strSourceFile, strDesFile, true);
                    LogFileUtility.Info(strProgramLog, "\t\t- Copy file complete");
                    isComplete = true;
                }

            }
            catch (Exception ex)
            {
                LogFileUtility.Fatal(strProgramLog, ex);
            }

            return isComplete;

        }
        #endregion 
    }
}

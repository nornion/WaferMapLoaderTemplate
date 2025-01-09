/***************************************************************************************************
 * Library type: NEDA Map loader
 * Descrition：  Load TXT wafer map to MapViewer for view and edit
 * Scheme:
 *      Namespace:  Nornion     -- Must be Nornion
 *      Class:      XXXXXX      -- Must be same as DLL file name and AssemblyName
 *      Properties:
 *          Title:      Name that displayed on Wafer Map Viewer menu
 *          Desc:       Description of lib features, display as tooltip
 *          FileNameList: List of file names that will be parsed in this libaray
 *          Icon:       Icon image: example -> SINF3.png (without path)
 *      Method:
 *          //This is the function you should compose
 *          public void Load()  -- Load() method called by Wafer Map Viewer
 *      Event:
 *          ProgressChanged:    Event to report program (int i)
 *          LoadComplete:       Event to return data (Dictionary<string, List<WDF>>, Dictionary<string, List<string>>)
 *          ReportError:        Event to report error (string msg)
 *      
 *-------------------------------------------------------------------------------------------------
 * -- VERSION | DATE     | AUTHOR NAME   | CHANGE
 * =================================================================================================
 * -- 1.0.0   | 20241226 | Eng Poh Zoh     | Initial release of SIN3 MapLoader
 **************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;

namespace Nornion
{
    //delegate to report progress
    public delegate void ProgressChangedEventHandler(int i);
    //delegate to return map result
    public delegate void LoadCompleteEventHandler(Dictionary<string, List<WDF>> MapListDict,
        Dictionary<string, List<string>> MapHeaderTextDict);
    //delegate to report error
    public delegate void ErrorHappendEventHandler(string msg);

    public class Sinf3MapLoader
    {
        #region Events
        //report parsing progress event, [Do not change name or type]
        public event ProgressChangedEventHandler ProgressChanged;
        //return result event [Do not change name or type]
        public event LoadCompleteEventHandler LoadComplete;
        //report error [Do not change name or type]
        public event ErrorHappendEventHandler ReportError;
        #endregion


        #region Constructions
        //Construct Function with properties initialized
        public Sinf3MapLoader()
        {
            Title = "SINF R3 MapLoader"; //display in Wafer Map Viewer Menu
            Desc = "Load SINF R3 Map"; //display in tooltip
            FileExtFilter = "Map Files|*.*"; //data extension filter
            Icon = "SINF3.png";//Icon.png, file name only, no path allowed, this file will be put in same directory as MapLoader.dll
        }
        #endregion


        #region Properties
        //Library title
        public string Title { get; set; }

        //Library description
        public string Desc { get; set; }

        //Icon of the library
        public string Icon { get; set; }

        //Target file extension
        public string FileExtFilter { get; set; }

        //Map data file list, will be set by Wafer Map Viewer
        public List<string> FileNameList { get; set; }

        //Map Header string contains original map header, which could be write back directly, it can be null
        private Dictionary<string, List<string>> MapHeaderTextDict = new Dictionary<string, List<string>>();
        #endregion


        #region Methods
        //Main method called by Wafer Map Viewer to parse all files in the list
        public void Load()
        {
            //Key=LOT_ID, Value=List of Wafers' data
            Dictionary<string, List<WDF>> MapListDict = new Dictionary<string, List<WDF>>();
            MapHeaderTextDict = new Dictionary<string, List<string>>();
            int i = 1;
            foreach (string FileName in FileNameList)
            {
                //Populate each map file data into WDF
                WDF map = LoadOneMap(FileName);
                if(map!=null)
                {
                    if(!MapListDict.ContainsKey(map.LOT_ID))
                    {
                        MapListDict[map.LOT_ID] = new List<WDF>() { map };
                    }
                    else
                    {
                        //Add WDF to existing lot's list
                        MapListDict[map.LOT_ID].Add(map);
                    }
                }
                //Report progress
                ProgressChanged((int)(i * 100.0 / FileNameList.Count));
                i++;
            }

            //all parsing complete, report data to Wafer Map Viewer, MapHeaderTextDict could be null
            LoadComplete(MapListDict, MapHeaderTextDict);
        }

        //private function to parse 1 file and return data in WDF format
        private WDF LoadOneMap(string FileName)
        {
            WDF MapObj = null;
            if (!string.IsNullOrEmpty(FileName) && File.Exists(FileName))
            {
                
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string LineText = "";
                bool FirstLine = true;
                bool ReadingMapData = false;
                string MapHeaderText = "";
                int BinDigits = 1;//How many digits of a bin number, could be 1, 2, 3 ...
                List<string> SKIPDI = new List<string>();//bin numbers which means no die or skip

                //Initialize Wafer Map Object
                MapObj = new WDF();
                MapObj.MapFileName = Path.GetFileName(FileName);
                MapObj.MinXCoord = MapObj.MinYCoord = MapObj.MaxYCoord = MapObj.MaxXCoord = 1;//SINF3 coordinate start from (1,1)
                MapObj.MapType = MapTypes.Bin;
                MapObj.PassBinList = new List<ushort>() { 1 };
                MapObj.BinSumDict = new Dictionary<ushort, int>();
                //indicate what's the original wafer map format
                MapObj.WaferInfoDict["MAPFMT"] = "SINF3"; //Default STDF if MAPFMT does not exist in WaferInfoDict

                while ((LineText = sr.ReadLine()) != null)
                {
                    LineText = LineText.Trim();
                    if (!string.IsNullOrEmpty(LineText))
                    {
                        //Header
                        if (!LineText.StartsWith("MAP001") && !ReadingMapData)
                        {
                            if (!LineText.StartsWith("SUMP") && !LineText.StartsWith("ORLOC") && !LineText.StartsWith("FNLOC"))
                            {
                                //SUMPAS, FNLOC, ORLOC maybe updated before write back to SINF3, need to write according to actual data
                                MapHeaderText += LineText + "\r\n";
                                //Add to Header fields to WaferInfoDict for futuer export to other format
                                if(LineText.Contains(" "))
                                {
                                    string[] val_arr = LineText.Split(' ');
                                    if (val_arr.Length > 1)
                                    {
                                        //HEADER FIELD that need to be write back to SINF3/SINF file without change
                                        MapObj.WaferInfoDict["HEADER:" + val_arr[0].Trim()] = val_arr[1].Trim();
                                    }
                                }
                            }

                            //First line check
                            if (FirstLine)
                            {
                                //First line of SINF3 map data
                                if (LineText.Contains("VERSID"))
                                {
                                    //Update first Header
                                    string[] val_arr = LineText.Split(' ');
                                    if (val_arr.Length > 1)
                                    {
                                        MapObj.LotInfoDict["JOB_REV"] = val_arr[1].Trim();
                                    }
                                }
                                else
                                {
                                    MapObj = null;
                                    //Invalid Map file
                                    ReportError("Error, Invalid SINF map file, first line should be [VERSID 3.x], extraction aborted! " + Path.GetFileName(FileName));
                                    break;
                                }
                            }
                            else
                            {
                                //VERSID 3.2
                                if (LineText.Contains(" "))
                                {
                                    string[] val_arr = LineText.Split(' ');
                                    switch (val_arr[0].Trim().ToUpper())
                                    {
                                        case "DESIGN":
                                            {
                                                if (val_arr.Length > 1)
                                                {
                                                    MapObj.LotInfoDict["PART_TYP"] = val_arr[1].Trim();
                                                    MapObj.LotInfoDict["JOB_NAM"] = val_arr[1].Trim(); 
                                                }
                                            }
                                            break;
                                        case "LOTMEA":
                                            {
                                                MapObj.LOT_ID = val_arr[1].Trim();
                                                MapObj.LotInfoDict["LOT_ID"] = MapObj.LOT_ID;
                                            }
                                            break;
                                        case "MAPID1":
                                            {
                                                MapObj.WAFER_ID = val_arr[1].Trim();
                                                MapObj.WaferInfoDict["WAFER_ID"] = MapObj.WAFER_ID;
                                            }
                                            break;
                                        case "WAFDIA":
                                            {
                                                string wf_siz = val_arr[1].Trim();
                                                MapObj.WaferInfoDict["WAFER_SIZ"] = wf_siz;
                                                if(!string.IsNullOrEmpty(wf_siz) && IsDigit(wf_siz))
                                                {
                                                    float ws = float.Parse(wf_siz);
                                                    MapObj.WaferInfoDict["WF_UNITS"] = "0";
                                                    if (ws > 0)
                                                    {
                                                        if (ws < 20)
                                                        {
                                                            MapObj.WaferInfoDict["WF_UNITS"] = "1";
                                                        }
                                                        else if( ws < 100)
                                                        {
                                                            MapObj.WaferInfoDict["WF_UNITS"] = "2";
                                                        }
                                                        else
                                                        {
                                                            MapObj.WaferInfoDict["WF_UNITS"] = "3";
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case "FNLOC1":
                                            {
                                                switch (val_arr[1].Trim())
                                                {
                                                    case "0":
                                                        {
                                                            MapObj.WaferNotch = NotchDirection.DOWN;
                                                            MapObj.WaferInfoDict["WF_FLAT"] = "D";
                                                        }
                                                        break;
                                                    case "90":
                                                        {
                                                            MapObj.WaferNotch = NotchDirection.LEFT;
                                                            MapObj.WaferInfoDict["WF_FLAT"] = "L";
                                                        }
                                                        break;
                                                    case "180":
                                                        {
                                                            MapObj.WaferNotch = NotchDirection.UP;
                                                            MapObj.WaferInfoDict["WF_FLAT"] = "U";
                                                        }
                                                        break;
                                                    case "270":
                                                        {
                                                            MapObj.WaferNotch = NotchDirection.RIGHT;
                                                            MapObj.WaferInfoDict["WF_FLAT"] = "R";
                                                        }
                                                        break;
                                                    case "360":
                                                        {
                                                            MapObj.WaferNotch = NotchDirection.UP;
                                                            MapObj.WaferInfoDict["WF_FLAT"] = "D";
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "PASBIN":
                                            {
                                                string s_pbs = val_arr[1].Trim();
                                                List<string> pbl = new List<string>();
                                                if (s_pbs.Contains(","))
                                                {
                                                    foreach(string s in s_pbs.Split(','))
                                                    {
                                                        pbl.Add(s.Trim());
                                                    }
                                                }
                                                else
                                                    pbl.Add(s_pbs);
                                                foreach (string bs in pbl)
                                                {
                                                    UInt16 b = UInt16.Parse(bs);
                                                    if(!MapObj.PassBinList.Contains(b))
                                                        MapObj.PassBinList.Add(b);
                                                }
                                            }
                                            break;
                                        case "PASB01":
                                            {
                                                string s_pbs = val_arr[1].Trim();
                                                List<string> pbl = new List<string>();
                                                if (s_pbs.Contains(","))
                                                {
                                                    foreach (string s in s_pbs.Split(','))
                                                    {
                                                        pbl.Add(s.Trim());
                                                    }
                                                }
                                                else
                                                    pbl.Add(s_pbs);
                                                foreach (string bs in pbl)
                                                {
                                                    UInt16 b = UInt16.Parse(bs);
                                                    if (!MapObj.PassBinList.Contains(b))
                                                        MapObj.PassBinList.Add(b);
                                                }
                                            }
                                            break;
                                        case "COMPRE":
                                            {
                                                BinDigits = int.Parse(val_arr[1].Trim());
                                            }
                                            break;
                                        case "TIMEST":
                                            {
                                                MapObj.WaferInfoDict["START_T"] = DateTime.ParseExact(val_arr[1].Trim(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                                if (!MapObj.LotInfoDict.ContainsKey("START_T"))
                                                    MapObj.LotInfoDict["START_T"] = MapObj.WaferInfoDict["START_T"];
                                            }
                                            break;
                                        case "SKIPDI":
                                            {
                                                if (!string.IsNullOrEmpty(val_arr[1].Trim()))
                                                {
                                                    if (val_arr[1].Trim().Contains(","))
                                                    {
                                                        foreach(string s in val_arr[1].Trim().Split(','))
                                                        {
                                                            SKIPDI.Add(s.Trim());
                                                        }
                                                    }
                                                    else
                                                        SKIPDI.Add(val_arr[1].Trim());
                                                }
                                            }break;
                                        case "ORLOC1":
                                            {
                                                //ORLOC1 3 -> Bottom Left
                                                switch(val_arr[1].Trim())
                                                {
                                                    case "1":
                                                        {
                                                            //MapObj.WaferInfoDict["POS_X"] = "L";
                                                            //MapObj.WaferInfoDict["POS_Y"] = "D";
                                                            MapObj.UpdateOriginLocation(OriginLocations.UpperRight);
                                                        }break;
                                                    case "2":
                                                        {
                                                            //MapObj.WaferInfoDict["POS_X"] = "R";
                                                            //MapObj.WaferInfoDict["POS_Y"] = "D";
                                                            MapObj.UpdateOriginLocation(OriginLocations.UpperLeft);
                                                        }
                                                        break;
                                                    case "3":
                                                        {
                                                            //MapObj.WaferInfoDict["POS_X"] = "R";
                                                            //MapObj.WaferInfoDict["POS_Y"] = "U";
                                                            MapObj.UpdateOriginLocation(OriginLocations.LowerLeft);
                                                        }
                                                        break;
                                                    case "4":
                                                        {
                                                            //MapObj.WaferInfoDict["POS_X"] = "L";
                                                            //MapObj.WaferInfoDict["POS_Y"] = "U";
                                                            MapObj.UpdateOriginLocation(OriginLocations.LowerRight);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "ROWCNT":
                                            {
                                                MapObj.MaxYCoord = (Int16)(Int16.Parse(val_arr[1].Trim()));//(Int16)(Int16.Parse(val_arr[1].Trim()) - 1);
                                            }
                                            break;
                                        case "COLCNT":
                                            {
                                                MapObj.MaxXCoord = (Int16)(Int16.Parse(val_arr[1].Trim()));//(Int16.Parse(val_arr[1].Trim()) - 1);
                                            }
                                            break;
                                        case "REFPX1":
                                            {
                                                MapObj.WaferInfoDict["CENTER_X"] = val_arr[1].Trim();
                                            }
                                            break;
                                        case "REFPY1":
                                            {
                                                MapObj.WaferInfoDict["CENTER_Y"] = val_arr[1].Trim();
                                            }
                                            break;
                                        case "XDIES1":
                                            {
                                                MapObj.WaferInfoDict["DIE_WID"] = val_arr[1].Trim();
                                            }
                                            break;
                                        case "YDIES1":
                                            {
                                                MapObj.WaferInfoDict["DIE_HT"] = val_arr[1].Trim();
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        //Map data
                        else
                        {
                            //Finish Header
                            if (!ReadingMapData)
                            {
                                if (MapHeaderTextDict.ContainsKey(MapObj.LOT_ID))
                                    MapHeaderTextDict[MapObj.LOT_ID].Add(MapHeaderText);
                                else
                                    MapHeaderTextDict[MapObj.LOT_ID] = new List<string>() { MapHeaderText };
                                ReadingMapData = true;
                                //Create Map Table
                                if (MapObj.MaxXCoord > 0 && MapObj.MaxYCoord > 0)
                                {
                                    //Initialize WDF MapTable 
                                    MapObj.InitializeMapTable(MapObj.WAFER_ID, MapObj.MaxXCoord, MapObj.MaxYCoord);
                                }
                            }

                            //Start map data processing
                            string[] val_arr = LineText.Split(' ');
                            string srid = val_arr[0].Replace("MAP", "").TrimStart('0');//row_id start from 1
                            if (string.IsNullOrEmpty(srid)) srid = "0";

                            int row_id = int.Parse(srid) - 1; //start from 0
                            Int16 y = (Int16)(row_id + MapObj.MinYCoord);
                            //If Origin at bottom
                            if(MapObj.WaferInfoDict.ContainsKey("POS_Y") && MapObj.WaferInfoDict["POS_Y"] == "U")
                            {
                                y = (Int16)(MapObj.MaxYCoord - row_id);
                            }

                            int col_id = 0;
                            string linedata = val_arr[1].Trim();
                            for (int j = 0; j < linedata.Length; j+=BinDigits)
                            {
                                Int16 x = (Int16)(col_id + MapObj.MinXCoord);
                                string sbc = "";
                                for(int bdi=0; bdi<BinDigits; bdi++)
                                {
                                    sbc += linedata[bdi + j].ToString();
                                }
                                //char bc = LineText[j];
                                if (!SKIPDI.Contains(sbc))
                                {
                                    UInt16 b = UInt16.Parse(sbc);
                                    if (MapObj.PassBinList.Contains(b))
                                        //Col_ID->X, Row_ID->Y
                                        MapObj.UpdateDie(x, y,  b, 'P');
                                    else
                                        MapObj.UpdateDie(x, y,  b, 'F');
                                }
                                col_id++;
                            }
                        }
                        FirstLine = false;
                    }
                }
                sr.Close();
                sr.Dispose();
                fs.Close(); fs.Dispose();
            }
            return MapObj;
        }
        #endregion


        #region Utility
        private bool IsDigit(string str)
        {
            return Regex.IsMatch(str, "[\\d]+");
        }
        private bool IsInt(string str)
        {
            return Regex.IsMatch(str, "[-]?[\\d]+");
        }
        private bool IsFloat(string str)
        {
            return Regex.IsMatch(str, "[-]?[\\d]+[.]*[\\d]*");
        }
        private UInt32? GetTestNum(string tn_str)
        {
            UInt32? tn = null;
            string[] tn_arr = tn_str.Split('.');
            if(tn_arr.Length>1)
            {
                //change numbers from 5 dig to 4 dig after dot(.)
                tn_arr[1] = tn_arr[1].PadLeft(4, '0');
            }
            tn_str = tn_arr[0] + tn_arr[1];
            if (IsDigit(tn_str))
                tn = UInt32.Parse(tn_str);
            return tn;
        }
        #endregion
    }
}

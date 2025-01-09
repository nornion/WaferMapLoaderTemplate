using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Reflection;
using System.IO;
using Nornion;

namespace ExtendedByDLL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Sinf3MapLoader sinf3 = new Sinf3MapLoader();


        #region BackgroundWorker_Related
        private void InitBackgroundWorker()
        {
            backgroundWorker1.DoWork += DoExtractionWork;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(UpdateProgressStatus);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExtractComplete);
        }
        //Functions for backgroundworker
        private void DoExtractionWork(object sender, DoWorkEventArgs e)
        {
            sinf3.Load();
        }
        public void ExtractComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusLabel.Text = "Loading complete, rendering result...";
            StatusProgressBar.Visible = false;
            Application.DoEvents();
            //Binding data
            if (MapListDict != null && MapListDict.Count>0)
            {
                List<string> LotList = new List<string>();
                foreach(string lot in MapListDict.Keys)
                {
                    LotList.Add(lot);
                }
                Curr_Lot = LotList[0];
                comboBox1.DataSource = LotList;
                if(MapListDict[Curr_Lot].Count>0)
                {
                    Curr_Wafer_Idx = 0;
                }

                if (LotList.Count > 0)
                {
                    Curr_Wafer_Idx = 0;
                    labelTabIdx.Text = "1/" + LotList.Count.ToString();
                    dataGridView1.DataSource = MapListDict[Curr_Lot][Curr_Wafer_Idx].MapTable;
                    buttonPreTable.Enabled = true;
                    buttonNextTable.Enabled = true;
                }
            }
            StatusLabel.Text = "result is rendered out.";
        }
        public void UpdateProgressStatus(object sender, ProgressChangedEventArgs e)
        {
            StatusLabel.Text = e.ProgressPercentage.ToString() + "%";
            StatusProgressBar.Value = e.ProgressPercentage;
        }

        //Function for SINF3_MapLoader Events
        public void ReportProgress_Function(int i)
        {
            if (i > 100) i = 100;
            backgroundWorker1.ReportProgress(i);
        }
        public void LoadComplete_Function(Dictionary<string, List<WDF>> map_list_dict,
                Dictionary<string, List<string>> MapHeaderTextDict)
        {
            MapListDict = map_list_dict;
        }
        public void ReportError_Function(string msg)
        {
            MessageBox.Show(msg);
        }
        #endregion



        #region GlobalVariables
        Dictionary<string, List<WDF>> MapListDict = new Dictionary<string, List<WDF>>();
        string Curr_Lot = "";
        int Curr_Wafer_Idx = -1;
        //Map Daa file list pass to library to extract
        List<string> LogFileNameList = new List<string>();
        #endregion



        #region main_code_block_of_form
        private void Form1_Load(object sender, EventArgs e)
        {
            //Setup openFileDialog 
            openFileDialog1.Filter = sinf3.FileExtFilter;
            labelTitle.Text = sinf3.Title;
            labelDesc.Text = sinf3.Desc;
            labelIcon.Text = sinf3.Icon;

            //setup SINF3 MapLoader events
            sinf3.LoadComplete += LoadComplete_Function;
            sinf3.ProgressChanged += ReportProgress_Function;
            sinf3.ReportError += ReportError_Function;

            //Initialize backgroundworker
            InitBackgroundWorker();
        }
        private void buttonInvokeMethod_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "background extract is started";
            StatusProgressBar.Visible = true;
            Application.DoEvents();
            dataGridView1.DataSource = null;
            labelTabIdx.Text = "0/0";
            //Start Invoke Async
            backgroundWorker1.RunWorkerAsync();
            
        }
        private void buttonPreTable_Click(object sender, EventArgs e)
        {
            if(Curr_Wafer_Idx>=1)
            {
                Curr_Wafer_Idx--;
                dataGridView1.DataSource = MapListDict[Curr_Lot][Curr_Wafer_Idx].MapTable;
                StatusLabel.Text = "Lot = " + Curr_Lot +", Wafer = " + MapListDict[Curr_Lot][Curr_Wafer_Idx].WAFER_ID;
                labelTabIdx.Text = (Curr_Wafer_Idx + 1).ToString() + "/" + MapListDict[Curr_Lot].Count;
            }
        }
        private void buttonNextTable_Click(object sender, EventArgs e)
        {
            if (Curr_Wafer_Idx < MapListDict[Curr_Lot].Count - 1)
            {
                Curr_Wafer_Idx++;
                dataGridView1.DataSource = MapListDict[Curr_Lot][Curr_Wafer_Idx].MapTable;
                StatusLabel.Text = "Lot = " + Curr_Lot + ", Wafer = " + MapListDict[Curr_Lot][Curr_Wafer_Idx].WAFER_ID;
                labelTabIdx.Text = (Curr_Wafer_Idx + 1).ToString() + "/" + MapListDict[Curr_Lot].Count;
            }
        }
        private void buttonOpenFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> fnList = new List<string>();
                foreach (string fn in openFileDialog1.FileNames)
                {
                    if (!string.IsNullOrEmpty(fn))
                    {
                        if (File.Exists(fn))
                        {
                            fnList.Add(fn);
                        }
                    }
                }

                if (fnList.Count > 0)
                {
                    //Set FileNameList
                    sinf3.FileNameList = fnList;
                    buttonInvokeMethod.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No file selected or file not exist!");
                }
            }
        }
        #endregion
    }
}

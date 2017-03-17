using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.XFeatures2D;
using Emgu.Util;

using TsRemoteLib;
using ToshibaTools;
using NeptuneClassLibWrap;
using CoordinatesConvert;
using Excel = Microsoft.Office.Interop.Excel;
using CosmeticImiTechCamera;
using System.Drawing.Imaging;


namespace CosmeticDetected
{
    public partial class Form1 : Form
    {
        //是否使用CCD
        public bool Use_Camera = true;
        //public bool Use_Camera = false;

        //是否要儲存不需要用的圖檔
        public bool Use_SaveImage = true;
        //public bool Use_SaveImage = false;

        //是否要計算重心
        //public bool Center_Gravity_Calculation = true;
        public bool Center_Gravity_Calculation = false;

        //是否要進行透視變換
        public bool Use_Perspective_Transform = true;
        //public bool Use_Perspective_Transform = false;

        //機械手臂是否連線
        public bool Mechanical_Arm_Connection = false;

        //CCD相機是否連線
        public ImiTechCamera Camera1 = null;

        Matrix matrix = new Matrix();
        ObjectTsRemote TsRemote = new ObjectTsRemote();
        //取得手臂座標
        TsPointS ArmPoint;
        delegate void GetArmPoint();   //顯示手臂座標位置


        Point[] Record_Points_Image;
        //Point[] Record_Points_Image_All;
        Point[] Record_Points_Mechanical_Arm;
        //Point[] Record_Points_Mechanical_Arm_All;

        PointF[] Record_Points_ImageF_All;
        PointF[] Record_Points_Mechanical_ArmF_All;

        VectorOfPoint Record_Points_Image_All_VOP = new VectorOfPoint();    //原始影像特徵點位
        VectorOfPoint Record_Points_Mechanical_Arm_All_VOP = new VectorOfPoint();    //原始影像特徵點位

        //double[,] Homography_Point = new double[3, 3];
        //VectorOfPoint Homography_Point = new VectorOfPoint();


        //VectorOfPoint Homography_Point = new VectorOfPoint();    //原始影像特徵點位
        //VectorOfPoint Record_Points_Mechanical_Arm_VOP = new VectorOfPoint();    //原始影像特徵點位



        //double[,] Record_Points_Mechanical_Arm;
        //double[,] Record_Points_Mechanical_Arm_All;

        /*int Get_Record_Points_Image = 0;
        int Get_Record_Points_Mechanical_Arm = 0;*/

        //*********            選擇定位標靶      *****************
        private Point RectStartPoint;                         // ROI 範圍座標起始點 (picture coordinates)
        private Rectangle Rect = new Rectangle();             // ImageBox ROI範圍 (picture coordinates)
        private Rectangle RealImageRect = new Rectangle();    // 實際影像ROI範圍 (image coordinates)  
        private int thickness = 3;

        //選取定位點的ROI用
        Image<Gray, Byte> ROI_gray = null;
        internal Image<Bgr, Byte> m_img_cpy = null;
        Image<Bgr, Byte> ROI_color = null;

        //bool IsSelectAnchor = false;
        //********************************************************

        //***********      //偵測軸心    *************************
        Thread GetAxis = null;    //偵測軸心
        //偵測手臂軸心所需使用的計數與判斷
        PointF[] AnchPoint = new PointF[50];
        PointF[] CenterP = new PointF[2];   //計算手臂軸心時，紀錄定位點偏移兩個點的座標
        bool IsGetAxisPos = false;    //是否已取得手臂軸心位置

        delegate void UpdadeListView(int index, string text, string ang);
        delegate void UpdateLabel(double a1, string ra, string ImgAxis, string ArmAxis);
        delegate void PRecord(PointF[] PointTrip, PointF Axis_P, double R1, double R2, double Ang1);

        delegate void RecordPPP(int index, PointF P);
        int Reindex = 1;
        double nowang = 0;
        double angle_pi1;
        double r0;
        double r1;

        //int PrecordIndex = 1;
        //**************************************************************

        //***********       取得轉換座標、定位校正****************
        //Thread GetTransCoordinate = null;   //取得轉換座標
        //Thread Localization = null;    //定位校正

        PointF[] shiftpoint = new PointF[2];   //定位移動目標點

        bool IsPosition = false;   //在定位時，在影像顯示位置
        //********************************************************

        //手臂軸心位置
        PointF AxisImgPos = new PointF();//Image
        static double AxisX = 0;//Arm
        static double AxisY = 0;

        //手臂復歸位置
        static double RevertPosX = 0;
        static double RevertPosY = 0;

        //手臂軸心影像中位移量
        static float ImgX = 0;
        static float ImgY = 0;

        //軸心量測時偏轉角度
        double RotAng = 10;

        //在影像中心點繪製十字的設定
        PointF Img_center = new PointF();

        int index = 0;
        //int status;    //***

        int recordcount = 0;

        EventWaitHandle WaitCapture = new EventWaitHandle(false, EventResetMode.AutoReset);    //暫停該程序，等待取像

        //定位校正時，是否有偵測到標靶
        bool IsAnchorPositionMode = false;

        //座標轉換系數
        static double ImgXMoveX = 0;
        static double ImgXMoveY = 0;
        static double ImgYMoveX = 0;
        static double ImgYMoveY = 0;
        static double LengthPropertionX = 0;
        static double LengthPropertionY = 0;

        //手臂與攝影機座標間的偏差角
        static double ArmImgAngle = 0;

        bool IsCoodinateConv = false;

        //******************************************

        int record_a = 0;

        //手臂高度位置
        static double UpLocalVal;
        static double DownLocalVal;
        static double PositionLocalVal;

        //*******    讓TextBox只能輸入數字   ***
        byte BS;
        string temp_text = "";
        //**************************************
        [DllImport("simple_grabber.dll")]
        static extern Point point_add(Point aa , Point bb);

        Point tt1, tt2, tt3;
        public Form1()
        {
            InitializeComponent();
            tt1.X = 10;
            tt1.X = 20; 

            tt2.X = 30;
            tt2.X = 40;
            tt3 = point_add(tt1, tt2);
            //Camera1.ImiTechCamera();

            //simple_grabber;
        }


        float[] peSrcX1 = { 0, 100, 0, 100 };
        float[] peSrcY1 = { 0, 0, 100, 100 };
        float[] peDestX1 = { 25, 75, 0, 100 };
        float[] peDestY1 = { 50, 50, 100, 100 };
        uint ucNumPts1 = 4;
        double[] pePTMatrix1 = new double[9];

        double Rotation_MatrixX, Rotation_MatrixY;

        double[,] Rotation_Matrix_Point = new double[2, 4];//特徵點旋轉
        int[,] Rotation_Matrix_Int = new int[2, 4];//特徵點旋轉加上偏移量的結果
        Ini Main_Ini = new Ini();

        float[,] srcp = { { 43, 18 }, { 280, 40 }, { 19, 223 }, { 304, 200 } };
        float[,] dstp = { { 0, 0 }, { 320, 0 }, { 0, 240 }, { 320, 240 } };
        float[,] homog = new float[3, 3];
        double[] homog_Global = new double[9];

        private void Form1_Load(object sender, EventArgs e)
        {
            Main_Ini.Read_ini_Cfg();

            label_Number_Points.Text = "紀錄點位數量：" + Main_Ini.Record_Points_Mechanical_Arm_All_length.ToString();

            for (int i = 0; i < Main_Ini.Record_Points_Mechanical_Arm_All_length; i++)
            {
                peSrcX1[i] = Main_Ini.Record_Points_Mechanical_Arm_All[i].X;
                peSrcY1[i] = Main_Ini.Record_Points_Mechanical_Arm_All[i].Y;
            }

            for (int i = 0; i < Main_Ini.Record_Points_Image_All_length; i++)
            {
                peDestX1[i] = Main_Ini.Record_Points_Image_All[i].X;
                peDestY1[i] = Main_Ini.Record_Points_Image_All[i].Y;
            }


            GenerateHomographyMatrix(peSrcX1, peSrcY1, peDestX1, peDestY1, ucNumPts1, pePTMatrix1);

            //getComponents_Point(pePTMatrix1);
            /*for (int i = 0; i < 4; i++)
            {
                pePTMatrix1[i] = Math.Round(pePTMatrix1[i], 5);
            }*/


            /*for (int i = 0; i < 4; i++)
            {
                COMMONK_PerformCoordinatesTransform(pePTMatrix1, peDestX1[i], peDestY1[i], Rotation_MatrixX, Rotation_MatrixY);
                Rotation_Matrix_Int[0, i] = (int)Rotation_MatrixX;
                Rotation_Matrix_Int[1, i] = (int)Rotation_MatrixY;
            }*/



            /*p_Point, q_Point, r_Point;
            translation_Point, scale_Point;
            shear_Point, theta_Point;*/
            /*for (int i = 0; i < 4; i++)
            {
                Rotation_Matrix_Point[0, i] = (peDestX1[i] * Math.Cos((theta_Point * (double)Math.PI) / 180)) - (peDestY1[i] * Math.Sin((theta_Point * (double)Math.PI) / 180));
                Rotation_Matrix_Point[1, i] = (peDestY1[i] * Math.Cos((theta_Point * (double)Math.PI) / 180)) + (peDestX1[i] * Math.Sin((theta_Point * (double)Math.PI) / 180));
                /*double aaa = (Original_Feature_points_Click[i].X - image_CountX1_ALL);
                double bbb = (Original_Feature_points_Click[i].Y - image_CountY1_ALL);
                Rotation_Matrix[0, i] = (aaa * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - (bbb * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountX1_ALL;
                Rotation_Matrix[1, i] = (bbb * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + (aaa * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountY1_ALL;*/
            //}


            //ImiTechCamera

            if (Use_Camera == true)
            {
                Camera1 = new ImiTechCamera(Display_Img, ImageMatch, Proc_time, framerate, Camera_comboBox);
                Camera1.MainForm = this;

                Camera1.ShowAnchorPosition = label11;
                Img_center = new PointF((float)ImiTechCamera.RoiRegion.Width / 2, (float)ImiTechCamera.RoiRegion.Height / 2);

                Camera1.MoveAssignPosition = AssignPositionMove;
                Camera1.NumericUpDownX = textBox2;
                Camera1.NumericUpDownY = textBox3;

                RevertPosX = 173.0;
                RevertPosY = 256.0;

                label21.Text = "上升高度位置：" + UpLocalVal.ToString();
                label22.Text = "下降高度位置：" + DownLocalVal.ToString();
                label23.Text = "定位高度位置：" + PositionLocalVal.ToString();
            }
        }

        //關閉程式後
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (Camera1 != null)
                {
                    if (Camera1.m_bPlay)
                    {
                        Camera1.m_bPlay = false;
                        Camera1.m_Thread.Join();
                        Camera1.Cam1.AcquisitionStop();
                    }
                    if (Camera1.Neptune1 != null)
                        Camera1.Neptune1.UninitLibrary();
                }
            }
            catch
            {
                Console.WriteLine(e.ToString());
            }
        }

        ////關閉程式中
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Use_Camera == true)
            {
                Camera1.stop();
                TsRemote.stopControl();
                TsRemote = null;

                if (Camera1 != null)
                {
                    Camera1.stop();
                    if (Camera1.m_Thread != null)
                        Camera1.m_Thread.Abort();
                }
            }
        }

        TsPointS Pount5, Pount1, Pount2, Pount3, Pount4;
        private void Process_Click(object sender, EventArgs e)
        {
            if (Process.Text == "執行跑點")
            {
                if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
                {
                    MessageBox.Show("機械手臂尚未連線");
                    return;
                }
                Pount5 = new TsPointS();
                Pount1 = new TsPointS();
                Pount2 = new TsPointS();
                Pount3 = new TsPointS();
                Pount4 = new TsPointS();


                //大手臂
                /*
                Pount5.X = 390;
                Pount5.Y = -150;
                Pount5.Z = 300;
                Pount5.C = 0;

                Pount1.X = 390;
                Pount1.Y = -150;
                Pount1.Z = 250;
                Pount1.C = 0;

                Pount2.X = 390;
                Pount2.Y = -20;
                //Pount2.Y = -50;
                Pount2.Z = 250;
                Pount2.C = 0;

                Pount4.X = 450;
                Pount4.Y = -90;
                //Pount4.Y = -150;
                Pount4.Z = 250;
                Pount4.C = 0;


                Pount3.X = 450;
                Pount3.Y = 40;
                //Pount3.Y = -50;
                Pount3.Z = 250;
                Pount3.C = 0;
                */


                //小手臂
                Pount5.X = -25;
                Pount5.Y = 200;
                Pount5.Z = 155;
                Pount5.C = 0;

                Pount1.X = -25;
                Pount1.Y = 200;
                Pount1.Z = 155;
                Pount1.C = 0;

                Pount2.X = -15;
                Pount2.Y = 250;
                //Pount2.Y = -50;
                Pount2.Z = 155;
                Pount2.C = 0;

                


                Pount3.X = 45;
                Pount3.Y = 250;
                //Pount3.Y = -50;
                Pount3.Z = 155;
                Pount3.C = 0;


                Pount4.X = 55;
                Pount4.Y = 200;
                //Pount4.Y = -150;
                Pount4.Z = 155;
                Pount4.C = 0;

                //public void SetGlobalPOINT(string name, int restore, TsPointS point);

                //public void SetGlobalPOINT(string name, int restore, TsPointS point);
                
                TsRemote._Robot.SetGlobalINT("UNDERTF", 0, 1);

                TsRemote._Robot.SetGlobalPOINT("UNDER(5)", 0, Pount5);
                TsRemote._Robot.SetGlobalPOINT("UNDER(1)", 0, Pount1);
                TsRemote._Robot.SetGlobalPOINT("UNDER(2)", 0, Pount2);
                TsRemote._Robot.SetGlobalPOINT("UNDER(3)", 0, Pount4);
                TsRemote._Robot.SetGlobalPOINT("UNDER(4)", 0, Pount3);

                TsRemote._Robot.SetGlobalINT("UNDERTF", 0, 1);

                //TsRemote._Robot.SetGlobalPOINT("UNDER(0)", 0, Pount4);

                //2017.02.16彥昌暫時拿掉
                /*Stopwatch watch = new Stopwatch();
                DateTime TT = DateTime.Now;
                ObjectDetector ObDetect = new ObjectDetector("07_07_1.bmp");
                ObDetect.ObjectDetected();
                Display_Img.Image = ObDetect.ColorImg;

                TimeSpan ts = DateTime.Now - TT;
                double GetOb = ts.TotalMilliseconds;
                Console.WriteLine("Time : " + GetOb.ToString());
                /*for (int i = 0; i < ObDetect.ObjectLocal.Count; i++)
                    Console.WriteLine("P" + (i + 1).ToString() + ":" + ObDetect.ObjectLocal[i].ToString());*/
                Process.Text = "停止跑點";
                Process.BackColor = Color.Red;

            }
            else if (Process.Text == "停止跑點")
            {
                TsRemote._Robot.SetGlobalINT("UNDERTF", 0, 0);
                Process.Text = "執行跑點";
                Process.BackColor = Color.Transparent;
                TsRemote._Robot.SetGlobalINT("UNDERTF", 0, 0);
            }

        }

        #region ImiTech Camera
        private void Search_Camera_Click(object sender, EventArgs e)
        {
            if (Use_Camera == true)
            {
                //Camera1 = new ImiTechCamera(Display_Img, ImageMatch, Proc_time, framerate, Camera_comboBox);
                Camera1.SearchCamera();
            }
        }

        #region Camera
        private void Camera_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Camera1.CameraSelection();
        }
        #endregion
        #endregion

        #region Toshiba Machain

        #region Control
        //設定速度按鈕
        private void btnOverride_Click(object sender, EventArgs e)
        {
            Override_Set(Convert.ToInt32(numericUpDown_Override.Value));
        }

        private void Override_Set(int Speed)
        {
            if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.CONNECTED) == 0)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }
            try
            {
                TsRemote._Robot.SetOVRD(Speed);
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("btnTest_Click: " + ex.Message);
            }
        }

        //手臂啟動連線按鈕
        private void btnStartControl_Click(object sender, EventArgs e)
        {
            if (TsRemote.startControl_Network(textBox_ToshibaIPAddr.Text, Convert.ToInt32(textBox_ToshibaPort.Text), 100, ObjectTsRemote.ALARMLEVEL.MessageOnly) == true)
            {
                GetPsnFbk();
                GetCoordinate();

                TsStatusMonitor status_m = TsRemote._Robot.GetStatusMonitor();

                numericUpDown_Override.Value = status_m.Override;

                btnStartControl.Visible = false;
                btnStopControl.Visible = true;
                Mechanical_Arm_Connection = true;
                //MessageBox.Show("連線成功");
                return;
            }
            string errmsg = "連線失敗\n";
            //已連線的情況下，可進行查詢錯誤可能原因
            errmsg += TsRemote.getStatusAll();
            TsRemote.stopControl();
            MessageBox.Show(errmsg);
        }

        //手臂關閉連線按鈕
        private void btnStopControl_Click(object sender, EventArgs e)
        {
            TsRemote.stopControl();
            btnStartControl.Visible = true;
            btnStopControl.Visible = false;
            Mechanical_Arm_Connection = false;
        }

        //手臂重置警告訊息按鈕
        private void btnResetAlarm_Click(object sender, EventArgs e)
        {
            TsRemote.ResetAll();
            btnResetAlarm.BackColor = Color.Transparent;
            btnResetAlarm.ForeColor = Color.Black;
        }

        #endregion

        #region Move
        //手臂移動按鈕
        private void btnStartMove_Click(object sender, EventArgs e)
        {
            if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }

            try
            {
                double StartX = ArmPoint.X;
                double StartY = ArmPoint.Y;

                ArmPoint = new TsPointS();
                ArmPoint.X = Convert.ToDouble(numericUpDownX.Value);
                ArmPoint.Y = Convert.ToDouble(numericUpDownY.Value);
                ArmPoint.Z = Convert.ToDouble(numericUpDownZ.Value);
                ArmPoint.C = Convert.ToDouble(numericUpDownC.Value);
                //以下指定無用途
                //TsRemote._Robot.MvConfig = (ConfigS)domainUpDown_Config.SelectedIndex;
                TsRemote._Robot.Moves(ArmPoint);
                Thread.Sleep(10);
                double EndX = ArmPoint.X;
                double EndY = ArmPoint.Y;

                if (IsGetAxisPos)
                {
                    //將實際手臂位置移動轉換成影像移動距離
                    ConvertCoordinate.ArmCovtoImg((EndX - StartX), (EndY - StartY), out ImgX, out ImgY);
                    AxisImgPos.X += ImgX;
                    AxisImgPos.Y += ImgY;
                    AxisX = ArmPoint.X;
                    AxisY = ArmPoint.Y;
                }
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("btnStartMove_Click: " + ex.Message);
            }
            GetPsnFbk();
            GetCoordinate();
        }

        private void GetPsnFbk()
        {
            try
            {
                ArmPoint = TsRemote._Robot.GetPsnFbkWork();
                numericUpDownX.Value = Convert.ToInt32(ArmPoint.X);
                numericUpDownY.Value = Convert.ToInt32(ArmPoint.Y);
                int tmp = Convert.ToInt32(ArmPoint.Z);
                numericUpDownZ.Value = (tmp > numericUpDownZ.Maximum) ? numericUpDownY.Maximum : ((tmp < numericUpDownZ.Minimum) ? numericUpDownZ.Minimum : tmp);
                numericUpDownC.Value = Convert.ToInt32(ArmPoint.C);
                numericUpDownT.Value = Convert.ToInt32(ArmPoint.T);
                //Console.WriteLine("C:" + Convert.ToInt32(ArmPoint.C));
            }
            catch { }
        }
        #endregion

        #region StopMove
        //手臂緊急停止按鈕
        private void btnStopMove_Click(object sender, EventArgs e)
        {
            //string a = TsRemote.getStatusAll();
            //string a = TsRemote._Robot.GetStatus().RunStatus.ToString();
            if (btnStopMove.Text == "復歸" && TsRemote._Robot.GetStatusMonitor().ServoStatus == 0)
            {
                //取得機械手臂狀態
                TsRemote.getStatusAll();
                TsRemote._Robot.GetStatus();
                TsRemote._Robot.GetStatusAll();
                TsRemote._Robot.GetStatusMonitor();
                //取得機械手臂狀態
                TsRemote._Robot.ServoOn();
                TsRemote.setServoOn();

                if (TsRemote.startControl() == false)//可以開啟
                    return;
                //string a = TsRemote._Robot.GetStatusMonitor().ServoStatus.ToString();
                Thread.Sleep(100);
                btnStopMove.Text = "緊急停止";
                btnStopMove.BackColor = Color.Transparent;

                //MessageBox.Show(a);
            }
            else if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }
            else if (btnStopMove.Text == "緊急停止")
            {
                //TsRemote._Robot.ResetMove();
                //TsRemote._Robot.ServoOff();

                TsRemote.setServoOff();//可以關閉
                //TsRemote.stopControl();//可以關閉
                //TsRemote._Robot.ProgramStop();
                Thread.Sleep(6000);

                //ProgramBreak()
                //TsRemote.stopControl();//可以停
                btnStopMove.Text = "復歸";
                btnStopMove.BackColor = Color.Red;

                btnStartMove.Visible = true;
                /*string a = TsRemote._Robot.GetStatusMonitor().ServoStatus.ToString();
                MessageBox.Show(a);*/
            }
            //TsRemote._Robot.WatchDogStop();
            //TsRemote._Robot.ProgramStop();
        }
        #endregion

        /// <summary>
        /// 手臂移動
        /// </summary>
        /// <param name="Point"></param>
        private void Robot_Moves(TsPointS Point)
        {
            try
            {
                TsRemote._Robot.Moves(Point);
            }
            catch
            {
                //Error processing
                Console.WriteLine("機械手臂異常停止");
            }
            this.BeginInvoke(new GetArmPoint(GetPsnFbk));
            GetCoordinate();
        }

        #region Log
        private void ReceivedStatusEvent(TsStatusMonitor para)
        {
            //切換到畫面的執行緒，進行畫面更新
            object[] dataArray = new object[1];
            dataArray[0] = para;
            this.BeginInvoke(new ReceivedStatusEventDelegate(UpdateUI_ReceivedStatusEvent), dataArray);
        }

        private delegate void ReceivedStatusEventDelegate(TsStatusMonitor para);
        private void UpdateUI_ReceivedStatusEvent(TsStatusMonitor para)
        {
            if (para.EmergencyStop == 1 || para.SafetyStop == 1 || para.SafetySwitch == 1)
            {
                btnResetAlarm.BackColor = Color.Red;
                btnResetAlarm.ForeColor = Color.White;
            }
            else
            {
                btnResetAlarm.BackColor = Color.Transparent;
                btnResetAlarm.ForeColor = Color.Black;
            }

            try
            {

            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("UpdateUI_ReceivedStatusEvent: " + ex.Message);
            }
        }

        //command(Cmd): 目標座標
        //feedback(Fbk): 目前所在座標
        private void GetCoordinate()
        {
            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            toolStripStatusLabel_Content.Text = ArmPoint.X.ToString("000.0000") + ", " + ArmPoint.Y.ToString("000.0000") + ", " + ArmPoint.Z.ToString("000.0000") + ", " + ArmPoint.C.ToString("000.0000");
        }

        #endregion
        #endregion

        #region 選取定位點
        private void Select_Anchor_Click(object sender, EventArgs e)
        {
            if (Mechanical_Arm_Connection == false)
            {

                MessageBox.Show("機械手臂尚未連線");
                return;
            }
            Camera1.IsSelectRegion = true;
            Camera1.AnchorDetect = new AnchorDectector();
            //this.Display_Img.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseMove);
            //this.Display_Img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseDown);
            //this.Display_Img.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseUp);

            ROI_gray = Camera1.GrabImage.Copy();
            m_img_cpy = Camera1.ColorGrabImage.Copy();

        }

        //VectorOfPoint RectStartPoint11;
        //Point RectStartPoint33;
        Point[] StartPoint_All;
        Point[] StartPoint;

        int Get_Point_YN = 0;
        int Get_Point_YN_Count = 0;
        int SelectPointSize = 2;

        int Get_Point_YN_Image = 0;
        int Get_Point_YN_Image_Count = 0;

        int Mechanical_Arm_YN = 0;

        //int Get_Point_Count = 0;

        private void imageBox_Point_MouseDown(object sender, MouseEventArgs e)
        {
            RectStartPoint = e.Location;
            Invalidate();

            //紀錄點膠位置
            if (Get_Point_YN == 1)
            {
                //RectStartPoint33 = RectStartPoint;
                StartPoint = new Point[Get_Point_YN_Count + 1];
                if (StartPoint_All != null)
                {
                    for (int i = 0; i < StartPoint_All.Length; i++)
                    {
                        StartPoint[i] = StartPoint_All[i];
                        //StartPoint_All[i].Y = (StartPoint_All[i].Y * Original_Image_Gray.Width) / Display_Img.Size.Width;
                        //StartPoint_All[i].X = (StartPoint_All[i].X * Original_Image_Gray.Height) / Display_Img.Size.Height;
                    }
                }
                RectStartPoint.X = (RectStartPoint.X * Original_Image_Gray.Width) / imageBox_Point.Size.Width;
                RectStartPoint.Y = (RectStartPoint.Y * Original_Image_Gray.Height) / imageBox_Point.Size.Height;

                StartPoint[Get_Point_YN_Count] = RectStartPoint;

                //RectStartPoint1[Get_Point_YN_Count] = RectStartPoint;
                //Point[] values = ;
                //[Get_Point_YN_Count] = RectStartPoint;
                // = RectStartPoint;
                for (int k = 0; k < StartPoint.Length; k++)
                {
                    if (StartPoint[k].Y < Original_Image_Gray_Point.Height && StartPoint[k].X < Original_Image_Gray_Point.Width && StartPoint[k].Y >= 0 && StartPoint[k].X >= 0)
                    {
                        for (int i = -SelectPointSize; i <= SelectPointSize; i++)
                        {
                            for (int j = -SelectPointSize; j <= SelectPointSize; j++)
                            {
                                Original_Image_Gray_Point[StartPoint[k].Y + i, StartPoint[k].X + j] = new Gray(255);
                            }
                        }
                        //Original_Image_Gray[StartPoint[k].Y, StartPoint[k].X] = new Gray(255);
                    }
                }
                imageBox_Point.Image = Original_Image_Gray_Point;

                Original_Feature_points_Click = new VectorOfPoint(StartPoint.ToArray());

                Get_Point_YN_Count++;
                StartPoint_All = StartPoint;
            }

        }



        private void Display_Img_MouseDown(object sender, MouseEventArgs e)
        {
            RectStartPoint = e.Location;
            Invalidate();

            //手臂點選影像移動至點選位置
            if (Mechanical_Arm_YN == 1)
            {
                if (Camera1 == null)
                {
                    MessageBox.Show("相機尚未連線");
                    return;
                }
                if (Mechanical_Arm_Connection == false)
                {
                    MessageBox.Show("機械手臂尚未連線");
                    return;
                }


                if (TsRemote._Robot.GetStatus().RunStatus == 0)//判斷手臂無動作時進入
                {
                    RectStartPoint.X = (RectStartPoint.X * Camera1.GrabImage.Width) / Display_Img.Size.Width;
                    RectStartPoint.Y = (RectStartPoint.Y * Camera1.GrabImage.Height) / Display_Img.Size.Height;

                    COMMONK_PerformCoordinatesTransform(pePTMatrix1, RectStartPoint.X, RectStartPoint.Y, Rotation_MatrixX, Rotation_MatrixY);
                    //Rotation_Matrix_Int[0, i] = (int)Rotation_MatrixX;
                    //Rotation_Matrix_Int[1, i] = (int)Rotation_MatrixY;

                    ArmPoint = new TsPointS();

                    numericUpDownX.Value = (decimal)Rotation_MatrixX;
                    numericUpDownY.Value = (decimal)Rotation_MatrixY;

                    ArmPoint.X = Convert.ToDouble(numericUpDownX.Value);
                    ArmPoint.Y = Convert.ToDouble(numericUpDownY.Value);
                    ArmPoint.Z = Convert.ToDouble(numericUpDownZ.Value);
                    ArmPoint.C = Convert.ToDouble(numericUpDownC.Value);
                    //以下指定無用途
                    //TsRemote._Robot.MvConfig = (ConfigS)domainUpDown_Config.SelectedIndex;
                    TsRemote._Robot.Moves(ArmPoint);
                    Thread.Sleep(10);

                    //LinearMove(Rotation_MatrixX, Rotation_MatrixY);
                }
            }

            //紀錄點選影像及手臂X和Y的座標位置
            if (Get_Point_YN_Image == 1)
            {
                if (Get_Point_YN_Image_Count == 4)
                {
                    return;
                }
                //紀錄點選影像位置的X和Y的座標
                Record_Points_Image = new Point[Get_Point_YN_Image_Count + 1];
                if (Main_Ini.Record_Points_Image_All != null)
                {
                    for (int i = 0; i < Main_Ini.Record_Points_Image_All.Length; i++)
                    {
                        Record_Points_Image[i] = Main_Ini.Record_Points_Image_All[i];
                        //StartPoint_All[i].Y = (StartPoint_All[i].Y * Original_Image_Gray.Width) / Display_Img.Size.Width;
                        //StartPoint_All[i].X = (StartPoint_All[i].X * Original_Image_Gray.Height) / Display_Img.Size.Height;
                    }
                }
                if (Use_Camera == false)
                {
                    if (openFileDialog1.FileName == "openFileDialog1")
                    {
                        MessageBox.Show("圖片尚未載入");
                        return;
                    }
                    RectStartPoint.X = (RectStartPoint.X * Original_Image_Gray.Width) / Display_Img.Size.Width;
                    RectStartPoint.Y = (RectStartPoint.Y * Original_Image_Gray.Height) / Display_Img.Size.Height;
                }
                else if (Use_Camera == true)
                {
                    if (Camera_comboBox.SelectedItem == null)
                    {
                        MessageBox.Show("相機尚未找到");
                        return;
                    }
                    RectStartPoint.X = (RectStartPoint.X * Camera1.GrabImage.Width) / Display_Img.Size.Width;
                    RectStartPoint.Y = (RectStartPoint.Y * Camera1.GrabImage.Height) / Display_Img.Size.Height;
                }

                Record_Points_Image[Get_Point_YN_Image_Count] = RectStartPoint;

                /*for (int k = 0; k < Record_Points_Image.Length; k++)
                {
                    if (Record_Points_Image[k].Y < Original_Image_Gray_Point.Height && Record_Points_Image[k].X < Original_Image_Gray_Point.Width && Record_Points_Image[k].Y >= 0 && Record_Points_Image[k].X >= 0)
                    {
                        for (int i = -SelectPointSize; i <= SelectPointSize; i++)
                        {
                            for (int j = -SelectPointSize; j <= SelectPointSize; j++)
                            {
                                Original_Image_Gray_Point[Record_Points_Image[k].Y + i, Record_Points_Image[k].X + j] = new Gray(255);
                            }
                        }
                        //Original_Image_Gray[StartPoint[k].Y, StartPoint[k].X] = new Gray(255);
                    }
                }*/

                //Display_Img.Image = Original_Image_Gray_Point;

                //Original_Feature_points_Click = new VectorOfPoint(StartPoint.ToArray());

                //Get_Point_YN_Image_Count++;
                Main_Ini.Record_Points_Image_All = Record_Points_Image;


                //紀錄手臂當下X和Y的座標
                Record_Points_Mechanical_Arm = new Point[Get_Point_YN_Image_Count + 1];//特徵點旋轉加上偏移量的結果

                if (Main_Ini.Record_Points_Mechanical_Arm_All != null)
                {
                    for (int i = 0; i < Main_Ini.Record_Points_Mechanical_Arm_All.Length; i++)
                    {
                        //for (int j = 0; j < 2; j++)
                        {
                            Record_Points_Mechanical_Arm[i] = Main_Ini.Record_Points_Mechanical_Arm_All[i];
                            //StartPoint_All[i].Y = (StartPoint_All[i].Y * Original_Image_Gray.Width) / Display_Img.Size.Width;
                            //StartPoint_All[i].X = (StartPoint_All[i].X * Original_Image_Gray.Height) / Display_Img.Size.Height;
                        }
                    }
                }

                Record_Points_Mechanical_Arm[Get_Point_YN_Image_Count].X = (int)(numericUpDownX.Value);//特徵點旋轉加上偏移量的結果
                Record_Points_Mechanical_Arm[Get_Point_YN_Image_Count].Y = (int)(numericUpDownY.Value);//特徵點旋轉加上偏移量的結果

                Get_Point_YN_Image_Count++;

                Main_Ini.Record_Points_Mechanical_Arm_All = Record_Points_Mechanical_Arm;
                Main_Ini.Record_Points_Mechanical_Arm_All_length = Main_Ini.Record_Points_Mechanical_Arm_All.Length;

                if (Get_Point_YN_Image_Count == 4)
                {
                    btn_Record_Points_Image.Enabled = true;
                }

                label_Number_Points.Text = "紀錄點位數量：" + Get_Point_YN_Image_Count.ToString();
            }
        }

        private void Display_Img_MouseMove(object sender, MouseEventArgs e)
        {
            if (Camera1 == null)
            {
                return;
            }

            if (!Camera1.m_img_loaded)
                return;
            if (Get_Point_YN != 1)
            {
                #region 設定ImageBox上 ROI座標(picture coordinates)：Rect
                int X0, Y0;
                //滑鼠座標: picture coordinates(e.X, e.Y) 
                //影像座標: image coordinates(X0, Y0)
                //Coordinates at input picture box
                if (e.Button != MouseButtons.Left)   // 如果移動中同時按下左鍵繼續往下, 否則離開
                    return;
                Point tempEndPoint = e.Location;

                // ROI左上角座標 (picture coordinates)
                Rect.Location = new Point(Math.Min(RectStartPoint.X, tempEndPoint.X), Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                // ROI寬度和高度 (picture coordinates)
                Rect.Size = new Size(Math.Abs(RectStartPoint.X - tempEndPoint.X), Math.Abs(RectStartPoint.Y - tempEndPoint.Y));

                #endregion

                #region 設定實際影像上 ROI座標(image coordinates)：RealImageRect
                //(1) picture coordinates: RectStartPoint.X, RectStartPoint.Y
                //      image coordinates: X0, Y0
                //(2) picture coordinates: tempEndPoint.X, tempEndPoint.Y
                //      image coordinates: X1, Y1
                //
                //Coordinates at real image - Create ROI
                Utilities.ConvertCoordinates(Display_Img, Camera1.GrabImage, out X0, out Y0, RectStartPoint.X, RectStartPoint.Y);
                int X1, Y1;
                Utilities.ConvertCoordinates(Display_Img, Camera1.GrabImage, out X1, out Y1, tempEndPoint.X, tempEndPoint.Y);
                RealImageRect.Location = new Point(Math.Min(X0, X1), Math.Min(Y0, Y1));
                RealImageRect.Size = new Size(Math.Abs(X0 - X1), Math.Abs(Y0 - Y1));
                m_img_cpy.ROI = System.Drawing.Rectangle.Empty;
                Camera1.ColorGrabImage = m_img_cpy.Clone();
                Camera1.ColorGrabImage.Draw(RealImageRect, new Bgr(200, 142, 175), thickness, LineType.EightConnected, 0);
                Display_Img.Image = Camera1.ColorGrabImage;

                #endregion
                ((PictureBox)sender).Invalidate();
            }
        }

        private void Display_Img_MouseUp(object sender, MouseEventArgs e)
        {
            if (Camera1 == null)
            {
                return;
            }

            if (!Camera1.m_img_loaded)
                return;
            //if (Get_Point_YN != 1)
            {
                Camera1.IsSelectRegion = false;
                Camera1.WaitProcess.Set();
                //Define ROI. Valida altura e largura para evitar index range exception.
                if (RealImageRect.Width > 0 && RealImageRect.Height > 0)
                {
                    ROI_gray.ROI = RealImageRect;
                }

                this.Display_Img.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseMove);
                //this.Display_Img.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseDown);
                this.Display_Img.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseUp);
                CalAxisPoint.Enabled = true;
                //定位點計算，並記錄特徵
                if (ROI_gray != null)
                    Camera1.IsSelectAnchor = Camera1.AnchorDetect.SelectAnchorPoint(ROI_gray, out ROI_color);
                ROI_Img.Image = ROI_color;
                m_img_cpy = null;
            }
        }
        #endregion

        #region 計算取得手臂軸心位置
        private void CalAxisPoint_Click(object sender, EventArgs e)
        {
            if (Mechanical_Arm_Connection == false)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }

            if (Reindex != 0)
                Reset();

            GetAxis = new Thread(RotFindPoint);

            GetAxis.Start();

            IsGetAxisPos = true;
            ConvtCoodinate.Enabled = true;
        }

        private void RotFindPoint()
        {
            PointF[] P = new PointF[2];
            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            //初始原點
            recordcount = 0;
            Reindex = 1;
            P[0] = Camera1.AnchorPosition;

            nowang = ArmPoint.C;
            string Atext = "角度：\n" + nowang.ToString() + "\n";
            string Ptext = "( " + P[0].X.ToString() + " , " + P[0].Y.ToString() + " )";
            this.BeginInvoke(new UpdadeListView(record), Reindex, Ptext, Atext);

            //偏移點
            Rotate_Point(RotAng);
            Thread.Sleep(200);
            recordcount = 0;
            Reindex = 2;

            P[1] = Camera1.AnchorPosition;

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            nowang = ArmPoint.C;
            Atext += nowang.ToString() + "\n";

            AxisX = ArmPoint.X;
            AxisY = ArmPoint.Y;

            string ArmP = "(" + AxisX.ToString() + "," + AxisY.ToString() + ")";

            //角度驗證
            double m0 = (AxisImgPos.Y - P[0].Y) / (AxisImgPos.X - P[0].X);
            double m1 = (AxisImgPos.Y - P[1].Y) / (AxisImgPos.X - P[1].X);

            double ang1 = Math.Abs((m0 - m1) / (1 + m0 * m1));
            angle_pi1 = Math.Atan(ang1);
            angle_pi1 = angle_pi1 * 180 / Math.PI;

            r0 = Math.Sqrt((AxisImgPos.X - P[0].X) * (AxisImgPos.X - P[0].X) + (AxisImgPos.Y - P[0].Y) * (AxisImgPos.Y - P[0].Y));
            r1 = Math.Sqrt((AxisImgPos.X - P[1].X) * (AxisImgPos.X - P[1].X) + (AxisImgPos.Y - P[1].Y) * (AxisImgPos.Y - P[1].Y));

            string r = "Radius=" + r0.ToString() + "\n" + r1.ToString();
            this.BeginInvoke(new UpdateLabel(UpdateLabeltext), angle_pi1, r, AxisImgPos.ToString(), ArmP);
            //**********

            Atext = "角度：\n" + nowang.ToString() + "\n";
            Ptext = "( " + P[1].X.ToString() + " , " + P[1].Y.ToString() + " )";
            this.BeginInvoke(new UpdadeListView(record), index, Ptext, Atext);

            AxisImgPos = ConvertCoordinate.CalRotAxisCenter(RotAng, P[0], P[1]);
            Rotate_Point(-RotAng);


            Reindex = 3;
            Atext = "角度：\n" + nowang.ToString() + "\n";
            Ptext = "( " + AxisImgPos.X.ToString() + " , " + AxisImgPos.Y.ToString() + " )";
            this.BeginInvoke(new UpdadeListView(record), index, Ptext, Atext);

            CenterP = P;
        }

        private void UpdateLabeltext(double angle1, string ra, string ImgAxis, string ArmAxis)
        {
            label3.Text = "角度1：" + angle1.ToString();
            label15.Text = ra;
            label4.Text = "手臂復歸位置：(" + RevertPosX.ToString() + "," + RevertPosY.ToString() + ")";
            label6.Text = "手臂軸心位置：" + ArmAxis;
            label7.Text = "影像軸心位置：" + ImgAxis;
        }

        private void record(int i, string text, string ang)
        {
            if (i > 2)
                return;

            ListViewItem lvi = new ListViewItem();

            lvi.ImageIndex = i;

            //第0行的內容
            if (i == 1)
                lvi.Text = "Origin";
            else if (i == 2)
                lvi.Text = "Move1";
            else if (i == 3)
                lvi.Text = "軸心位置";

            //第1行的內容
            lvi.SubItems.Add(text);

            label10.Text = ang;
            this.listView1.Items.Add(lvi);
        }

        private void Reset()
        {
            index = 0;
            int LivCount = listView1.Items.Count;
            for (int i = 1; i <= LivCount; i++)
                listView1.Items.RemoveAt(0);
        }
        #endregion

        #region 定位校正與取得座標轉換
        private void ConvtCoodinate_Click(object sender, EventArgs e)
        {
            CalTransCoordinate();
            //Position.
        }

        private void CalTransCoordinate()
        {
            PointF StartImgP;
            PointF CenterImgP;
            PointF EndImgP;

            double shiftX = 0;
            double shiftY = 0;

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();

            double StartX = ArmPoint.X;
            double StartY = ArmPoint.Y;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out StartImgP);

            double MoveX = 0;
            double MoveY = 0;


            if (StartImgP.X <= Img_center.X && StartImgP.Y <= Img_center.Y)
            {
                MoveX = 10.0;
                MoveY = 10.0;
            }
            else if (StartImgP.X > Img_center.X && StartImgP.Y <= Img_center.Y)
            {
                MoveX = 10.0;
                MoveY = -10.0;
            }
            else if (StartImgP.X <= Img_center.X && StartImgP.Y > Img_center.Y)
            {
                MoveX = -10.0;
                MoveY = 10.0;
            }
            else if (StartImgP.X > Img_center.X && StartImgP.Y > Img_center.Y)
            {
                MoveX = -10.0;
                MoveY = -10.0;
            }
            else if (StartImgP.X == Img_center.X && StartImgP.Y == Img_center.Y)
            {
                MoveX = 10.0;
                MoveY = 10.0;
            }

            LinearMove(0, MoveY);
            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            double CenterX = ArmPoint.X;
            double CenterY = ArmPoint.Y;

            Thread.Sleep(70);
            IsAnchorPositionMode = Camera1.GetAnchorPosition(out CenterImgP);

            ConvertCoordinate.CalCovtoCoodinateX((CenterX - StartX), (CenterY - StartY), (CenterImgP.X - StartImgP.X), (CenterImgP.Y - StartImgP.Y));

            //計算手臂影像之偏角
            //ArmToImgAngle((CenterX - StartX), (CenterY - StartY), (CenterImgP.X - StartImgP.X), (CenterImgP.Y - StartImgP.Y), out ArmImgAngle);

            LinearMove(MoveX, 0);
            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            double EndX = ArmPoint.X;
            double EndY = ArmPoint.Y;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out EndImgP);

            ConvertCoordinate.CalCovtoCoodinateY((EndX - CenterX), (EndY - CenterY), (EndImgP.X - CenterImgP.X), (EndImgP.Y - CenterImgP.Y));

            IsCoodinateConv = true;

            //有計算座標轉換，解開定位與移動控制項
            if (IsCoodinateConv == true)
            {
                Position.Enabled = true;
                MoveToPoint.Enabled = true;
                CalMoveDistance.Enabled = true;
                //CalAxisPoint.Enabled = true;
                //AnchorMoveAssignPoint.Enabled = true;
                //MoveToImgCenter.Enabled = true;
            }


            //計算位移量
            //??????
            ConvertCoordinate.ImgCovtoArm((Img_center.X - Camera1.AnchorPosition.X), (Img_center.Y - Camera1.AnchorPosition.Y), out shiftX, out shiftY);

            label5.Text = "\n" + ImgXMoveX.ToString();
            label5.Text += "\n" + ImgXMoveY.ToString();
            label5.Text += "\n" + ImgYMoveX.ToString();
            label5.Text += "\n" + ImgYMoveY.ToString();
            label5.Text += "\n" + LengthPropertionX.ToString();
            label5.Text += "\n" + LengthPropertionY.ToString();

            label5.Text += "\n" + shiftX.ToString();
            label5.Text += "\n" + shiftY.ToString();
        }

        private void MoveToImgCenter_Click(object sender, EventArgs e)
        {
            PointF StartPoint;
            PointF EndPoint;

            double shiftX = 0;
            double shiftY = 0;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out StartPoint);

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();

            double StartX = ArmPoint.X;
            double StartY = ArmPoint.Y;


            ConvertCoordinate.ImgCovtoArm((Img_center.X - StartPoint.X), (Img_center.Y - StartPoint.Y), out shiftX, out shiftY);


            LinearMove(shiftX, shiftY);

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out EndPoint);

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            double EndX = ArmPoint.X;
            double EndY = ArmPoint.Y;

            if (IsGetAxisPos)
            {
                ConvertCoordinate.ArmCovtoImg((EndX - StartX), (EndY - StartY), out ImgX, out ImgY);
                AxisImgPos.X += ImgX;
                AxisImgPos.Y += ImgY;
                AxisX = ArmPoint.X;
                AxisY = ArmPoint.Y;

                label4.Text = "手臂復歸位置：(" + RevertPosX.ToString() + "," + RevertPosY.ToString() + ")";
                label6.Text = "手臂軸心位置：(" + AxisX.ToString() + "," + AxisX.ToString() + ")";
                label7.Text = "影像軸心位置：(" + AxisImgPos.X.ToString() + "," + AxisImgPos.Y.ToString() + ")";
            }


            double angXY = ConvertCoordinate.ImgToArmAngle((EndX - StartX), (EndY - StartY), (EndPoint.X - StartPoint.X), (EndPoint.Y - StartPoint.Y));
            record(record_a, shiftX, (EndX - StartX), shiftY, (EndY - StartY), (Img_center.X - StartPoint.X), (EndPoint.X - StartPoint.X), (Img_center.Y - StartPoint.Y), (EndPoint.Y - StartPoint.Y), angXY);
            record_a++;
        }

        private void ModifyPosition(float DiffX, float DiffY)
        {
            double shiftX = 0;
            double shiftY = 0;

            ConvertCoordinate.ImgCovtoArm(DiffX, DiffY, out shiftX, out shiftY);
            LinearMove(shiftX, shiftY);
        }
        bool PositionYN = false;
        private void Position_Click(object sender, EventArgs e)
        {
            IsPosition = true;
            PositionYN = true;

            //經座標轉換後須位移的手臂移動量
            double shiftX = 0;
            double shiftY = 0;

            //轉換座標參數暫存
            double tempImgXMoveX = 0;
            double tempImgXMoveY = 0;
            double tempImgYMoveX = 0;
            double tempImgYMoveY = 0;
            double tempLengthX = 0;
            double tempLengthY = 0;

            //手臂位置紀錄
            double[] RecordArmPointX = new double[3];
            double[] RecordArmPointY = new double[3];
            //
            double SX = 0;
            double SY = 0;

            PointF NowPoint;
            PointF ImgStart;
            PointF ImgEnd;



            //如果標靶未在影像中心，移到影像中心
            IsAnchorPositionMode = Camera1.GetAnchorPosition(out NowPoint);
            if ((Img_center.X - NowPoint.X) > 0.05 || (Img_center.Y - NowPoint.Y) > 0.05)
            {
                ConvertCoordinate.ImgCovtoArm((Img_center.X - NowPoint.X), (Img_center.Y - NowPoint.Y), out  shiftX, out shiftY);
                LinearMove(shiftX, shiftY);
            }
            //*************************************************

            PointF shiftpoint1 = new PointF(Img_center.X, Img_center.Y + 150);
            PointF shiftpoint2 = new PointF(Img_center.X + 150, Img_center.Y + 150);
            shiftpoint[0] = shiftpoint1;
            shiftpoint[1] = shiftpoint2;

            /////////

            //Y軸定位
            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            RecordArmPointX[0] = ArmPoint.X;
            RecordArmPointY[0] = ArmPoint.Y;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out ImgStart);

            ConvertCoordinate.ImgCovtoArm((shiftpoint1.X - Img_center.X), (shiftpoint1.Y - Img_center.Y), out  shiftX, out shiftY);
            LinearMove(shiftX, shiftY);
            SX = shiftX;
            SY = shiftY;

            //位置修正
            IsAnchorPositionMode = Camera1.GetAnchorPosition(out ImgEnd);
            if ((shiftpoint1.X - ImgEnd.X) > 0.05 || (shiftpoint1.Y - ImgEnd.Y) > 0.05)
            {
                ConvertCoordinate.ImgCovtoArm((shiftpoint1.X - ImgEnd.X), (shiftpoint1.Y - ImgEnd.Y), out  shiftX, out shiftY);
                LinearMove(shiftX, shiftY);
            }

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            RecordArmPointX[1] = ArmPoint.X;
            RecordArmPointY[1] = ArmPoint.Y;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out ImgEnd);

            double angXY = ConvertCoordinate.ImgToArmAngle((RecordArmPointX[1] - RecordArmPointX[0]), (RecordArmPointY[1] - RecordArmPointY[0]), (ImgEnd.X - ImgStart.X), (ImgEnd.Y - ImgStart.Y));
            record(record_a, SX, (RecordArmPointX[1] - RecordArmPointX[0]), SY, (RecordArmPointY[1] - RecordArmPointY[0]), (shiftpoint1.X - Img_center.X), (ImgEnd.X - ImgStart.X), (shiftpoint1.Y - Img_center.Y), (ImgEnd.Y - ImgStart.Y), angXY);
            record_a++;
            ConvertCoordinate.CalCovtoCoodinateY((RecordArmPointX[1] - RecordArmPointX[0]), (RecordArmPointY[1] - RecordArmPointY[0]), (ImgEnd.X - ImgStart.X), (ImgEnd.Y - ImgStart.Y));


            //Y軸定位
            IsAnchorPositionMode = Camera1.GetAnchorPosition(out ImgStart);

            ConvertCoordinate.ImgCovtoArm((shiftpoint2.X - shiftpoint1.X), (shiftpoint2.Y - shiftpoint1.Y), out  shiftX, out shiftY);
            LinearMove(shiftX, shiftY);
            SX = shiftX;
            SY = shiftY;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out ImgEnd);
            if ((shiftpoint2.X - shiftpoint1.X) > 0.05 || (shiftpoint2.Y - shiftpoint1.Y) > 0.05)
            {
                ConvertCoordinate.ImgCovtoArm((shiftpoint2.X - ImgEnd.X), (shiftpoint2.Y - ImgEnd.Y), out  shiftX, out shiftY);
                LinearMove(shiftX, shiftY);
            }

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            RecordArmPointX[2] = ArmPoint.X;
            RecordArmPointY[2] = ArmPoint.Y;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out ImgEnd);
            ConvertCoordinate.CalCovtoCoodinateX((RecordArmPointX[2] - RecordArmPointX[1]), (RecordArmPointY[2] - RecordArmPointY[1]), (ImgEnd.X - ImgStart.X), (ImgEnd.Y - ImgStart.Y));


            angXY = ConvertCoordinate.ImgToArmAngle((RecordArmPointX[1] - RecordArmPointX[0]), (RecordArmPointY[1] - RecordArmPointY[0]), (ImgEnd.X - ImgStart.X), (ImgEnd.Y - ImgStart.Y));
            record(record_a, SX, (RecordArmPointX[1] - RecordArmPointX[0]), SY, (RecordArmPointY[1] - RecordArmPointY[0]), (shiftpoint2.X - shiftpoint1.X), (ImgEnd.X - ImgStart.X), (shiftpoint2.Y - shiftpoint1.Y), (ImgEnd.Y - ImgStart.Y), angXY);
            record_a++;

            if (IsGetAxisPos)
            {
                ConvertCoordinate.ArmCovtoImg((RecordArmPointX[2] - RecordArmPointX[0]), (RecordArmPointY[2] - RecordArmPointY[0]), out ImgX, out ImgY);
                AxisImgPos.X += ImgX;
                AxisImgPos.Y += ImgY;
                AxisX = ArmPoint.X;
                AxisY = ArmPoint.Y;

                label4.Text = "手臂復歸位置：(" + RevertPosX.ToString() + "," + RevertPosY.ToString() + ")";
                label6.Text = "手臂軸心位置：(" + AxisX.ToString() + "," + AxisY.ToString() + ")";
                label7.Text = "影像軸心位置：(" + AxisImgPos.X.ToString() + "," + AxisImgPos.Y.ToString() + ")";
            }

            ////////
            ImgXMoveX = tempImgXMoveX;
            ImgXMoveY = tempImgXMoveY;
            ImgYMoveX = tempImgYMoveX;
            ImgYMoveY = tempImgYMoveY;

            LengthPropertionX = tempLengthX;
            LengthPropertionY = tempLengthY;

            ////
            label5.Text = "\n" + ImgXMoveX.ToString();
            label5.Text += "\n" + ImgXMoveY.ToString();
            label5.Text += "\n" + ImgYMoveX.ToString();
            label5.Text += "\n" + ImgYMoveY.ToString();
            label5.Text += "\n" + LengthPropertionX.ToString();
            label5.Text += "\n" + LengthPropertionY.ToString();

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out ImgEnd);
            ConvertCoordinate.ImgCovtoArm((Img_center.X - ImgEnd.X), (Img_center.Y - ImgEnd.Y), out shiftX, out shiftY);

            label5.Text += "\n" + shiftX.ToString();
            label5.Text += "\n" + shiftY.ToString();

            AnchorMoveAssignPoint.Enabled = true;
            MoveToImgCenter.Enabled = true;

            IsPosition = false;
        }
        #endregion

        /*#region 計算手臂與影像座標轉換
        //手臂座標轉換成影像座標
        private void CalCovtoCoodinateX(double ArmMoveX, double ArmMoveY, float ImgMoveX, float ImgMoveY, out double ImgXMoveXL, out double ImgXMoveYL, out double LengthX)
        {
            LengthX = Math.Sqrt((ImgMoveX * ImgMoveX) + (ImgMoveY * ImgMoveY));

            double slope = 0;
            double angle = 0;

            //計算影像位移方向與X軸的夾角
            if (ImgMoveX != 0)
            {
                slope = ImgMoveY / ImgMoveX;
                angle = Math.Abs(slope);
                angle = Math.Atan(angle);
            }
            else
                angle = 90;

            //判斷在X軸上方或下方，作向量旋轉
            if (slope > 0)
            {
                ImgXMoveXL = ArmMoveX * Math.Cos(angle) - ArmMoveY * Math.Sin(angle);
                ImgXMoveYL = (ArmMoveX * Math.Sin(angle)) + ArmMoveY * Math.Cos(angle);
            }
            else
            {
                ImgXMoveXL = ArmMoveX * Math.Cos(angle) + ArmMoveY * Math.Sin(angle);
                ImgXMoveYL = (-(ArmMoveX * Math.Sin(angle))) + ArmMoveY * Math.Cos(angle);
            }

            if (ImgMoveX < 0)
            {
                ImgXMoveXL = -ImgXMoveXL;
                ImgXMoveYL = -ImgXMoveYL;
            }
            return;
        }

        private void CalCovtoCoodinateY(double ArmMoveX, double ArmMoveY, float ImgMoveX, float ImgMoveY, out double ImgYMoveXL, out double ImgYMoveYL, out double LengthY)
        {
            LengthY = Math.Sqrt((ImgMoveX * ImgMoveX) + (ImgMoveY * ImgMoveY));
            double slope = 0;
            double angle = 0;

            //計算影像位移方向與Y軸的夾角
            if (ImgMoveX != 0)
            {
                slope = ImgMoveY / ImgMoveX;
                angle = Math.Abs((slope - 0) / (1 + slope * 0));
                angle = Math.Atan(angle);
                angle = (Math.PI / 2) - angle;
            }
            else
                angle = 0;


            //判斷在Y軸左方或右方，作向量旋轉
            if (slope > 0)
            {
                ImgYMoveXL = ArmMoveX * Math.Cos(angle) + ArmMoveY * Math.Sin(angle);
                ImgYMoveYL = (-(ArmMoveX * Math.Sin(angle))) + ArmMoveY * Math.Cos(angle);
            }
            else
            {
                ImgYMoveXL = ArmMoveX * Math.Cos(angle) - ArmMoveY * Math.Sin(angle);
                ImgYMoveYL = (ArmMoveX * Math.Sin(angle)) + ArmMoveY * Math.Cos(angle);
            }

            if (ImgMoveY < 0)
            {
                ImgYMoveXL = -ImgYMoveXL;
                ImgYMoveYL = -ImgYMoveYL;
            }

            return;
        }

        //影像移動量座標轉換成手臂座標
        private void ImgCovtoArm(float ImgMoveX, float ImgMoveY, out double ArmMoveX, out double ArmMoveY)
        {
            double ProperX = ImgMoveX / LengthPropertionX;
            double ProperY = ImgMoveY / LengthPropertionY;

            double ImgXContMoveX = ProperX * ImgXMoveX;
            double ImgXContMoveY = ProperX * ImgXMoveY;

            double ImgYContMoveX = ProperY * ImgYMoveX;
            double ImgYContMoveY = ProperY * ImgYMoveY;

            ArmMoveX = ImgXContMoveX + ImgYContMoveX;
            ArmMoveY = ImgXContMoveY + ImgYContMoveY;
        }

        private void ArmCovtoImg(double ArmMoveX, double ArmMoveY, out float ImgMoveX, out float ImgMoveY)
        {
            double DetA = ImgXMoveX * ImgYMoveY - ImgYMoveX * ImgXMoveY;
            double ProperX = (ArmMoveX * ImgYMoveY - ArmMoveY * ImgYMoveX) / DetA;
            double ProperY = (ArmMoveY * ImgXMoveX - ArmMoveX * ImgXMoveY) / DetA;

            ImgMoveX = (float)(ProperX * LengthPropertionX);
            ImgMoveY = (float)(ProperY * LengthPropertionY);
        }

        private double ImgToArmAngle(double ArmMoveX, double ArmMoveY, float ImgMoveX, float ImgMoveY)
        {
            double slopeI = 0;
            double slopeA = 0;
            double angle = 0;
            if (ArmMoveX != 0 && ImgMoveX != 0)
            {
                slopeA = ArmMoveY / ArmMoveX;
                slopeI = (double)(ImgMoveY / ImgMoveX);
                angle = Math.Abs((slopeA - slopeI) / (1 + (slopeA * slopeI)));
                angle = Math.Atan(angle);
                angle = angle * 180 / Math.PI;
            }
            if (slopeA > slopeI)
                return -angle;
            else
                return angle;
        }

        #endregion*/

        #region 手臂動作位移的指令
        private void Rotate_Point(double ang)
        {
            if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
            {
                MessageBox.Show("尚未連線");
                return;
            }

            try
            {
                ArmPoint = TsRemote._Robot.GetPsnFbkWork();

                ArmPoint.C += ang;

                TsRemote._Robot.Moves(ArmPoint);
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("btnStartMove_Click: " + ex.Message);
            }

            this.BeginInvoke(new GetArmPoint(GetPsnFbk));
            GetCoordinate();
        }

        private void LinearMove(double MoveX, double MoveY)
        {
            if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
            {
                MessageBox.Show("尚未連線");
                return;
            }

            try
            {
                ArmPoint = TsRemote._Robot.GetPsnFbkWork();
                ArmPoint.X += MoveX;
                ArmPoint.Y += MoveY;

                TsRemote._Robot.Moves(ArmPoint);
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("btnStartMove_Click: " + ex.Message);
            }

            this.BeginInvoke(new GetArmPoint(GetPsnFbk));
            GetPsnFbk();

            GetCoordinate();
        }

        private void RotateToZero()
        {
            if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
            {
                MessageBox.Show("尚未連線");
                return;
            }

            try
            {
                ArmPoint = TsRemote._Robot.GetPsnFbkWork();
                ArmPoint.C = 0;

                TsRemote._Robot.Moves(ArmPoint);
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("btnStartMove_Click: " + ex.Message);
            }

            this.BeginInvoke(new GetArmPoint(GetPsnFbk));
            GetPsnFbk();


            GetCoordinate();
        }

        private void ZAxisMove(double MoveZ)
        {
            if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }

            try
            {
                ArmPoint = TsRemote._Robot.GetPsnFbkWork();
                ArmPoint.Z = MoveZ;

                TsRemote._Robot.Moves(ArmPoint);
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("btnStartMove_Click: " + ex.Message);
            }

            this.BeginInvoke(new GetArmPoint(GetPsnFbk));
            GetPsnFbk();

            GetCoordinate();
        }

        private void MovePoint(double MoveX, double MoveY)
        {
            if ((TsRemote.ConnectStatus & ObjectTsRemote.CONNECTTYPE.WATCHDOG) == 0)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }

            try
            {
                ArmPoint = TsRemote._Robot.GetPsnFbkWork();
                ArmPoint.X = MoveX;
                ArmPoint.Y = MoveY;

                TsRemote._Robot.Moves(ArmPoint);
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("btnStartMove_Click: " + ex.Message);
            }

            this.BeginInvoke(new GetArmPoint(GetPsnFbk));
            GetPsnFbk();

            GetCoordinate();
        }
        #endregion

        #region ListView紀錄
        private void record(int i, double ArmXMove, double ArmXRealMove, double ArmYMove, double ArmYRealMove, float ImgXMove, float ImgXRealMove, float ImgYMove, float ImgYRealMove, double ang)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.ImageIndex = i;

            lvi.Text = ArmXMove.ToString();

            lvi.SubItems.Add(ArmXRealMove.ToString());
            lvi.SubItems.Add(ArmYMove.ToString());
            lvi.SubItems.Add(ArmYRealMove.ToString());
            lvi.SubItems.Add(ImgXMove.ToString());
            lvi.SubItems.Add(ImgXRealMove.ToString());
            lvi.SubItems.Add(ImgYMove.ToString());
            lvi.SubItems.Add(ImgYRealMove.ToString());
            lvi.SubItems.Add(ang.ToString());

            this.listView2.Items.Add(lvi);
        }

        private void Export_Excel_Click(object sender, EventArgs e)
        {
            ToExcel(listView2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int LivCount = listView2.Items.Count;
            for (int i = 1; i <= LivCount; i++)
                listView2.Items.RemoveAt(0);
            record_a = 0;
        }

        private void ToExcel(ListView LV)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
            ws.Name = "Move";

            int a = 1;
            foreach (ColumnHeader ch in LV.Columns)
            {
                ws.Cells[1, a] = ch.Text;
                a++;
            }


            int i = 1;
            int i2 = 2;
            foreach (ListViewItem lvi in LV.Items)
            {
                i = 1;
                foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                {
                    ws.Cells[i2, i] = lvs.Text;
                    i++;
                }
                i2++;

            }

        }

        private void ToExcelPoint(ListView LV1, ListView LV2)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
            ws.Name = "Point1";

            int a = 1;
            foreach (ColumnHeader ch in LV1.Columns)
            {
                ws.Cells[1, a] = ch.Text;
                a++;
            }


            int i = 1;
            int i2 = 2;
            foreach (ListViewItem lvi in LV1.Items)
            {
                i = 1;
                foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                {
                    ws.Cells[i2, i] = lvs.Text;
                    i++;
                }
                i2++;

            }


            wb.Sheets.Add(Type.Missing, ws, Type.Missing, Type.Missing);
            ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[2];
            ws.Name = "Point2";

            a = 1;
            foreach (ColumnHeader ch in LV2.Columns)
            {
                ws.Cells[1, a] = ch.Text;
                a++;
            }

            i = 1;
            i2 = 2;
            foreach (ListViewItem lvi in LV2.Items)
            {
                i = 1;
                foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                {
                    ws.Cells[i2, i] = lvs.Text;
                    i++;
                }
                i2++;

            }
        }

        private void LocalRecordToExl_Click(object sender, EventArgs e)
        {
            RecordMeanPoint(listView3, listView4);
            ToExcelPoint(listView3, listView4);
        }

        private void RecordMeanPoint(ListView LV1, ListView LV2)
        {
            for (int i = 0; i < 6; i++)
            {
                ListViewItem lvi = new ListViewItem();

                if (i == 0)
                {
                    lvi.Text = "MeanPoint";
                    lvi.SubItems.Add(CenterP[i].X.ToString());
                    lvi.SubItems.Add(CenterP[i].Y.ToString());
                    this.listView3.Items.Add(lvi);
                }
                else if (i == 1)
                {
                    lvi.Text = "MeanPoint";
                    lvi.SubItems.Add(CenterP[i].X.ToString());
                    lvi.SubItems.Add(CenterP[i].Y.ToString());
                    this.listView4.Items.Add(lvi);
                }
                else if (i == 2)
                {
                    lvi.Text = "CentralPoint";
                    lvi.SubItems.Add(AxisImgPos.X.ToString());
                    lvi.SubItems.Add(AxisImgPos.Y.ToString());
                    this.listView4.Items.Add(lvi);
                }
                else if (i == 3)
                {
                    lvi.Text = "Angle1";
                    lvi.SubItems.Add(angle_pi1.ToString());
                    this.listView4.Items.Add(lvi);
                }
                else if (i == 4)
                {
                    lvi.Text = "R1";
                    lvi.SubItems.Add(r0.ToString());
                    this.listView4.Items.Add(lvi);
                }
                else if (i == 5)
                {
                    lvi.Text = "R2";
                    lvi.SubItems.Add(r1.ToString());
                    this.listView4.Items.Add(lvi);
                }
            }
        }

        private void RemoveExl_Click(object sender, EventArgs e)
        {
            int LivCount = listView3.Items.Count;
            for (int i = 1; i <= LivCount; i++)
                listView3.Items.RemoveAt(0);
            record_a = 0;

            LivCount = listView4.Items.Count;
            for (int i = 1; i <= LivCount; i++)
                listView4.Items.RemoveAt(0);
        }

        private void RecordLocal(int i, PointF local)
        {
            ListViewItem lvi = new ListViewItem();


            lvi.Text = i.ToString();
            lvi.SubItems.Add(local.X.ToString());
            lvi.SubItems.Add(local.Y.ToString());
            if (Reindex == 1)
                this.listView3.Items.Add(lvi);
            else if (Reindex == 2)
                this.listView4.Items.Add(lvi);
        }
        #endregion

        private void AnchorMoveAssignPoint_Click(object sender, EventArgs e)
        {
            double shiftX = 0;
            double shiftY = 0;

            PointF nowPoint;
            PointF EndImgP1;

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out nowPoint);

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();

            double StartX = ArmPoint.X;
            double StartY = ArmPoint.Y;

            ConvertCoordinate.ImgCovtoArm((Convert.ToSingle(textBox2.Value) - nowPoint.X), (Convert.ToSingle(textBox3.Value) - nowPoint.Y), out shiftX, out shiftY);

            LinearMove(shiftX, shiftY);

            IsAnchorPositionMode = Camera1.GetAnchorPosition(out EndImgP1);

            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            double EndX = ArmPoint.X;
            double EndY = ArmPoint.Y;

            if (IsGetAxisPos)
            {
                ConvertCoordinate.ArmCovtoImg((EndX - StartX), (EndY - StartY), out ImgX, out ImgY);
                AxisImgPos.X += ImgX;
                AxisImgPos.Y += ImgY;
                AxisX = ArmPoint.X;
                AxisY = ArmPoint.Y;

                label4.Text = "手臂復歸位置：(" + RevertPosX.ToString() + "," + RevertPosY.ToString() + ")";
                label6.Text = "手臂軸心位置：(" + AxisX.ToString() + "," + AxisY.ToString() + ")";
                label7.Text = "影像軸心位置：(" + AxisImgPos.X.ToString() + "," + AxisImgPos.Y.ToString() + ")";
            }
            double angXY = ConvertCoordinate.ImgToArmAngle((EndX - StartX), (EndY - StartY), (EndImgP1.X - nowPoint.X), (EndImgP1.Y - nowPoint.Y));
            record(record_a, shiftX, (EndX - StartX), shiftY, (EndY - StartY), (Convert.ToSingle(textBox2.Value) - nowPoint.X), (EndImgP1.X - nowPoint.X), (Convert.ToSingle(textBox3.Value) - nowPoint.Y), (EndImgP1.Y - nowPoint.Y), angXY);
            record_a++;
        }

        private void AnchorMoveAssignPoint_Move(double textBoxX, double textBoxY)
        {
            double shiftX = 0;
            double shiftY = 0;

            ConvertCoordinate.ImgCovtoArm((Convert.ToSingle(textBoxX) - AxisImgPos.X), (Convert.ToSingle(textBoxY) - AxisImgPos.Y), out shiftX, out shiftY);

            LinearMove(shiftX, shiftY);

            //if (IsGetAxisPos)
            {
                ModifyAxisLocal();
            }
        }

        /// <summary>
        /// 手臂自動移動(針對點位)
        /// </summary>
        /// <param name="textBoxX">移動X座標</param>
        /// <param name="textBoxY">移動Y座標</param>
        private void AnchorMoveAssignPointH_Move(double textBoxX, double textBoxY)
        {
            if (TsRemote._Robot.GetStatus().RunStatus == 0)//判斷手臂無動作時進入
            {
                //無矯正方需要
                //COMMONK_PerformCoordinatesTransform(pePTMatrix1, textBoxX, textBoxY, Rotation_MatrixX, Rotation_MatrixY);


                //Rotation_Matrix_Int[0, i] = (int)Rotation_MatrixX;
                //Rotation_Matrix_Int[1, i] = (int)Rotation_MatrixY;

                ArmPoint = new TsPointS();

                numericUpDownX.Value = (decimal)textBoxX;
                numericUpDownY.Value = (decimal)textBoxY;

                //numericUpDownX.Value = (decimal)Rotation_MatrixX;
                //numericUpDownY.Value = (decimal)Rotation_MatrixY;

                ArmPoint.X = Convert.ToDouble(numericUpDownX.Value);
                ArmPoint.Y = Convert.ToDouble(numericUpDownY.Value);
                ArmPoint.Z = Convert.ToDouble(numericUpDownZ.Value);
                ArmPoint.C = Convert.ToDouble(numericUpDownC.Value);
                //以下指定無用途
                //TsRemote._Robot.MvConfig = (ConfigS)domainUpDown_Config.SelectedIndex;
                TsRemote._Robot.Moves(ArmPoint);
                Thread.Sleep(10);

                //LinearMove(Rotation_MatrixX, Rotation_MatrixY);
            }
        }

        /*private void ModifyAxisLocal()
        {
            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            ConvertCoordinate.ArmCovtoImg((ArmPoint.X - AxisX), (ArmPoint.Y - AxisY), out ImgX, out ImgY);
            AxisImgPos.X += ImgX;
            AxisImgPos.Y += ImgY;
            AxisX = ArmPoint.X;
            AxisY = ArmPoint.Y;
        }*/

        private void MoveToPoint_Click(object sender, EventArgs e)
        {
            if (Mechanical_Arm_Connection == false)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }
            double shiftX = 0;
            double shiftY = 0;
            Camera1.WaitProcess.Set();

            for (int i = 0; i < Camera1.ObDetect.ObjectLocal.Count; i++)
            {
                ConvertCoordinate.ImgCovtoArm((Camera1.ObDetect.ObjectLocal[i].X - AxisImgPos.X), (Camera1.ObDetect.ObjectLocal[i].Y - AxisImgPos.Y), out shiftX, out shiftY);

                if (shiftX < 340 && -340 < shiftX && shiftY > -340 && shiftY < 340)
                {
                    LinearMove(shiftX, shiftY);
                    ZAxisMove(270);
                    ZAxisMove(280);
                    ModifyAxisLocal();

                    label4.Text = "手臂復歸位置：(" + RevertPosX.ToString() + "," + RevertPosY.ToString() + ")";
                    label6.Text = "手臂軸心位置：(" + AxisX.ToString() + "," + AxisY.ToString() + ")";
                    label7.Text = "影像軸心位置：(" + AxisImgPos.X.ToString() + "," + AxisImgPos.Y.ToString() + ")";

                    MovePoint(RevertPosX, RevertPosY);
                    ModifyAxisLocal();

                    label4.Text = "手臂復歸位置：(" + RevertPosX.ToString() + "," + RevertPosY.ToString() + ")";
                    label6.Text = "手臂軸心位置：(" + AxisX.ToString() + "," + AxisY.ToString() + ")";
                    label7.Text = "影像軸心位置：(" + AxisImgPos.X.ToString() + "," + AxisImgPos.Y.ToString() + ")";
                }
                else
                {
                    label13.Text = "位移距離：超出範圍";
                }
            }
        }

        private void CalMoveDistance_Click(object sender, EventArgs e)
        {
            if (Mechanical_Arm_Connection == false)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }

            double shiftX = 0;
            double shiftY = 0;

            ConvertCoordinate.ImgCovtoArm((Camera1.ObDetect.ObjectLocal[0].X - (float)AxisImgPos.X), (Camera1.ObDetect.ObjectLocal[0].Y - (float)AxisImgPos.Y), out shiftX, out shiftY);

            if (shiftX < 340 && -340 < shiftX && shiftY > -340 && shiftY < 340)
            {
                label13.Text = "位移距離：" + shiftX.ToString() + " , " + shiftY.ToString();
            }
            else
            {
                label13.Text = "位移距離：" + shiftX.ToString() + " , " + shiftY.ToString() + "(超過範圍)";
            }
            Camera1.WaitProcess.Set();
        }

        private void RevertToOrigin_Click(object sender, EventArgs e)
        {
            if (Mechanical_Arm_Connection == false)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }
            MovePoint(RevertPosX, RevertPosY);
            //RotateToZero();

            ModifyAxisLocal();

            label4.Text = "手臂復歸位置：(" + RevertPosX.ToString() + "," + RevertPosY.ToString() + ")";
            label6.Text = "手臂軸心位置：(" + AxisX.ToString() + "," + AxisY.ToString() + ")";
            label7.Text = "影像軸心位置：(" + AxisImgPos.X.ToString() + "," + AxisImgPos.Y.ToString() + ")";
        }

        private void ModifyAxisLocal()
        {
            ArmPoint = TsRemote._Robot.GetPsnFbkWork();
            ConvertCoordinate.ArmCovtoImg((ArmPoint.X - AxisX), (ArmPoint.Y - AxisY), out ImgX, out ImgY);
            AxisImgPos.X += ImgX;
            AxisImgPos.Y += ImgY;
            AxisX = ArmPoint.X;
            AxisY = ArmPoint.Y;
        }

        private void SetParam_Click(object sender, EventArgs e)
        {
            ArmImgAngle = Convert.ToDouble(numericUpDown2.Value);
            if (0 <= Convert.ToDouble(UpLocal.Text) && Convert.ToDouble(UpLocal.Text) <= 300)
                UpLocalVal = Convert.ToDouble(UpLocal.Text);
            if (0 <= Convert.ToDouble(DownLocal.Text) && Convert.ToDouble(DownLocal.Text) <= 300)
                DownLocalVal = Convert.ToDouble(DownLocal.Text);
            if (0 <= Convert.ToDouble(PositionLocal.Text) && Convert.ToDouble(PositionLocal.Text) <= 300)
                PositionLocalVal = Convert.ToDouble(PositionLocal.Text);

            label21.Text = "上升高度位置：" + UpLocalVal.ToString();
            label22.Text = "下降高度位置：" + DownLocalVal.ToString();
            label23.Text = "定位高度位置：" + PositionLocalVal.ToString();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            BS = Convert.ToByte(e.KeyChar);

            TextBox textbox = (TextBox)sender;
            temp_text = textbox.Text;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;

            if ((BS < 48 || BS > 57) && BS != 45 && BS != 46 && BS != 0 && BS != 8 && temp_text.Length > 0)
            {
                BS = 0;
                textbox.Text = temp_text;
                return;
            }
            else if (BS == 8)
            {
                BS = 0;
                return;
            }
            else if (BS == 0)
            {
                return;
            }
            else if (textbox.SelectionStart <= 1 && BS == 46)
            {
                BS = 0;
                textbox.Text = temp_text;
                return;
            }
            else if (textbox.SelectionStart >= 2 && BS == 45)
            {
                BS = 0;
                textbox.Text = temp_text;
                return;
            }
        }

        private void LocateHeight_Click(object sender, EventArgs e)
        {
            ZAxisMove(PositionLocalVal);
        }

        private void ZAxisUp_Click(object sender, EventArgs e)
        {
            ZAxisMove(UpLocalVal);
        }

        private void ZAxisDown_Click(object sender, EventArgs e)
        {
            ZAxisMove(DownLocalVal);
        }

        private void ObDetected_Click(object sender, EventArgs e)
        {
            Camera1.IsDetect = true;
        }

        bool isc = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isc)
                isc = true;
            else
                isc = false;
            Thread ttt = new Thread(GetValue);
            ttt.Start();
        }

        private void GetValue()
        {
            while (isc)
                Console.WriteLine("Point:" + Camera1.AnchorPosition.ToString());
        }

        Image im; //原始影像5620
        double m0, m0_Long, m0_Short, m0_1, m0_2;
        double m1, m1_Long, m1_Short, m1_1, m1_2;
        int ImageLoad = 0;
        int AnalysisImage = 0;



        int image_CountX1_ALL, image_CountY1_ALL;
        int image_CountX1, image_CountY1;   //原始影像特徵點(求斜率用)
        int image_CountX1_Rotate, image_CountY1_Rotate;   //原始影像特徵點(求斜率用)(旋轉後)
        //int image_CountX1_Rotate_ALL, image_CountY1_Rotate_ALL;   //原始影像特徵點(求斜率用)(旋轉後)全部

        //int image_CountX2_ALL, image_CountY2_ALL;
        int image_CountX2, image_CountY2;   //拍攝影像特徵點(求斜率用)
        int image_CountX2_Rotate, image_CountY2_Rotate;//拍攝影像特徵點(求斜率用)(旋轉後)

        Image<Bgr, Byte> Original_Image;  //原始影像(RGB色板)
        //Image<Bgr, Byte> Original_Image_Drawing;  //原始影像(RGB色板)
        Image<Bgr, Byte> Original_Image_Point;  //原始影像(RGB色板)
        //double Original_Image_Rotation_Angle;
        //Mat[] mat_Original_Image;
        //Mat[] mat_Original_Image_H;
        //MCvMat mOriginal_Image;
        Image<Gray, Byte> Original_Image_Gray;  //灰階影像
        Image<Gray, Byte> Original_Image_Gray_Point;  //灰階影像
        Image<Gray, byte> Original_Image_Binary; //二值畫影像
        Image<Gray, byte> Original_Image_Morphology; //形態學處理過後

        /*Image<Bgr, Byte> Original_Image_Rotate;  //原始影像(RGB色板)
        Image<Gray, Byte> Original_Image_Gray_Rotate;  //灰階影像
        Image<Gray, byte> Original_Image_Binary_Rotate; //二值畫影像
        Image<Gray, byte> Original_Image_Binary_Rotate_Morphology; //形態學處理過後*/

        Image<Bgr, Byte> Take_Image;  //拍攝影像(RGB色板)
        //double Take_Image_Rotation_Angle;
        //Mat[] mat_Take_Image;
        //Mat[] mat_Take_Image_H;
        //MCvMat mTake_Image;
        Image<Gray, Byte> Take_Image_Gray;  //灰階影像
        Image<Gray, byte> Take_Image_Binary; //二值畫影像
        Image<Gray, byte> Take_Image_Binary_Morphology; //形態學處理過後

        //Image<Bgr, Byte> Take_Image_Rotate;  //拍攝影像旋轉後(RGB色板)
        Image<Gray, Byte> Take_Image_Gray_Rotate;  //灰階影像旋轉後
        Image<Gray, byte> Take_Image_Binary_Rotate; //二值畫影像旋轉後
        Image<Gray, byte> Take_Image_Binary_Rotate_Morphology; //形態學處理過旋轉後

        VectorOfPoint Original_Feature_points_All = new VectorOfPoint();    //原始影像所有點位
        //double Original_Feature_points_All_double_X;
        //double Original_Feature_points_All_double_Y;

        //MKeyPoint[] Original_Feature_MKeyPoint;    //原始影像特徵點位
        VectorOfPoint Original_Feature_points = new VectorOfPoint();    //原始影像特徵點位

        VectorOfPoint Original_Feature_points_Click = new VectorOfPoint();    //原始影像特徵點位

        VectorOfPoint Original_Feature_points_Rotation_Matrix = new VectorOfPoint();    //原始影像特徵點位(旋轉矩陣)
        //VectorOfPoint Original_Feature_points_Rotation_MatrixH = new VectorOfPoint();    //原始影像特徵點位(旋轉矩陣)

        Point[] StartPoint_Click;

        Point[] StartPoint_Click_Point;
        PointF[] StartPoint_Click_PointF;
        Point[] StartPoint_Click_All;

        VectorOfPoint Original_Feature_points_Rotate_All = new VectorOfPoint();    //原始影像所有點位
        VectorOfPoint Original_Feature_points_Rotate = new VectorOfPoint();    //原始影像所有點位

        VectorOfPoint Take_Feature_points_All = new VectorOfPoint();    //拍攝影像所有點位
        //MKeyPoint[] Take_Feature_MKeyPoint;    //原始影像特徵點位
        VectorOfPoint Take_Feature_points = new VectorOfPoint();    //拍攝影像特徵點位

        VectorOfPoint Take_Feature_points_Rotate_All = new VectorOfPoint(); //拍攝影像所有點位旋轉後
        VectorOfPoint Take_Feature_points_Rotate = new VectorOfPoint(); //拍攝影像特徵點位旋轉後

        int Mechanical_Moving = 0; //手臂還在移動

        //double angle_pi1_m1, angle_pi1_m0;
        double angle_pi1_image; //拍攝影像要旋轉多少角度
        //double angle_pi1_image_m0, angle_pi1_image_m1;

        double Take_image_MOVE_X;    //X要平移多少
        double Take_image_MOVE_Y;    //Y要平移多少

        double Original_image_MOVE_X;    //X要平移多少
        double Original_image_MOVE_Y;    //Y要平移多少

        Image<Gray, byte> rotateImage_NEW;
        //Image<Bgr, byte> rotateImage_NEW_RGB;
        //Image<Bgr, byte> rotateImage_Original;

        int X_Long_MAX = 0, X_Short_MAX;
        int Y_Long_MAX = 0, Y_Short_MAX;
        bool homography_data;

        Mat[] Original_Src_Image;
        Mat[] Take_Dst_Image;

        int Record_Points_Mechanical_Arm_All_X_Min;//找特徵點最短點X
        int Record_Points_Mechanical_Arm_All_Y_Min;//找特徵點最短點Y
        int Record_Points_Mechanical_Arm_All_X_Max;//找特徵點最長點X
        int Record_Points_Mechanical_Arm_All_Y_Max;//找特徵點最長點Y

        //讀取影像按鈕(可設定CCD或讀影像)
        private void btn_ImageLoad_Click(object sender, EventArgs e)
        {
            Original_Feature_points_All.Clear();
            //影像
            if (Use_Camera == false)
            {
                openFileDialog1.InitialDirectory = @"C:\Users\Public\Pictures\Sample Pictures";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    im = Image.FromFile(openFileDialog1.FileName);
                    Original_Image = new Image<Bgr, byte>(openFileDialog1.FileName);
                    Original_Image_Gray = new Image<Gray, byte>(Original_Image.Bitmap);

                    Display_Img.Image = Original_Image_Gray;
                    imageBox1.Image = Original_Image_Gray;
                    Original_Image.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model.bmp");
                }
                else
                {
                    if (openFileDialog1.FileName == "openFileDialog1")
                    {
                        btn_Get_Point.Enabled = false;
                    }
                    return;
                }
            }
            else
            {
                if (Camera1 == null)
                {
                    MessageBox.Show("相機尚未連線");
                    return;
                }
                if (Camera_comboBox.SelectedItem == null)
                {
                    MessageBox.Show("相機尚未找到");
                    return;
                }
                //CCd專用
                //Original_Image = Camera1.GrabImage;
                //Original_Image_Gray = Camera1.GrabImage;
                imageBox1.Image = Camera1.GrabImage;
                Camera1.GrabImage.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model.bmp");
            }
            Original_Image = new Image<Bgr, byte>("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model.bmp");
            Original_Image_Gray = new Image<Gray, byte>(Original_Image.Bitmap);

            Record_Points_Mechanical_Arm_All_X_Min = 99999;
            Record_Points_Mechanical_Arm_All_Y_Min = 99999;
            Record_Points_Mechanical_Arm_All_X_Max = -99999;
            Record_Points_Mechanical_Arm_All_Y_Max = -99999;

            if (Use_Perspective_Transform == true)
            {
                for (int i = 0; i < Main_Ini.Record_Points_Mechanical_Arm_All_length; i++)
                {
                    //找最大
                    if (Main_Ini.Record_Points_Mechanical_Arm_All[i].X > Record_Points_Mechanical_Arm_All_X_Max)
                    {
                        Record_Points_Mechanical_Arm_All_X_Max = Main_Ini.Record_Points_Mechanical_Arm_All[i].X;
                    }
                    if (Main_Ini.Record_Points_Mechanical_Arm_All[i].Y > Record_Points_Mechanical_Arm_All_Y_Max)
                    {
                        Record_Points_Mechanical_Arm_All_Y_Max = Main_Ini.Record_Points_Mechanical_Arm_All[i].Y;
                    }

                    //找最小
                    if (Main_Ini.Record_Points_Mechanical_Arm_All[i].X < Record_Points_Mechanical_Arm_All_X_Min)
                    {
                        Record_Points_Mechanical_Arm_All_X_Min = Main_Ini.Record_Points_Mechanical_Arm_All[i].X;
                    }
                    if (Main_Ini.Record_Points_Mechanical_Arm_All[i].Y < Record_Points_Mechanical_Arm_All_Y_Min)
                    {
                        Record_Points_Mechanical_Arm_All_Y_Min = Main_Ini.Record_Points_Mechanical_Arm_All[i].Y;
                    }
                }

                for (int i = 0; i < Main_Ini.Record_Points_Mechanical_Arm_All_length; i++)
                {
                    //srcp[i, 0] = (Main_Ini.Record_Points_Mechanical_Arm_All[i].X - 360) / 110 * Original_Image_Gray.Height;
                    //srcp[i, 1] = (Main_Ini.Record_Points_Mechanical_Arm_All[i].Y + 170) / 120 * Original_Image_Gray.Width;

                    srcp[i, 0] = ((Main_Ini.Record_Points_Mechanical_Arm_All[i].X - Record_Points_Mechanical_Arm_All_X_Min) / (Record_Points_Mechanical_Arm_All_X_Max - Record_Points_Mechanical_Arm_All_X_Min)) * Original_Image_Gray.Height;
                    srcp[i, 1] = ((Main_Ini.Record_Points_Mechanical_Arm_All[i].Y - Record_Points_Mechanical_Arm_All_Y_Min) / (Record_Points_Mechanical_Arm_All_Y_Max - Record_Points_Mechanical_Arm_All_Y_Min)) * Original_Image_Gray.Height;

                    //srcp[i, 0] = (Main_Ini.Record_Points_Mechanical_Arm_All[i].X - 360);
                    //srcp[i, 1] = (Main_Ini.Record_Points_Mechanical_Arm_All[i].Y + 170);

                    dstp[i, 0] = (Main_Ini.Record_Points_Image_All[i].X);
                    dstp[i, 1] = (Main_Ini.Record_Points_Image_All[i].Y);
                }

                Matrix<float> c1 = new Matrix<float>(srcp);
                Matrix<float> c2 = new Matrix<float>(dstp);
                Matrix<float> homogm = new Matrix<float>(homog);

                Mat mask = new Mat();
                //Matrix mask1 = new Matrix();
                //Matrix<float> c1 = new Matrix<float>(srcp);
                //Matrix<float> c2 = new Matrix<float>(dstp);
                //Matrix<float> homogm = new Matrix<float>(homog);
                Mat[] src_img = Original_Image_Gray.Mat.Split();
                Mat[] dst_img = src_img;
                Mat src_img_1 = new Mat(Original_Image_Gray.Height, Original_Image_Gray.Height, Emgu.CV.CvEnum.DepthType.Default, 1);
                //Mat src_img_1 = new Mat(2000, 2000, Emgu.CV.CvEnum.DepthType.Default, 1);
                //
                mask = CvInvoke.GetPerspectiveTransform(c2, c1);
                CvInvoke.FindHomography(c2, c1, homogm, Emgu.CV.CvEnum.HomographyMethod.Default, 0, mask);
                CvInvoke.WarpPerspective(src_img[0], dst_img[0], homogm, src_img_1.Size, Emgu.CV.CvEnum.Inter.Linear);

                dst_img[0].Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_H.bmp");
                Original_Src_Image = dst_img;
                imageBox1.Image = dst_img[0];
                Display_Img.Image = dst_img[0];
            }
            else
            {
                Original_Src_Image = Original_Image_Gray.Mat.Split();
                Original_Src_Image[0].Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_H.bmp");
                imageBox1.Image = Original_Src_Image[0];
                Display_Img.Image = Original_Src_Image[0];
 
            }

            Original_Image = new Image<Bgr, byte>("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_H.bmp");
            Original_Image_Gray = new Image<Gray, byte>(Original_Image.Bitmap);

            /*Original_Image.Rotate(30, new Bgr(255, 255, 255)).Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\1124.bmp"); ;//實際灰階影像
            Original_Image.Rotate(-30, new Bgr(255, 255, 255)).Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\1125.bmp"); ;//實際灰階影像*/

            //Original_Image_Point = Original_Image;
            //Original_Image_Gray_Point = Original_Image_Gray;

            //
            //pictureBox1.Image = Original_Image_Gray;
            
            //pictureBox1.Visible = true;
            imageBox1.Visible = true;
            btn_Get_Point.Enabled = true;

            //Original_Image_Gray.Rotate(45, new Gray(255)).Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\2221.bmp"); ;
            //比對影像取得灰階影像
            Original_Image_Binary = Original_Image_Gray.ThresholdBinary(new Gray(127), new Gray(255));
            //比對影像形態學處理
            Original_Image_Morphology = Original_Image_Binary.Erode(10);
            Original_Image_Morphology = Original_Image_Morphology.Dilate(10);
            //比對影像邊緣偵測
            Original_Image_Morphology = Original_Image_Morphology.Canny(100, 500);

            //特徵點使用
            /*//Construct the SIFT feature detector object
            SIFTDetector sift = new SIFTDetector(
                0,  //the desired number of features
                3,    //the number of octave layers
                0.04, //feature threshold
                10,   //detector parameter
                1.6); //sigma
            //feature point detection
            VectorOfKeyPoint keypoints = sift.DetectKeyPointsRaw(image, null);
            //draw keypoints on an image
            Image<Bgr, byte> result = Features2DToolbox.DrawKeypoints<Gray>(
                image,     //original image
                keypoints, //vector of keypoints 
                new Bgr(255, 255, 255), // keypoint color
                Features2DToolbox.KeypointDrawType.DRAW_RICH_KEYPOINTS); //drawing type*/

            /*fileAddress = dlg.FileName;
            cap = new Emgu.CV.Capture(dlg.FileName);
            cap.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_POS_FRAMES, 3945);
            imGray = cap.QueryGrayFrame();*/
            //Emgu.CV.Features2D.SIFTDetector siftDet = new Emgu.CV.Features2D.SIFTDetector();
            //siftDet.DetectKeyPoints(imGray);
            //MessageBox.Show("test SIFT");

            //Emgu.CV.Features2D.SIFTDetector siftDet1 = new Emgu.CV.Features2D.SIFTDetector();

            /*Emgu.CV.XFeatures2D.SIFT siftDet = new Emgu.CV.XFeatures2D.SIFT();
            
            //siftDet.DetectKeyPoints(imGray);
            Original_Feature_MKeyPoint = siftDet.Detect(Original_Image_Gray);
            Original_Image_Rotation_Angle = 0;
            for (int i = 0; i < Original_Feature_MKeyPoint.Length; i++)
            {
                Original_Image_Rotation_Angle += Original_Feature_MKeyPoint[i].Angle;
            }
            Original_Image_Rotation_Angle = Original_Image_Rotation_Angle / Original_Feature_MKeyPoint.Length;*/

            //比對影像轉Mat格式
            //mat_Original_Image = Original_Image_Gray.Mat.Split();

            //mOriginal_Image = (MCvMat)Marshal.PtrToStructure(Original_Image_Gray.Ptr, typeof(MCvMat));

            //Display_Img.Image = Original_Image_Gray;

            /*cannyFrame2 = Take_Image_Binary_Morphology;
            cannyFrame3 = Original_Image_Gray;*/

            //cannyFrame1.
            //CvInvoke.FindContours(cannyFrame1,cannyFrame2,1);
            //cannyFrame1.FindCornerSubPix(new PointF[1][] { corners }, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));  
            //CvInvoke.Threshold
            //findContours(image, contours, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_NONE);
            //VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            Mat hierachy = new Mat();
            IInputOutputArray result1 = new Mat();

            //比對影像找全部邊緣
            Original_Feature_points_All = FindLargestContour(Original_Image_Morphology, result1);
            //比對影像找邊緣特徵點
            CvInvoke.ApproxPolyDP(Original_Feature_points_All, Original_Feature_points, 10, true);

            //比對影像求斜率
            /*m0 = ((double)(Original_Feature_points[1].Y - Original_Feature_points[0].Y) / (double)(Original_Feature_points[1].X - Original_Feature_points[0].X));

            double X_Long = Original_Feature_points[1].X - Original_Feature_points[0].X;
            double Y_Long = Original_Feature_points[1].Y - Original_Feature_points[0].Y;
            m0_Long = Math.Sqrt((X_Long * X_Long) + (Y_Long * Y_Long));

            angle_pi1_m0 = Math.Atan(m0);
            angle_pi1_m0 = (double)(angle_pi1_m0 * 180) / (double)Math.PI;

            if (angle_pi1_m0 < 0)
            {
                angle_pi1_m0 = 180 - 90 + angle_pi1_m0;
            }*/
            if (Center_Gravity_Calculation == true)
            {
                image_CountY1 = 0;
                image_CountX1 = 0;
                //找重心特徵點
                /*
                for (int k = 0; k < Original_Feature_points.Size; k++)
                {
                    image_CountY1 += Original_Feature_points[k].Y;
                    image_CountX1 += Original_Feature_points[k].X;
                    //if (Feature_points_All[k].Y == j && Feature_points_All[k].X == i)
                    {
                        //cannyFrame3[Feature_points_All[k].Y, Feature_points_All[k].X] = new Gray(0);
                    }
                }
                image_CountX1 = image_CountX1 / Original_Feature_points.Size;
                image_CountY1 = image_CountY1 / Original_Feature_points.Size;
                */
            
                //比對影像找重心ALL
                for (int k = 0; k < Original_Feature_points_All.Size; k++)
                {
                    image_CountY1_ALL += Original_Feature_points_All[k].Y;
                    image_CountX1_ALL += Original_Feature_points_All[k].X;
                }
                image_CountX1_ALL = image_CountX1_ALL / Original_Feature_points_All.Size;
                image_CountY1_ALL = image_CountY1_ALL / Original_Feature_points_All.Size;

                //Console.WriteLine("第一張圖重心 X = " + image_CountX1_ALL.ToString() + " Y = " + image_CountY1_ALL.ToString());

                //比對影像所有邊緣用紅色顯示
                /*for (int k = 0; k < Original_Feature_points_All.Size; k++)
                {
                    //在圖案內才畫圖，超過則不畫
                    if (Original_Feature_points_All[k].Y < Original_Image.Height && Original_Feature_points_All[k].X < Original_Image.Width && Original_Feature_points_All[k].Y >= 0 && Original_Feature_points_All[k].X >= 0)
                    {
                        Original_Image[Original_Feature_points_All[k].Y, Original_Feature_points_All[k].X] = new Bgr(0, 0, 255);
                    }
                }

                //比對影像所有特徵點用藍色顯示
                for (int k = 0; k < Original_Feature_points.Size; k++)
                {
                    //在圖案內才畫圖，超過則不畫
                    if (Original_Feature_points[k].Y < Original_Image.Height && Original_Feature_points[k].X < Original_Image.Width && Original_Feature_points[k].Y >= 0 && Original_Feature_points[k].X >= 0)
                    {
                        Original_Image[Original_Feature_points[k].Y, Original_Feature_points[k].X] = new Bgr(255, 0, 0);
                    }
                }
                //Original_Image[image_CountY1, image_CountX1] = new Bgr(255, 0, 0);

                //比對影像重心用黃色顯示//在圖案內才畫圖，超過則不畫
                if (image_CountX1_ALL < Original_Image.Height && image_CountY1_ALL < Original_Image.Width && image_CountY1_ALL >= 0 && image_CountX1_ALL >= 0)
                {
                    Original_Image[image_CountY1_ALL, image_CountX1_ALL] = new Bgr(0, 255, 255);
                }*/

            }

            /*btnAuto.Enabled = true;
            chang_Image = 0;
            case_1 = 0;*/

            //比對影像取得完畢
            ImageLoad = 1;
            btn_AnalysisImage.Enabled = true;
            //CCD
            /*imageBox1.Visible = true;
            grayFrame = Camera1.GrabImage;
            imageBox1.Image = grayFrame;*/
        }

        Point Center_Of_Gravity;
        string Displays_Numbers;

        //分析影像按鈕
        private void btn_AnalysisImage_Click(object sender, EventArgs e)
        {
            //Take_Feature_points.Clear();
            Take_Feature_points_All.Clear();
            Take_Feature_points_Rotate_All.Clear();
            image_CountY2_Rotate = 0;
            image_CountX2_Rotate = 0;

            double[,] Rotation_Matrix = new double[2, Original_Feature_points.Size];//特徵點旋轉
            double[,] Rotation_Matrix_All = new double[2, Original_Feature_points_All.Size];//所有點旋轉(需要求旋轉後的重心)

            int[,] Rotation_Matrix_Int = new int[2, Original_Feature_points.Size];//特徵點旋轉加上偏移量的結果
            int[,] Rotation_Matrix_Int_All = new int[2, Original_Feature_points_All.Size];//所有點旋轉加上偏移量的結果

            //double Rotation_Matrix_X = 0, Rotation_Matrix_Y = 0;

            //重心累計需要歸零
            double Rotation_Matrix_X_All = 0, Rotation_Matrix_Y_All = 0;
            /*int Rotation_Matrix_Xi = 0, Rotation_Matrix_Yi = 0;
            double Rotation_Matrix_X_Count, Rotation_Matrix_Y_Count;*/

            //影像
            if (Use_Camera == false)
            {
                openFileDialog1.InitialDirectory = @"C:\Users\Public\Pictures\Sample Pictures";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Take_Image = new Image<Bgr, byte>(openFileDialog1.FileName);
                    Take_Image_Gray = new Image<Gray, byte>(Take_Image.Bitmap);
                    Take_Image.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest.bmp");
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (Camera1 == null)
                {
                    MessageBox.Show("相機尚未連線");
                    return;
                }
                //CCd專用
                Take_Image_Gray = Camera1.GrabImage;
                Take_Image_Gray.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest.bmp");
            }

            Take_Image = new Image<Bgr, byte>("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest.bmp");
            Take_Image_Gray = new Image<Gray, byte>(Take_Image.Bitmap);

            //計算運算時間
            DateTime date1 = DateTime.Now;

            //實際影像重心累計需要歸零
            image_CountY2 = 0;
            image_CountX2 = 0;
            //實際影像顯示
            cannyImageBox.Visible = true;
            //pictureBox1.Visible = true;


            if (Use_Perspective_Transform == true)
            {
                Matrix<float> c1 = new Matrix<float>(srcp);
                Matrix<float> c2 = new Matrix<float>(dstp);
                Matrix<float> homogm = new Matrix<float>(homog);

                Mat mask = new Mat();
                Matrix mask1 = new Matrix();
                //Matrix<float> c1 = new Matrix<float>(srcp);
                //Matrix<float> c2 = new Matrix<float>(dstp);
                //Matrix<float> homogm = new Matrix<float>(homog);
                Mat[] src_img = Take_Image_Gray.Mat.Split();
                Mat[] dst_img = src_img;
                Mat src_img_1 = new Mat(Take_Image_Gray.Height, Take_Image_Gray.Height, Emgu.CV.CvEnum.DepthType.Default, 1);
                //Mat src_img_1 = new Mat(2000, 2000, Emgu.CV.CvEnum.DepthType.Default, 1);

                //
                mask = CvInvoke.GetPerspectiveTransform(c2, c1);
                CvInvoke.FindHomography(c2, c1, homogm, Emgu.CV.CvEnum.HomographyMethod.Default, 0, mask);

                CvInvoke.WarpPerspective(src_img[0], dst_img[0], homogm, src_img_1.Size, Emgu.CV.CvEnum.Inter.Linear);

                dst_img[0].Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest_H.bmp");
                Take_Dst_Image = dst_img;

                Display_Img.Image = dst_img[0];
                Take_Image = new Image<Bgr, byte>("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest_H.bmp");
                Take_Image_Gray = new Image<Gray, byte>(Take_Image.Bitmap);
            }
            else
            {
                Take_Dst_Image = Take_Image_Gray.Mat.Split();
                //Take_Dst_Image[0].Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest_H.bmp");
                Display_Img.Image = Take_Dst_Image[0];
 
            }
           
            //Original_Src_Image;
            
            //將實際影像顯示至畫面
            cannyImageBox.Image = Take_Image_Gray;

            //實際影像取得灰階影像
            Take_Image_Binary = Take_Image_Gray.ThresholdBinary(new Gray(127), new Gray(255));
            //實際影像形態學處理
            Take_Image_Binary_Morphology = Take_Image_Binary.Erode(10);
            Take_Image_Binary_Morphology = Take_Image_Binary_Morphology.Dilate(10);
            //實際影像邊緣偵測
            Take_Image_Binary_Morphology = Take_Image_Binary_Morphology.Canny(100, 500);

            //VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierachy = new Mat();

            //hierachy = Take_Image_Binary_Morphology;

            IInputOutputArray result = new Mat();
            //VectorOfPoint approxContour = new VectorOfPoint();

            //Take_Image_Gray.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\22.bmp");
            //Take_Image_Gray.Rotate(45, new Gray(255)).Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\2221.bmp"); ;

            //實際影像找全部邊緣
            Take_Feature_points_All = FindLargestContour(Take_Image_Binary_Morphology, result);

            //取得實際影像特徵點
            /*Emgu.CV.XFeatures2D.SIFT siftDet = new Emgu.CV.XFeatures2D.SIFT();

            Take_Feature_MKeyPoint = siftDet.Detect(Take_Image_Gray);//Detected

            Take_Image_Rotation_Angle = 0;

            Take_Feature_MKeyPoint = siftDet.Detect(Take_Image_Gray);*/

            /*for (int i = 0; i < Take_Feature_MKeyPoint.Length; i++)
            {
                Take_Image_Rotation_Angle += Take_Feature_MKeyPoint[i].Angle;
            }
            Take_Image_Rotation_Angle = Take_Image_Rotation_Angle / Take_Feature_MKeyPoint.Length;*/


            /*for (int i = 0; i < Take_Feature_points.Length; i++)
            {
                for (int j = 0; j < Original_Feature_points.Length; j++)
                {
                    if (Take_Feature_points[i].Angle == Original_Feature_points[i].Angle)
                    {
                        Take_Feature_points[i].Angle = 0;
                    }
                }
            }*/

            /*double aaa_dd = Original_Image_Rotation_Angle - Take_Image_Rotation_Angle;

            angle_pi1_image = Math.Atan(aaa_dd);
            angle_pi1_image = (double)(angle_pi1_image * 180) / (double)Math.PI;*/

            //mTake_Image = (MCvMat)Marshal.PtrToStructure(Take_Image_Gray.Ptr, typeof(MCvMat));
            //siftDet.DetectKeyPoints(imGray);
            //Cv.GetRotationMatrix2D

            //實際影像找邊緣特徵點
            CvInvoke.ApproxPolyDP(Take_Feature_points_All, Take_Feature_points, 10, true);
         
            /*for (int i = 0; i < Take_Feature_points.Size; i++)
            {
                for (int j = 0; j < Take_Feature_points.Size; j++)
                {
                    if (X_Long_MAX != 0 || Y_Long_MAX != 0)
                    { }
                    else
                    {
                        if (i != j)
                        {
                            double X_Long = Take_Feature_points[i].X - Take_Feature_points[j].X;
                            double Y_Long = Take_Feature_points[i].Y - Take_Feature_points[j].Y;
                            m1_Long = Math.Sqrt((X_Long * X_Long) + (Y_Long * Y_Long));

                            if (Math.Abs(m1_Long - m0_Long) <= 2)
                            {
                                X_Long_MAX = i;
                                Y_Long_MAX = j;
                            }
                            if ((int)m1_Long == (int)m0_Long)
                            {
                                X_Long_MAX = i;
                                Y_Long_MAX = j;
                                break;
                            }
                        }
                    }
                }
            }

            //double X_Long = Take_Feature_points[1].X - Take_Feature_points[0].X;
            //double Y_Long = Take_Feature_points[1].Y - Take_Feature_points[0].Y;
            //m0_Long = Math.Sqrt((X_Long * X_Long) + (Y_Long * Y_Long));

            if ((Take_Feature_points[X_Long_MAX].Y - Take_Feature_points[Y_Long_MAX].Y) == 0 || (Take_Feature_points[X_Long_MAX].X - Take_Feature_points[Y_Long_MAX].X) == 0)
            {
                m1 = 90;
            }
            else
            {
                //m1 = ((double)(Take_Feature_points[1].Y - Take_Feature_points[0].Y) / (double)(Take_Feature_points[1].X - Take_Feature_points[0].X));
                m1 = ((double)(Take_Feature_points[X_Long_MAX].Y - Take_Feature_points[Y_Long_MAX].Y) / (double)(Take_Feature_points[X_Long_MAX].X - Take_Feature_points[Y_Long_MAX].X));
            }
            //m1 = ((double)(Take_Feature_points[2].Y - Take_Feature_points[3].Y) / (double)(Take_Feature_points[2].X - Take_Feature_points[3].X));
            angle_pi1_m1 = Math.Atan(m1);
            angle_pi1_m1 = (double)(angle_pi1_m1 * 180) / (double)Math.PI;

            double ang1 = Math.Abs((double)(m0 - m1) / (double)(1 + m0 * m1));
            angle_pi1_image = Math.Atan(ang1);
            angle_pi1_image = (double)(angle_pi1_image * 180) / (double)Math.PI;

            if (angle_pi1_m1 < 0)
            {
                angle_pi1_m1 = 180 - 90 + angle_pi1_m1;
            }

            if (angle_pi1_m1 < angle_pi1_m0)
            {
                //angle_pi1_image = -angle_pi1_image;
            }

            angle_pi1_image = -angle_pi1_image;
            Console.WriteLine(angle_pi1_m1.ToString() +","+ angle_pi1_m0.ToString());*/

            //IOutputArray 
            //IInputArray
            /*IInputArray c1 = new Matrix<float>(srcp);
            IInputArray c2 = new Matrix<float>(dstp);
            IOutputArray homogm = new Matrix<float>(homog);*/

            /*Matrix<float> c1 = new Matrix<float>(srcp);
            Matrix<float> c2 = new Matrix<float>(dstp);
            Matrix<float> homogm = new Matrix<float>(homog);

            Mat mask = new Mat();
            Matrix mask1 = new Matrix();
            //Matrix<float> c1 = new Matrix<float>(srcp);
            //Matrix<float> c2 = new Matrix<float>(dstp);
            //Matrix<float> homogm = new Matrix<float>(homog);

            //CvInvoke.FindHomography(c1, c2, homogm, Emgu.CV.CvEnum.HomographyMethod.Default, 0, mask);
            mask = CvInvoke.GetPerspectiveTransform(c1, c2);

            Image<Bgr, byte> newImage = Take_Image.WarpPerspective(mask1, Emgu.CV.CvEnum.Inter.Cubic, Emgu.CV.CvEnum.Warp.Default, Emgu.CV.CvEnum.BorderType.Wrap, new Bgr(0, 0, 0));*/
            //mask, Take_Image_Gray.Width, Take_Image_Gray.Height, Emgu.CV.CvEnum.Inter.Cubic, Emgu.CV.CvEnum.Warp.Default,Emgu.CV.CvEnum.BorderType.Wrap, new Bgr(0, 0, 0)

            //CvInvoke.WarpPerspective(mat_Original_Image, mat_Original_Image_H, pePTMatrix1, Size dsize, int flags=INTER_LINEAR, intborderMode=BORDER_CONSTANT, const Scalar& borderValue=Scalar());
            //Image<Bgr, byte> newImage = Take_Image_Gray.WarpPerspective(pePTMatrix1, Take_Image_Gray.Width, Take_Image_Gray.Height, Emgu.CV.CvEnum.Inter.Cubic, Emgu.CV.CvEnum.Warp.Default, new Bgr(0, 0, 0));
            //Image<Bgr, byte> newImage = Take_Image.WarpPerspective(pePTMatrix1, Emgu.CV.CvEnum.Inter.Cubic, Emgu.CV.CvEnum.Warp.Default, Emgu.CV.CvEnum.BorderType.Wrap, new Bgr(0, 0, 0));
                  
            /*Size targetImageSize;
            InputArray pePTMatrixl_H = pePTMatrix1;
            using (var cutImagePortion = new Mat())
            {
                CvInvoke.WarpPerspective(mat_Original_Image[0], cutImagePortion, pePTMatrix1, targetImageSize, Inter.Cubic);
                //return cutImagePortion.ToImage<TColor, TDepth>();
            }*/
            
            //實際影像轉Mat格式
            //mat_Take_Image = Take_Image_Gray.Mat.Split();
            homography_data = false;
            long Operation_Hours;

            //找出兩張影像旋轉角度及匹配影像

            //Original_Src_Image
            //Take_Dst_Image, Original_Src_Image;
            Mat Matching_Images = Draw(Original_Src_Image[0], Take_Dst_Image[0], out Operation_Hours, angle_pi1_image);
            //Mat Matching_Images = Draw(mat_Original_Image[0], mat_Take_Image[0], out Operation_Hours, angle_pi1_image);
            if (Use_SaveImage == true)
            {
                Matching_Images.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\SURT.bmp");
            }
            //顯示匹配影像
            Display_Img.Image = Matching_Images;

            if (Center_Gravity_Calculation == true)
            {
                double m0_Long_new;
                m0_Long = 0;
                m0_Short = 9999;
                X_Long_MAX = 0;
                Y_Long_MAX = 0;
                X_Short_MAX = 0;
                Y_Short_MAX = 0;
                //如果找不到旋轉角度
                if (homography_data == false || Original_Feature_points.Size < 5 && Take_Feature_points.Size < 5)
                {
                    for (int i = 0; i < Original_Feature_points.Size; i++)
                    {
                        for (int j = 0; j < Original_Feature_points.Size; j++)
                        {
                            if (i != j)
                            {
                                double X_Long = Original_Feature_points[i].X - Original_Feature_points[j].X;
                                double Y_Long = Original_Feature_points[i].Y - Original_Feature_points[j].Y;
                                m0_Long_new = Math.Sqrt((X_Long * X_Long) + (Y_Long * Y_Long));

                                if (m0_Long_new > m0_Long)
                                {
                                    m0_Long = m0_Long_new;
                                    X_Long_MAX = i;
                                    Y_Long_MAX = j;
                                }
                                if (m0_Long_new < m0_Short)
                                {
                                    m0_Short = m0_Long_new;
                                    X_Short_MAX = i;
                                    Y_Short_MAX = j;
                                }
                            }
                        }
                    }

                    m0 = ((double)(Original_Feature_points[X_Long_MAX].Y - Original_Feature_points[Y_Long_MAX].Y) / (double)(Original_Feature_points[X_Long_MAX].X - Original_Feature_points[Y_Long_MAX].X));
                    m0_2 = ((double)(Original_Feature_points[X_Short_MAX].Y - Original_Feature_points[Y_Short_MAX].Y) / (double)(Original_Feature_points[X_Short_MAX].X - Original_Feature_points[Y_Short_MAX].X));

                    /*angle_pi1_image_m0 = (double)(Math.Atan(m0) * 180) / (double)Math.PI;
                    angle_pi1_image_m0 = (double)(Math.Atan(m0_2) * 180) / (double)Math.PI;*/

                    /*double ang1 = Math.Abs((double)(m0 - m0_2) / (double)(1 + m0 * m0_2));
                    angle_pi1_image_m0 = Math.Atan(ang1);
                    angle_pi1_image_m0 = (double)(angle_pi1_image_m0 * 180) / (double)Math.PI;*/

                    //angle_pi1_image_m0 = (double)(Math.Atan(m0) * 180) / (double)Math.PI;

                    /*angle_pi1_image_m0 = Math.Atan(m0);
                    angle_pi1_image_m0 = (double)(angle_pi1_image_m0 * 180) / (double)Math.PI;*/

                    Y_Long_MAX = 0;
                    X_Long_MAX = 0;
                    X_Short_MAX = 0;
                    Y_Short_MAX = 0;
                    for (int i = 0; i < Take_Feature_points.Size; i++)
                    {
                        for (int j = 0; j < Take_Feature_points.Size; j++)
                        {
                            if (i != j)
                            {
                                double X_Long = Take_Feature_points[i].X - Take_Feature_points[j].X;
                                double Y_Long = Take_Feature_points[i].Y - Take_Feature_points[j].Y;
                                m1_Long = Math.Sqrt((X_Long * X_Long) + (Y_Long * Y_Long));

                                if (Math.Abs(m1_Long - m0_Long) <= 0.5)
                                {
                                    X_Long_MAX = i;
                                    Y_Long_MAX = j;
                                }
                                if (Math.Abs(m1_Long - m0_Short) <= 0.5)
                                {
                                    X_Short_MAX = i;
                                    Y_Short_MAX = j;
                                }
                            }
                        }
                    }
                    m1 = ((double)(Take_Feature_points[X_Long_MAX].Y - Take_Feature_points[Y_Long_MAX].Y) / (double)(Take_Feature_points[X_Long_MAX].X - Take_Feature_points[Y_Long_MAX].X));
                    m1_2 = ((double)(Take_Feature_points[X_Short_MAX].Y - Take_Feature_points[Y_Short_MAX].Y) / (double)(Take_Feature_points[X_Short_MAX].X - Take_Feature_points[Y_Short_MAX].X));
                    /*angle_pi1_image_m1 = Math.Atan(m1);
                    angle_pi1_image_m1 = (double)(angle_pi1_image_m1 * 180) / (double)Math.PI;*/

                    //double ang1 = Math.Abs((double)(m0_2 - m1_2) / (double)(1 + m0_2 * m1_2));
                    double ang1 = Math.Abs((double)(m0 - m1) / (double)(1 + m0 * m1));
                    angle_pi1_image = Math.Atan(ang1);
                    angle_pi1_image = (double)(angle_pi1_image * 180) / (double)Math.PI;

                    /*ang1 = Math.Abs((double)(m1 - m1_2) / (double)(1 + m1 * m1_2));
                    angle_pi1_image_m1 = Math.Atan(ang1);
                    angle_pi1_image_m1 = (double)(angle_pi1_image_m1 * 180) / (double)Math.PI;*/

                    /*if (m0 <0 )
                        angle_pi1_image = -angle_pi1_image;
                    if ( m0_2<0)
                        angle_pi1_image = -angle_pi1_image;*/

                    /*if (m0 < 0 && m0_2 > 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }*/

                    /*if (m1 < 0 && m1_2 > 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }

                    if (m0 > 0 && m0_2 > 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }*/

                    /*if (m0 > 0 && m0_2 < 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }
                    else if (m1 > 0 && m1_2 < 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }
                 
                    if (m1 > 0 && m1_2 > 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }*/

                    if (m1_2 < 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }
                    else if (m0_2 > m1_2)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }

                    /*if (angle_pi1_image_m1 >= angle_pi1_image_m0)
                        angle_pi1_image = -angle_pi1_image;*/

                    /*if (Take_Feature_points[Y_Long_MAX].X > Take_Feature_points[X_Long_MAX].X)//&& (Math.Abs(Math.Round(RotationAngle, 0)) < 90 || Math.Abs(Math.Round(RotationAngle, 0)) > 0)
                    {
                        angle_pi1_image = -angle_pi1_image;
                    }*/

                    /*if (angle_pi1_m1 < 0)
                    {
                        angle_pi1_m1 = 180 - 90 + angle_pi1_m1;
                    }


                    if (angle_pi1_m1 < angle_pi1_m0)
                    {
                        //angle_pi1_image = -angle_pi1_image;
                    }*/
                }

                //找實際影像全部重心
                for (int k = 0; k < Take_Feature_points_All.Size; k++)
                {
                    image_CountY2 += Take_Feature_points_All[k].Y;
                    image_CountX2 += Take_Feature_points_All[k].X;
                }
                image_CountX2 = image_CountX2 / Take_Feature_points_All.Size;
                image_CountY2 = image_CountY2 / Take_Feature_points_All.Size;

                //Console.WriteLine("第二張圖重心 X = " + image_CountX2.ToString() + " Y = " + image_CountY2.ToString());

                //影像旋轉
                //Take_Image_Rotate = Take_Image.Rotate(-angle_pi1_image, new Bgr(System.Drawing.Color.White));       //更改後影像(轉的跟原始影像一樣)
                Take_Image_Gray_Rotate = Take_Image_Gray.Rotate(-angle_pi1_image, new Gray(255));//實際灰階影像
                //Take_Image_Rotate = Take_Image.Rotate(-angle_pi1_image, new Bgr(255, 255, 255));//實際原始影像
                /*Original_Image_Rotate = Original_Image.Rotate(angle_pi1_image, new Bgr(System.Drawing.Color.White));    //原始影像(轉的跟更改後影像一樣)
                Original_Image_Gray_Rotate = Original_Image_Gray.Rotate(angle_pi1_image, new Gray(255));    //原始影像(轉的跟更改後影像一樣)
                //Original_Image_Rotate = Original_Image.Rotate(angle_pi1_image, new Bgr(255, 255, 255));    //原始影像(轉的跟更改後影像一樣)

                IInputOutputArray result1 = new Mat();
                Original_Image_Binary_Rotate = Original_Image_Gray_Rotate.ThresholdBinary(new Gray(127), new Gray(255));
                Original_Image_Binary_Rotate_Morphology = Original_Image_Binary_Rotate.Erode(10);
                Original_Image_Binary_Rotate_Morphology = Original_Image_Binary_Rotate_Morphology.Dilate(10);
                Original_Image_Binary_Rotate_Morphology = Original_Image_Binary_Rotate_Morphology.Canny(100, 500);
                Original_Feature_points_Rotate_All = FindLargestContour(Original_Image_Binary_Rotate_Morphology, result1);

                CvInvoke.ApproxPolyDP(Original_Feature_points_Rotate_All, Original_Feature_points_Rotate, 10, true);*/

                /*for (int i = 0; i < Original_Feature_points_Rotate_All.Size; i++)
                {
                    Rotation_Matrix_X += Original_Feature_points_Rotate_All[i].X;
                    Rotation_Matrix_Y += Original_Feature_points_Rotate_All[i].Y;
                }
                image_CountX1_Rotate_ALL = (int)(Rotation_Matrix_X / Original_Feature_points_Rotate_All.Size);
                image_CountY1_Rotate_ALL = (int)(Rotation_Matrix_Y / Original_Feature_points_Rotate_All.Size);

                Original_image_MOVE_X = image_CountX2 - image_CountX1_Rotate_ALL;
                Original_image_MOVE_Y = image_CountY2 - image_CountY1_Rotate_ALL;*/

                //將比對影像特徵點進行矩陣旋轉
                for (int i = 0; i < Original_Feature_points.Size; i++)
                {
                    Rotation_Matrix[0, i] = (((Original_Feature_points[i].X) * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - ((Original_Feature_points[i].Y) * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)));
                    Rotation_Matrix[1, i] = (((Original_Feature_points[i].Y) * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + ((Original_Feature_points[i].X) * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)));
                    /*double aaa = (Original_Feature_points[i].X - image_CountX1_ALL);
                    double bbb = (Original_Feature_points[i].Y - image_CountY1_ALL);
                    Rotation_Matrix[0, i] = (((aaa) * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - ((bbb) * Math.Sin((angle_pi1_image * (double)Math.PI) / 180))) + image_CountX1_ALL;
                    Rotation_Matrix[1, i] = (((bbb) * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + ((aaa) * Math.Sin((angle_pi1_image * (double)Math.PI) / 180))) + image_CountY1_ALL;*/
                }
                //將比對影像邊緣所有點進行矩陣旋轉
                for (int i = 0; i < Original_Feature_points_All.Size; i++)
                {
                    Rotation_Matrix_All[0, i] = (Original_Feature_points_All[i].X * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - (Original_Feature_points_All[i].Y * Math.Sin((angle_pi1_image * (double)Math.PI) / 180));
                    Rotation_Matrix_All[1, i] = (Original_Feature_points_All[i].Y * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + (Original_Feature_points_All[i].X * Math.Sin((angle_pi1_image * (double)Math.PI) / 180));
                    /*double aaa = (Original_Feature_points_All[i].X - image_CountX1_ALL);
                    double bbb = (Original_Feature_points_All[i].Y - image_CountY1_ALL);
                    Rotation_Matrix_All[0, i] = (aaa * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - (bbb * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountX1_ALL;
                    Rotation_Matrix_All[1, i] = (bbb * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + (aaa * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountY1_ALL;*/
                }
                //取得比對影像旋轉後重心ALL
                for (int i = 0; i < Original_Feature_points_All.Size; i++)
                {
                    Rotation_Matrix_X_All += Rotation_Matrix_All[0, i];
                    Rotation_Matrix_Y_All += Rotation_Matrix_All[1, i];
                }
                image_CountX1_Rotate = (int)(Rotation_Matrix_X_All / Original_Feature_points_All.Size);
                image_CountY1_Rotate = (int)(Rotation_Matrix_Y_All / Original_Feature_points_All.Size);

                //Original_Image = new Image<Bgr, byte>("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model.bmp");
                //Original_Image_Gray = new Image<Gray, byte>(Original_Image.Bitmap);
                //Original_Image = imageBox1.Image
                //儲存未修改前的點位

                Displays_Numbers = "X=" + image_CountX1_Rotate.ToString() + "  Y=" + image_CountY1_Rotate.ToString();
                Center_Of_Gravity = new Point(image_CountX1_Rotate + 10, image_CountY1_Rotate + 10);

                if (Use_SaveImage == true)
                {
                    if (Use_Camera == false)
                    {
                        CvInvoke.PutText(
                               Original_Image,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 255, 0).MCvScalar);

                        for (int k = 0; k < (Rotation_Matrix_All.Length / 2); k++)
                        {
                            //在圖案內才畫圖，超過則不畫
                            if (Rotation_Matrix_All[1, k] < Original_Image.Height && Rotation_Matrix_All[0, k] < Original_Image.Width && Rotation_Matrix_All[1, k] >= 0 && Rotation_Matrix_All[0, k] >= 0)
                            {
                                Original_Image[(int)Rotation_Matrix_All[1, k], (int)Rotation_Matrix_All[0, k]] = new Bgr(0, 255, 0);
                            }
                        }
                        //Original_Image[image_CountY1, image_CountX1] = new Bgr(255, 0, 0);

                        //比對影像儲存
                        Original_Image.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_H2.bmp");
                    }
                    else
                    {
                        CvInvoke.PutText(
                               Original_Image_Gray,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 255, 0).MCvScalar);

                        for (int k = 0; k < (Rotation_Matrix_All.Length / 2); k++)
                        {
                            //在圖案內才畫圖，超過則不畫
                            if (Rotation_Matrix_All[1, k] < Original_Image_Gray.Height && Rotation_Matrix_All[0, k] < Original_Image_Gray.Width && Rotation_Matrix_All[1, k] >= 0 && Rotation_Matrix_All[0, k] >= 0)
                            {
                                Original_Image_Gray[(int)Rotation_Matrix_All[1, k], (int)Rotation_Matrix_All[0, k]] = new Gray(127);
                            }
                        }
                        //Original_Image[image_CountY1, image_CountX1] = new Bgr(255, 0, 0);

                        //比對影像儲存
                        Original_Image_Gray.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_H2.bmp");
                    }
                }

                //計算比對影像需要位移多少
                Original_image_MOVE_X = image_CountX2 - image_CountX1_Rotate;
                Original_image_MOVE_Y = image_CountY2 - image_CountY1_Rotate;

                /*Original_image_MOVE_X = translation.X;
                Original_image_MOVE_Y = translation.Y;*/

                //取得比對影像特徵點位移後的點位
                for (int i = 0; i < Original_Feature_points.Size; i++)
                {
                    Rotation_Matrix_Int[0, i] = (int)(Rotation_Matrix[0, i] + Original_image_MOVE_X);
                    Rotation_Matrix_Int[1, i] = (int)(Rotation_Matrix[1, i] + Original_image_MOVE_Y);
                }

                //取得比對影像邊緣所有點位移後的點位ALL
                for (int i = 0; i < Original_Feature_points_All.Size; i++)
                {
                    Rotation_Matrix_Int_All[0, i] = (int)(Rotation_Matrix_All[0, i] + Original_image_MOVE_X);
                    Rotation_Matrix_Int_All[1, i] = (int)(Rotation_Matrix_All[1, i] + Original_image_MOVE_Y);
                }

                //比對影像特徵點改為Point模式
                StartPoint_Click = new Point[Original_Feature_points.Size];
                for (int i = 0; i < StartPoint_Click.Length; i++)
                {
                    StartPoint_Click[i].X = Rotation_Matrix_Int[0, i];
                    StartPoint_Click[i].Y = Rotation_Matrix_Int[1, i];
                }

                //比對影像像邊緣所有點改為Point模式ALL
                StartPoint_Click_All = new Point[Original_Feature_points_All.Size];
                for (int i = 0; i < StartPoint_Click_All.Length; i++)
                {
                    StartPoint_Click_All[i].X = Rotation_Matrix_Int_All[0, i];
                    StartPoint_Click_All[i].Y = Rotation_Matrix_Int_All[1, i];
                }

                /*for (int i = 0; i < Original_Feature_points_All.Size; i++)
                {
                    Rotation_Matrix_Int[0, i] = (int)(Rotation_Matrix_All[0, i] + Original_image_MOVE_X);
                    Rotation_Matrix_Int[1, i] = (int)(Rotation_Matrix_All[1, i] + Original_image_MOVE_Y);
                }

                StartPoint_Click = new Point[Original_Feature_points.Size];

                for (int i = 0; i < StartPoint_Click.Length; i++)
                {
                    StartPoint_Click[i].X = Rotation_Matrix_Int[0, i];
                    StartPoint_Click[i].Y = Rotation_Matrix_Int[1, i];
                }*/

                //旋轉後轉灰階
                //Take_Image_Gray = new Image<Gray, byte>(Take_Image.Bitmap);
                //Take_Image_Gray_Rotate = new Image<Gray, byte>(Take_Image_Rotate.Bitmap);
                //Original_Image_Gray_Rotate = new Image<Gray, byte>(Original_Image_Rotate.Bitmap);

                //原始影像切二值(旋轉後)
                Take_Image_Binary_Rotate = Take_Image_Gray_Rotate.ThresholdBinary(new Gray(127), new Gray(255));
                //Original_Image_Binary_Rotate = Original_Image_Gray_Rotate.ThresholdBinary(new Gray(127), new Gray(255));

                //原始影像形態學處理(旋轉後)
                Take_Image_Binary_Rotate_Morphology = Take_Image_Binary_Rotate.Erode(10);
                Take_Image_Binary_Rotate_Morphology = Take_Image_Binary_Rotate_Morphology.Dilate(10);
                //原始影像邊緣偵測(旋轉後)
                Take_Image_Binary_Rotate_Morphology = Take_Image_Binary_Rotate_Morphology.Canny(100, 500);

                //Original_Image_Binary_Rotate_Morphology = Original_Image_Binary_Rotate.Erode(10);
                //Original_Image_Binary_Rotate_Morphology = Original_Image_Binary_Rotate_Morphology.Dilate(10);
                //Original_Image_Binary_Rotate_Morphology = Original_Image_Binary_Rotate_Morphology.Canny(100, 500);

                //原始影像找尋邊緣所有點位ALL
                Take_Feature_points_Rotate_All = FindLargestContour(Take_Image_Binary_Rotate_Morphology, result);
                //Original_Feature_points_Rotate_All = FindLargestContour(Original_Image_Binary_Rotate_Morphology, result);

                //原始影像特徵點位
                CvInvoke.ApproxPolyDP(Take_Feature_points_Rotate_All, Take_Feature_points_Rotate, 10, true);
                //CvInvoke.ApproxPolyDP(Original_Feature_points_Rotate_All, Original_Feature_points_Rotate, 10, true);

                //原始影像所有點位重心ALL
                for (int k = 0; k < Take_Feature_points_Rotate_All.Size; k++)
                {
                    image_CountY2_Rotate += Take_Feature_points_Rotate_All[k].Y;
                    image_CountX2_Rotate += Take_Feature_points_Rotate_All[k].X;
                }
                image_CountX2_Rotate = image_CountX2_Rotate / Take_Feature_points_Rotate_All.Size;
                image_CountY2_Rotate = image_CountY2_Rotate / Take_Feature_points_Rotate_All.Size;

                //算出原始影像位移量
                Take_image_MOVE_X = image_CountX1_ALL - image_CountX2_Rotate;
                Take_image_MOVE_Y = image_CountY1_ALL - image_CountY2_Rotate;

                //找重心(原始影像翻轉後)
                /*for (int k = 0; k < Original_Feature_points_Rotate.Size; k++)
                {
                    image_CountY1_Rotate += Original_Feature_points_Rotate[k].Y;
                    image_CountX1_Rotate += Original_Feature_points_Rotate[k].X;
                }
                image_CountX1_Rotate = image_CountX1_Rotate / Original_Feature_points_Rotate.Size;
                image_CountY1_Rotate = image_CountY1_Rotate / Original_Feature_points_Rotate.Size;

                Original_image_MOVE_X = image_CountX2 - image_CountX1_Rotate;
                Original_image_MOVE_Y = image_CountY2 - image_CountY1_Rotate;*/

                //將原始影像移動至比對影像位置
                rotateImage_NEW = do_shift(Take_Image_Gray_Rotate, (int)Take_image_MOVE_X, (int)Take_image_MOVE_Y);
            }

            //rotateImage_NEW_RGB = do_shift_RGB(Original_Image_Rotate, (int)Original_image_MOVE_X, (int)Original_image_MOVE_Y);
            //Original_Image_Gray_Rotate = do_shift(Original_Image_Gray_Rotate, (int)-Original_image_MOVE_X, (int)-Original_image_MOVE_Y);

            /*for (int k = 0; k < Original_Feature_points.Size; k++)
            {
                Take_Image_Gray[StartPoint_Click[k].Y, StartPoint_Click[k].X] = new Gray(0);
            }*/

            /*for (int k = 0; k < Original_Feature_points_Rotate.Size; k++)
            {
                Take_Image[(int)(Original_Feature_points_Rotate[k].Y + Original_image_MOVE_Y), (int)(Original_Feature_points_Rotate[k].X + Original_image_MOVE_X)] = new Bgr(0, 0, 255);
            }
            Take_Image[(int)(image_CountY1_Rotate_ALL + Original_image_MOVE_Y), (int)(image_CountX1_Rotate_ALL + Original_image_MOVE_X)] = new Bgr(255, 0, 0);
            */
            if (Use_SaveImage == true)
            {
                if (Use_Camera == false)
                {
                    for (int k = 0; k < Original_Feature_points_All.Size; k++)
                    {
                        COMMONK_PerformCoordinatesTransform(homog_Global, (double)Original_Feature_points_All[k].X, (double)Original_Feature_points_All[k].Y, Rotation_MatrixX, Rotation_MatrixY);

                        if (Rotation_MatrixY < Take_Image.Height && Rotation_MatrixX < Take_Image.Width && Rotation_MatrixY >= 0 && Rotation_MatrixX >= 0)
                        {
                            Take_Image[(int)Rotation_MatrixY, (int)Rotation_MatrixX] = new Bgr(0, 255, 0);
                        }
                    }

                    if (Center_Gravity_Calculation == true)
                    {
                        //將旋轉後比對影像邊緣所有點畫紅色
                        for (int k = 0; k < Original_Feature_points_All.Size; k++)
                        {
                            //在圖案內才畫圖，超過則不畫
                            if (StartPoint_Click_All[k].Y < Take_Image.Height && StartPoint_Click_All[k].X < Take_Image.Width && StartPoint_Click_All[k].Y >= 0 && StartPoint_Click_All[k].X >= 0)
                            {
                                Take_Image[StartPoint_Click_All[k].Y, StartPoint_Click_All[k].X] = new Bgr(0, 0, 255);
                            }
                        }

                        //將旋轉後比對影像特徵點畫藍色
                        for (int k = 0; k < Original_Feature_points.Size; k++)
                        {
                            //在圖案內才畫圖，超過則不畫
                            if (StartPoint_Click[k].Y < Take_Image.Height && StartPoint_Click[k].X < Take_Image.Width && StartPoint_Click[k].Y >= 0 && StartPoint_Click[k].X >= 0)
                            {
                                Take_Image[StartPoint_Click[k].Y, StartPoint_Click[k].X] = new Bgr(255, 0, 0);
                            }
                        }

                        //將旋轉後比對影像重心畫黃色//在圖案內才畫圖，超過則不畫
                        if (image_CountY1_Rotate + Original_image_MOVE_Y < Take_Image.Height && image_CountX1_Rotate + Original_image_MOVE_X < Take_Image.Width && image_CountX1_Rotate + Original_image_MOVE_X >= 0 && image_CountY1_Rotate + Original_image_MOVE_Y >= 0)
                        {
                            Take_Image[(int)(image_CountY1_Rotate + Original_image_MOVE_Y), (int)(image_CountX1_Rotate + Original_image_MOVE_X)] = new Bgr(0, 255, 255);
                        }

                        //重心寫字
                        /*Displays_Numbers = "X=" + image_CountX2.ToString() + "  Y=" + image_CountY2.ToString();
                        Center_Of_Gravity = new Point(image_CountX2 + 10, image_CountY2 + 10);
                        CvInvoke.PutText(
                               Take_Image,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 255, 0).MCvScalar);


                        Displays_Numbers = "Rotation Angle = " + (-angle_pi1_image).ToString();
                        Center_Of_Gravity = new Point(30, 50);

                        CvInvoke.PutText(
                               Take_Image,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 0, 255).MCvScalar);


                        Displays_Numbers = "Displacement  X = " + Take_image_MOVE_X.ToString() + " , Y =" + Take_image_MOVE_Y.ToString();
                        Center_Of_Gravity = new Point(30, 100);

                        CvInvoke.PutText(
                               Take_Image,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 0, 255).MCvScalar);*/

                        //H算出之結果------
                        Rotation_Matrix_X_All = 0;
                        Rotation_Matrix_Y_All = 0;
                        Rotation_Matrix_All = new double[2, Original_Feature_points_All.Size];//所有點旋轉(需要求旋轉後的重心)
                        for (int i = 0; i < Original_Feature_points_All.Size; i++)
                        {
                            Rotation_Matrix_All[0, i] = (Original_Feature_points_All[i].X * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - (Original_Feature_points_All[i].Y * Math.Sin((angle_pi1_image * (double)Math.PI) / 180));
                            Rotation_Matrix_All[1, i] = (Original_Feature_points_All[i].Y * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + (Original_Feature_points_All[i].X * Math.Sin((angle_pi1_image * (double)Math.PI) / 180));
                            //double aaa = (Original_Feature_points_All[i].X - image_CountX1_ALL);
                            //double bbb = (Original_Feature_points_All[i].Y - image_CountY1_ALL);
                            //Rotation_Matrix_All[0, i] = (aaa * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - (bbb * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountX1_ALL;
                            //Rotation_Matrix_All[1, i] = (bbb * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + (aaa * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountY1_ALL;

                            //Rotation_Matrix_All[0, i] = (Original_Feature_points_All[i].X * Math.Cos((theta * (double)Math.PI) / 180)) - (Original_Feature_points_All[i].Y * Math.Sin((theta * (double)Math.PI) / 180));
                            //Rotation_Matrix_All[1, i] = (Original_Feature_points_All[i].Y * Math.Cos((theta * (double)Math.PI) / 180)) + (Original_Feature_points_All[i].X * Math.Sin((theta * (double)Math.PI) / 180));
                        }

                        //取得比對影像邊緣所有點位移後的點位ALL
                        for (int i = 0; i < Original_Feature_points_All.Size; i++)
                        {
                            Rotation_Matrix_Int_All[0, i] = (int)(Rotation_Matrix_All[0, i] + translation.X);
                            Rotation_Matrix_Int_All[1, i] = (int)(Rotation_Matrix_All[1, i] + translation.Y);
                        }

                        //比對影像像邊緣所有點改為Point模式ALL
                        StartPoint_Click_All = new Point[Original_Feature_points_All.Size];
                        for (int i = 0; i < StartPoint_Click_All.Length; i++)
                        {
                            StartPoint_Click_All[i].X = Rotation_Matrix_Int_All[0, i];
                            StartPoint_Click_All[i].Y = Rotation_Matrix_Int_All[1, i];
                        }

                        //將旋轉後比對影像邊緣所有點畫藍色
                        for (int k = 0; k < Original_Feature_points_All.Size; k++)
                        {
                            //在圖案內才畫圖，超過則不畫
                            if (StartPoint_Click_All[k].Y < Take_Image.Height && StartPoint_Click_All[k].X < Take_Image.Width && StartPoint_Click_All[k].Y >= 0 && StartPoint_Click_All[k].X >= 0)
                            {
                                Take_Image[StartPoint_Click_All[k].Y, StartPoint_Click_All[k].X] = new Bgr(255, 0, 0);
                            }
                        }

                        /*Displays_Numbers = "X=" + translation.X.ToString() + "  Y=" + translation.Y.ToString();
                        Center_Of_Gravity = new Point(10, Take_Image_Gray.Height - 50);
                        CvInvoke.PutText(
                               Take_Image,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 255, 0).MCvScalar);*/
                    //H算出之結果-----
                    }
                    //儲存影像
                    //rotateImage_NEW.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\rotate_NEW.bmp");
                    //Take_Image_Gray.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\rotateImage_Original.bmp");
                    //rotateImage_NEW.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\rotate_NEW.bmp");
                    //Take_Image.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\rotateImage_Original.bmp");
                    Take_Image.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest_H2.bmp");
                }
                else
                {
                    for (int k = 0; k < Original_Feature_points_All.Size; k++)
                    {
                        COMMONK_PerformCoordinatesTransform(homog_Global, (double)Original_Feature_points_All[k].X, (double)Original_Feature_points_All[k].Y, Rotation_MatrixX, Rotation_MatrixY);

                        if (Rotation_MatrixY < Take_Image.Height && Rotation_MatrixX < Take_Image.Width && Rotation_MatrixY >= 0 && Rotation_MatrixX >= 0)
                        {
                            Take_Image_Gray[(int)Rotation_MatrixY, (int)Rotation_MatrixX] = new Gray(127);
                        }
                    }
                    if (Center_Gravity_Calculation == true)
                    {
                        //將旋轉後比對影像邊緣所有點畫紅色
                        for (int k = 0; k < Original_Feature_points_All.Size; k++)
                        {
                            //在圖案內才畫圖，超過則不畫
                            if (StartPoint_Click_All[k].Y < Take_Image_Gray.Height && StartPoint_Click_All[k].X < Take_Image_Gray.Width && StartPoint_Click_All[k].Y >= 0 && StartPoint_Click_All[k].X >= 0)
                            {
                                Take_Image_Gray[StartPoint_Click_All[k].Y, StartPoint_Click_All[k].X] = new Gray(127);
                            }
                        }

                        //將旋轉後比對影像特徵點畫藍色
                        for (int k = 0; k < Original_Feature_points.Size; k++)
                        {
                            //在圖案內才畫圖，超過則不畫
                            if (StartPoint_Click[k].Y < Take_Image_Gray.Height && StartPoint_Click[k].X < Take_Image_Gray.Width && StartPoint_Click[k].Y >= 0 && StartPoint_Click[k].X >= 0)
                            {
                                Take_Image_Gray[StartPoint_Click[k].Y, StartPoint_Click[k].X] = new Gray(255);
                            }
                        }

                        //將旋轉後比對影像重心畫黃色//在圖案內才畫圖，超過則不畫
                        if (image_CountY1_Rotate + Original_image_MOVE_Y < Take_Image_Gray.Height && image_CountX1_Rotate + Original_image_MOVE_X < Take_Image_Gray.Width && image_CountX1_Rotate + Original_image_MOVE_X >= 0 && image_CountY1_Rotate + Original_image_MOVE_Y >= 0)
                        {
                            Take_Image_Gray[(int)(image_CountY1_Rotate + Original_image_MOVE_Y), (int)(image_CountX1_Rotate + Original_image_MOVE_X)] = new Gray(255);
                        }

                        //重心寫字
                        Displays_Numbers = "X=" + image_CountX2.ToString() + "  Y=" + image_CountY2.ToString();
                        Center_Of_Gravity = new Point(image_CountX2 + 10, image_CountY2 + 10);
                        CvInvoke.PutText(
                               Take_Image_Gray,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 255, 0).MCvScalar);

                        Displays_Numbers = "Rotation Angle = " + (-angle_pi1_image).ToString();
                        Center_Of_Gravity = new Point(30, 50);

                        CvInvoke.PutText(
                               Take_Image_Gray,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 0, 255).MCvScalar);

                        Displays_Numbers = "Displacement  X = " + Take_image_MOVE_X.ToString() + " , Y =" + Take_image_MOVE_Y.ToString();
                        Center_Of_Gravity = new Point(30, 100);

                        CvInvoke.PutText(
                               Take_Image_Gray,
                               Displays_Numbers,
                               Center_Of_Gravity,
                               FontFace.HersheyComplex,
                               1.0,
                               new Bgr(0, 0, 255).MCvScalar);
                    }
                    Take_Image_Gray.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest_H2.bmp");
                }
            }

            btn_Move.Enabled = true;

            //原始影像分析完畢
            AnalysisImage = 1;

            /*btnAuto.Enabled = true;
            chang_Image = 0;
            case_1 = 0;*/

            TimeSpan s = DateTime.Now - date1;
            Console.WriteLine("運算時間時間：{0}毫秒", s.TotalMilliseconds.ToString());
            //另存影像旋轉後的檔案
            //順時針旋轉30度(以畫面的中心旋轉)，背景用黑色補滿
            /*Image<Bgr, byte> rotateImage = Original_Image.Rotate(30, new Bgr(System.Drawing.Color.Black));
            rotateImage.Save("rotate.bmp");*/

            //CCD
            /*aaa = new VectorOfPoint();
            cannyImageBox.Visible = true;
            //grayFrame = Original_Image.Convert<Gray, Byte>();
            
            Original_Image_Binary = grayFrame.ThresholdBinary(new Gray(127), new Gray(255));
            Image<Gray, byte> Original_Image2 = Original_Image_Binary.Erode(10);

            Image<Gray, byte> Original_Image3 = Original_Image2.Dilate(10);

            Image<Gray, Byte> cannyFrame1 = Original_Image3.Canny(100, 500);
            cannyFrame2 = cannyFrame1;
            cannyFrame3 = grayFrame;

            //cannyFrame1.
            //CvInvoke.FindContours(cannyFrame1,cannyFrame2,1);
            //cannyFrame1.FindCornerSubPix(new PointF[1][] { corners }, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));  
            //CvInvoke.Threshold
            //findContours(image, contours, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_NONE);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierachy = new Mat();

            IInputOutputArray result = new Mat();

            Original_Image3.Save("c:\\myBitmap.bmp");
            Feature_points = FindLargestContour(cannyFrame2, result);

            for (int k = 0; k < Feature_points.Size; k++)
            {
                //if (Feature_points[k].Y == j && Feature_points[k].X == i)
                {
                    cannyFrame3[Feature_points[k].Y, Feature_points[k].X] = new Gray(0);
                }
            }
            cannyImageBox.Image = cannyFrame3;
            cannyFrame3.Save("c:\\myBitmap2.bmp");*/
        }

        /// <summary>
        /// 比對影像找全部邊緣
        /// </summary>
        /// <param name="cannyEdges"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static VectorOfPoint FindLargestContour(IInputOutputArray cannyEdges, IInputOutputArray result)
        {
            int largest_contour_index = 0;
            double largest_area = 0;
            VectorOfPoint largestContour;

            using (Mat hierachy = new Mat())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                //IOutputArray hirarchy;
                CvInvoke.FindContours(cannyEdges, contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxNone);

                for (int i = 0; i < contours.Size; i++)
                {
                    MCvScalar color = new MCvScalar(0, 0, 255);

                    double a = CvInvoke.ContourArea(contours[i], false);  //  Find the area of contour
                    if (a > largest_area)
                    {
                        largest_area = a;
                        largest_contour_index = i;                //Store the index of largest contour
                    }
                    CvInvoke.DrawContours(result, contours, largest_contour_index, new MCvScalar(255, 0, 0));
                }
                CvInvoke.DrawContours(result, contours, largest_contour_index, new MCvScalar(0, 0, 255), 3, LineType.EightConnected, hierachy);
                largestContour = new VectorOfPoint(contours[largest_contour_index].ToArray());
            }
            return largestContour;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            /*cannyImageBox.Visible = false;
            pictureBox1.Visible = false;*/
        }

        //手臂邊緣移動按鈕
        private void btn_Move_Click(object sender, EventArgs e)
        {
            if (AnalysisImage == 0 || ImageLoad == 0)
            {
                MessageBox.Show("尚未取得比對圖片及旋轉圖片");
                return;
            }

            //如果沒有用手點特徵點 則改成自己找尋特徵點
            if (Original_Feature_points_Click.Size == 0)
            {
                //全部點
                Original_Feature_points_Click = Original_Feature_points_All;
                //Original_Feature_points_Click = Original_Feature_points;
            }

            StartPoint_Click_Point = new Point[Original_Feature_points_Click.Size];
            StartPoint_Click_PointF = new PointF[Original_Feature_points_Click.Size];

            if (Center_Gravity_Calculation == true)
            {
                double[,] Rotation_Matrix = new double[2, Original_Feature_points_Click.Size];
                int[,] Rotation_Matrix_Int = new int[2, Original_Feature_points_Click.Size];
                float[,] Rotation_Matrix_float = new float[2, Original_Feature_points_Click.Size];

                /*double Rotation_Matrix_X = 0, Rotation_Matrix_Y = 0;
                int Rotation_Matrix_Xi = 0, Rotation_Matrix_Yi = 0;
                double Rotation_Matrix_X_Count, Rotation_Matrix_Y_Count;*/

                //Original_Feature_points_Rotation_Matrix = Original_Feature_points_Click;

                //x′ = xcosθ−ysinθ
                //y′ = ycosθ+xsinθ
                //(angle_pi1_image*(double)Math.PI)/180

                for (int i = 0; i < Original_Feature_points_Click.Size; i++)
                {
                    Rotation_Matrix[0, i] = (Original_Feature_points_Click[i].X * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - (Original_Feature_points_Click[i].Y * Math.Sin((angle_pi1_image * (double)Math.PI) / 180));
                    Rotation_Matrix[1, i] = (Original_Feature_points_Click[i].Y * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + (Original_Feature_points_Click[i].X * Math.Sin((angle_pi1_image * (double)Math.PI) / 180));
                    /*double aaa = (Original_Feature_points_Click[i].X - image_CountX1_ALL);
                    double bbb = (Original_Feature_points_Click[i].Y - image_CountY1_ALL);
                    Rotation_Matrix[0, i] = (aaa * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) - (bbb * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountX1_ALL;
                    Rotation_Matrix[1, i] = (bbb * Math.Cos((angle_pi1_image * (double)Math.PI) / 180)) + (aaa * Math.Sin((angle_pi1_image * (double)Math.PI) / 180)) + image_CountY1_ALL;*/
                }

                //Rotation_Matrix[0, 0] = (2 * Math.Cos(-60)) - (-4 * Math.Sin(-60));
                //Rotation_Matrix[1, 0] = (-4* Math.Cos(-60)) + (2 * Math.Sin(-60));
                //1 + Math.Sqrt(3) * 2;

                //驗算角度用
                /*m0 = ((double)(Original_Feature_points[1].Y - Original_Feature_points[0].Y) / (double)(Original_Feature_points[1].X - Original_Feature_points[0].X));
                m1 = ((double)(Rotation_Matrix[1, 1] - Rotation_Matrix[1, 0]) / (double)(Rotation_Matrix[0, 1] - (Rotation_Matrix[0, 0])));

                angle_pi1_m1 = Math.Atan(m1);
                angle_pi1_m1 = (double)(angle_pi1_m1 * 180) / (double)Math.PI;

                double ang1 = Math.Abs((double)(m0 - m1) / (double)(1 + m0 * m1));
                angle_pi1_image = Math.Atan(ang1);
                angle_pi1_image = (double)(angle_pi1_image * 180) / (double)Math.PI;*/

                /*for (int i = 0; i < Original_Feature_points_Click.Size; i++)
                {
                    Rotation_Matrix_X += Rotation_Matrix[0, i];
                    Rotation_Matrix_Y += Rotation_Matrix[1, i];
                }
                Rotation_Matrix_Xi = (int)(Rotation_Matrix_X / Original_Feature_points_Click.Size);
                Rotation_Matrix_Yi = (int)(Rotation_Matrix_Y / Original_Feature_points_Click.Size);*/

                //Rotation_Matrix_X_Count = image_CountX2 - Original_image_MOVE_X;
                //Rotation_Matrix_Y_Count = image_CountY2 - Original_image_MOVE_Y;

                for (int i = 0; i < Original_Feature_points_Click.Size; i++)
                {
                    Rotation_Matrix_Int[0, i] = (int)(Rotation_Matrix[0, i] + Original_image_MOVE_X);
                    Rotation_Matrix_Int[1, i] = (int)(Rotation_Matrix[1, i] + Original_image_MOVE_Y);

                    Rotation_Matrix_float[0, i] = (float)(Rotation_Matrix[0, i] + Original_image_MOVE_X);
                    Rotation_Matrix_float[1, i] = (float)(Rotation_Matrix[1, i] + Original_image_MOVE_Y);

                }

                for (int i = 0; i < Original_Feature_points_Click.Size; i++)
                {
                    StartPoint_Click_Point[i].X = Rotation_Matrix_Int[0, i];
                    StartPoint_Click_Point[i].Y = Rotation_Matrix_Int[1, i];

                    StartPoint_Click_PointF[i].X = Rotation_Matrix_float[0, i];
                    StartPoint_Click_PointF[i].Y = Rotation_Matrix_float[1, i];
                }
            }
            else
            {
                for (int k = 0; k < Original_Feature_points_Click.Size; k++)
                {
                    COMMONK_PerformCoordinatesTransform(homog_Global, (double)Original_Feature_points_Click[k].X, (double)Original_Feature_points_Click[k].Y, Rotation_MatrixX, Rotation_MatrixY);
                    StartPoint_Click_PointF[k].X = (float)Rotation_MatrixX;
                    StartPoint_Click_PointF[k].Y = (float)Rotation_MatrixY;
                }
            }

            string Displays_Numbers = "X=" + Take_Image.ToString() + "  Y=" + Take_Image.ToString();
            Center_Of_Gravity = new Point(image_CountX1_Rotate + 10, image_CountY1_Rotate + 10);

            if (Use_Camera == false)
            {
                //重心寫字
                Displays_Numbers = "X=" + translation.X.ToString() + "  Y=" + translation.Y.ToString();
                Center_Of_Gravity = new Point(30, 50);
                CvInvoke.PutText(
                       Take_Image,
                       Displays_Numbers,
                       Center_Of_Gravity,
                       FontFace.HersheyComplex,
                       1.0,
                       new Bgr(0, 255, 0).MCvScalar);

                Displays_Numbers = "Rotation Angle = " + (-Math.Round(angle_pi1_image, 4)).ToString();
                Center_Of_Gravity = new Point(30, 100);
                CvInvoke.PutText(
                       Take_Image,
                       Displays_Numbers,
                       Center_Of_Gravity,
                       FontFace.HersheyComplex,
                       1.0,
                       new Bgr(0, 0, 255).MCvScalar);

                Displays_Numbers = "Good Match Number: = " + CorrectModelKeyPoints.ToString();
                Center_Of_Gravity = new Point(30, 150);
                CvInvoke.PutText(
                       Take_Image,
                       Displays_Numbers,
                       Center_Of_Gravity,
                       FontFace.HersheyComplex,
                       1.0,
                       new Bgr(255, 0, 0).MCvScalar);
                
                //將旋轉後比對影像特徵點畫紅色
                for (int k = 0; k < Original_Feature_points_Click.Size; k++)
                {
                    if (StartPoint_Click_PointF[k].Y < Take_Image.Height && StartPoint_Click_PointF[k].X < Take_Image.Width && StartPoint_Click_PointF[k].Y >= 0 && StartPoint_Click_PointF[k].X >= 0)
                    {
                        Take_Image[(int)StartPoint_Click_PointF[k].Y, (int)StartPoint_Click_PointF[k].X] = new Bgr(255, 0, 255);
                    }
                }

                Take_Image.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest_FeaturePoints.bmp");
                Console.WriteLine("分析完畢，影像以儲存");

                if (CorrectModelKeyPoints <= 15)
                {
                    if (Form1.InputBox("錯誤資訊", "影像分析有錯誤，是否繼續執行") == DialogResult.OK)
                    {
                        Console.WriteLine("執行手臂移動");
                    }
                    else
                    {
                        return;
                    }
                }
                return;
            }
            else
            {
                //重心寫字
                Displays_Numbers = "X=" + translation.X.ToString() + "  Y=" + translation.Y.ToString();
                Center_Of_Gravity = new Point(30, 100);
                CvInvoke.PutText(
                       Take_Image_Gray,
                       Displays_Numbers,
                       Center_Of_Gravity,
                       FontFace.HersheyComplex,
                       1.0,
                       new Bgr(0, 255, 0).MCvScalar);

                Displays_Numbers = "Rotation Angle = " + (-Math.Round(angle_pi1_image, 4)).ToString();
                Center_Of_Gravity = new Point(30, 50);
                CvInvoke.PutText(
                       Take_Image_Gray,
                       Displays_Numbers,
                       Center_Of_Gravity,
                       FontFace.HersheyComplex,
                       1.0,
                       new Bgr(0, 0, 255).MCvScalar);

                Displays_Numbers = "Good Match Number: = " + CorrectModelKeyPoints.ToString();
                Center_Of_Gravity = new Point(30, 150);
                CvInvoke.PutText(
                       Take_Image_Gray,
                       Displays_Numbers,
                       Center_Of_Gravity,
                       FontFace.HersheyComplex,
                       1.0,
                       new Bgr(255, 0, 0).MCvScalar);

                //將旋轉後比對影像特徵點畫藍色
                for (int k = 0; k < Original_Feature_points_Click.Size; k++)
                {
                    if (StartPoint_Click_PointF[k].Y < Take_Image_Gray.Height && StartPoint_Click_PointF[k].X < Take_Image_Gray.Width && StartPoint_Click_PointF[k].Y >= 0 && StartPoint_Click_PointF[k].X >= 0)
                    {
                        Take_Image_Gray[(int)StartPoint_Click_PointF[k].Y, (int)StartPoint_Click_PointF[k].X] = new Gray(255);
                    }
                }
                Take_Image_Gray.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Dest_FeaturePoints.bmp");
                Console.WriteLine("分析完畢，影像以儲存");

                if (CorrectModelKeyPoints <= 15)
                {
                    if (Form1.InputBox("錯誤資訊", "影像分析有錯誤，是否繼續執行") == DialogResult.OK)
                    {
                        Console.WriteLine("執行手臂移動");
                    }
                    else
                    {
                        return;
                    }
                }
            }

            for (int k = 0; k < Original_Feature_points_Click.Size; k++)
            {
                StartPoint_Click_Point[k].X = (int)(((StartPoint_Click_PointF[k].X / Original_Image_Gray.Height) * (Record_Points_Mechanical_Arm_All_X_Max - Record_Points_Mechanical_Arm_All_X_Min)) + Record_Points_Mechanical_Arm_All_X_Min);
                StartPoint_Click_Point[k].Y = (int)(((StartPoint_Click_PointF[k].Y / Original_Image_Gray.Height) * (Record_Points_Mechanical_Arm_All_Y_Max - Record_Points_Mechanical_Arm_All_Y_Min)) + Record_Points_Mechanical_Arm_All_Y_Min);
            }

            //判斷相同點位即刪除-----------------------
            Point[] StartPoint_Click_Point_NotRepeating = new Point[Original_Feature_points_Click.Size];
            int StartPoint_Click_Point_NotRepeating_X =0, StartPoint_Click_Point_NotRepeating_Y = 0;
            int StartPoint_Click_Point_Count = 0;
            for (int k = 0; k < Original_Feature_points_Click.Size; k++)
            {
                if (StartPoint_Click_Point_NotRepeating_X != StartPoint_Click_Point[k].X || StartPoint_Click_Point_NotRepeating_Y != StartPoint_Click_Point[k].Y)
                {
                    StartPoint_Click_Point_NotRepeating_X = StartPoint_Click_Point[k].X;
                    StartPoint_Click_Point_NotRepeating_Y = StartPoint_Click_Point[k].Y;
                    StartPoint_Click_Point_NotRepeating[StartPoint_Click_Point_Count] = StartPoint_Click_Point[k];
                    StartPoint_Click_Point_Count++;
                }
            }

            StartPoint_Click_Point = new Point[StartPoint_Click_Point_Count];
            for (int k = 0; k < StartPoint_Click_Point_Count; k++)
            {
                StartPoint_Click_Point[k] = StartPoint_Click_Point_NotRepeating[k];
            }
            //判斷相同點位即刪除-----------------------

            Original_Feature_points_Rotation_Matrix = new VectorOfPoint(StartPoint_Click_Point.ToArray());

            //Original_Feature_points_Rotate;
            //Mechanical_Move(Original_Feature_points);

            if (Mechanical_Arm_Connection == false)
            {
                MessageBox.Show("機械手臂尚未連線");
                return;
            }

            if (Camera1 == null)
            {
                MessageBox.Show("相機尚未連線");
                return;
            }

            //馬達針對特徵點位移
            if (Original_Feature_points_Rotation_Matrix.Size != 0)
            {
                Mechanical_Move(Original_Feature_points_Rotation_Matrix);
            }
        }

        /// <summary>
        /// 手臂移動
        /// </summary>
        /// <param name="Original_Feature_points_F">輸入需移動之點位</param>
        private void Mechanical_Move(VectorOfPoint Original_Feature_points_F)
        {
            if (Mechanical_Moving == 0)
            {
                Mechanical_Moving = 1;
                //邊緣找尋

                for (int i = 0; i < Original_Feature_points_F.Size; i++)
                {
                    if (TsRemote._Robot.GetStatus().RunStatus == 0)//判斷手臂無動作時進入
                    {
                        //AnchorMoveAssignPoint_Move(Original_Feature_points_F[i].X - Original_image_MOVE_X, Original_Feature_points_F[i].Y - Original_image_MOVE_Y);
                        //AnchorMoveAssignPoint_Move(Original_Feature_points_F[i].X, Original_Feature_points_F[i].Y);
                        AnchorMoveAssignPointH_Move(Original_Feature_points_F[i].X, Original_Feature_points_F[i].Y);
                    }
                    Console.WriteLine("Feature_points[" + i + "].X = " + Original_Feature_points_F[i].X.ToString());
                    Console.WriteLine("Feature_points[" + i + "].Y = " + Original_Feature_points_F[i].Y.ToString());
                    Console.WriteLine("第" + (i + 1) + "點");
                    //Thread.Sleep(10);
                }
                Console.WriteLine("結束");
            }
            Mechanical_Moving = 0;
        }

        /// <summary>
        /// 影像平移(灰階)
        /// </summary>
        /// <param name="image">輸入影像</param>
        /// <param name="ox">需移動X量</param>
        /// <param name="oy">需移動Y量</param>
        /// <returns></returns>
        private Image<Gray, byte> do_shift(Image<Gray, byte> image, int ox, int oy)
        {
            //double Negative;
            int y, x;
            Image<Gray, byte> inputImage = new Image<Gray, byte>(new Size(image.Width, image.Height));
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    x = j - ox;
                    y = i - oy;

                    if (x < image.Width && y < image.Height && x >= 0 && y >= 0)
                    {
                        inputImage[i, j] = image[y, x];
                    }
                    else
                    {
                        inputImage[i, j] = new Gray(255);
                    }
                    /*Negative = 255 - (image[i, j].Red + image[i, j].Green + image[i, j].Blue) / 3;
                    inputImage[i, j] = new Bgr((int)Negative, (int)Negative, (int)Negative);*/
                }
            }
            return inputImage;
        }

        /// <summary>
        /// 影像平移(彩色)
        /// </summary>
        /// <param name="image">輸入影像</param>
        /// <param name="ox">需移動X量</param>
        /// <param name="oy">需移動Y量</param>
        /// <returns></returns>
        private Image<Bgr, byte> do_shift_RGB(Image<Bgr, byte> image, int ox, int oy)
        {
            //double Negative;
            int y, x;
            Image<Bgr, byte> inputImage = new Image<Bgr, byte>(image.Width, image.Height, new Bgr(255, 255, 255));
            //Image<Bgr, Byte> img2 = new Image<Bgr, Byte>(480, 320, new Bgr(255, 0, 0));
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    x = j - ox;
                    y = i - oy;

                    if (x < image.Width && y < image.Height && x >= 0 && y >= 0)
                    {
                        inputImage[i, j] = image[y, x];
                    }
                    else
                    {
                        inputImage[i, j] = new Bgr(255, 255, 255);
                    }
                    /*Negative = 255 - (image[i, j].Red + image[i, j].Green + image[i, j].Blue) / 3;
                    inputImage[i, j] = new Bgr((int)Negative, (int)Negative, (int)Negative);*/
                }
            }
            return inputImage;
        }

        //取得點位按鈕
        private void btn_Get_Point_Click(object sender, EventArgs e)
        {
            if (btn_Get_Point.Text == "取得點位")
            {
                imageBox_Point.Visible = true;
                Display_Img.Visible = false;

                if (Use_Camera == true)
                {
                    Camera1.GetPointYN(1);
                }
                Thread.Sleep(100);

                Original_Image_Gray_Point = new Image<Gray, byte>("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_H.bmp");

                //imageBox_Point.Size = new Size(408, 408);
                
                imageBox_Point.Image = Original_Image_Gray_Point;

                
                Get_Point_YN = 1;
                btn_Get_Point.Text = "結束取點";
                btn_Get_Point.BackColor = Color.Red;
                Get_Point_YN_Count = 0;
                //Original_Feature_points_All.Clear();

                StartPoint_All = new Point[0];
                Original_Feature_points_Click.Clear();
            }
            else if (btn_Get_Point.Text == "結束取點")
            {
                imageBox_Point.Visible = false;
                Display_Img.Visible = true;

                if (Use_SaveImage == true)
                {
                    Original_Image_Point = new Image<Bgr, byte>("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_H.bmp");
                    //Original_Image_Point = new Image<Bgr, byte>(openFileDialog1.FileName);
                    for (int k = 0; k < Original_Feature_points_Click.Size; k++)
                    {
                        Original_Image_Point[Original_Feature_points_Click[k].Y, Original_Feature_points_Click[k].X] = new Bgr(255, 0, 255);
                    }
                    Original_Image_Point.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_FeaturePoints.bmp");
                }

                if (Use_Camera == true)
                {
                    Camera1.GetPointYN(0);
                }
                Get_Point_YN = 0;
                btn_Get_Point.Text = "取得點位";
                btn_Get_Point.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 尋找兩張影像的H轉換公式
        /// </summary>
        /// <param name="modelImage"></param>
        /// <param name="observedImage"></param>
        /// <param name="matchTime"></param>
        /// <param name="modelKeyPoints"></param>
        /// <param name="observedKeyPoints"></param>
        /// <param name="matches"></param>
        /// <param name="mask"></param>
        /// <param name="homography"></param>
        public static void FindMatch(Mat modelImage, Mat observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.8;
            double hessianThresh = 300;

            Stopwatch watch;
            homography = null;

            modelKeyPoints = new VectorOfKeyPoint();
            observedKeyPoints = new VectorOfKeyPoint();

            using (UMat uModelImage = modelImage.ToUMat(AccessType.Read))
            using (UMat uObservedImage = observedImage.ToUMat(AccessType.Read))
            {
                SURF surfCPU = new SURF(hessianThresh);
                //extract features from the object image
                UMat modelDescriptors = new UMat();
                surfCPU.DetectAndCompute(uModelImage, null, modelKeyPoints, modelDescriptors, false);

                watch = Stopwatch.StartNew();

                // extract features from the observed image
                UMat observedDescriptors = new UMat();
                surfCPU.DetectAndCompute(uObservedImage, null, observedKeyPoints, observedDescriptors, false);
                BFMatcher matcher = new BFMatcher(DistanceType.L2);
                matcher.Add(modelDescriptors);

                matcher.KnnMatch(observedDescriptors, matches, k, null);
                mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                mask.SetTo(new MCvScalar(255));
                Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                int nonZeroCount = CvInvoke.CountNonZero(mask);
                if (nonZeroCount >= 4)
                {
                    nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                       matches, mask, 1.5, 20);
                    if (nonZeroCount >= 4)
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                           observedKeyPoints, matches, mask, 2);

                    //double[,] HomoData = homography;
                }
                watch.Stop();
            }
            matchTime = watch.ElapsedMilliseconds;
        }
        int CorrectModelKeyPoints;
        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        public Mat Draw(Mat modelImage, Mat observedImage, out long matchTime, double angle_image)
        {
            Mat homography;
            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            //double PY1 = 0, PX1 = 0, PY2 = 0, PX2 = 0, PY3 = 0, PX3 = 0, PY4 = 0, PX4 = 0;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, matches,
                   out mask, out homography);

                //Draw the matched keypoints
                Mat result = new Mat();
                Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                   matches, result, new MCvScalar(0, 255, 255), new MCvScalar(0, 255, 255), mask);

                CorrectModelKeyPoints = 0;
                int abcd = 0;
                for (int i = 0; i < matches.Size; i++)
                {
                    abcd = 0;
                    var a = matches[i].ToArray();
                    foreach (var e in a)
                    {
                        if (mask.GetData()[i] == 1)
                        {
                            ;
                            Point p = new Point(e.TrainIdx, e.QueryIdx);
                            //Console.WriteLine(string.Format("Point: {0}", p));
                            CorrectModelKeyPoints++;
                            if (abcd == 0)
                            {
                                if (e.TrainIdx < observedKeyPoints.Size)
                                {
                                    //Console.WriteLine(string.Format("observedKeyPoints: {0}", observedKeyPoints[e.TrainIdx].Point));
                                }
                                
                                abcd++;
                            }
                            else if (abcd == 1)
                            {
                                //Console.WriteLine(string.Format("modelKeyPoints: {0}", modelKeyPoints[e.TrainIdx].Point));
                            }
                        }
                        
                    }        
                    //Console.WriteLine("-----------------------"+i);

                }
                //Console.WriteLine(CorrectModelKeyPoints.ToString());

                /*for (int i = 0; i < modelKeyPoints.Size; i++)
                {
                    if (Math.Abs(modelKeyPoints[i].Point.X - 691.7708) <= 1 && Math.Abs(modelKeyPoints[i].Point.Y - 546.2124) <= 1)
                    {
                        for (int j = 0; j < observedKeyPoints.Size; j++)
                        {

                            if (Math.Abs(observedKeyPoints[j].Point.X - 575.71002) <= 1)
                            {
                                Console.WriteLine(string.Format("modelKeyPoints: {0},observedKeyPoints: {1}", i,j));
                            }
                        }

                    }
                }*/

                /*double dd = 10;
                double aaa, x, y, bbb;
                for (int i = 0; i < modelKeyPoints.Size; i++)
                {
                    //if (Math.Abs(modelKeyPoints[i].Point.X - 691.7708) <= 1 && Math.Abs(modelKeyPoints[i].Point.Y - 546.2124) <= 1)
                    {
                        for (int j = 0; j < observedKeyPoints.Size; j++)
                        {
                            x = modelKeyPoints[i].Point.X;
                            y = modelKeyPoints[i].Point.Y;
                            aaa = ((0.743358388566574 * x + (-0.731549679966268) * y) + 480.431060388048) / ((3.87397077128226E-05 * x) - (-1.25443971675149E-05 * y) + 1);
                            bbb = ((0.74208591423569 * x + (0.715825509335082) * y) - 345.283001519944) / ((3.87397077128226E-05 * x) - (-1.25443971675149E-05 * y) + 1);

                            if (Math.Abs(observedKeyPoints[j].Point.X - aaa) <= dd && Math.Abs(observedKeyPoints[j].Point.Y - bbb) <= dd && mask.GetData()[j] == 1)
                            {
                                //Console.WriteLine(string.Format("modelKeyPoints: {0},observedKeyPoints: {1}", i, j));

                                //Console.WriteLine(string.Format("modelKeyPointsX: {0},modelKeyPointsY: {1}", modelKeyPoints[i].Point.X, modelKeyPoints[i].Point.Y));
                                //Console.WriteLine(string.Format("observedKeyPointsX: {0},observedKeyPointsY: {1}", observedKeyPoints[j].Point.X, observedKeyPoints[j].Point.Y));

                                //Original_Image[(int)modelKeyPoints[i].Point.Y, (int)modelKeyPoints[i].Point.X] = new Bgr(255, 255, 0);
                                //Take_Image[(int)observedKeyPoints[j].Point.Y, (int)observedKeyPoints[j].Point.X] = new Bgr(255, 255, 0);
                                
                            }
                        }
                    }
                }*/

                //Console.WriteLine("modelKeyPoints X = " + modelKeyPoints[0].Point.X.ToString() + ", Y = " + modelKeyPoints[0].Point.Y.ToString());
                //Console.WriteLine("observedKeyPoints X = " + observedKeyPoints[0].Point.X.ToString() + ", Y = " + observedKeyPoints[0].Point.Y.ToString());

                #region draw the projected region on the image

                if (homography != null)
                {
                    //Console.WriteLine("homography找到的角度 = "+ -theta);
                    //angle_pi1_image = -theta;

                    //draw a rectangle along the projected model
                    Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
                    PointF[] pts = new PointF[]
               {
                  new PointF(rect.Left, rect.Bottom),
                  new PointF(rect.Right, rect.Bottom),
                  new PointF(rect.Right, rect.Top),
                  new PointF(rect.Left, rect.Top)
               };
                    homog_Global[0] = BitConverter.ToDouble(homography.GetData(0, 0), 0);
                    homog_Global[1] = BitConverter.ToDouble(homography.GetData(0, 1), 0);
                    homog_Global[2] = BitConverter.ToDouble(homography.GetData(0, 2), 0);
                    homog_Global[3] = BitConverter.ToDouble(homography.GetData(0, 3), 0);
                    homog_Global[4] = BitConverter.ToDouble(homography.GetData(0, 4), 0);
                    homog_Global[5] = BitConverter.ToDouble(homography.GetData(0, 5), 0);
                    homog_Global[6] = BitConverter.ToDouble(homography.GetData(0, 6), 0);
                    homog_Global[7] = BitConverter.ToDouble(homography.GetData(0, 7), 0);
                    homog_Global[8] = BitConverter.ToDouble(homography.GetData(0, 8), 0);

                    getComponents(homography, theta_Image);
                    pts = CvInvoke.PerspectiveTransform(pts, homography);
                    homography_data = true;
                    //可以找旋轉
                    RotatedRect IdentifiedImage = CvInvoke.MinAreaRect(pts);
                    double RotationAngle = IdentifiedImage.Angle;
                    if (pts[1].Y >= pts[0].Y)//&& (Math.Abs(Math.Round(RotationAngle, 0)) < 90 || Math.Abs(Math.Round(RotationAngle, 0)) > 0)
                    {
                        angle_pi1_image = RotationAngle + 90;
                    }
                    else if (pts[1].Y < pts[0].Y)// && (Math.Abs(Math.Round(RotationAngle, 0)) < 90 || Math.Abs(Math.Round(RotationAngle, 0)) > 0)
                    {
                        angle_pi1_image = RotationAngle;
                    }

                    if (Math.Round(RotationAngle, 0) < -89 || Math.Round(RotationAngle, 0) >= 89)
                    {
                        angle_pi1_image = RotationAngle - 180;
                    }
                    else if (Math.Round(RotationAngle, 0) >= -1 && Math.Round(RotationAngle, 0) <= 1)
                    {
                        angle_pi1_image = RotationAngle - 90;
                    }

                    if (RotationAngle == 0)
                    {
                        angle_pi1_image = 0;
                    }
                    else if (RotationAngle == -90)
                    {
                        angle_pi1_image = 0;
                    }

                    if (Math.Abs(angle_pi1_image + theta_Image) >= 5)
                    {
                        angle_pi1_image = -theta_Image;
                        Console.WriteLine("旋轉角度不同");
                    }

                    //Console.WriteLine("MinAreaRect公式找到的角度 = "+ angle_pi1_image.ToString());

                    Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
                    using (VectorOfPoint vp = new VectorOfPoint(points))
                    {
                        CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5);
                    }
                }
                #endregion
                return result;
            }
        }

        double p_Image, q_Image, r_Image;
        Point translation, scale;
        double shear_Image, theta_Image;

        //分析homography(影像)
        void getComponents(Mat normalised_homography, double theta1)
        {
            double a = BitConverter.ToDouble(normalised_homography.GetData(0, 0), 0);
            double b = BitConverter.ToDouble(normalised_homography.GetData(0, 1), 0);
            double c = BitConverter.ToDouble(normalised_homography.GetData(0, 2), 0);
            double d = BitConverter.ToDouble(normalised_homography.GetData(0, 3), 0);
            double e = BitConverter.ToDouble(normalised_homography.GetData(0, 4), 0);
            double f = BitConverter.ToDouble(normalised_homography.GetData(0, 5), 0);
            double g = BitConverter.ToDouble(normalised_homography.GetData(0, 6), 0);
            double h = BitConverter.ToDouble(normalised_homography.GetData(0, 7), 0);
            double i = BitConverter.ToDouble(normalised_homography.GetData(0, 8), 0);

            //儲存參數
            /*string[] lines = { a.ToString(), b.ToString(), c.ToString(), d.ToString(), e.ToString(), f.ToString(), g.ToString(), h.ToString(), i.ToString() };
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Public\Pictures\Sample Pictures\homography_Data.txt"))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    //if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }*/

            p_Image = Math.Sqrt(a * a + b * b);
            r_Image = (a * e - b * d) / (p_Image);
            q_Image = (a * d + b * e) / (a * e - b * d);

            translation.X = (int)c;
            translation.Y = (int)f;
            scale.X = (int)p_Image;
            scale.Y = (int)r_Image;
            shear_Image = q_Image;

            theta1 = (float)(Math.Atan2(b, a) * (180 / Math.PI));
            theta_Image = theta1;
        }

        double p_Point, q_Point, r_Point;
        Point translation_Point, scale_Point;
        double shear_Point, theta_Point;

        //分析homography(點位)
        void getComponents_Point(float[] normalised_homography)
        {
            double a = normalised_homography[0];
            double b = normalised_homography[1];
            double c = normalised_homography[2];
            double d = normalised_homography[3];
            double e = normalised_homography[4];
            double f = normalised_homography[5];
            double g = normalised_homography[6];
            double h = normalised_homography[7];
            double i = normalised_homography[8];

            //儲存參數
            /*string[] lines = { a.ToString(), b.ToString(), c.ToString(), d.ToString(), e.ToString(), f.ToString(), g.ToString(), h.ToString(), i.ToString() };
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Public\Pictures\Sample Pictures\homography_Data.txt"))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    //if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }*/

            p_Point = Math.Sqrt(a * a + b * b);
            r_Point = (a * e - b * d) / (p_Point);
            q_Point = (a * d + b * e) / (a * e - b * d);

            translation_Point.X = (int)c;
            translation_Point.Y = (int)f;
            scale_Point.X = (int)p_Point;
            scale_Point.Y = (int)r_Point;
            shear_Point = q_Point;

            theta_Point = (float)(Math.Atan2(b, a) * (180 / Math.PI));
        }

        //求homography(點位)
        public bool GenerateHomographyMatrix(float[] peSrcX, float[] peSrcY, float[] peDestX, float[] peDestY, uint ucNumPts, double[] pePTMatrix)
        {
            //char n;
            short offset;

            float[] PosA, PosAT, PosH;
            float[] PosATA, PosATAAT, Mtx;

            if (ucNumPts > 20)//限制點數不可太多,  in fact, NumPts=4
                return false;

            PosA = new float[(8 * 2 * ucNumPts * sizeof(float))];

            PosAT = new float[(8 * 2 * ucNumPts * sizeof(float))];
            PosATA = new float[(8 * 8 * sizeof(float))];
            PosATAAT = new float[(8 * 2 * ucNumPts * sizeof(float))];
            PosH = new float[(2 * ucNumPts * sizeof(float))];
            Mtx = new float[(9 * sizeof(float))];

            for (int n = 0; n < ucNumPts; n++)
            {
                offset = (short)(n * 8 * 2);

                PosA[0 + offset] = ((float)(peSrcX[n]));
                PosA[1 + offset] = ((float)(peSrcY[n]));
                PosA[2 + offset] = 1.0f;
                PosA[3 + offset] = 0;
                PosA[4 + offset] = 0;
                PosA[5 + offset] = 0;
                PosA[6 + offset] = -((float)(peDestX[n] * (peSrcX[n])));
                PosA[7 + offset] = -((float)(peDestX[n] * (peSrcY[n])));
                PosA[8 + offset] = 0;
                PosA[9 + offset] = 0;
                PosA[10 + offset] = 0;
                PosA[11 + offset] = ((float)(peSrcX[n]));
                PosA[12 + offset] = ((float)(peSrcY[n]));
                PosA[13 + offset] = 1.0f;
                PosA[14 + offset] = -((float)(peDestY[n] * (peSrcX[n])));
                PosA[15 + offset] = -((float)(peDestY[n] * (peSrcY[n])));
            }
            for (int n = 0; n < ucNumPts; n++)
            {
                PosH[(int)(n << 1)] = ((float)(peDestX[n]));
                PosH[(int)(n << 1) + 1] = ((float)(peDestY[n]));
            }

            matrix.alMATRIX_Transpose(PosA, (int)(2 * ucNumPts), 8, PosAT);                        //Transport matrix
            matrix.alMATRIX_Multiplaction(PosAT, PosA, 8, (int)(2 * ucNumPts), 8, PosATA);       //AT*A=ATA
            matrix.alMATRIX_InverseNbyN(PosATA, 8);                                       //inv(ATA)
            matrix.alMATRIX_Multiplaction(PosATA, PosAT, 8, 8, (int)(2 * ucNumPts), PosATAAT);   //inv(ATA)*AT
            matrix.alMATRIX_Multiplaction(PosATAAT, PosH, 8, (int)(2 * ucNumPts), 1, Mtx);       //0~7 element
            Mtx[8] = 1;
            matrix.alMATRIX_InverseNbyN(Mtx, 3);

            for (int n = 0; n < 9; n++)
                pePTMatrix[n] = Mtx[n];

            /*free(PosA);
            free(PosAT);
            free(PosATA);
            free(PosATAAT);
            free(PosH);
            free(Mtx);*/

            return true;
        }

        //分析homography(點位)(直接分析)
        void COMMONK_PerformCoordinatesTransform(double[] a_peM33, double a_eSrcX, double a_eSrcY, double a_peDestX, double a_peDestY)
        {
            double eS;

            eS = a_peM33[6] * a_eSrcX + a_peM33[7] * a_eSrcY + a_peM33[8] * 1;
            if (eS == 0) eS = 1;
            a_peDestX = (a_peM33[0] * a_eSrcX + a_peM33[1] * a_eSrcY + a_peM33[2] * 1) / eS;
            Rotation_MatrixX = a_peDestX;
            a_peDestY = (a_peM33[3] * a_eSrcX + a_peM33[4] * a_eSrcY + a_peM33[5] * 1) / eS;
            Rotation_MatrixY = a_peDestY;
        }

        //紀錄X和Y的手臂座標
        private void btn_Record_Points_Mechanical_Click(object sender, EventArgs e)
        {
            /*Record_Points_Mechanical_Arm = new Point[Get_Record_Points_Image + 1];//特徵點旋轉加上偏移量的結果

            if (Main_Ini.Record_Points_Mechanical_Arm_All != null)
            {
                for (int i = 0; i < Main_Ini.Record_Points_Mechanical_Arm_All.Length; i++)
                {
                    //for (int j = 0; j < 2; j++)
                    {
                        Record_Points_Mechanical_Arm[i] = Main_Ini.Record_Points_Mechanical_Arm_All[i];
                        //StartPoint_All[i].Y = (StartPoint_All[i].Y * Original_Image_Gray.Width) / Display_Img.Size.Width;
                        //StartPoint_All[i].X = (StartPoint_All[i].X * Original_Image_Gray.Height) / Display_Img.Size.Height;
                    }
                }
            }

            Record_Points_Mechanical_Arm[Get_Record_Points_Image].X = (int)(numericUpDownX.Value);//特徵點旋轉加上偏移量的結果
            Record_Points_Mechanical_Arm[Get_Record_Points_Image].Y = (int)(numericUpDownY.Value);//特徵點旋轉加上偏移量的結果

            Get_Record_Points_Image++;
            Main_Ini.Record_Points_Mechanical_Arm_All = Record_Points_Mechanical_Arm;
            Main_Ini.Record_Points_Mechanical_Arm_All_length = Main_Ini.Record_Points_Mechanical_Arm_All.Length;*/
        }

        //清空X和Y的手臂座標
        private void btn_Re_Record_Click(object sender, EventArgs e)
        {
            /*btn_Record_Points_Mechanical_Arm.Enabled = true;
            Get_Record_Points_Image = 0;
            Main_Ini.Record_Points_Mechanical_Arm_All = new Point[0];*/
        }

        //紀錄影像X和Y的座標(開啟或關閉其功能)
        private void btn_Record_Points_Image_Click(object sender, EventArgs e)
        {
            if (btn_Record_Points_Image.Text == "取得點位(手臂影像校正用)")
            {
                /*if (Use_Camera == false)
                {
                    Original_Image_Gray_Point = new Image<Gray, byte>(Original_Image.Bitmap);
                }
                else
                {
                    Original_Image_Gray_Point = Camera1.GrabImage;
                }*/
                //Display_Img.Image = Original_Image_Gray_Point;
                Get_Point_YN_Image = 1;
                btn_Record_Points_Image.Text = "結束取點(手臂影像校正用)";
                btn_Record_Points_Image.BackColor = Color.Red;
                Get_Point_YN_Image_Count = 0;
                //Original_Feature_points_All.Clear();

                /*if (Use_Camera == true)
                {
                    Camera1.GetPointYN(1);
                }

                StartPoint_All = new Point[0];
                Original_Feature_points_Click.Clear();*/

                //btn_Record_Points_Mechanical_Arm.Enabled = true;
                //Get_Record_Points_Image = 0;
                Main_Ini.Record_Points_Mechanical_Arm_All = new Point[0];

                Get_Point_YN_Image_Count = 0;
                Main_Ini.Record_Points_Image_All = new Point[0];

                label_Number_Points.Text = "紀錄點位數量：" + Get_Point_YN_Image_Count.ToString();

                btn_Compute_Homography.Enabled = false;
                btn_Record_Points_Image.Enabled = false;
                //StartPoint_All.Reverse();
                //StartPoint_All.Clone();
            }
            else if (btn_Record_Points_Image.Text == "結束取點(手臂影像校正用)")
            {
                //Original_Image_Point.Clone();
                /*if (Use_Camera == false)
                {
                    Original_Image_Point = new Image<Bgr, byte>(openFileDialog1.FileName);
                    for (int k = 0; k < Original_Feature_points_Click.Size; k++)
                    {
                        Original_Image_Point[Original_Feature_points_Click[k].Y, Original_Feature_points_Click[k].X] = new Bgr(255, 0, 255);
                    }
                    Original_Image_Point.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_FeaturePoints.bmp");
                }
                else
                {
                    Original_Image_Gray = Camera1.GrabImage;
                    for (int k = 0; k < Original_Feature_points_Click.Size; k++)
                    {
                        Original_Image_Gray[Original_Feature_points_Click[k].Y, Original_Feature_points_Click[k].X] = new Gray(255);
                    }
                    Original_Image_Gray.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\新增資料夾\\Model_FeaturePoints.bmp");

                }
                //Original_Image_Point = Original_Image;
                if (Use_Camera == true)
                {
                    Camera1.GetPointYN(0);
                }*/
                Get_Point_YN_Image = 0;
                btn_Record_Points_Image.Text = "取得點位(手臂影像校正用)";
                btn_Record_Points_Image.BackColor = Color.Transparent;

                Main_Ini.Record_Points_Image_All_length = Main_Ini.Record_Points_Image_All.Length;

                btn_Compute_Homography.Enabled = true;
                //StartPoint_All;
            }
        }

        //儲存校正點位按鈕
        private void btn_Compute_Homography_Click(object sender, EventArgs e)
        {
            /*float[] peDestX1 = { 25, 75, 0, 100 };
            float[] peDestY1 = { 50, 50, 100, 100 };*/

            /*Record_Points_Mechanical_ArmF_All = new PointF[4];
            Record_Points_Mechanical_ArmF_All[0] = new PointF(0, 0);
            Record_Points_Mechanical_ArmF_All[1] = new PointF(100, 0);
            Record_Points_Mechanical_ArmF_All[2] = new PointF(0, 100);
            Record_Points_Mechanical_ArmF_All[3] = new PointF(100, 100);

            Record_Points_ImageF_All = new PointF[4];
            Record_Points_ImageF_All[0] = new PointF(25, 50);
            Record_Points_ImageF_All[1] = new PointF(75, 50);
            Record_Points_ImageF_All[2] = new PointF(0, 100);
            Record_Points_ImageF_All[3] = new PointF(100, 100);*/

            /*Record_Points_Mechanical_Arm_All_VOP = new VectorOfPoint(Record_Points_Mechanical_Arm_All.ToArray());
            Record_Points_Image_All_VOP = new VectorOfPoint(Record_Points_Image_All.ToArray());*/

            //Record_Points_Image_All_VOP = new VectorOfPoint(pePTMatrix1.ToArray());
            /*HomographyMethod method = HomographyMethod.Default;
            IOutputArray mask = null;
            IOutputArray Homography_Point = null;*/

            //將點位紀錄起來
            Main_Ini.Write_ini_Cfg();

            //將新的資訊更改至手臂
            for (int i = 0; i < Main_Ini.Record_Points_Mechanical_Arm_All_length; i++)
            {
                peSrcX1[i] = Main_Ini.Record_Points_Mechanical_Arm_All[i].X;
                peSrcY1[i] = Main_Ini.Record_Points_Mechanical_Arm_All[i].Y;
            }

            for (int i = 0; i < Main_Ini.Record_Points_Image_All_length; i++)
            {
                peDestX1[i] = Main_Ini.Record_Points_Image_All[i].X;
                peDestY1[i] = Main_Ini.Record_Points_Image_All[i].Y;
            }
            GenerateHomographyMatrix(peSrcX1, peSrcY1, peDestX1, peDestY1, ucNumPts1, pePTMatrix1);

            //目前有問題
            //CvInvoke.FindHomography(Record_Points_Mechanical_ArmF_All, Record_Points_ImageF_All, Homography_Point, method, 3, mask);
        }

        private void btn_Click_Move_Click(object sender, EventArgs e)
        {
            if (btn_Click_Move.Text == "點選移動(開啟)")
            {
                Mechanical_Arm_YN = 1;
                btn_Click_Move.Text = "點選移動(關閉)";
                btn_Click_Move.BackColor = Color.Red;
            }
            else if (btn_Click_Move.Text == "點選移動(關閉)")
            {
                Mechanical_Arm_YN = 0;
                btn_Click_Move.Text = "點選移動(開啟)";
                btn_Click_Move.BackColor = Color.Transparent;
            }
        }

        //影像比對如果特徵點數過少，彈出警告式窗
        public static DialogResult InputBox(string title, string promptText)
        {
            Form form = new Form();
            Label label = new Label();
            //TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            //value = "";
            form.Text = title;
            label.Text = promptText;
            //textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            //textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(20, 72, 75, 23);
            buttonCancel.SetBounds(101, 72, 75, 23);

            label.AutoSize = true;
            //textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(200, 107);
            form.Controls.AddRange(new Control[] { label, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(200, label.Right), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            //value = textBox.Text;
            return dialogResult;
        }
    
        /*public static Image<TColor, TDepth> GetAxisAlignedImagePart<TColor, TDepth>(
    this Image<TColor, TDepth> input,
    Quadrilateral rectSrc,
    Quadrilateral rectDst,
    Size targetImageSize)
            where TColor : struct, IColor
            where TDepth : new()
        {
            var src = new[] { rectSrc.P0, rectSrc.P1, rectSrc.P2, rectSrc.P3 };
            var dst = new[] { rectDst.P0, rectDst.P1, rectDst.P2, rectDst.P3 };

            using (var matrix = CvInvoke.GetPerspectiveTransform(src, dst))
            {
                using (var cutImagePortion = new Mat())
                {
                    CvInvoke.WarpPerspective(input, cutImagePortion, matrix, targetImageSize, Inter.Cubic);
                    return cutImagePortion.ToImage<TColor, TDepth>();
                }
            }
        }*/
    }
}

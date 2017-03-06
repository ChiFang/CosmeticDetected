using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.UI;
using System.Diagnostics;

using NeptuneClassLibWrap;
using CosmeticDetected;

namespace CosmeticImiTechCamera
{
    public class ImiTechCamera
    {
        /** \brief 視窗物件 */
        public Form MainForm = new Form();

        /** \brief 執行攝影機取像 */
        public Thread m_Thread = null;
        //internal Thread m_Thread = null;

        /** \brief 搜尋攝影機 */
        public NeptuneClassLibCLR Neptune1 = null;
        //internal NeptuneClassLibCLR Neptune1 = null;

        /** \brief 存放取得的攝影機名稱和資訊的結構 */
        NEPTUNE_CAM_INFO[] m_CamInfo = null;

        /** \brief 控制攝影機 */
        public CameraInstance Cam1 = null;
        //internal CameraInstance Cam1 = null;

        /** \brief 是否連續取像 */
        public bool m_bPlay = false;
        //internal bool m_bPlay = false;

        /** \brief 紀錄Frame數量 */
        private uint framecount = 0;

        private int Get_Point_YN = 0;

        /** \brief 計算FPS */
        System.Timers.Timer FPS_count = new System.Timers.Timer(1000);

        /** \brief 取像後存放的影像變數 */
        public Image<Gray, Byte> GrabImage = null;
        //internal Image<Gray, Byte> GrabImage = null;

        /** \brief 彩色影像變數 */
        public Image<Bgr, Byte> ColorGrabImage = null;
        //internal Image<Bgr, Byte> ColorGrabImage = null;

        /** \brief 計算時間 */
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        /** \brief 委派顯示灰階影像與時間 */
        delegate void ShowGrayImg(Image<Gray, Byte> Gray, string Time);

        /** \brief 委派顯示彩色影像與時間 */
        delegate void ShowRGBImg(Image<Bgr, Byte> RGBImg, string Time);

        /** \brief 委派顯示FPS */
        delegate void ShowFPS(uint frame);

        /** \暫停取像，等待其他程序 */
        internal EventWaitHandle WaitProcess = new EventWaitHandle(false, EventResetMode.AutoReset);

        /** \是否進行影像對位 */
        internal bool IsDetect = false;

        /** \輸出縮小後的影像 */
        internal Image<Gray, Byte> SaveImg = null;

        /** \brief 顯示攝影機擷取之影像的ImageBox */
        internal ImageBox Display;

        /** \brief 進行影像對位按鈕RadioButton */
        internal RadioButton Registration;

        /** \brief 顯示處理時間的TextBox */
        internal TextBox TimeText;

        /** \brief 顯示FPS的TextBox */
        internal TextBox FPSText;

        /** \brief 選擇開啟攝影機的ComboBox */
        internal ComboBox CameraSelect;

        /** \brief ROI範圍 */
        internal static Rectangle RoiRegion = new Rectangle(74, 85, 1270, 833);

        /** \brief 顯示標靶位置 */
        internal Label ShowAnchorPosition;

        /** \brief 選擇定位點時，暫停取像 */
        internal bool IsSelectRegion = false;

        /** \brief 是否有選定標靶 */
        internal bool IsSelectAnchor = false;

        /** \brief 偵測定位標靶 */
        internal AnchorDectector AnchorDetect = null;

        /** \brief 標靶位置 */
        internal PointF AnchorPosition = new PointF();

        /** \brief 是否有偵測到標靶 */
        internal bool IsAnchorDetected = false;

        /** \brief 在影像中心點繪製十字的設定 */
        Cross2DF CentralCross = new Cross2DF();

        /** \brief 繪製標靶十字設定 */
        Cross2DF DrawCross = new Cross2DF();

        /** \brief 選擇定位點時，是否有載入到影像*/
        internal bool m_img_loaded = false;

        /** \brief 偵測物件*/
        internal ObjectDetector ObDetect = null;

        /** \brief 是否使用指定位置位移*/
        internal CheckBox MoveAssignPosition;

        /** \brief 指定位置X*/
        internal NumericUpDown NumericUpDownX;

        /** \brief 指定位置Y*/
        internal NumericUpDown NumericUpDownY;

        /// <summary>
        /// 建構式1
        /// </summary>
        public ImiTechCamera()
        { 
        }

        /// <summary>
        /// 建構式2
        /// </summary>
        /// <param name="CameraDisplay">[IN] 顯示攝影機擷取之影像的ImageBox</param>
        /// <param name="DisplayMode">[IN] 進行影像對位按鈕RadioButton</param>
        /// <param name="Time">[IN] 顯示處理時間的TextBox</param>
        /// <param name="FPS">[IN] 顯示FPS的TextBox</param>
        /// <param name="ComboBoxCamera">[IN] 選擇開啟攝影機的ComboBox</param>
        public ImiTechCamera(ImageBox CameraDisplay, RadioButton DisplayMode, TextBox Time, TextBox FPS, ComboBox ComboBoxCamera)
        {
            Display = CameraDisplay;
            Registration = DisplayMode;
            TimeText = Time;
            FPSText = FPS;
            CameraSelect = ComboBoxCamera;
        }

        /// <summary>
        /// 搜尋攝影機
        /// </summary>
        public void SearchCamera()
        {
            //Camera Set
            InitCtrl();
            Neptune1 = new NeptuneClassLibCLR();
            Neptune1.InitLibrary();
            UpdateCameraList();
        }

        /// <summary>
        /// 初始設定
        /// </summary>
        private void InitCtrl()
        {
            CameraSelect.SelectedIndex = -1;
            FPSText.Text = "";
        }

        /// <summary>
        /// 在ComboBoxg上顯示可選擇的攝影機
        /// </summary>
        private void UpdateCameraList()
        {
            uint nCam = DeviceManager.Instance.GetTotalCamera();
            m_CamInfo = new NEPTUNE_CAM_INFO[nCam];
            DeviceManager.Instance.GetCameraList(m_CamInfo, nCam);

            CameraSelect.SelectedIndex = -1;
            CameraSelect.Items.Clear();
            for (int i = 0; i < nCam; i++)
                CameraSelect.Items.Add(m_CamInfo[i].strVendor + " : " + m_CamInfo[i].strModel + " S/N: " + m_CamInfo[i].strSerial);

            CameraSelect.Enabled = true;
        }

        /// <summary>
        /// 攝影機進行取像
        /// </summary>
        public void play()
        {
            NEPTUNE_IMAGE_SIZE ImgSize = new NEPTUNE_IMAGE_SIZE();
            Cam1.GetImageSize(ref ImgSize);

            //設定快門時間
            //NEPTUNE_FEATURE featureShutter = new NEPTUNE_FEATURE();
            //featureShutter.Value = 747;
            //Cam1.SetFeature(ENeptuneFeature.NEPTUNE_FEATURE_SHUTTER, featureShutter);

            if (Cam1.AcquisitionStart(ENeptuneGrabMode.NEPTUNE_GRAB_CONTINUOUS) != ENeptuneError.NEPTUNE_ERR_Success)
            {
                MessageBox.Show("Acquisition start error!");
                return;
            }

            if (m_Thread == null)
                m_Thread = new Thread(AcquisitionThread);
            m_bPlay = true;
            m_Thread.Start();
            CameraSelect.Enabled = false;
            FPS_count.Elapsed += FPS_count_Tick;
            FPS_count.AutoReset = true;
            FPS_count.Enabled = true;

            m_img_loaded = true;
        }

        /// <summary>
        /// 攝影機停止取像
        /// </summary>
        public void stop()
        {
            if (m_bPlay)
            {
                m_bPlay = false;
                Cam1.AcquisitionStop();
                m_Thread.Abort();
                m_Thread = null;

                if (Cam1.GetCameraType() == ENeptuneDevType.NEPTUNE_DEV_TYPE_GIGE || Cam1.GetCameraType() == ENeptuneDevType.NEPTUNE_DEV_TYPE_USB3)
                {
                    FPSText.Enabled = true;
                }
                CameraSelect.Enabled = true;
            }
            m_img_loaded = false;
        }
        
        public void GetPointYN(int aa)
        {
            Get_Point_YN = aa;
        }

        /// <summary>
        /// 取像Thread執行的函式
        /// </summary>
        private void AcquisitionThread()
        {
            string strPixelFormat = "Mono8";
            bool bYUV = strPixelFormat.Contains("YUV") ? true : false;
            FrameDataPtr data = new FrameDataPtr();
            //data = new Image<Gray, byte>();

            while (m_bPlay)
            {
                if (Get_Point_YN != 1)
                {
                    ENeptuneError eErr = Cam1.WaitEventDataStream(ref data, 1000);
                    if (eErr != ENeptuneError.NEPTUNE_ERR_Success)
                        continue;

                    sw.Reset();//碼表歸零
                    sw.Start();//碼表開始計時  
                    GrabImage = new Image<Gray, byte>((Int32)data.GetWidth(), (Int32)data.GetHeight(), (Int32)data.GetWidth(), data.GetBufferPtr());

                    Cam1.QueueBufferDataStream(data.GetBufferIndex());
                    framecount++;
                    GrabImage.ROI = RoiRegion;

                    if (Registration.Checked || IsDetect)
                    {
                        ObDetect = new ObjectDetector(GrabImage);
                        ObDetect.ObjectDetected();
                        ColorGrabImage = ObDetect.ColorImg;
                        IsDetect = false;
                        sw.Stop();
                        MainForm.BeginInvoke(new ShowRGBImg(UpdateImageUI), ColorGrabImage, sw.Elapsed.TotalMilliseconds.ToString());
                        WaitProcess.WaitOne();
                    }
                    else if (IsSelectRegion)
                    {
                        ColorGrabImage = GrabImage.Convert<Bgr, Byte>();
                        WaitProcess.WaitOne();
                        sw.Stop();
                    }
                    else
                    {
                        ColorGrabImage = GrabImage.Convert<Bgr, Byte>();
                        if (IsSelectAnchor)
                            IsAnchorDetected = AnchorDetect.DetectAnchorPoint(GrabImage, out AnchorPosition);
                        if (IsAnchorDetected)
                        {
                            CentralCross = new Cross2DF(new PointF(RoiRegion.Width / 2, RoiRegion.Height / 2), 20, 20);
                            ColorGrabImage.Draw(CentralCross, new Bgr(200, 150, 50), 2);
                            DrawCross = new Cross2DF(AnchorPosition, 20, 20);
                            ColorGrabImage.Draw(DrawCross, new Bgr(0, 0, 255), 2);
                            if (MoveAssignPosition.Checked)
                            {
                                DrawCross = new Cross2DF(new PointF(Convert.ToSingle(NumericUpDownX.Value), Convert.ToSingle(NumericUpDownY.Value)), 20, 20);
                                ColorGrabImage.Draw(DrawCross, new Bgr(140, 159, 22), 2);
                            }
                        }
                        sw.Stop();
                        {
                            MainForm.BeginInvoke(new ShowRGBImg(UpdateImageUI), ColorGrabImage, sw.Elapsed.TotalMilliseconds.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 顯示攝影機影像(RGB)和處理時間
        /// </summary>
        /// <param name="Input">[IN] 輸入欲顯示影像</param>
        /// <param name="ReadTime">[IN] 欲顯示的處理時間</param>
        private void UpdateImageUI(Image<Bgr, Byte> Input, string ReadTime)
        {
            if (IsSelectAnchor && IsAnchorDetected)
                ShowAnchorPosition.Text = "標靶位置：" + AnchorPosition.ToString();
            else
                ShowAnchorPosition.Text = "標靶位置：";
            TimeText.Text = ReadTime;
            Display.Image = Input;
        }

        /// <summary>
        /// 顯示攝影機影像(Gray)和處理時間
        /// </summary>
        /// <param name="Input">[IN] 輸入欲顯示影像</param>
        /// <param name="ReadTime">[IN] 欲顯示的處理時間</param>
        private void ShowImage(Image<Gray, Byte> Input, string ReadTime)
        {
            TimeText.Text = ReadTime;
            Display.Image = Input;
        }

        /// <summary>
        /// 選擇並開啟攝影機
        /// </summary>
        public void CameraSelection()
        {
            //要換開其他攝影機時
            if (Cam1 != null)
            {
                Cam1.CameraClose();
                Cam1 = null;
            }
            //取得串列資料
            NeptuneDevice iDev = DeviceManager.Instance.GetDeviceFromSerial(m_CamInfo[CameraSelect.SelectedIndex].strSerial);
            //重新建立Cam1
            try
            {
                Cam1 = new CameraInstance(iDev, ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE);

                //Open Camera
                play();
            }
            catch (System.Exception exp)
            {
                string strExp = "Can not select camera! - " + exp.Message;
                MessageBox.Show(strExp);
                InitCtrl();
                return;
            }
        }


        //int Canstop = 0;
        /// <summary>
        /// Timer觸發功能
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void FPS_count_Tick(Object source, ElapsedEventArgs e)
        {
            MainForm.BeginInvoke(new ShowFPS(ShowFPSText), framecount);
            framecount = 0;
        }

        /// <summary>
        /// 顯示FPS
        /// </summary>
        /// <param name="frame">[IN] FPS</param>
        private void ShowFPSText(uint frame)
        {
            FPSText.Text = frame.ToString();
        }

        /// <summary>
        /// 回傳標靶位置
        /// </summary>
        /// <param name="Anchor">[OUT] 輸出標靶位置</param>
        /// <returns>目前是否有偵測到標靶</returns>
        internal bool GetAnchorPosition(out PointF Anchor)
        {
            if (IsAnchorDetected)
            {
                Anchor = AnchorPosition;
                return true;
            }
            else
            {
                Anchor = new PointF(620, 414);
                return false;
            }
        }


    }
}

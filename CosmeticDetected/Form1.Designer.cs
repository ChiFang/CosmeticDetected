namespace CosmeticDetected
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Display_Img = new Emgu.CV.UI.ImageBox();
            this.Process = new System.Windows.Forms.Button();
            this.Search_Camera = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Camera_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.framerate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Proc_time = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ImageMatch = new System.Windows.Forms.RadioButton();
            this.Instant_mode = new System.Windows.Forms.RadioButton();
            this.groupBox_Toshiba = new System.Windows.Forms.GroupBox();
            this.label_Override = new System.Windows.Forms.Label();
            this.label_ToshibaPort = new System.Windows.Forms.Label();
            this.label_ToshibaIP = new System.Windows.Forms.Label();
            this.btnResetAlarm = new System.Windows.Forms.Button();
            this.btnOverride = new System.Windows.Forms.Button();
            this.numericUpDown_Override = new System.Windows.Forms.NumericUpDown();
            this.btnStopControl = new System.Windows.Forms.Button();
            this.textBox_ToshibaIPAddr = new System.Windows.Forms.TextBox();
            this.btnStartControl = new System.Windows.Forms.Button();
            this.textBox_ToshibaPort = new System.Windows.Forms.TextBox();
            this.groupBox_MovePos = new System.Windows.Forms.GroupBox();
            this.label_Number_Points = new System.Windows.Forms.Label();
            this.btn_Compute_Homography = new System.Windows.Forms.Button();
            this.btn_Re_Record = new System.Windows.Forms.Button();
            this.btn_Record_Points_Mechanical_Arm = new System.Windows.Forms.Button();
            this.btn_Record_Points_Image = new System.Windows.Forms.Button();
            this.btnStopMove = new System.Windows.Forms.Button();
            this.labWarning_Signs = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.labelY = new System.Windows.Forms.Label();
            this.numericUpDownZ = new System.Windows.Forms.NumericUpDown();
            this.labelZ = new System.Windows.Forms.Label();
            this.btnStartMove = new System.Windows.Forms.Button();
            this.numericUpDownT = new System.Windows.Forms.NumericUpDown();
            this.labelC = new System.Windows.Forms.Label();
            this.labelT = new System.Windows.Forms.Label();
            this.numericUpDownC = new System.Windows.Forms.NumericUpDown();
            this._StatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_Content = new System.Windows.Forms.ToolStripStatusLabel();
            this.ROI_Img = new Emgu.CV.UI.ImageBox();
            this.Select_Anchor = new System.Windows.Forms.Button();
            this.ConvtCoodinate = new System.Windows.Forms.Button();
            this.Position = new System.Windows.Forms.Button();
            this.CalAxisPoint = new System.Windows.Forms.Button();
            this.MoveToImgCenter = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label10 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.AnchorMoveAssignPoint = new System.Windows.Forms.Button();
            this.ListviewP = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.ArmXMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ArmXRealMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ArmYMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ArmYRealMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImgXMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImgXRealMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImgYMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImgYRealMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Angle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button8 = new System.Windows.Forms.Button();
            this.Export_Excel = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.RemoveExl = new System.Windows.Forms.Button();
            this.LocalRecordToExl = new System.Windows.Forms.Button();
            this.listView3 = new System.Windows.Forms.ListView();
            this.Num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LocalX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LocalY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button5 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.listView4 = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.SetParam = new System.Windows.Forms.Button();
            this.PositionLocal = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.DownLocal = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.UpLocal = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.RevertToOrigin = new System.Windows.Forms.Button();
            this.MoveToPoint = new System.Windows.Forms.Button();
            this.CalMoveDistance = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.LocateHeight = new System.Windows.Forms.Button();
            this.ZAxisDown = new System.Windows.Forms.Button();
            this.ZAxisUp = new System.Windows.Forms.Button();
            this.AssignPositionMove = new System.Windows.Forms.CheckBox();
            this.ObDetected = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_ImageLoad = new System.Windows.Forms.Button();
            this.btn_AnalysisImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cannyImageBox = new Emgu.CV.UI.ImageBox();
            this.btn_Close = new System.Windows.Forms.Button();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.btn_Move = new System.Windows.Forms.Button();
            this.btn_Get_Point = new System.Windows.Forms.Button();
            this.btn_Click_Move = new System.Windows.Forms.Button();
            this.imageBox_Point = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.Display_Img)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox_Toshiba.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Override)).BeginInit();
            this.groupBox_MovePos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownC)).BeginInit();
            this._StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROI_Img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox2)).BeginInit();
            this.ListviewP.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Point)).BeginInit();
            this.SuspendLayout();
            // 
            // Display_Img
            // 
            this.Display_Img.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Display_Img.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.Display_Img.Location = new System.Drawing.Point(246, 68);
            this.Display_Img.Name = "Display_Img";
            this.Display_Img.Size = new System.Drawing.Size(559, 408);
            this.Display_Img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Display_Img.TabIndex = 2;
            this.Display_Img.TabStop = false;
            this.Display_Img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseDown);
            this.Display_Img.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseMove);
            this.Display_Img.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Display_Img_MouseUp);
            // 
            // Process
            // 
            this.Process.Location = new System.Drawing.Point(12, 553);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(105, 42);
            this.Process.TabIndex = 3;
            this.Process.Text = "執行跑點";
            this.Process.UseVisualStyleBackColor = true;
            this.Process.Click += new System.EventHandler(this.Process_Click);
            // 
            // Search_Camera
            // 
            this.Search_Camera.Location = new System.Drawing.Point(656, 4);
            this.Search_Camera.Name = "Search_Camera";
            this.Search_Camera.Size = new System.Drawing.Size(75, 23);
            this.Search_Camera.TabIndex = 215;
            this.Search_Camera.Text = "搜尋攝影機";
            this.Search_Camera.UseVisualStyleBackColor = true;
            this.Search_Camera.Click += new System.EventHandler(this.Search_Camera_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(254, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 209;
            this.label1.Text = "Camera：";
            // 
            // Camera_comboBox
            // 
            this.Camera_comboBox.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Camera_comboBox.FormattingEnabled = true;
            this.Camera_comboBox.Location = new System.Drawing.Point(317, 6);
            this.Camera_comboBox.Name = "Camera_comboBox";
            this.Camera_comboBox.Size = new System.Drawing.Size(256, 21);
            this.Camera_comboBox.TabIndex = 208;
            this.Camera_comboBox.SelectionChangeCommitted += new System.EventHandler(this.Camera_comboBox_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(252, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 210;
            this.label2.Text = "FPS：";
            // 
            // framerate
            // 
            this.framerate.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.framerate.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.framerate.Location = new System.Drawing.Point(299, 39);
            this.framerate.Name = "framerate";
            this.framerate.ReadOnly = true;
            this.framerate.Size = new System.Drawing.Size(100, 23);
            this.framerate.TabIndex = 211;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(434, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 212;
            this.label9.Text = "Process time：";
            // 
            // Proc_time
            // 
            this.Proc_time.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Proc_time.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Proc_time.Location = new System.Drawing.Point(521, 39);
            this.Proc_time.Name = "Proc_time";
            this.Proc_time.Size = new System.Drawing.Size(100, 23);
            this.Proc_time.TabIndex = 213;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(627, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 13);
            this.label8.TabIndex = 214;
            this.label8.Text = "ms";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ImageMatch);
            this.groupBox1.Controls.Add(this.Instant_mode);
            this.groupBox1.Location = new System.Drawing.Point(255, 482);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 77);
            this.groupBox1.TabIndex = 216;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode";
            // 
            // ImageMatch
            // 
            this.ImageMatch.AutoSize = true;
            this.ImageMatch.Location = new System.Drawing.Point(6, 43);
            this.ImageMatch.Name = "ImageMatch";
            this.ImageMatch.Size = new System.Drawing.Size(71, 16);
            this.ImageMatch.TabIndex = 196;
            this.ImageMatch.Text = "物件偵測";
            this.ImageMatch.UseVisualStyleBackColor = true;
            // 
            // Instant_mode
            // 
            this.Instant_mode.AutoSize = true;
            this.Instant_mode.Checked = true;
            this.Instant_mode.Location = new System.Drawing.Point(6, 21);
            this.Instant_mode.Name = "Instant_mode";
            this.Instant_mode.Size = new System.Drawing.Size(71, 16);
            this.Instant_mode.TabIndex = 193;
            this.Instant_mode.TabStop = true;
            this.Instant_mode.Text = "即時影像";
            this.Instant_mode.UseVisualStyleBackColor = true;
            // 
            // groupBox_Toshiba
            // 
            this.groupBox_Toshiba.Controls.Add(this.label_Override);
            this.groupBox_Toshiba.Controls.Add(this.label_ToshibaPort);
            this.groupBox_Toshiba.Controls.Add(this.label_ToshibaIP);
            this.groupBox_Toshiba.Controls.Add(this.btnResetAlarm);
            this.groupBox_Toshiba.Controls.Add(this.btnOverride);
            this.groupBox_Toshiba.Controls.Add(this.numericUpDown_Override);
            this.groupBox_Toshiba.Controls.Add(this.btnStopControl);
            this.groupBox_Toshiba.Controls.Add(this.textBox_ToshibaIPAddr);
            this.groupBox_Toshiba.Controls.Add(this.btnStartControl);
            this.groupBox_Toshiba.Controls.Add(this.textBox_ToshibaPort);
            this.groupBox_Toshiba.Location = new System.Drawing.Point(12, 15);
            this.groupBox_Toshiba.Name = "groupBox_Toshiba";
            this.groupBox_Toshiba.Size = new System.Drawing.Size(212, 256);
            this.groupBox_Toshiba.TabIndex = 217;
            this.groupBox_Toshiba.TabStop = false;
            this.groupBox_Toshiba.Text = "Toshiba";
            // 
            // label_Override
            // 
            this.label_Override.AutoSize = true;
            this.label_Override.Location = new System.Drawing.Point(16, 172);
            this.label_Override.Name = "label_Override";
            this.label_Override.Size = new System.Drawing.Size(53, 12);
            this.label_Override.TabIndex = 93;
            this.label_Override.Text = "速度上限";
            // 
            // label_ToshibaPort
            // 
            this.label_ToshibaPort.AutoSize = true;
            this.label_ToshibaPort.Font = new System.Drawing.Font("新細明體", 9F);
            this.label_ToshibaPort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label_ToshibaPort.Location = new System.Drawing.Point(15, 52);
            this.label_ToshibaPort.Name = "label_ToshibaPort";
            this.label_ToshibaPort.Size = new System.Drawing.Size(108, 12);
            this.label_ToshibaPort.TabIndex = 92;
            this.label_ToshibaPort.Text = "TCP Port (Command)";
            // 
            // label_ToshibaIP
            // 
            this.label_ToshibaIP.AutoSize = true;
            this.label_ToshibaIP.Font = new System.Drawing.Font("新細明體", 9F);
            this.label_ToshibaIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label_ToshibaIP.Location = new System.Drawing.Point(16, 24);
            this.label_ToshibaIP.Name = "label_ToshibaIP";
            this.label_ToshibaIP.Size = new System.Drawing.Size(52, 12);
            this.label_ToshibaIP.TabIndex = 90;
            this.label_ToshibaIP.Text = "IP address";
            // 
            // btnResetAlarm
            // 
            this.btnResetAlarm.Location = new System.Drawing.Point(96, 75);
            this.btnResetAlarm.Name = "btnResetAlarm";
            this.btnResetAlarm.Size = new System.Drawing.Size(100, 75);
            this.btnResetAlarm.TabIndex = 2;
            this.btnResetAlarm.Text = "重置警告訊息";
            this.btnResetAlarm.UseVisualStyleBackColor = true;
            this.btnResetAlarm.Click += new System.EventHandler(this.btnResetAlarm_Click);
            // 
            // btnOverride
            // 
            this.btnOverride.Location = new System.Drawing.Point(16, 195);
            this.btnOverride.Name = "btnOverride";
            this.btnOverride.Size = new System.Drawing.Size(180, 49);
            this.btnOverride.TabIndex = 25;
            this.btnOverride.Text = "設定";
            this.btnOverride.UseVisualStyleBackColor = true;
            this.btnOverride.Click += new System.EventHandler(this.btnOverride_Click);
            // 
            // numericUpDown_Override
            // 
            this.numericUpDown_Override.Location = new System.Drawing.Point(75, 167);
            this.numericUpDown_Override.Name = "numericUpDown_Override";
            this.numericUpDown_Override.Size = new System.Drawing.Size(121, 22);
            this.numericUpDown_Override.TabIndex = 24;
            this.numericUpDown_Override.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnStopControl
            // 
            this.btnStopControl.BackColor = System.Drawing.Color.Red;
            this.btnStopControl.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold);
            this.btnStopControl.ForeColor = System.Drawing.Color.White;
            this.btnStopControl.Location = new System.Drawing.Point(18, 117);
            this.btnStopControl.Name = "btnStopControl";
            this.btnStopControl.Size = new System.Drawing.Size(74, 33);
            this.btnStopControl.TabIndex = 2;
            this.btnStopControl.Text = "停止";
            this.btnStopControl.UseVisualStyleBackColor = false;
            this.btnStopControl.Visible = false;
            this.btnStopControl.Click += new System.EventHandler(this.btnStopControl_Click);
            // 
            // textBox_ToshibaIPAddr
            // 
            this.textBox_ToshibaIPAddr.Location = new System.Drawing.Point(73, 19);
            this.textBox_ToshibaIPAddr.Name = "textBox_ToshibaIPAddr";
            this.textBox_ToshibaIPAddr.Size = new System.Drawing.Size(123, 22);
            this.textBox_ToshibaIPAddr.TabIndex = 3;
            this.textBox_ToshibaIPAddr.Text = "192.168.0.124";
            // 
            // btnStartControl
            // 
            this.btnStartControl.BackColor = System.Drawing.Color.Green;
            this.btnStartControl.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold);
            this.btnStartControl.ForeColor = System.Drawing.Color.White;
            this.btnStartControl.Location = new System.Drawing.Point(18, 75);
            this.btnStartControl.Name = "btnStartControl";
            this.btnStartControl.Size = new System.Drawing.Size(74, 36);
            this.btnStartControl.TabIndex = 0;
            this.btnStartControl.Text = "啟動";
            this.btnStartControl.UseVisualStyleBackColor = false;
            this.btnStartControl.Click += new System.EventHandler(this.btnStartControl_Click);
            // 
            // textBox_ToshibaPort
            // 
            this.textBox_ToshibaPort.Location = new System.Drawing.Point(129, 47);
            this.textBox_ToshibaPort.Name = "textBox_ToshibaPort";
            this.textBox_ToshibaPort.Size = new System.Drawing.Size(67, 22);
            this.textBox_ToshibaPort.TabIndex = 4;
            this.textBox_ToshibaPort.Text = "1000";
            // 
            // groupBox_MovePos
            // 
            this.groupBox_MovePos.Controls.Add(this.label_Number_Points);
            this.groupBox_MovePos.Controls.Add(this.btn_Compute_Homography);
            this.groupBox_MovePos.Controls.Add(this.btn_Re_Record);
            this.groupBox_MovePos.Controls.Add(this.btn_Record_Points_Mechanical_Arm);
            this.groupBox_MovePos.Controls.Add(this.btn_Record_Points_Image);
            this.groupBox_MovePos.Controls.Add(this.btnStopMove);
            this.groupBox_MovePos.Controls.Add(this.labWarning_Signs);
            this.groupBox_MovePos.Controls.Add(this.labelX);
            this.groupBox_MovePos.Controls.Add(this.numericUpDownX);
            this.groupBox_MovePos.Controls.Add(this.numericUpDownY);
            this.groupBox_MovePos.Controls.Add(this.labelY);
            this.groupBox_MovePos.Controls.Add(this.numericUpDownZ);
            this.groupBox_MovePos.Controls.Add(this.labelZ);
            this.groupBox_MovePos.Controls.Add(this.btnStartMove);
            this.groupBox_MovePos.Controls.Add(this.numericUpDownT);
            this.groupBox_MovePos.Controls.Add(this.labelC);
            this.groupBox_MovePos.Controls.Add(this.labelT);
            this.groupBox_MovePos.Controls.Add(this.numericUpDownC);
            this.groupBox_MovePos.Location = new System.Drawing.Point(12, 283);
            this.groupBox_MovePos.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_MovePos.Name = "groupBox_MovePos";
            this.groupBox_MovePos.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_MovePos.Size = new System.Drawing.Size(212, 265);
            this.groupBox_MovePos.TabIndex = 218;
            this.groupBox_MovePos.TabStop = false;
            this.groupBox_MovePos.Text = "直線座標移動";
            // 
            // label_Number_Points
            // 
            this.label_Number_Points.AutoSize = true;
            this.label_Number_Points.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Number_Points.Location = new System.Drawing.Point(1, 236);
            this.label_Number_Points.Name = "label_Number_Points";
            this.label_Number_Points.Size = new System.Drawing.Size(157, 21);
            this.label_Number_Points.TabIndex = 258;
            this.label_Number_Points.Text = "紀錄點位數量：";
            // 
            // btn_Compute_Homography
            // 
            this.btn_Compute_Homography.Enabled = false;
            this.btn_Compute_Homography.Location = new System.Drawing.Point(109, 183);
            this.btn_Compute_Homography.Name = "btn_Compute_Homography";
            this.btn_Compute_Homography.Size = new System.Drawing.Size(87, 34);
            this.btn_Compute_Homography.TabIndex = 257;
            this.btn_Compute_Homography.Text = "儲存校正點位";
            this.btn_Compute_Homography.UseVisualStyleBackColor = true;
            this.btn_Compute_Homography.Click += new System.EventHandler(this.btn_Compute_Homography_Click);
            // 
            // btn_Re_Record
            // 
            this.btn_Re_Record.Location = new System.Drawing.Point(232, 202);
            this.btn_Re_Record.Name = "btn_Re_Record";
            this.btn_Re_Record.Size = new System.Drawing.Size(88, 39);
            this.btn_Re_Record.TabIndex = 257;
            this.btn_Re_Record.Text = "手臂座標重新紀錄";
            this.btn_Re_Record.UseVisualStyleBackColor = true;
            this.btn_Re_Record.Visible = false;
            this.btn_Re_Record.Click += new System.EventHandler(this.btn_Re_Record_Click);
            // 
            // btn_Record_Points_Mechanical_Arm
            // 
            this.btn_Record_Points_Mechanical_Arm.Enabled = false;
            this.btn_Record_Points_Mechanical_Arm.Location = new System.Drawing.Point(232, 247);
            this.btn_Record_Points_Mechanical_Arm.Name = "btn_Record_Points_Mechanical_Arm";
            this.btn_Record_Points_Mechanical_Arm.Size = new System.Drawing.Size(88, 30);
            this.btn_Record_Points_Mechanical_Arm.TabIndex = 257;
            this.btn_Record_Points_Mechanical_Arm.Text = "紀錄手臂座標";
            this.btn_Record_Points_Mechanical_Arm.UseVisualStyleBackColor = true;
            this.btn_Record_Points_Mechanical_Arm.Visible = false;
            this.btn_Record_Points_Mechanical_Arm.Click += new System.EventHandler(this.btn_Record_Points_Mechanical_Click);
            // 
            // btn_Record_Points_Image
            // 
            this.btn_Record_Points_Image.Location = new System.Drawing.Point(108, 133);
            this.btn_Record_Points_Image.Name = "btn_Record_Points_Image";
            this.btn_Record_Points_Image.Size = new System.Drawing.Size(98, 40);
            this.btn_Record_Points_Image.TabIndex = 257;
            this.btn_Record_Points_Image.Text = "取得點位(手臂影像校正用)";
            this.btn_Record_Points_Image.UseVisualStyleBackColor = true;
            this.btn_Record_Points_Image.Click += new System.EventHandler(this.btn_Record_Points_Image_Click);
            // 
            // btnStopMove
            // 
            this.btnStopMove.Location = new System.Drawing.Point(108, 76);
            this.btnStopMove.Name = "btnStopMove";
            this.btnStopMove.Size = new System.Drawing.Size(88, 33);
            this.btnStopMove.TabIndex = 39;
            this.btnStopMove.Text = "緊急停止";
            this.btnStopMove.UseVisualStyleBackColor = true;
            this.btnStopMove.Click += new System.EventHandler(this.btnStopMove_Click);
            // 
            // labWarning_Signs
            // 
            this.labWarning_Signs.AutoSize = true;
            this.labWarning_Signs.Font = new System.Drawing.Font("新細明體", 9.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labWarning_Signs.ForeColor = System.Drawing.Color.Red;
            this.labWarning_Signs.Location = new System.Drawing.Point(4, 192);
            this.labWarning_Signs.Name = "labWarning_Signs";
            this.labWarning_Signs.Size = new System.Drawing.Size(13, 13);
            this.labWarning_Signs.TabIndex = 38;
            this.labWarning_Signs.Text = "  ";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(9, 36);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(13, 12);
            this.labelX.TabIndex = 24;
            this.labelX.Text = "X";
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.Location = new System.Drawing.Point(28, 31);
            this.numericUpDownX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(71, 22);
            this.numericUpDownX.TabIndex = 23;
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.Location = new System.Drawing.Point(28, 59);
            this.numericUpDownY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(71, 22);
            this.numericUpDownY.TabIndex = 25;
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(9, 64);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(13, 12);
            this.labelY.TabIndex = 26;
            this.labelY.Text = "Y";
            // 
            // numericUpDownZ
            // 
            this.numericUpDownZ.Location = new System.Drawing.Point(28, 87);
            this.numericUpDownZ.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDownZ.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownZ.Name = "numericUpDownZ";
            this.numericUpDownZ.Size = new System.Drawing.Size(71, 22);
            this.numericUpDownZ.TabIndex = 27;
            this.numericUpDownZ.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(9, 92);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(12, 12);
            this.labelZ.TabIndex = 28;
            this.labelZ.Text = "Z";
            // 
            // btnStartMove
            // 
            this.btnStartMove.Location = new System.Drawing.Point(108, 31);
            this.btnStartMove.Name = "btnStartMove";
            this.btnStartMove.Size = new System.Drawing.Size(88, 33);
            this.btnStartMove.TabIndex = 33;
            this.btnStartMove.Text = "移動";
            this.btnStartMove.UseVisualStyleBackColor = true;
            this.btnStartMove.Click += new System.EventHandler(this.btnStartMove_Click);
            // 
            // numericUpDownT
            // 
            this.numericUpDownT.Enabled = false;
            this.numericUpDownT.Location = new System.Drawing.Point(28, 140);
            this.numericUpDownT.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownT.Minimum = new decimal(new int[] {
            600,
            0,
            0,
            -2147483648});
            this.numericUpDownT.Name = "numericUpDownT";
            this.numericUpDownT.Size = new System.Drawing.Size(71, 22);
            this.numericUpDownT.TabIndex = 29;
            // 
            // labelC
            // 
            this.labelC.AutoSize = true;
            this.labelC.Location = new System.Drawing.Point(9, 118);
            this.labelC.Name = "labelC";
            this.labelC.Size = new System.Drawing.Size(13, 12);
            this.labelC.TabIndex = 32;
            this.labelC.Text = "C";
            // 
            // labelT
            // 
            this.labelT.AutoSize = true;
            this.labelT.Enabled = false;
            this.labelT.Location = new System.Drawing.Point(9, 144);
            this.labelT.Name = "labelT";
            this.labelT.Size = new System.Drawing.Size(12, 12);
            this.labelT.TabIndex = 30;
            this.labelT.Text = "T";
            // 
            // numericUpDownC
            // 
            this.numericUpDownC.Location = new System.Drawing.Point(28, 112);
            this.numericUpDownC.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownC.Minimum = new decimal(new int[] {
            600,
            0,
            0,
            -2147483648});
            this.numericUpDownC.Name = "numericUpDownC";
            this.numericUpDownC.Size = new System.Drawing.Size(71, 22);
            this.numericUpDownC.TabIndex = 31;
            // 
            // _StatusStrip
            // 
            this._StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusLabel_Content});
            this._StatusStrip.Location = new System.Drawing.Point(0, 837);
            this._StatusStrip.Name = "_StatusStrip";
            this._StatusStrip.Size = new System.Drawing.Size(1284, 22);
            this._StatusStrip.TabIndex = 219;
            this._StatusStrip.Text = "狀態列";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel.Text = "狀態";
            // 
            // toolStripStatusLabel_Content
            // 
            this.toolStripStatusLabel_Content.Name = "toolStripStatusLabel_Content";
            this.toolStripStatusLabel_Content.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel_Content.Text = "內容";
            // 
            // ROI_Img
            // 
            this.ROI_Img.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ROI_Img.Location = new System.Drawing.Point(823, 103);
            this.ROI_Img.Name = "ROI_Img";
            this.ROI_Img.Size = new System.Drawing.Size(155, 142);
            this.ROI_Img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ROI_Img.TabIndex = 220;
            this.ROI_Img.TabStop = false;
            // 
            // Select_Anchor
            // 
            this.Select_Anchor.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Select_Anchor.Location = new System.Drawing.Point(1042, 25);
            this.Select_Anchor.Name = "Select_Anchor";
            this.Select_Anchor.Size = new System.Drawing.Size(91, 30);
            this.Select_Anchor.TabIndex = 221;
            this.Select_Anchor.Text = "Select Anchor";
            this.Select_Anchor.UseVisualStyleBackColor = true;
            this.Select_Anchor.Click += new System.EventHandler(this.Select_Anchor_Click);
            // 
            // ConvtCoodinate
            // 
            this.ConvtCoodinate.Enabled = false;
            this.ConvtCoodinate.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ConvtCoodinate.Location = new System.Drawing.Point(1041, 122);
            this.ConvtCoodinate.Name = "ConvtCoodinate";
            this.ConvtCoodinate.Size = new System.Drawing.Size(109, 37);
            this.ConvtCoodinate.TabIndex = 224;
            this.ConvtCoodinate.Text = "取得轉換座標";
            this.ConvtCoodinate.UseVisualStyleBackColor = true;
            this.ConvtCoodinate.Click += new System.EventHandler(this.ConvtCoodinate_Click);
            // 
            // Position
            // 
            this.Position.Enabled = false;
            this.Position.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Position.Location = new System.Drawing.Point(1041, 169);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(92, 33);
            this.Position.TabIndex = 223;
            this.Position.Text = "Position";
            this.Position.UseVisualStyleBackColor = true;
            this.Position.Click += new System.EventHandler(this.Position_Click);
            // 
            // CalAxisPoint
            // 
            this.CalAxisPoint.Enabled = false;
            this.CalAxisPoint.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.CalAxisPoint.Location = new System.Drawing.Point(1041, 65);
            this.CalAxisPoint.Name = "CalAxisPoint";
            this.CalAxisPoint.Size = new System.Drawing.Size(119, 43);
            this.CalAxisPoint.TabIndex = 222;
            this.CalAxisPoint.Text = "Calculate axis center";
            this.CalAxisPoint.UseVisualStyleBackColor = true;
            this.CalAxisPoint.Click += new System.EventHandler(this.CalAxisPoint_Click);
            // 
            // MoveToImgCenter
            // 
            this.MoveToImgCenter.Enabled = false;
            this.MoveToImgCenter.Location = new System.Drawing.Point(1041, 215);
            this.MoveToImgCenter.Name = "MoveToImgCenter";
            this.MoveToImgCenter.Size = new System.Drawing.Size(112, 41);
            this.MoveToImgCenter.TabIndex = 225;
            this.MoveToImgCenter.Text = "位移至中心點";
            this.MoveToImgCenter.UseVisualStyleBackColor = true;
            this.MoveToImgCenter.Click += new System.EventHandler(this.MoveToImgCenter_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(821, 353);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 226;
            this.label3.Text = "角度1：";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(823, 374);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(307, 102);
            this.listView1.TabIndex = 227;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Point";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Posion";
            this.columnHeader2.Width = 150;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1136, 372);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 229;
            this.label10.Text = "角度：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1136, 430);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 228;
            this.label15.Text = "半徑：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(889, 553);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 233;
            this.label4.Text = "手臂復歸位置：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(889, 508);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 232;
            this.label7.Text = "影像軸心位置：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(889, 532);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 231;
            this.label6.Text = "手臂軸心位置：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(913, 488);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 230;
            this.label11.Text = "標靶位置：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(965, 277);
            this.textBox3.Maximum = new decimal(new int[] {
            1040,
            0,
            0,
            0});
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(68, 22);
            this.textBox3.TabIndex = 239;
            this.textBox3.Value = new decimal(new int[] {
            520,
            0,
            0,
            0});
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(868, 277);
            this.textBox2.Maximum = new decimal(new int[] {
            1388,
            0,
            0,
            0});
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(68, 22);
            this.textBox2.TabIndex = 238;
            this.textBox2.Value = new decimal(new int[] {
            694,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(946, 280);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(13, 12);
            this.label17.TabIndex = 237;
            this.label17.Text = "Y";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(849, 282);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(13, 12);
            this.label16.TabIndex = 236;
            this.label16.Text = "X";
            // 
            // AnchorMoveAssignPoint
            // 
            this.AnchorMoveAssignPoint.Enabled = false;
            this.AnchorMoveAssignPoint.Location = new System.Drawing.Point(1041, 277);
            this.AnchorMoveAssignPoint.Name = "AnchorMoveAssignPoint";
            this.AnchorMoveAssignPoint.Size = new System.Drawing.Size(83, 23);
            this.AnchorMoveAssignPoint.TabIndex = 235;
            this.AnchorMoveAssignPoint.Text = "位移";
            this.AnchorMoveAssignPoint.UseVisualStyleBackColor = true;
            this.AnchorMoveAssignPoint.Click += new System.EventHandler(this.AnchorMoveAssignPoint_Click);
            // 
            // ListviewP
            // 
            this.ListviewP.Controls.Add(this.tabPage1);
            this.ListviewP.Controls.Add(this.tabPage2);
            this.ListviewP.Controls.Add(this.tabPage3);
            this.ListviewP.Controls.Add(this.tabPage6);
            this.ListviewP.Location = new System.Drawing.Point(203, 583);
            this.ListviewP.Name = "ListviewP";
            this.ListviewP.SelectedIndex = 0;
            this.ListviewP.Size = new System.Drawing.Size(1075, 231);
            this.ListviewP.TabIndex = 234;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView2);
            this.tabPage1.Controls.Add(this.button8);
            this.tabPage1.Controls.Add(this.Export_Excel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1067, 205);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Move";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ArmXMove,
            this.ArmXRealMove,
            this.ArmYMove,
            this.ArmYRealMove,
            this.ImgXMove,
            this.ImgXRealMove,
            this.ImgYMove,
            this.ImgYRealMove,
            this.Angle});
            this.listView2.Location = new System.Drawing.Point(9, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(917, 193);
            this.listView2.TabIndex = 149;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // ArmXMove
            // 
            this.ArmXMove.Text = "ArmXMove";
            this.ArmXMove.Width = 89;
            // 
            // ArmXRealMove
            // 
            this.ArmXRealMove.Text = "ArmXRealMove";
            this.ArmXRealMove.Width = 103;
            // 
            // ArmYMove
            // 
            this.ArmYMove.Text = "ArmYMove";
            this.ArmYMove.Width = 89;
            // 
            // ArmYRealMove
            // 
            this.ArmYRealMove.Text = "ArmYRealMove";
            this.ArmYRealMove.Width = 102;
            // 
            // ImgXMove
            // 
            this.ImgXMove.Text = "ImgXMove";
            this.ImgXMove.Width = 95;
            // 
            // ImgXRealMove
            // 
            this.ImgXRealMove.Text = "ImgXRealMove";
            this.ImgXRealMove.Width = 115;
            // 
            // ImgYMove
            // 
            this.ImgYMove.Text = "ImgYMove";
            this.ImgYMove.Width = 96;
            // 
            // ImgYRealMove
            // 
            this.ImgYRealMove.Text = "ImgYRealMove";
            this.ImgYRealMove.Width = 112;
            // 
            // Angle
            // 
            this.Angle.Text = "Angle";
            this.Angle.Width = 112;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(932, 122);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(87, 31);
            this.button8.TabIndex = 151;
            this.button8.Text = "清除";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // Export_Excel
            // 
            this.Export_Excel.Location = new System.Drawing.Point(932, 66);
            this.Export_Excel.Name = "Export_Excel";
            this.Export_Excel.Size = new System.Drawing.Size(87, 31);
            this.Export_Excel.TabIndex = 150;
            this.Export_Excel.Text = "Excel";
            this.Export_Excel.UseVisualStyleBackColor = true;
            this.Export_Excel.Click += new System.EventHandler(this.Export_Excel_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.RemoveExl);
            this.tabPage2.Controls.Add(this.LocalRecordToExl);
            this.tabPage2.Controls.Add(this.listView3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1067, 205);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Point1";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // RemoveExl
            // 
            this.RemoveExl.Location = new System.Drawing.Point(702, 118);
            this.RemoveExl.Name = "RemoveExl";
            this.RemoveExl.Size = new System.Drawing.Size(87, 31);
            this.RemoveExl.TabIndex = 153;
            this.RemoveExl.Text = "清除";
            this.RemoveExl.UseVisualStyleBackColor = true;
            // 
            // LocalRecordToExl
            // 
            this.LocalRecordToExl.Location = new System.Drawing.Point(702, 63);
            this.LocalRecordToExl.Name = "LocalRecordToExl";
            this.LocalRecordToExl.Size = new System.Drawing.Size(87, 31);
            this.LocalRecordToExl.TabIndex = 152;
            this.LocalRecordToExl.Text = "Excel";
            this.LocalRecordToExl.UseVisualStyleBackColor = true;
            this.LocalRecordToExl.Click += new System.EventHandler(this.Export_Excel_Click);
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Num,
            this.LocalX,
            this.LocalY});
            this.listView3.Location = new System.Drawing.Point(6, 6);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(518, 193);
            this.listView3.TabIndex = 0;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // Num
            // 
            this.Num.Text = "Num";
            this.Num.Width = 106;
            // 
            // LocalX
            // 
            this.LocalX.Text = "LocalX";
            this.LocalX.Width = 198;
            // 
            // LocalY
            // 
            this.LocalY.Text = "LocalY";
            this.LocalY.Width = 208;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button5);
            this.tabPage3.Controls.Add(this.button10);
            this.tabPage3.Controls.Add(this.listView4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1067, 205);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Point2";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(705, 118);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 31);
            this.button5.TabIndex = 156;
            this.button5.Text = "清除";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(705, 63);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(87, 31);
            this.button10.TabIndex = 155;
            this.button10.Text = "Excel";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Export_Excel_Click);
            // 
            // listView4
            // 
            this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView4.Location = new System.Drawing.Point(9, 6);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(518, 193);
            this.listView4.TabIndex = 154;
            this.listView4.UseCompatibleStateImageBehavior = false;
            this.listView4.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Num";
            this.columnHeader3.Width = 106;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "LocalX";
            this.columnHeader4.Width = 198;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "LocalY";
            this.columnHeader5.Width = 208;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.SetParam);
            this.tabPage6.Controls.Add(this.PositionLocal);
            this.tabPage6.Controls.Add(this.label23);
            this.tabPage6.Controls.Add(this.DownLocal);
            this.tabPage6.Controls.Add(this.label22);
            this.tabPage6.Controls.Add(this.UpLocal);
            this.tabPage6.Controls.Add(this.label21);
            this.tabPage6.Controls.Add(this.numericUpDown2);
            this.tabPage6.Controls.Add(this.label14);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1067, 205);
            this.tabPage6.TabIndex = 6;
            this.tabPage6.Text = "設定";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // SetParam
            // 
            this.SetParam.Location = new System.Drawing.Point(194, 98);
            this.SetParam.Name = "SetParam";
            this.SetParam.Size = new System.Drawing.Size(100, 39);
            this.SetParam.TabIndex = 182;
            this.SetParam.Text = "設定";
            this.SetParam.UseVisualStyleBackColor = true;
            this.SetParam.Click += new System.EventHandler(this.SetParam_Click);
            // 
            // PositionLocal
            // 
            this.PositionLocal.Location = new System.Drawing.Point(207, 35);
            this.PositionLocal.Name = "PositionLocal";
            this.PositionLocal.Size = new System.Drawing.Size(100, 22);
            this.PositionLocal.TabIndex = 181;
            this.PositionLocal.Text = "80";
            this.PositionLocal.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.PositionLocal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(205, 10);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(89, 12);
            this.label23.TabIndex = 180;
            this.label23.Text = "定位高度位置：";
            // 
            // DownLocal
            // 
            this.DownLocal.Location = new System.Drawing.Point(10, 108);
            this.DownLocal.Name = "DownLocal";
            this.DownLocal.Size = new System.Drawing.Size(100, 22);
            this.DownLocal.TabIndex = 179;
            this.DownLocal.Text = "92.5";
            this.DownLocal.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.DownLocal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 82);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(89, 12);
            this.label22.TabIndex = 178;
            this.label22.Text = "下降高度位置：";
            // 
            // UpLocal
            // 
            this.UpLocal.Location = new System.Drawing.Point(10, 35);
            this.UpLocal.Name = "UpLocal";
            this.UpLocal.Size = new System.Drawing.Size(100, 22);
            this.UpLocal.TabIndex = 177;
            this.UpLocal.Text = "120";
            this.UpLocal.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.UpLocal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(8, 10);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(89, 12);
            this.label21.TabIndex = 176;
            this.label21.Text = "上升高度位置：";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(79, 168);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown2.TabIndex = 174;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 173);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 175;
            this.label14.Text = "角度補償：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 605);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 240;
            this.label5.Text = "轉換比例";
            // 
            // RevertToOrigin
            // 
            this.RevertToOrigin.Location = new System.Drawing.Point(488, 525);
            this.RevertToOrigin.Name = "RevertToOrigin";
            this.RevertToOrigin.Size = new System.Drawing.Size(85, 23);
            this.RevertToOrigin.TabIndex = 243;
            this.RevertToOrigin.Text = "手臂復歸";
            this.RevertToOrigin.UseVisualStyleBackColor = true;
            this.RevertToOrigin.Click += new System.EventHandler(this.RevertToOrigin_Click);
            // 
            // MoveToPoint
            // 
            this.MoveToPoint.Location = new System.Drawing.Point(490, 490);
            this.MoveToPoint.Name = "MoveToPoint";
            this.MoveToPoint.Size = new System.Drawing.Size(83, 23);
            this.MoveToPoint.TabIndex = 242;
            this.MoveToPoint.Text = "位移";
            this.MoveToPoint.UseVisualStyleBackColor = true;
            this.MoveToPoint.Click += new System.EventHandler(this.MoveToPoint_Click);
            // 
            // CalMoveDistance
            // 
            this.CalMoveDistance.Location = new System.Drawing.Point(394, 525);
            this.CalMoveDistance.Name = "CalMoveDistance";
            this.CalMoveDistance.Size = new System.Drawing.Size(88, 23);
            this.CalMoveDistance.TabIndex = 241;
            this.CalMoveDistance.Text = "位移計算";
            this.CalMoveDistance.UseVisualStyleBackColor = true;
            this.CalMoveDistance.Click += new System.EventHandler(this.CalMoveDistance_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(1138, 343);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown1.TabIndex = 244;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // LocateHeight
            // 
            this.LocateHeight.Location = new System.Drawing.Point(811, 305);
            this.LocateHeight.Name = "LocateHeight";
            this.LocateHeight.Size = new System.Drawing.Size(83, 23);
            this.LocateHeight.TabIndex = 245;
            this.LocateHeight.Text = "定位高度";
            this.LocateHeight.UseVisualStyleBackColor = true;
            this.LocateHeight.Click += new System.EventHandler(this.LocateHeight_Click);
            // 
            // ZAxisDown
            // 
            this.ZAxisDown.Location = new System.Drawing.Point(1195, 309);
            this.ZAxisDown.Name = "ZAxisDown";
            this.ZAxisDown.Size = new System.Drawing.Size(83, 23);
            this.ZAxisDown.TabIndex = 247;
            this.ZAxisDown.Text = "下降";
            this.ZAxisDown.UseVisualStyleBackColor = true;
            this.ZAxisDown.Click += new System.EventHandler(this.ZAxisDown_Click);
            // 
            // ZAxisUp
            // 
            this.ZAxisUp.Location = new System.Drawing.Point(1195, 274);
            this.ZAxisUp.Name = "ZAxisUp";
            this.ZAxisUp.Size = new System.Drawing.Size(83, 23);
            this.ZAxisUp.TabIndex = 246;
            this.ZAxisUp.Text = "上升";
            this.ZAxisUp.UseVisualStyleBackColor = true;
            this.ZAxisUp.Click += new System.EventHandler(this.ZAxisUp_Click);
            // 
            // AssignPositionMove
            // 
            this.AssignPositionMove.AutoSize = true;
            this.AssignPositionMove.Location = new System.Drawing.Point(965, 309);
            this.AssignPositionMove.Name = "AssignPositionMove";
            this.AssignPositionMove.Size = new System.Drawing.Size(96, 16);
            this.AssignPositionMove.TabIndex = 248;
            this.AssignPositionMove.Text = "指定位置位移";
            this.AssignPositionMove.UseVisualStyleBackColor = true;
            // 
            // ObDetected
            // 
            this.ObDetected.Location = new System.Drawing.Point(394, 490);
            this.ObDetected.Name = "ObDetected";
            this.ObDetected.Size = new System.Drawing.Size(83, 23);
            this.ObDetected.TabIndex = 249;
            this.ObDetected.Text = "物件偵測";
            this.ObDetected.UseVisualStyleBackColor = true;
            this.ObDetected.Click += new System.EventHandler(this.ObDetected_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(584, 512);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 251;
            this.label13.Text = "位移距離：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(608, 486);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 250;
            this.label12.Text = "位置：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(656, 553);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 42);
            this.button1.TabIndex = 252;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_ImageLoad
            // 
            this.btn_ImageLoad.Location = new System.Drawing.Point(1172, 73);
            this.btn_ImageLoad.Name = "btn_ImageLoad";
            this.btn_ImageLoad.Size = new System.Drawing.Size(99, 22);
            this.btn_ImageLoad.TabIndex = 253;
            this.btn_ImageLoad.Text = "載入影像";
            this.btn_ImageLoad.UseVisualStyleBackColor = true;
            this.btn_ImageLoad.Click += new System.EventHandler(this.btn_ImageLoad_Click);
            // 
            // btn_AnalysisImage
            // 
            this.btn_AnalysisImage.Enabled = false;
            this.btn_AnalysisImage.Location = new System.Drawing.Point(1172, 102);
            this.btn_AnalysisImage.Name = "btn_AnalysisImage";
            this.btn_AnalysisImage.Size = new System.Drawing.Size(99, 22);
            this.btn_AnalysisImage.TabIndex = 254;
            this.btn_AnalysisImage.Text = "分析影像";
            this.btn_AnalysisImage.UseVisualStyleBackColor = true;
            this.btn_AnalysisImage.Click += new System.EventHandler(this.btn_AnalysisImage_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(1172, 490);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(99, 97);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 255;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // cannyImageBox
            // 
            this.cannyImageBox.Location = new System.Drawing.Point(1059, 490);
            this.cannyImageBox.Name = "cannyImageBox";
            this.cannyImageBox.Size = new System.Drawing.Size(101, 100);
            this.cannyImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cannyImageBox.TabIndex = 256;
            this.cannyImageBox.TabStop = false;
            this.cannyImageBox.Visible = false;
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(1172, 162);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(99, 22);
            this.btn_Close.TabIndex = 254;
            this.btn_Close.Text = "取消功能";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Visible = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(1172, 488);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(101, 100);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 256;
            this.imageBox1.TabStop = false;
            this.imageBox1.Visible = false;
            // 
            // btn_Move
            // 
            this.btn_Move.Enabled = false;
            this.btn_Move.Location = new System.Drawing.Point(1172, 133);
            this.btn_Move.Name = "btn_Move";
            this.btn_Move.Size = new System.Drawing.Size(99, 22);
            this.btn_Move.TabIndex = 254;
            this.btn_Move.Text = "手臂邊緣移動";
            this.btn_Move.UseVisualStyleBackColor = true;
            this.btn_Move.Click += new System.EventHandler(this.btn_Move_Click);
            // 
            // btn_Get_Point
            // 
            this.btn_Get_Point.Enabled = false;
            this.btn_Get_Point.Location = new System.Drawing.Point(903, 56);
            this.btn_Get_Point.Name = "btn_Get_Point";
            this.btn_Get_Point.Size = new System.Drawing.Size(75, 35);
            this.btn_Get_Point.TabIndex = 257;
            this.btn_Get_Point.Text = "取得點位";
            this.btn_Get_Point.UseVisualStyleBackColor = true;
            this.btn_Get_Point.Click += new System.EventHandler(this.btn_Get_Point_Click);
            // 
            // btn_Click_Move
            // 
            this.btn_Click_Move.Location = new System.Drawing.Point(823, 251);
            this.btn_Click_Move.Name = "btn_Click_Move";
            this.btn_Click_Move.Size = new System.Drawing.Size(155, 20);
            this.btn_Click_Move.TabIndex = 257;
            this.btn_Click_Move.Text = "點選移動(開啟)";
            this.btn_Click_Move.UseVisualStyleBackColor = true;
            this.btn_Click_Move.Click += new System.EventHandler(this.btn_Click_Move_Click);
            // 
            // imageBox_Point
            // 
            this.imageBox_Point.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.imageBox_Point.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox_Point.Location = new System.Drawing.Point(246, 68);
            this.imageBox_Point.Name = "imageBox_Point";
            this.imageBox_Point.Size = new System.Drawing.Size(559, 408);
            this.imageBox_Point.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox_Point.TabIndex = 258;
            this.imageBox_Point.TabStop = false;
            this.imageBox_Point.Visible = false;
            this.imageBox_Point.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox_Point_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 859);
            this.Controls.Add(this.btn_Click_Move);
            this.Controls.Add(this.btn_Get_Point);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.cannyImageBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Move);
            this.Controls.Add(this.btn_AnalysisImage);
            this.Controls.Add(this.btn_ImageLoad);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ObDetected);
            this.Controls.Add(this.AssignPositionMove);
            this.Controls.Add(this.ZAxisDown);
            this.Controls.Add(this.ZAxisUp);
            this.Controls.Add(this.LocateHeight);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.RevertToOrigin);
            this.Controls.Add(this.MoveToPoint);
            this.Controls.Add(this.CalMoveDistance);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.AnchorMoveAssignPoint);
            this.Controls.Add(this.ListviewP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MoveToImgCenter);
            this.Controls.Add(this.ConvtCoodinate);
            this.Controls.Add(this.Position);
            this.Controls.Add(this.CalAxisPoint);
            this.Controls.Add(this.Select_Anchor);
            this.Controls.Add(this.ROI_Img);
            this.Controls.Add(this._StatusStrip);
            this.Controls.Add(this.groupBox_MovePos);
            this.Controls.Add(this.groupBox_Toshiba);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Search_Camera);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Camera_comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.framerate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Proc_time);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Process);
            this.Controls.Add(this.Display_Img);
            this.Controls.Add(this.imageBox_Point);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Display_Img)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_Toshiba.ResumeLayout(false);
            this.groupBox_Toshiba.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Override)).EndInit();
            this.groupBox_MovePos.ResumeLayout(false);
            this.groupBox_MovePos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownC)).EndInit();
            this._StatusStrip.ResumeLayout(false);
            this._StatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROI_Img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox2)).EndInit();
            this.ListviewP.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Point)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox Display_Img;
        private System.Windows.Forms.Button Process;
        private System.Windows.Forms.Button Search_Camera;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox Camera_comboBox;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox framerate;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox Proc_time;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ImageMatch;
        private System.Windows.Forms.RadioButton Instant_mode;
        private System.Windows.Forms.GroupBox groupBox_Toshiba;
        private System.Windows.Forms.Label label_Override;
        private System.Windows.Forms.Label label_ToshibaPort;
        private System.Windows.Forms.Label label_ToshibaIP;
        private System.Windows.Forms.Button btnResetAlarm;
        private System.Windows.Forms.Button btnOverride;
        private System.Windows.Forms.NumericUpDown numericUpDown_Override;
        private System.Windows.Forms.Button btnStopControl;
        private System.Windows.Forms.TextBox textBox_ToshibaIPAddr;
        private System.Windows.Forms.Button btnStartControl;
        private System.Windows.Forms.TextBox textBox_ToshibaPort;
        private System.Windows.Forms.GroupBox groupBox_MovePos;
        private System.Windows.Forms.Button btnStopMove;
        private System.Windows.Forms.Label labWarning_Signs;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.NumericUpDown numericUpDownZ;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Button btnStartMove;
        private System.Windows.Forms.NumericUpDown numericUpDownT;
        private System.Windows.Forms.Label labelC;
        private System.Windows.Forms.Label labelT;
        private System.Windows.Forms.NumericUpDown numericUpDownC;
        private System.Windows.Forms.StatusStrip _StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Content;
        private Emgu.CV.UI.ImageBox ROI_Img;
        private System.Windows.Forms.Button Select_Anchor;
        private System.Windows.Forms.Button ConvtCoodinate;
        private System.Windows.Forms.Button Position;
        private System.Windows.Forms.Button CalAxisPoint;
        private System.Windows.Forms.Button MoveToImgCenter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown textBox3;
        private System.Windows.Forms.NumericUpDown textBox2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button AnchorMoveAssignPoint;
        private System.Windows.Forms.TabControl ListviewP;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader ArmXMove;
        private System.Windows.Forms.ColumnHeader ArmXRealMove;
        private System.Windows.Forms.ColumnHeader ArmYMove;
        private System.Windows.Forms.ColumnHeader ArmYRealMove;
        private System.Windows.Forms.ColumnHeader ImgXMove;
        private System.Windows.Forms.ColumnHeader ImgXRealMove;
        private System.Windows.Forms.ColumnHeader ImgYMove;
        private System.Windows.Forms.ColumnHeader ImgYRealMove;
        private System.Windows.Forms.ColumnHeader Angle;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button Export_Excel;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button RemoveExl;
        private System.Windows.Forms.Button LocalRecordToExl;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader Num;
        private System.Windows.Forms.ColumnHeader LocalX;
        private System.Windows.Forms.ColumnHeader LocalY;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button SetParam;
        private System.Windows.Forms.TextBox PositionLocal;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox DownLocal;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox UpLocal;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button RevertToOrigin;
        private System.Windows.Forms.Button MoveToPoint;
        private System.Windows.Forms.Button CalMoveDistance;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button LocateHeight;
        private System.Windows.Forms.Button ZAxisDown;
        private System.Windows.Forms.Button ZAxisUp;
        private System.Windows.Forms.CheckBox AssignPositionMove;
        private System.Windows.Forms.Button ObDetected;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_ImageLoad;
        private System.Windows.Forms.Button btn_AnalysisImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Emgu.CV.UI.ImageBox cannyImageBox;
        private System.Windows.Forms.Button btn_Close;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button btn_Move;
        private System.Windows.Forms.Button btn_Get_Point;
        private System.Windows.Forms.Button btn_Re_Record;
        private System.Windows.Forms.Button btn_Record_Points_Mechanical_Arm;
        private System.Windows.Forms.Button btn_Record_Points_Image;
        private System.Windows.Forms.Button btn_Compute_Homography;
        private System.Windows.Forms.Button btn_Click_Move;
        private System.Windows.Forms.Label label_Number_Points;
        private Emgu.CV.UI.ImageBox imageBox_Point;
    }
}


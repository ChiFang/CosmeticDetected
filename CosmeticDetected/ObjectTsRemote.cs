using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//for Thread
using System.Threading;

//for TsRemote
using TsRemoteLib;

//for FILE
using System.IO;

namespace ToshibaTools
{
    public class DPoint : TsPointS
    {
        public double r = 0, g = 0, b = 0;
        public DPoint() { ; }
        public DPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static double ComputeDistance(TsPointS PA, TsPointS PB)
        {
            return Math.Sqrt(Math.Pow(PA.X - PB.X, 2) + Math.Pow(PA.Y - PB.Y, 2));
        }

        public static double ComputeDefaultAngle(TsPointS PA, TsPointS PB)
        {
            return ComputeDefaultAngle(PA, PB, -1);
        }

        //將方向指向(0, 0), 計算與Y軸的夾角
        public static double ComputeDefaultAngle(TsPointS start, TsPointS center, double radio)
        {
            if (radio <= 0)
            {
                radio = ComputeDistance(start, center);
            }
            double default_angle = (center.X - start.X) / radio;
            if (default_angle > 1) default_angle = 1.0;
            else if (default_angle < -1) default_angle = -1.0;
            return Arc2Angle(Math.Asin(default_angle));
        }

        public static double Angle2Arc(double angle)
        {
            return angle * Math.PI / 180;
        }

        public static double Arc2Angle(double arc)
        {
            return arc * 180 / Math.PI;
        }
    }

    class ObjectTsRemote
    {
        #region Field
        //收到資料後呼叫
        public delegate void CallbackReceiveStatusEvent(TsStatusMonitor para);
        public event CallbackReceiveStatusEvent _statusback = null;

        public static class ALARMLEVEL
        {
            public static int NoAlarm = 0;
            public static int MessageOnly = 1;
            //Stops when the operation that is currently performed reaches the target point
            public static int StopWhenFinished = 2;
            public static int StopsImmediately = 8;
        }

        //動作流程:
        //a. 連線->ServoON: 可取得狀態
        //b. 連線->ServoON->WatchDog: 可控制
        public static class CONNECTTYPE
        {
            public static int INI = 0x00;
            public static int CONNECTED = 0x01;
            public static int SERVOON = 0x02;
            public static int WATCHDOG = 0x04;
        }
        #endregion

        public int ConnectStatus = CONNECTTYPE.INI;
        //P.4 Overview
        //T為外部軸，目前沒有接
        //Move?會等待執行完畢
        //DirectDo不會等待執行完畢
        public TsRemoteS _Robot = new TsRemoteS();

        #region TsRemote Actions
        public bool startControl_Network(string IPAddress, int IPPort, int interval, int alarmLevel)
        {
            if (doConncet_Network(IPAddress, IPPort) == false) return false;
            if (setServoOn() == false) return false;
            if (startWatchDog(interval, alarmLevel) == false) return false;
            return true;
        }

        public bool startControl()
        {
            if (setServoOn() == false) return false;
            return true;
        }

        public void stopControl()
        {
            stopWatchDog();
            setServoOff();
            disConnect();
        }

        public bool doConncet_Network(string IPAddress, int IPPort)
        {
            //已經開啟，直接回傳
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) > 0) return true;
            try
            {
                //P.88
                //openmode: 0=Server(not installed); 1=Client
                //destAddr: 目標IPv4位置
                //destPort: 目標Port
                if (_Robot.SetIPaddr(0, IPAddress, IPPort) == false)
                {
                    //Return value: true=Successful; false=Failed
                    return false;
                }

                //P.31
                //int type Specify a connection type. Connection under RS-232C(COM) is not supported.
                //0:RS-232C(function not yet installed)
                //1:TCP/IP
                //Return value: true:Connect successful; false:Connect failed
                if (_Robot.Connect(1) == true)
                {
                    ConnectStatus |= CONNECTTYPE.CONNECTED;

                    checkServo();
                    return true;
                }
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("doConncet_Network: " + ex.Message);
            }
            return false;
        }

        public bool disConnect()
        {
            //已經關閉，直接回傳
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return true;
            try
            {
                //P.34
                if (_Robot.Disconnect() == true)
                {
                    ConnectStatus &= ~CONNECTTYPE.CONNECTED;
                    return true;
                }
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("disConnect: " + ex.Message);
            }
            return false;
        }

        public bool checkServo()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return false;

            try
            {
                TsStatusAllS status = _Robot.GetStatusAll();
                if (status.ServoStatus == 1) ConnectStatus |= CONNECTTYPE.SERVOON;
                else ConnectStatus &= ~CONNECTTYPE.SERVOON;
                return true;
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("checkServo: " + ex.Message);
            }
            return false;
        }

        public bool setServoOn()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return false;
            //已經開啟，直接回傳
            if ((ConnectStatus & CONNECTTYPE.SERVOON) > 0) return true;
            try
            {
                //P.78 Sets the servo to ON.
                _Robot.ServoOn();
                TsStatusAllS status = _Robot.GetStatusAll();
                //仍為關閉
                if (status.ServoStatus == 0) return false;
                ConnectStatus |= CONNECTTYPE.SERVOON;
                return true;
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("setServoOn: " + ex.Message);
            }
            return false;
        }

        public bool setServoOff()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return false;
            //已經關閉，直接回傳
            if ((ConnectStatus & CONNECTTYPE.SERVOON) == 0) return true;
            try
            {
                _Robot.ServoOff();
                TsStatusAllS status = _Robot.GetStatusAll();
                //仍為開啟
                if (status.ServoStatus == 1) return false;
                ConnectStatus &= ~CONNECTTYPE.SERVOON;
                return true;
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("setServoOff: " + ex.Message);
            }
            return false;
        }
        
        //必須開啟看門狗，才能執行動作
        //interval: Communication interval of mutual monitoring (100 to 1000msec)
        //alarmLevel: 
        //  0=No alarm
        //  1=1-200 Host Port Time Out
        //      Message only (The robot does not stop)
        //  2=2-137 Host Port Time Out
        //      Stops when the operation that is currently performed reaches the target point.
        //  8=8-352 Host Port Time Out
        //      Stops immediately.
        public bool startWatchDog(int interval, int alarmLevel)
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return false;
            //已經開啟，直接回傳
            if ((ConnectStatus & CONNECTTYPE.WATCHDOG) > 0) return true;
            try
            {
                //P.94
                _Robot.WatchDogStart(interval, 0, alarmLevel, new TsRemoteS.TSStatusEvent(TSStatusEvent));
                //Return value: None
                ConnectStatus |= CONNECTTYPE.WATCHDOG;
                return true;
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("startWatchDog: " + ex.Message);
            }
            return false;
        }

        public bool stopWatchDog()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return false;
            //已經關閉，直接回傳
            if ((ConnectStatus & CONNECTTYPE.WATCHDOG) == 0) return true;
            try
            {
                _Robot.WatchDogStop();
                //Return value: None
                ConnectStatus &= ~CONNECTTYPE.WATCHDOG;
                return true;
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("stopWatchDog: " + ex.Message);
            }
            return false;
        }

        public void TSStatusEvent(TsStatusMonitor para)
        {
            //Evaluate the value of TsStatusMonitor and perform event processing.
            if (para.EmergencyStop == 1)
            {
                //Processing when the Emergency Stop button is pressed.
                Console.WriteLine("EmergencyStop");
            }
            else if (para.SafetyStop == 1)
            {
                //Processing when the safety switch is pressed.
                Console.WriteLine("SafetyStop");
            }
            else
            {
                //Processing when the safety switch is pressed.
            }

            checkServo();

            if (_statusback != null) _statusback(para);
        }

        public void DirectDo(string command)
        {
            DirectDo(command, null, false);
        }

        public void DirectDo(string command, TsPointS stopPoint)
        {
            DirectDo(command, stopPoint, true);
        }

        double maxangle = -1;
        public void DirectDo(string command, TsPointS stopPoint, bool isWait)
        {
            _Robot.DirectDo(command);
            if (isWait == false || stopPoint == null) return;
            Thread.Sleep(100);

            TsPointS point_bak = null;
            int MoveStopCnt = 0;
            double bakAngle = 999;
            while ((ConnectStatus & CONNECTTYPE.SERVOON) > 0)
            {
                TsPointS point = _Robot.GetPsnFbkWork();
                if ((Math.Abs(point.X - stopPoint.X) < 0.01) &&
                    (Math.Abs(point.Y - stopPoint.Y) < 0.01) &&
                    (Math.Abs(point.Z - stopPoint.Z) < 0.01) /*&&
                    (Math.Abs(point.C - stopPoint.C) < 0.01) &&
                    (Math.Abs(point.T - stopPoint.T) < 0.01)*/
                                                              ) break;
                //計算角度，中心為(500, 0)
                double angle = (500 - point.X) / 200.0;
                if (angle > 1) angle = 1.0;
                else if (angle < -1) angle = -1.0;
                angle = DPoint.Arc2Angle(Math.Asin(angle))*2;
                //超過一半
                if ((point.Y - 0) < 0) angle = 180 + (180 - angle);
                if (bakAngle == 999) bakAngle = angle;
                if (maxangle < angle) maxangle = angle;
                //Console.WriteLine(DateTime.Now.TimeOfDay + ": " + point.X.ToString("000.000") + ", " + point.Y.ToString("000.000") + ", " + point.Z.ToString("000.000") + ", " + point.C.ToString("000.000") + ", " + angle.ToString("000.000") + ", " + (angle - bakAngle).ToString("000.000"));
                bakAngle = angle;
                if (point_bak == null)
                {
                    point_bak = point;
                    continue;
                }

                //幾乎沒有動作，則表示機械手停止
                if ((Math.Abs(point.X - point_bak.X) < 0.0001) &&
                    (Math.Abs(point.Y - point_bak.Y) < 0.0001) &&
                    (Math.Abs(point.Z - point_bak.Z) < 0.0001) /*&&
                    (Math.Abs(point.C - point_bak.C) < 0.0001) &&
                    (Math.Abs(point.T - point_bak.T) < 0.0001)*/)
                {
                    MoveStopCnt++;
                    if (MoveStopCnt > 10)
                    {
                        Console.WriteLine("機器手強制停止");
                        break;
                    }
                }
                //else MoveStopCnt = 0;
                point_bak = point;
                //Thread.Sleep(50);
            }
            Thread.Sleep(1000);//等待穩定
        }
        #endregion

        #region TsRemote Reset
        public void ResetAll()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return;
            try
            {
                _Robot.ResetALARM();
                _Robot.ResetMove();
                _Robot.ResetCYCLE();
                _Robot.ResetPRG();
                _Robot.ResetSTEP();
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("ResetAll: " + ex.Message);
            }
        }
        public void ResetALARM()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return;
            try
            {
                _Robot.ResetALARM();
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("resetAlarm: " + ex.Message);
            }
        }
        public void ResetMove()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return;
            try
            {
                _Robot.ResetMove();
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("ResetMove: " + ex.Message);
            }
        }
        #endregion

        //SetGlobal?: 設定全域變數
        public string getStatusAll()
        {
            if ((ConnectStatus & CONNECTTYPE.CONNECTED) == 0) return "";
            try
            {
                //P.55 Acquires all the statuses of the robot.
                TsStatusAllS status = _Robot.GetStatusAll();
                //int JogCoordnate JOG-guided coordinate selection status
                //  0=Joint; 1=Tool; 2=Work; 3=World
                //Console.Write(status.JogCoordnate + ", ");

                //int MasterMode Master mode status
                //  0=Teaching; 1=Internal; 2=Ext.Sig; 3=Ext.Host(Ext.232C/ExtEther)
                //Console.Write(status.MasterMode + ", ");

                //int ServoStatus Servo status: 0=OFF; 1=ON
                //Console.Write(status.ServoStatus + ", ");

                //int EmergencySwitch Emergency Stop switch status: 0=OFF; 1=ON
                //Console.Write(status.EmergencySwitch + ", ");

                //int JogCoordnate JOG-guided coordinate selection status: 0=Joint; 1=Tool; 2=Work; 3=World
                //Console.Write(status.JogCoordnate + ", ");

                string messages = "";
                if (status.ServoStatus == 0) messages += "ServoStatus=OFF";
                else messages += "ServoStatus=ON";
                if (status.EmergencySwitch == 0) messages += "\nEmergencySwitch=OFF";
                else messages += "\nEmergencySwitch=ON";
                if (status.MasterMode == 0) messages += "\nMasterMode=Teaching";
                else if (status.MasterMode == 1) messages += "\nMasterMode=Internal";
                else if (status.MasterMode == 2) messages += "\nMasterMode=Ext.Sig";
                else if (status.MasterMode == 3) messages += "\nMasterMode=Ext.Host(Ext.232C/ExtEther)";
                else messages += "\nMasterMode=?";
                if (status.JogCoordnate == 0) messages += "\nJogCoordnate=Joint";
                else if (status.JogCoordnate == 1) messages += "\nJogCoordnate=Tool";
                else if (status.JogCoordnate == 2) messages += "\nJogCoordnate=Work";
                else if (status.JogCoordnate == 3) messages += "\nJogCoordnate=World";
                else messages += "\nJogCoordnate=?";
                
                //整體速度限制
                TsStatusMonitor status_m = _Robot.GetStatusMonitor();
                messages += "\nOverride=" + status_m.Override;

                return messages;
            }
            catch (TsRemoteSException ex)
            {
                //Error processing
                Console.WriteLine("getStatusAll: " + ex.Message);
            }
            return "";
        }
    }
}

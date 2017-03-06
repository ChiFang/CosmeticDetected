using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
//using AlgorithmTool;

namespace CosmeticDetected
{
    public class Ini
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section,
        string key, string def, StringBuilder retVal,
        int size, string filePath);

        public Point[] Record_Points_Image_All;
        public int Record_Points_Image_All_length;
        public Point[] Record_Points_Mechanical_Arm_All;
        public int Record_Points_Mechanical_Arm_All_length;

        /** \brief Define: file name1   */
        public const string filename_Cfg1 = "Record_Points_Image.ini";

        public const string filename_Cfg2 = "Record_Points_Mechanical_Arm.ini";

        public void Read_ini_Cfg()
        {
            int size = 3000;//temp file source size
            StringBuilder temp = new StringBuilder(size); //temp file source
            
            #region 

            GetPrivateProfileString("Record_Points_Image", "Record_Points_All_length", "", temp, size, ".\\" + filename_Cfg1);
            Record_Points_Image_All_length = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("Record_Points_Mechanical_Arm", "Record_Points_Mechanical_Arm_All_length", "", temp, size, ".\\" + filename_Cfg2);
            Record_Points_Mechanical_Arm_All_length = Convert.ToInt32(temp.ToString());

            Record_Points_Image_All = new Point[Record_Points_Image_All_length];
            Record_Points_Mechanical_Arm_All = new Point[Record_Points_Mechanical_Arm_All_length];



            for (int i = 0; i < Record_Points_Image_All_length; i++)
            {
                GetPrivateProfileString("Record_Points_Image", "X" + (i + 1).ToString(), "", temp, size, ".\\" + filename_Cfg1);
                Record_Points_Image_All[i].X = Convert.ToInt32(temp.ToString());
                GetPrivateProfileString("Record_Points_Image", "Y" + (i + 1).ToString(), "", temp, size, ".\\" + filename_Cfg1);
                Record_Points_Image_All[i].Y = Convert.ToInt32(temp.ToString());
            }

            for (int i = 0; i < Record_Points_Mechanical_Arm_All_length; i++)
            {
                GetPrivateProfileString("Record_Points_Mechanical_Arm", "X" + (i + 1).ToString(), "", temp, size, ".\\" + filename_Cfg2);
                Record_Points_Mechanical_Arm_All[i].X = Convert.ToInt32(temp.ToString());
                GetPrivateProfileString("Record_Points_Mechanical_Arm", "Y" + (i + 1).ToString(), "", temp, size, ".\\" + filename_Cfg2);
                Record_Points_Mechanical_Arm_All[i].Y = Convert.ToInt32(temp.ToString());
            }
            #endregion
        }

        public void Write_ini_Cfg()
        {
            WritePrivateProfileString("Record_Points_Image", "Record_Points_All_length", Record_Points_Image_All_length.ToString(), ".\\" + filename_Cfg1);
            for (int i = 0; i < Record_Points_Image_All_length; i++)
            {
                WritePrivateProfileString("Record_Points_Image", "X" + (i + 1).ToString(), Record_Points_Image_All[i].X.ToString(), ".\\" + filename_Cfg1);
                WritePrivateProfileString("Record_Points_Image", "Y" + (i + 1).ToString(), Record_Points_Image_All[i].Y.ToString(), ".\\" + filename_Cfg1);
            }

            WritePrivateProfileString("Record_Points_Mechanical_Arm", "Record_Points_Mechanical_Arm_All_length", Record_Points_Mechanical_Arm_All_length.ToString(), ".\\" + filename_Cfg2);
            for (int i = 0; i < Record_Points_Mechanical_Arm_All_length; i++)
            {
                WritePrivateProfileString("Record_Points_Mechanical_Arm", "X" + (i + 1).ToString(), Record_Points_Mechanical_Arm_All[i].X.ToString(), ".\\" + filename_Cfg2);
                WritePrivateProfileString("Record_Points_Mechanical_Arm", "Y" + (i + 1).ToString(), Record_Points_Mechanical_Arm_All[i].Y.ToString(), ".\\" + filename_Cfg2);
            }
        }
    }
}

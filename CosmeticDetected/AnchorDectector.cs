using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;

namespace CosmeticDetected
{
    class AnchorDectector
    {
        static VectorOfVectorOfPoint Contours =new VectorOfVectorOfPoint();
        VectorOfPoint StampContour;
        int Contour_select_index = 0;

        //*********沒有任何作用****************
        static Mat hierachy = new Mat();
        static Point offset = new Point();
        //*************************************

        //儲存物件輪廓的座標點
        static Point[][] PointCon;
        static PointF[][] PointtConF;

        //物件重心繪製十字設定
        Cross2DF DrawCross = new Cross2DF();
        public static SizeF CrossSize = new SizeF(20F, 20F);

        //偵測定位點時，偵測的最小面積設定
        const short small_area = 1500;

        //紀錄定位點形狀的最小矩形的長寬
        static float Anchor_point_width;
        static float Anchor_point_height;

        //紀錄最小矩形長寬的容忍範圍
        static float[] Anchor_width_range;
        static float[] Anchor_height_range;

        public bool DetectAnchorPoint(Image<Gray, Byte> Gray_Img, out PointF Gravity)
        {
            PointF Center = new PointF();
            bool IsDetectrd = false;

            if (DetectObject(Gray_Img) == false)
            {
                Gravity = new PointF();
                return IsDetectrd;
            }


            for (int i = 0; i < Contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(Contours[i], false);
                if (area > small_area)
                {
                    
                    RotatedRect RotRect_pot = CvInvoke.MinAreaRect(PointtConF[i]);

                    float LengthSide = RotRect_pot.Size.Width;
                    float ShortSide = RotRect_pot.Size.Height;
                    //矩行的長與短
                    if (RotRect_pot.Size.Width < RotRect_pot.Size.Height)
                    {
                        LengthSide = RotRect_pot.Size.Height;
                        ShortSide = RotRect_pot.Size.Width;
                    }
                    if (Anchor_width_range[0] < LengthSide && LengthSide < Anchor_width_range[1] && Anchor_height_range[0] < ShortSide && ShortSide < Anchor_height_range[1])
                    {
                        //Contour_select_index = i;
                        
                        IsDetectrd = true;
                        StampContour = new VectorOfPoint(Contours[i].ToArray());
                        Center = Calculate_Gravity(StampContour);
                    }
                }
            }
            Gravity = Center;
            return IsDetectrd;
        }

        public bool SelectAnchorPoint(Image<Gray, Byte> Gray_Img, out Image<Bgr, Byte> ROI_color)
        {
            ROI_color = Gray_Img.Convert<Bgr, Byte>();

            if (DetectObject(Gray_Img) == false)
                return false;


            for (int i = 0; i < Contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(Contours[i], false);
                if (area > small_area)
                {
                    Contour_select_index = i;
                    RotatedRect RotRect_pot = CvInvoke.MinAreaRect(PointtConF[i]);

                    if (RotRect_pot.Size.Width > RotRect_pot.Size.Height)
                    {
                        Anchor_point_width = RotRect_pot.Size.Width;
                        Anchor_point_height = RotRect_pot.Size.Height;
                    }
                    else
                    {
                        Anchor_point_width = RotRect_pot.Size.Height;
                        Anchor_point_height = RotRect_pot.Size.Width;
                    }
                    
                    Calculate_Rect_Range();
                    CvInvoke.DrawContours(ROI_color, Contours, i, new MCvScalar(0, 0, 255), 2, LineType.EightConnected, hierachy, 2147483647, offset);
                    
                    DrawCross.Center = RotRect_pot.Center;
                }
            }
            StampContour = new VectorOfPoint(Contours[Contour_select_index].ToArray());
            
            DrawCross.Size = CrossSize;
            ROI_color.Draw(DrawCross, new Bgr(0, 255, 0), 2);

            
            return true;
        }

        private static bool DetectObject(Image<Gray, Byte> Gray_Img)
        {
            Gray_Img.SmoothMedian(3);
            Gray_Img = Gray_Img.Canny(250, 255);
            //CvInvoke.Threshold(Gray_Img, Gray_Img, 30, 255, ThresholdType.Binary);

            CvInvoke.FindContours(Gray_Img, Contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxNone, offset);

            //沒有偵測到任何物件則返回
            if (Contours.Size == 0)
                return false;

            PointCon = Contours.ToArrayOfArray();
            PointtConF = Array.ConvertAll<Point[], PointF[]>(PointCon, new Converter<Point[], PointF[]>(PonitToPointF));

            return true;
        
        }

        private static void Calculate_Rect_Range()
        {
            Anchor_width_range =new float [2] {Anchor_point_width - Anchor_point_width * 0.08F , Anchor_point_width + Anchor_point_width * 0.08F};
            Anchor_height_range = new float [2] {Anchor_point_height - Anchor_point_height * 0.08F, Anchor_point_height + Anchor_point_height * 0.08F};
        }

        public static PointF[] PonitToPointF(Point[] pot)
        {
            PointF[] PotF = new PointF[pot.Length];
            int num = 0;
            foreach (var point in pot)
            {
                PotF[num].X = (float)point.X;
                PotF[num].Y = (float)point.Y;
                num++;
            }

            return PotF;
        }

        public static PointF PonitToPointF(Point pot)
        {
            PointF PotF = new PointF();
            int num = 0;

            PotF.X = (float)pot.X;
            PotF.Y = (float)pot.Y;
            num++;


            return PotF;
        }

        public static PointF Calculate_Gravity(VectorOfPoint contour_point)
        {
            PointF Gravity = new PointF();

            for (int j = 0; j < contour_point.Size; j++)
            {
                Gravity.X += contour_point[j].X;
                Gravity.Y += contour_point[j].Y;
            }

            Gravity.X = Gravity.X / contour_point.Size;
            Gravity.Y = Gravity.Y / contour_point.Size;

            return Gravity;
        }




    }
}

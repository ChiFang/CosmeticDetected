using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using System.Runtime.InteropServices;


namespace CoordinatesConvert
{
    class Utilities
    {
        /* Picture to image coordinates transformation
             * 輸入 picture coordinates (xp, yp)
             * 輸出 image coordinates (xi, yi)
             */

        public static void ConvertCoordinates(ImageBox pic, Image<Gray,Byte> Img, out int xi, out int yi, int xp, int yp)
        {
            int pic_hgt = pic.ClientSize.Height;
            int pic_wid = pic.ClientSize.Width;
            int img_hgt = Img.Height;
            int img_wid = Img.Width;

            xi = xp;
            yi = yp;
            switch (pic.SizeMode)
            {
                case PictureBoxSizeMode.AutoSize:
                    break;
                case PictureBoxSizeMode.Normal:
                    //限制範圍
                    if (xi > img_wid)
                        xi = img_wid;
                   
                    if (yi > img_hgt)
                        yi = img_hgt;

                    break;
                case PictureBoxSizeMode.CenterImage:
                    xi = xp - (pic_wid - img_wid) / 2;

                    //限制範圍
                    if (xi < 0)
                        xi = 0;
                    else if (xi > img_wid)
                        xi = img_wid;

                    yi = yp - (pic_hgt - img_hgt) / 2;

                    //限制範圍
                    if (yi < 0)
                        yi = 0;
                    else if (yi > img_hgt)
                        yi = img_hgt;

                    break;
                case PictureBoxSizeMode.StretchImage:      
                    // image coordinates(xi, yi),  picture coordinates(xp, yp)
                    xi = (int)(img_wid * xp / (float)pic_wid);
                    yi = (int)(img_hgt * yp / (float)pic_hgt);
                    break;
                case PictureBoxSizeMode.Zoom:
                    float pic_aspect = pic_wid / (float)pic_hgt;
                    float img_aspect = img_wid / (float)img_hgt;
                    if (pic_aspect > img_aspect)
                    {
                        // The PictureBox is wider/shorter than the image.
                        yi = (int)(img_hgt * yp / (float)pic_hgt);

                        // The image fills the height of the PictureBox.
                        // Get its width.
                        float scaled_width = img_wid * pic_hgt / img_hgt;
                        float dx = (pic_wid - scaled_width) / 2;
                        xi = (int)((xp - dx) * img_hgt / (float)pic_hgt);

                        //限制範圍
                        if (xi < 0)
                            xi = 0;
                        else if (xi > img_wid)
                            xi = img_wid;

                        if (yi < 0)
                            yi = 0;
                        else if (yi > img_hgt)
                            yi = img_hgt;
                            
                    }
                    else
                    {
                        // The PictureBox is taller/thinner than the image.
                        xi = (int)(img_wid * xp / (float)pic_wid);

                        // The image fills the height of the PictureBox.
                        // Get its height.
                        float scaled_height = img_hgt * pic_wid / img_wid;
                        float dy = (pic_hgt - scaled_height) / 2;                        
                        yi = (int)((yp - dy) * img_wid / pic_wid);

                        //限制範圍
                        if (yi < 0)
                            yi = 0;
                        else if (yi > img_hgt)
                            yi = img_hgt;

                        if (xi < 0)
                            xi = 0;
                        else if (xi > img_wid)
                            xi = img_wid;
                            
                    }
                    break;
            }
        }

    }
}

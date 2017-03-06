using System;
//using "alMatrix.h"
//using "basedef.h"

/*using math.h
using <stdlib.h>
using <string.h>*/

/** \cond DOXYGEN_EXCLUDE */

namespace CosmeticDetected
{
    class Matrix
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void alMATRIX_Unit(float[] a_peMatrix, int a_lN)
        {
            int i;
            //MemSet(a_peMatrix, a_lN * a_lN * sizeof(float));

            for (i = 0; i < a_lN; i++)
            {
                a_peMatrix[i * (a_lN + 1)] = 1.0f;
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void alMATRIX_Add(float[] a_peMatrix1, float[] a_peMatrix2, int a_lM, int a_lN, float[] a_peMatrix3)
        {
            int i, lElementCnt;
            lElementCnt = a_lM * a_lN;

            for (i = 0; i < lElementCnt; i++)
            {
                a_peMatrix3[i] = a_peMatrix1[i] + a_peMatrix2[i];
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void alMATRIX_Minus(float[] a_peMatrix1, float[] a_peMatrix2, int a_lM, int a_lN, float[] a_peMatrix3)
        {
            int i, lElementCnt;
            lElementCnt = a_lM * a_lN;

            for (i = 0; i < lElementCnt; i++)
            {
                a_peMatrix3[i] = a_peMatrix1[i] - a_peMatrix2[i];
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int alMATRIX_InverseNbyN(float[] a_peMatrix, int n)
        {
            int i, j, k, l, u, v;
            int[] is1, js;
            float d, p;

            is1 = new int[(sizeof(int) * n)]; //is=new INT32 [n];
            js = new int[(sizeof(int) * n)]; //js=new INT32 [n];
            for (k = 0; k <= n - 1; k++)
            {
                d = 0.0f;
                for (i = k; i <= n - 1; i++)
                    for (j = k; j <= n - 1; j++)
                    //{ l=i*n+j; p=fabs(a_peMatrix[l]);
                    {
                        l = i * n + j; p = Math.Abs(a_peMatrix[l]);
                        if (p > d) { d = p; is1[k] = i; js[k] = j; }
                    }
                if (d + 1.0 == 1.0)
                {
                    //free(is1); free(js);
                    return (0);
                }
                if (is1[k] != k)
                    for (j = 0; j <= n - 1; j++)
                    {
                        u = k * n + j; v = is1[k] * n + j;
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
                if (js[k] != k)
                    for (i = 0; i <= n - 1; i++)
                    {
                        u = i * n + k; v = i * n + js[k];
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
                l = k * n + k;
                a_peMatrix[l] = 1.0f / a_peMatrix[l];
                for (j = 0; j <= n - 1; j++)
                    if (j != k)
                    { u = k * n + j; a_peMatrix[u] = a_peMatrix[u] * a_peMatrix[l]; }
                for (i = 0; i <= n - 1; i++)
                    if (i != k)
                        for (j = 0; j <= n - 1; j++)
                            if (j != k)
                            {
                                u = i * n + j;
                                a_peMatrix[u] = a_peMatrix[u] - a_peMatrix[i * n + k] * a_peMatrix[k * n + j];
                            }
                for (i = 0; i <= n - 1; i++)
                    if (i != k)
                    { u = i * n + k; a_peMatrix[u] = -a_peMatrix[u] * a_peMatrix[l]; }
            }
            for (k = n - 1; k >= 0; k--)
            {
                if (js[k] != k)
                    for (j = 0; j <= n - 1; j++)
                    {
                        u = k * n + j; v = js[k] * n + j;
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
                if (is1[k] != k)
                    for (i = 0; i <= n - 1; i++)
                    {
                        u = i * n + k; v = i * n + is1[k];
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
            }

            //free(is1); free(js);
            //delete [] is;
            //delete [] js;
            return (1);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int alMATRIX_Inverse2(float[] a_peMatrix, int a_lN, int[] a_plTemp1, int[] a_plTemp2)
        {
            int i, j, k, l, u, v;
            float d, p;

            for (k = 0; k <= a_lN - 1; k++)
            {
                d = 0.0f;
                for (i = k; i <= a_lN - 1; i++)
                    for (j = k; j <= a_lN - 1; j++)
                    {
                        //l = i*a_lN + j; p = fabs(a_peMatrix[l]);
                        l = i * a_lN + j; p = Math.Abs(a_peMatrix[l]);
                        if (p > d) { d = p; a_plTemp1[k] = i; a_plTemp2[k] = j; }
                    }
                if (d + 1.0 == 1.0)
                {
                    return (0);
                }
                if (a_plTemp1[k] != k)
                    for (j = 0; j <= a_lN - 1; j++)
                    {
                        u = k * a_lN + j; v = a_plTemp1[k] * a_lN + j;
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
                if (a_plTemp2[k] != k)
                    for (i = 0; i <= a_lN - 1; i++)
                    {
                        u = i * a_lN + k; v = i * a_lN + a_plTemp2[k];
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
                l = k * a_lN + k;
                a_peMatrix[l] = 1.0f / a_peMatrix[l];
                for (j = 0; j <= a_lN - 1; j++)
                    if (j != k)
                    {
                        u = k * a_lN + j; a_peMatrix[u] = a_peMatrix[u] * a_peMatrix[l];
                    }
                for (i = 0; i <= a_lN - 1; i++)
                    if (i != k)
                        for (j = 0; j <= a_lN - 1; j++)
                            if (j != k)
                            {
                                u = i * a_lN + j;
                                a_peMatrix[u] = a_peMatrix[u] - a_peMatrix[i * a_lN + k] * a_peMatrix[k * a_lN + j];
                            }
                for (i = 0; i <= a_lN - 1; i++)
                    if (i != k)
                    {
                        u = i * a_lN + k; a_peMatrix[u] = -a_peMatrix[u] * a_peMatrix[l];
                    }
            }
            for (k = a_lN - 1; k >= 0; k--)
            {
                if (a_plTemp2[k] != k)
                    for (j = 0; j <= a_lN - 1; j++)
                    {
                        u = k * a_lN + j; v = a_plTemp2[k] * a_lN + j;
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
                if (a_plTemp1[k] != k)
                    for (i = 0; i <= a_lN - 1; i++)
                    {
                        u = i * a_lN + k; v = i * a_lN + a_plTemp1[k];
                        p = a_peMatrix[u]; a_peMatrix[u] = a_peMatrix[v]; a_peMatrix[v] = p;
                    }
            }

            return (1);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void alMATRIX_Multiplaction(float[] a_peMatrixA, float[] a_peMatrixB, int m, int n, int k, float[] a_peMatrixC)
        {
            int i, j, l, u;
            for (i = 0; i <= m - 1; i++)
                for (j = 0; j <= k - 1; j++)
                {
                    u = i * k + j;
                    a_peMatrixC[u] = 0.0f;
                    for (l = 0; l <= n - 1; l++)
                        a_peMatrixC[u] = a_peMatrixC[u] + a_peMatrixA[i * n + l] * a_peMatrixB[l * k + j];
                }
            return;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void alMATRIX_Transpose(float[] a_peMatrixA, int m, int n, float[] a_peMatrixX)
        {
            //int j;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    a_peMatrixX[j * m + i] = a_peMatrixA[i * n + j];
            }
        }
    }


}
/** \endcond DOXYGEN_EXCLUDE */

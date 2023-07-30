using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace ComplexExplorer
{
    public struct Pixel
    {
        public int I { get; set; }
        public int J { get; set; }
        public Pixel(int ii, int jj) : this()
        {
            I = ii;
            J = jj;
        }
    }

    public partial class ComplexForm : Form
    {

        public const int imin = 0, imax = latBmp - 1, jmin = 0, jmax = latBmp - 1;
        private double xmin = 0.0, xmax = 1.0, ymin = 0.0, ymax = 1.0;
        private double dxdi = 1.0 / imax, dydj = 1.0 / jmax, didx = imax, djdy = jmax;


        public void setXminXmaxYminYmax(double xm, double XM, double ym, double YM)
        {
            xmin = xm;
            xmax = (XM != xm ? XM : XM + 1.0e-10);
            ymin = ym;
            ymax = (YM != ym ? YM : YM + 1.0e-10);
            dxdi = (xmax - xmin) / (imax - imin);
            dydj = (ymax - ymin) / (jmax - jmin);
            didx = (imax - imin) / (xmax - xmin);
            djdy = (jmax - jmin) / (ymax - ymin);
        }

        public void setXminXmaxYminYmax(Complex q, double r)
        {
            setXminXmaxYminYmax(q.Re - r, q.Re + r, q.Im - r, q.Im + r);
        }

        public int getI(double x)
        {
            return (int)Math.Round(imin + (x - xmin) * didx);
        }

        public int getJ(double y)
        {
            return (int)Math.Round(jmin + (y - ymin) * djdy);
        }

        public Pixel getP(double x, double y)
        {
            return new Pixel((int)Math.Round(imin + (x - xmin) * didx), (int)Math.Round(jmin + (y - ymin) * djdy));
        }
        public Pixel getP(Complex z)
        {
            return new Pixel((int)Math.Round(imin + (z.Re - xmin) * didx), (int)Math.Round(jmin + (z.Im - ymin) * djdy));
        }

        public double getX(int i)
        {
            return xmin + (i - imin) * dxdi;
        }
        public double getY(int j)
        {
            return ymin + (j - jmin) * dydj;
        }

        public Complex getZ(int i, int j)
        {
            return new Complex(xmin + (i - imin) * dxdi, ymin + (j - jmin) * dydj);
        }
        public Complex getZ(Pixel p)
        {
            return new Complex(xmin + (p.I - imin) * dxdi, ymin + (p.J - jmin) * dydj);
        }

        public Color getColor(int k)     //Atentie: k trebuie sa fie pozitiv 
        {
            return paleta[k % dimPal];
        }

        public void setPixel(int i, int j, Color c)
        {
            if (i < imin || imax < i || j < jmin || jmax < j) return;
            myBmp.SetPixel(i, jmax - j, c);
        }
        public void setPixel(Pixel p, Color c)
        {
            setPixel(p.I, p.J, c);
        }


        public void setPixel(double x, double y, Color c)
        {
            setPixel((int)Math.Round(imin + (x - xmin) * didx), (int)Math.Round(jmin + (y - ymin) * djdy), c);
        }

        public void setPixel(Complex z, Color c)
        {
            setPixel((int)Math.Round(imin + (z.Re - xmin) * didx), (int)Math.Round(jmin + (z.Im - ymin) * djdy), c);

        }

        public void setLine(int i0, int j0, int i1, int j1, Color c)
        {
            int i, j, dir;
            double m;   //panta dreptei
            //linie verticala:
            if (i0 == i1)
            {
                if (j0 <= j1)
                {
                    for (j = j0; j <= j1; j++)
                        setPixel(i0, j, c);
                }
                else
                {
                    for (j = j1; j <= j0; j++)
                        setPixel(i0, j, c);
                }
                return;
            }

            //linie orizontala sau oblica:          

            m = (double)(j1 - j0) / (double)(i1 - i0);
            if (-1 <= m && m <= 1)
            {
                dir = (i0 < i1 ? +1 : -1);
                i = i0;
                while (i != i1)
                {
                    setPixel(i, (int)Math.Round(j0 + m * (i - i0)), c);
                    i += dir;
                }
            }
            else     //m<-1 || m>1
            {
                dir = (j0 < j1 ? +1 : -1);
                j = j0;
                while (j != j1)
                {
                    setPixel((int)Math.Round(i0 + (j - j0) / m), j, c);
                    j += dir;
                }
            }
            setPixel(i1, j1, c);
            return;
        }
        public void setLine(Pixel p0, Pixel p1, Color c)
        {
            setLine(p0.I, p0.J, p1.I, p1.J, c);
        }

        public void setLine(double x0, double y0, double x1, double y1, Color c)
        {
            setLine((int)Math.Round(imin + (x0 - xmin) * didx), (int)Math.Round(jmin + (y0 - ymin) * djdy),
                (int)Math.Round(imin + (x1 - xmin) * didx), (int)Math.Round(jmin + (y1 - ymin) * djdy), c);
        }

        public void setLine(Complex z0, Complex z1, Color c)
        {
            setLine((int)Math.Round(imin + (z0.Re - xmin) * didx), (int)Math.Round(jmin + (z0.Im - ymin) * djdy),
                (int)Math.Round(imin + (z1.Re - xmin) * didx), (int)Math.Round(jmin + (z1.Im - ymin) * djdy), c);
        }
        public void setAxis()
        {
            setLine(xmin, 0.0, xmax, 0.0, PenColor);
            setLine(0.0, ymin, 0.0, ymax, PenColor);
        }
    }
}

using System;
using System.Drawing;

/*Resurse*/
//Centru cerc circumscris: https://en.wikipedia.org/wiki/Circumscribed_circle#Cartesian_coordinates_2

namespace ComplexExplorer
{
    public class Problema4Monede : ComplexForm
    {
        Complex[] Ct = new Complex[4];
        double R;

        Complex[] FiguraInitiala(double r) //un cerc care trece prin origine
        {
            int nrPuncte = 5000;
            Complex[] rez = new Complex[nrPuncte + 1];
            double delta = 2 * Math.PI / nrPuncte;

            for (int k = 0; k <= nrPuncte; k++)
            {
                rez[k] = Complex.setRoTheta(r, k * delta);
            }
            return rez;
        }

        public void drawCircle(Complex center, double radius, Color color)
        {
            Complex[] fig = FiguraInitiala(radius);
            for (int k = 0; k < fig.Length; k++)
            {
                setPixel(fig[k] + center, color);
            }
        }

        public override void makeImage()
        {
            /*EXEMPLE*/
            //Ct[0] = Complex.setReIm(0, 3); //3i
            //Ct[1] = Ct[0] * Complex.setRoTheta(1, 2 * Math.PI / 3); //-2.6-1.5i
            //Ct[2] = Ct[1] * Complex.setRoTheta(1, 2 * Math.PI / 3); //2.6-1.5i
            //R = 3;

            //Ct[0] = Complex.setReIm(2, Math.Sqrt(5)); //2+2.23i
            //Ct[1] = Complex.setReIm(-3, 0); //-3
            //Ct[2] = Complex.setReIm(1, -Math.Sqrt(8)); //1-2.82i
            //R = 3;

            //Ct[0] = Complex.setReIm(-Math.Sqrt(7), 3); //-2.64+3i
            //Ct[1] = Complex.setReIm(4, 0); //4
            //Ct[2] = Complex.setReIm(-2, -Math.Sqrt(12)); //-2-3.46i
            //R = 4;

            Ct[0] = Complex.setReIm(input1, input2);
            Ct[1] = Complex.setReIm(input3, input4);
            Ct[2] = Complex.setReIm(input5, input6);
            R = Raza;

            double lat = 10;
            setXminXmaxYminYmax(-lat, lat, -lat, lat);
            setAxis();
            Color col = Color.White;
            Color colInt = Color.Orange;
            Color colO = Color.Red;

            for (int i = 0; i < 3; i++)
            {
                drawCircle(Ct[i], R, col);
            }

            Complex A = Ct[0] + Ct[1], B = Ct[0] + Ct[2], C = Ct[1] + Ct[2];
            drawCircle(A, R / 20, colInt);
            drawCircle(B, R / 20, colInt);
            drawCircle(C, R / 20, colInt);

            //Centrul cercului circumscris triunghiului ABC: 
            double d = 2 * (A.Re * (B.Im - C.Im) + B.Re * (C.Im - A.Im) + C.Re * (A.Im - B.Im));
            double XCenterABC = ((A.Re * A.Re + A.Im * A.Im) * (B.Im - C.Im) + (B.Re * B.Re + B.Im * B.Im) * (C.Im - A.Im) + (C.Re * C.Re + C.Im * C.Im) * (A.Im - B.Im)) / d;
            double YCenterABC = ((A.Re * A.Re + A.Im * A.Im) * (C.Re - B.Re) + (B.Re * B.Re + B.Im * B.Im) * (A.Re - C.Re) + (C.Re * C.Re + C.Im * C.Im) * (B.Re - A.Re)) / d;

            drawCircle(Complex.setReIm(XCenterABC, YCenterABC), R, colO);

            setAxis();
            resetScreen();
        }
    }
}

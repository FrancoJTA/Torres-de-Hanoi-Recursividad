using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Recursividad
{
    internal class Ejercicios
    {
        public bool capicuabool(int n, int ax,int on) {
            if (ax > 0) return capicuabool(n, ax/10, on * 10 + ax % 10);
            else return n == on?true:false;
        }
        public int sumvec(List<int> v,int i) {
            if (i > 0) return sumvec(v, i - 1) + v[i];
            else return v[i];
        }
        public int multvec(List<int> v, int i)
        {
            if (i > 0) return multvec(v, i - 1) * v[i];
            else return v[i];
        }
        public int mintvec(List<int> v, int i)
        {
            if (i > 0) return Math.Min(mintvec(v, i - 1) , v[i]);
            else return v[i];
        }
        public int maxtvec(List<int> v, int i)
        {
            if (i > 0) return Math.Max(maxtvec(v, i - 1), v[i]);
            else return v[i];
        }
        public double fact(int n)
        {
            if (n>1) return n*fact(n-1);
            else return 1;
        }
        public int fib(int n) { 
            if(n==0) return 0;
            if (n==-1) return 1;
            return fib(n-1)+fib(n-2);
        }
        public int invertir(int ax,int on)
        {
            if (ax > 0) return invertir(ax / 10, on * 10 + ax % 10);
            else return on;
        }

        public int sumdig(int ax)
        {
            if (ax > 0) return ax % 10 + sumdig(ax/10);
            else return 0;
        }
        public int sumatoria(int n) { 
            if(n>1) return n+sumatoria(n-1);
            else return 1;
        }
        public bool poi(double n) { 
            return n%2==0 ? true : false;
        }
        public bool posionega(double n)
        {
            return n >= 0 ? true : false;
        }
    }
}

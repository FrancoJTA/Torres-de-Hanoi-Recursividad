using System;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace Recursividad
{

    public partial class MainWindow : Window
    {
        List<int> vvv = new List<int>(); 
        Ejercicios exe=new Ejercicios();
        bool algo=false;
        Canvas origen;
        Canvas destino;
        Canvas auxiliar;
        double speed=0.05;
        bool an=false;
        private int nP=0;
        private Brush custcolor;
        private int W = 250;
        private int H=25;
        private int contM = 0;
        public Dictionary<Canvas, int> TowerPos=new Dictionary<Canvas, int>();
        public Dictionary<int, Rectangle> Plates = new Dictionary<int, Rectangle>();
        public MainWindow()
        {
            InitializeComponent();
        }
        public void estab() {
            int a=0, b = 0, c=0;
            if (og1.IsChecked==true) {
                a = 1;
                b = 2;
                c = 3;
            }
            if (og2.IsChecked == true)
            {
                a = 2;
                b = 1;
                c = 3;
            }
            if (og3.IsChecked == true)
            {
                a = 3;
                b = 1;
                c = 2;
            }
            if (op2.IsChecked == true)
            {
                b = c;
            }
            c = 6 - a - b;
            origen=fin(a);
            destino=fin(b);
            auxiliar=fin(c);
        }
        public Canvas fin(int a) { 
            switch (a)
            {
                case 1:
                    return Torre1;
                case 2:
                    return Torre2;
                default:
                    return Torre3;
            }
        }
        private void btnPlate_Click(object sender, RoutedEventArgs e)
        {
            contM = 0;
            estab();
            Torre1.Children.Clear();
            Torre2.Children.Clear();
            Torre3.Children.Clear();
            TowerPos[Torre1] = 30;
            TowerPos[Torre2] = 30;
            TowerPos[Torre3] = 30;
            Random r = new Random();
            try {
                nP = int.Parse(txbNum.Text);
                if (nP > 0 && nP<14) {
                    algo = true;
                    lblP.Content = $"Numero Platos : {nP}";
                    lblMov.Content = $"Movimientos : {contM}";
                    double k = 0;
                    for (int i = nP; i >= 1; i--)
                    {
                        custcolor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
                        Rectangle rec = new Rectangle
                        {
                            Height = H,
                            Width = W - k,
                            Fill = custcolor,
                        };
                        origen.Children.Add(rec);
                        Canvas.SetLeft(rec, (W - rec.Width) / 2);
                        Canvas.SetBottom(rec, TowerPos[origen]);
                        Plates[i] = rec;
                        TowerPos[origen] += 30;
                        k += 20;
                    }
                }
                else MessageBox.Show("Fuera del Limite");
            }
            catch(Exception ex){
                MessageBox.Show("Invalido");
            }
            txbNum.Text = "";
        }
        public async Task animar(Canvas ori, Canvas dest, int P)
        {
            int a, b;   
            switch (ori.Name) {
                case "Torre1":
                    a = 1;
                    break;
                case "Torre2":
                    a = 2;
                    break;
                default: a = 3;
                    break;
            }
            switch (dest.Name)
            {
                case "Torre1":
                    b = 1;
                    break;
                case "Torre2":
                    b = 2;
                    break;
                default:
                    b = 3;
                    break;
            }
            DoubleAnimation animacionX = new DoubleAnimation
            {
                To =(b-a)*W+25+ (W - Plates[P].Width) / 2,
                Duration = TimeSpan.FromSeconds(speed)
            };

            DoubleAnimation animacionY = new DoubleAnimation
            {
                To = TowerPos[dest],
                Duration = TimeSpan.FromSeconds(speed)
            };
            animacionY.Completed += (s, e) => VolverAPosicionInicial(ori,dest,P);

            Plates[P].BeginAnimation(Canvas.LeftProperty, animacionX);
            Plates[P].BeginAnimation(Canvas.BottomProperty, animacionY);
            an = true;
            while (an)
            {
                await Task.Delay(1);
            }
        }
        public async Task towerOfHanoi(int n, Canvas from_rod,Canvas to_rod, Canvas aux_rod)
        {
            if (n == 0)
            {
                return;
            }
            await towerOfHanoi(n - 1, from_rod, aux_rod, to_rod);
            await animar(from_rod, to_rod, n);
            await towerOfHanoi(n - 1, aux_rod, to_rod, from_rod);
        }
        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try {
                speed = double.Parse(txtspeed.Text);
                speed /= 100;
                btnPlate.IsEnabled = false;
                btnStart.IsEnabled = false;
                if (algo)
                    await towerOfHanoi(nP, origen, destino, auxiliar);
                algo =false;
                btnPlate.IsEnabled = true;
                btnStart.IsEnabled = true;
            }
            catch (Exception ex) {
                MessageBox.Show("Invalido");
            }
        }
        
        private void VolverAPosicionInicial(Canvas ori, Canvas dest,int p)
        {
            Plates[p].BeginAnimation(Canvas.LeftProperty, null);
            Plates[p].BeginAnimation(Canvas.BottomProperty, null);
            ori.Children.Remove(Plates[p]);
            TowerPos[ori] -= 30;
            Canvas.SetLeft(Plates[p], (W - Plates[p].Width) / 2);
            Canvas.SetBottom(Plates[p], TowerPos[dest]);
            dest.Children.Add(Plates[p]);
            TowerPos[dest] += 30;
            contM++;
            lblMov.Content = $"Movimientos : {contM}";
            an = false;
        }

        //copiadisimo
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void OG_check(object sender, RoutedEventArgs e)
        {
            if (op1 == null || op2 == null)
            {
                return;
            }

            RadioButton selectedRadioButton = sender as RadioButton;

            if (selectedRadioButton != null)
            {
                switch (selectedRadioButton.Name)
                {
                    case "og1":
                        op1.Content = "B";
                        op2.Content = "C";
                        break;
                    case "og2":
                        op1.Content = "A";
                        op2.Content = "C";
                        break;
                    case "og3":
                        op1.Content = "A";
                        op2.Content = "B";
                        break;
                }
            }
        }

        private void btnCapBo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                string m = (exe.capicuabool(cosa, cosa, 0) ? "si" : "no")+"";
                String labal = $"El numero {m} es Capicua";
                lblFirst.Content = labal ;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void btnFact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                String t = $"Factorial de {cosa} es {exe.fact(cosa)}";
                lblFirst.Content = t;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void btnFib_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                String t = $"El numero {cosa} de Fibonacci es {exe.fib(cosa)}";
                lblFirst.Content = t;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void btnInv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                String t =$"Inverso de {cosa} es {exe.invertir(cosa,0)}";
                lblFirst.Content = t;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void btnSumaN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                String t = $"Suma hasta {cosa} es {exe.sumatoria(cosa)}";
                lblFirst.Content = t;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void btnPoI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                string m = exe.poi(cosa) ? "par" : "impar";
                String labal = $"El numero es {m}";
                lblFirst.Content = labal;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void btnPoN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                string m = exe.poi(cosa) ? "positivo" : "negativo";
                String labal = $"El numero es {m}";
                lblFirst.Content = labal;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void btnSumDig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtFirst.Text);
                String t = $"Sumda de Digitos de {cosa} es {exe.sumdig(cosa)}";
                lblFirst.Content = t;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cosa = int.Parse(txtSec.Text);
                vvv.Add(cosa);
                lsVec.ItemsSource=null;
                lsVec.ItemsSource=vvv;
            }
            catch
            {
                MessageBox.Show("Invalido");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vvv.Clear();
            lsVec.ItemsSource = null;
            lblSec.Content="";
            lsVec.Items.Clear();
        }

        private void btnVecSum_Click(object sender, RoutedEventArgs e)
        {
            if (vvv.Count > 0) {

                int s = exe.sumvec(vvv, vvv.Count - 1);
                lblSec.Content = $"Suma Total {s}";
            }

        }

        private void btnVecMult_Click(object sender, RoutedEventArgs e)
        {
            if (vvv.Count > 0)
            {

                int m = exe.multvec(vvv, vvv.Count - 1);
                lblSec.Content = $"Producto Total {m}";
            }
        }

        private void btnVecMin_Click(object sender, RoutedEventArgs e)
        {
            if (vvv.Count > 0)
            {

                int s = exe.mintvec(vvv, vvv.Count - 1);
                lblSec.Content = $"Valor Minimo {s}";
            }
        }

        private void btnVecMax_Click(object sender, RoutedEventArgs e)
        {
            if (vvv.Count > 0)
            {

                int s = exe.maxtvec(vvv, vvv.Count - 1);
                lblSec.Content = $"Valor Maximo {s}";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try {
                speed = double.Parse(txtspeed.Text);
                speed /= 100;
            }
            catch (Exception ex) { }
        }
    }
}
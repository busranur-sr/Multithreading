using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Threading;
namespace opsystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "miliseconds";
            label2.Text = "outputs";
            var myoutputs = new BindingList<decimal> { };
            listBox2.DataSource = myoutputs;
            myoutputs.Add(Gauss());
            Console.WriteLine("yes");
            withthreads WithThreads = new withthreads();
            (decimal[] values, decimal[] outputs) = WithThreads.makeintervals();
            plot(values);
            var myItems = new BindingList<decimal> { };
            listBox1.DataSource = myItems;
            foreach (decimal item in values)
            {
                myItems.Add(item);
            }
            
            foreach (decimal item in outputs)
            {
                myoutputs.Add(item);
            }

        }
        void plot(decimal[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                chart1.Series["Series1"].Points.AddXY(i, values[i]);

            }
            chart1.Series["Series1"].ChartType = SeriesChartType.FastLine;
            chart1.Series["Series1"].Color = Color.Red;
            chart1.ChartAreas[0].AxisX.Interval = 1;
        }
        decimal standart()
        {
            decimal result = 0;
            for (decimal i = 0; i <= Convert.ToDecimal(Math.Pow(10, 10)); i++)
            {
                result = result + i;
            }
            return result;
        }

        decimal Gauss()
        {
            decimal result = Convert.ToDecimal(Math.Pow(10, 10));
            result = ((result + 1) * result) / 2;
            return result;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }

    class withthreads
    {
        public decimal max = Convert.ToDecimal(Math.Pow(10, 10));
        public decimal sum = 0;
        public decimal[] results = new decimal[32];
        public decimal[] outputs = new decimal[32];
        public (decimal[] values, decimal[] outputs) makeintervals()
        {
            Stopwatch stopwatch = new Stopwatch();

            for (int i = 1; i < 33; i++)
            {
                Console.WriteLine("Şu anda çalışan thread sayısı" + i);
                stopwatch.Start();
                decimal[] starts = new decimal[i + 1];
                for (int j = 0; j < i; j++)
                {
                    starts[j] = Math.Floor(Convert.ToDecimal(j * (max / i)));
                }
                starts[i] = max;
                List<Thread> listoftthreads = new List<Thread>();
                for (int k = 0; k < i; k++)
                {
                    decimal valuestart = starts[k];
                    decimal valueend = starts[k + 1];

                    Thread thread1 = new Thread(() => createthread(valuestart, valueend));
                    thread1.Start();
                    listoftthreads.Add(thread1);
                }

                foreach (Thread item in listoftthreads)
                {
                    item.Join();
                }
                outputs[i - 1] = sum;
                sum = 0;
                stopwatch.Stop(); 
                results[i - 1] = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
            }

            return (results, outputs);

        }

        void createthread(decimal start, decimal end)
        {
            decimal result = 0;
            for (decimal i = start + 1; i <= end; i++)
            {
                result = result + i;
            }
            sum = sum + result;
        }


    }
}

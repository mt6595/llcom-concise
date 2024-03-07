using Newtonsoft.Json.Linq;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace llcom.Pages
{
    /// <summary>
    /// PlotPage.xaml 的交互逻辑
    /// </summary>
    public partial class PlotPage : Page
    {
        private const int MaxTable = 10;
        private const int MaxPoints = 1000;
        private double[][] DataY = new double[MaxTable][];
        private bool NeedRefresh = true;

        public PlotPage()
        {
            InitializeComponent();
            for (int i = 0; i < DataY.Length; i++)
            {
                if (DataY[i] == null)
                    DataY[i] = new double[MaxPoints];
                Plot.Plot.AddSignal(DataY[i]);
            }
            Plot.Plot.AxisAuto();
            Plot.Plot.Layout(0, 0, 0, 0, -5);

            //定时刷吧，要不然卡
            new Thread(() =>
            {
                while (true)
                {
                    if (NeedRefresh)
                    {
                        NeedRefresh = false;
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            try
                            {
                                Plot.Render();
                            }
                            catch { }
                        }));
                    }
                    Thread.Sleep(10);
                    if (Tools.Global.isMainWindowsClosed)
                        return;
                }
            }).Start();
            LuaEnv.LuaApis.LinePlotAdd += (s, e) => AddPoint(e.N, e.Line);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            for (int iTemp = 0; iTemp < DataY.Length; iTemp++)
                for (int jTemp = 0; jTemp < DataY[iTemp].Length; jTemp++)
                    DataY[iTemp][jTemp] = 0;
            //Plot.Plot.Clear();
            Refresh();
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Plot.Plot.AxisAuto();
            Refresh();
        }
        private void Plot_MouseMove(object sender, MouseEventArgs e)
        {
            Refresh();
        }
        private void Refresh() => NeedRefresh = true;
        private void AddPoint(double d, int line)
        {
            //RemoveAt
            if (line >= 10)
                return;
            Array.Copy(DataY[line], 1, DataY[line], 0, DataY[line].Length - 1);
            DataY[line][MaxPoints - 1] = d;
            Refresh();
        }
    }
}

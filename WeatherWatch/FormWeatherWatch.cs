using System;
using System.Windows.Forms;
using TheSkyXLib;

namespace WeatherWatch
{
    public partial class FormWeatherWatch : Form
    {
        public bool openFlag;

        public FormWeatherWatch()
        {
            InitializeComponent();
            this.Show();
            openFlag = true;

            WeatherReport.Text = "Monitoring CloudWatcher";
            this.Show();
            System.Windows.Forms.Application.DoEvents();

            WeatherFileReader wmon;

            while (openFlag)
            {
                wmon = new WeatherFileReader();
                WeatherReport.Text = wmon.WeatherDataListing();
                for (int i = 0; i < 11; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    System.Windows.Forms.Application.DoEvents();
                    if (!openFlag) { Close(); }
                    if (wmon.AlertFlag == WeatherFileReader.WeaAlert.Alert)
                    {
                        WeatherReport.Text = "Unsafe Weather Detected.  Closing Dome";
                        this.Show();
                        System.Windows.Forms.Application.DoEvents();
                        sky6Dome tsxd = new sky6Dome();
                        if (tsxd.IsConnected == 0)
                        { tsxd.Connect(); }
                        tsxd.CloseSlit();
                        while (tsxd.IsCloseComplete == 0)
                        {
                            tsxd.CloseSlit();
                            System.Threading.Thread.Sleep(10000);
                        }
                        tsxd = null;
                        WeatherReport.Text = "Unsafe Weather Detected.  Dome Closed";
                        this.Show();
                        System.Windows.Forms.Application.DoEvents();
                        break;
                    }
                }
                wmon = null;
            }
            Close();
            return;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            openFlag = false;
            return;
        }
    }
}

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace RefreshRateOptimizer
{
    public partial class MainForm : Form
    {
       
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int CDS_UPDATEREGISTRY = 0x01;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DM_DISPLAYFREQUENCY = 0x400000;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
        }

        private Button btnMaxHz;
        private Button btnReset60;
        private Label lblStatus;

        public MainForm()
        {
           
            this.Text = "Hz Changer";
            this.Size = new Size(320, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblStatus = new Label() { Text = "Status: Ready", Top = 20, Left = 20, Width = 260, TextAlign = ContentAlignment.MiddleCenter };

           
            btnMaxHz = new Button() { Text = "Set Max Refresh Rate", Top = 60, Left = 50, Width = 200, Height = 45, BackColor = Color.LightGreen };
            btnMaxHz.Click += SetMaxHz_Click;

           
            btnReset60 = new Button() { Text = "Reset to 60Hz", Top = 120, Left = 50, Width = 200, Height = 45, BackColor = Color.LightSalmon };
            btnReset60.Click += Reset60Hz_Click;

            this.Controls.Add(lblStatus);
            this.Controls.Add(btnMaxHz);
            this.Controls.Add(btnReset60);
        }

        private void SetMaxHz_Click(object sender, EventArgs e)
        {
            ApplyRefreshRate(true); 
        }

        private void Reset60Hz_Click(object sender, EventArgs e)
        {
            ApplyRefreshRate(false); 
        }

        private void ApplyRefreshRate(bool findMax)
        {
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(dm);

            if (EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm))
            {
                int targetHz = findMax ? GetMaxSupportedHz(dm.dmPelsWidth, dm.dmPelsHeight) : 60;

                if (dm.dmDisplayFrequency == targetHz)
                {
                    lblStatus.Text = $"Already at {targetHz}Hz";
                    return;
                }

                dm.dmDisplayFrequency = targetHz;
                dm.dmFields = DM_DISPLAYFREQUENCY;

                int result = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                if (result == DISP_CHANGE_SUCCESSFUL)
                    lblStatus.Text = $"Success! Set to {targetHz}Hz";
                else
                    lblStatus.Text = "Error: Mode not supported.";
            }
        }

        private int GetMaxSupportedHz(int width, int height)
        {
            DEVMODE tempMode = new DEVMODE();
            tempMode.dmSize = (short)Marshal.SizeOf(tempMode);
            int maxHz = 0;
            int modeNum = 0;

            while (EnumDisplaySettings(null, modeNum, ref tempMode))
            {
                if (tempMode.dmPelsWidth == width && tempMode.dmPelsHeight == height)
                {
                    if (tempMode.dmDisplayFrequency > maxHz)
                        maxHz = tempMode.dmDisplayFrequency;
                }
                modeNum++;
            }
            return maxHz;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}

#region

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ManagedWinapi.Windows;

#endregion

namespace DivvyClone
{
    public partial class FrmMain : Form
    {
        private const int WhKeyboardLl = 13;
        private const int WmKeydown = 0x0100;
        private static readonly LowLevelKeyboardProc Proc = HookCallback;
        private static IntPtr _hookId = IntPtr.Zero;
        private readonly int _heightStep = (SystemInformation.VirtualScreen.Height/6);
        private readonly int _widthStep = (SystemInformation.VirtualScreen.Width/6);

        public FrmMain()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            var windows = SystemWindow.AllToplevelWindows;

            foreach (var window in windows)
            {
                if (window.Title == "Unbenannt - Editor")
                {
                    var leftSteps = GetLeftSteps();
                    var rightSteps = GetRightSteps();
                    var topSteps = GetTopSteps();
                    var bottomSteps = GetBottomSteps();

                    window.Position = GetNewRect(leftSteps, topSteps, rightSteps, bottomSteps);
                    window.Size = GetNewSize(rightSteps, bottomSteps);
                }
            }
        }

        private int GetLeftSteps()
        {
            var checkedBoxes =
                (from CheckBox checkBox in
                     tableLayoutPanel1.Controls.Cast<Control>().Where(
                         control => control.GetType().Equals(typeof (CheckBox)))
                 where checkBox != null
                 where checkBox.Checked
                 select checkBox.Name).ToList();

            var leftStep = 0;

            if (checkedBoxes[0].StartsWith("A"))
            {
                leftStep = 1;
            }
            else if (checkedBoxes[0].StartsWith("B"))
            {
                leftStep = 2;
            }
            else if (checkedBoxes[0].StartsWith("C"))
            {
                leftStep = 3;
            }
            else if (checkedBoxes[0].StartsWith("D"))
            {
                leftStep = 4;
            }
            else if (checkedBoxes[0].StartsWith("E"))
            {
                leftStep = 5;
            }
            else if (checkedBoxes[0].StartsWith("F"))
            {
                leftStep = 6;
            }

            return leftStep - 1;
        }

        private int GetRightSteps()
        {
            var checkedBoxes =
                (from CheckBox checkBox in
                     tableLayoutPanel1.Controls.Cast<Control>().Where(
                         control => control.GetType().Equals(typeof (CheckBox)))
                 where checkBox != null
                 where checkBox.Checked
                 select checkBox.Name).ToList();

            var rightStep = 0;

            if (checkedBoxes.Last().StartsWith("A"))
            {
                rightStep = 1;
            }
            else if (checkedBoxes.Last().StartsWith("B"))
            {
                rightStep = 2;
            }
            else if (checkedBoxes.Last().StartsWith("C"))
            {
                rightStep = 3;
            }
            else if (checkedBoxes.Last().StartsWith("D"))
            {
                rightStep = 4;
            }
            else if (checkedBoxes.Last().StartsWith("E"))
            {
                rightStep = 5;
            }
            else if (checkedBoxes.Last().StartsWith("F"))
            {
                rightStep = 6;
            }

            return rightStep - 1;
        }

        private int GetTopSteps()
        {
            var checkedBoxes =
                (from CheckBox checkBox in
                     tableLayoutPanel1.Controls.Cast<Control>().Where(
                         control => control.GetType().Equals(typeof (CheckBox)))
                 where checkBox != null
                 where checkBox.Checked
                 select checkBox.Name).ToList();

            var topStep = 0;

            if (checkedBoxes.First().EndsWith("1"))
            {
                topStep = 1;
            }
            else if (checkedBoxes.First().EndsWith("2"))
            {
                topStep = 2;
            }
            else if (checkedBoxes.First().EndsWith("3"))
            {
                topStep = 3;
            }
            else if (checkedBoxes.First().EndsWith("4"))
            {
                topStep = 4;
            }
            else if (checkedBoxes.First().EndsWith("5"))
            {
                topStep = 5;
            }
            else if (checkedBoxes.First().EndsWith("6"))
            {
                topStep = 6;
            }

            return topStep - 1;
        }

        private int GetBottomSteps()
        {
            var checkedBoxes =
                (from CheckBox checkBox in
                     tableLayoutPanel1.Controls.Cast<Control>().Where(
                         control => control.GetType().Equals(typeof (CheckBox)))
                 where checkBox != null
                 where checkBox.Checked
                 select checkBox.Name).ToList();

            var bottomStep = 0;

            if (checkedBoxes.Last().EndsWith("1"))
            {
                bottomStep = 1;
            }
            else if (checkedBoxes.Last().EndsWith("2"))
            {
                bottomStep = 2;
            }
            else if (checkedBoxes.Last().EndsWith("3"))
            {
                bottomStep = 3;
            }
            else if (checkedBoxes.Last().EndsWith("4"))
            {
                bottomStep = 4;
            }
            else if (checkedBoxes.Last().EndsWith("5"))
            {
                bottomStep = 5;
            }
            else if (checkedBoxes.Last().EndsWith("6"))
            {
                bottomStep = 6;
            }

            return bottomStep - 1;
        }

        private Size GetNewSize(int rightSteps, int bottomSteps)
        {
            var right = _widthStep*rightSteps;
            var bottom = _heightStep*bottomSteps;

            return new Size(right, bottom);
        }

        private RECT GetNewRect(int leftSteps, int topSteps, int rightSteps, int bottomSteps)
        {
            var left = _widthStep*leftSteps;
            var top = _heightStep*topSteps;
            var right = _widthStep*rightSteps;
            var bottom = _heightStep*bottomSteps;

            return new RECT(left, top, right, bottom);
        }

        private static void FrmMainLoad(object sender, EventArgs e)
        {
            _hookId = SetHook(Proc);


            //foreach(var control in tableLayoutPanel1.Controls.Cast<Control>().Where(control => control.GetType().Equals(typeof (CheckBox))))
            //{
            //    control.MouseMove += ControlMouseMove;
            //}
        }

        //private void ControlMouseMove(object sender, MouseEventArgs e)
        //{
        //    var obj = (Control) sender;

        //    if ((obj != null))
        //    {
        //        Debug.WriteLine("Current Control: " + obj.Name);
        //    }
        //}

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WhKeyboardLl, proc,
                                        GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr) WmKeydown)
            {
                var vkCode = Marshal.ReadInt32(lParam);
                Debug.WriteLine((Keys) vkCode);
            }

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod,
                                                      uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        protected override void OnClosing(CancelEventArgs e)
        {
            UnhookWindowsHookEx(_hookId);
            base.OnClosing(e);
        }

        #region Nested type: LowLevelKeyboardProc

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        #endregion
    }
}
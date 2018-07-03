using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace xUnitForms
{
    public partial class DialogExpectation
    {
        internal class Win32
        {
            #region " Handle "

            private const string DIALOGCLASS = "#32770";
            private const int MAXTEXTLENGTH = 255;
            private const int DIALOGTEXTID = 0xFFFF;

            public static bool IsDialog(IntPtr handler)
            {
                return GetClassName(handler) == DIALOGCLASS;
            }

            public static string GetCaption(IntPtr handler)
            {
                System.Text.StringBuilder windowText = new System.Text.StringBuilder();
                windowText.Capacity = MAXTEXTLENGTH;
                GetWindowText(handler, windowText, windowText.Capacity);
                return windowText.ToString();
            }

            public static string GetClassName(IntPtr handler)
            {
                System.Text.StringBuilder className = new System.Text.StringBuilder();
                className.Capacity = MAXTEXTLENGTH;
                GetClassName(handler, className, className.Capacity);
                return className.ToString();
            }

            public static string GetText(IntPtr handler)
            {
                IntPtr handleToDialogText = Win32.GetDlgItem(handler, DIALOGTEXTID);
                System.Text.StringBuilder buffer = new System.Text.StringBuilder(MAXTEXTLENGTH);
                Win32.GetWindowText(handleToDialogText, buffer, buffer.Capacity);
                return buffer.ToString();
            }

            #endregion

            #region " Win32 "

            public enum WindowMessages : uint
            {
                WM_CLOSE = 0x10,
                WM_COMMAND = 0x111
            }

            public delegate IntPtr CBTCallback(int code, IntPtr wParam, IntPtr lParam);

            public delegate int WindowEnumProc(IntPtr hWnd, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern bool UnhookWindowsHookEx(IntPtr handleToHook);
            [DllImport("user32.dll")]
            public static extern IntPtr CallNextHookEx(IntPtr handleToHook, int nCode, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern IntPtr SetWindowsHookEx(int code, CBTCallback callbackFunction, IntPtr handleToInstance, int threadID);
            [DllImport("kernel32")]
            public static extern int GetCurrentThreadId();
            [DllImport("user32.dll")]
            private static extern int GetClassName(IntPtr handleToWindow, System.Text.StringBuilder className, int maxClassNameLength);
            [DllImport("user32.dll")]
            private static extern int GetWindowText(IntPtr handleToWindow, System.Text.StringBuilder windowText, int maxClassNameLength);
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            private static extern bool EnumChildWindows(IntPtr hWnd, WindowEnumProc func, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr handleToWindow, uint Message, UIntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll")]
            private static extern IntPtr GetDlgItem(IntPtr handleToWindow, int ControlId);

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xUnitForms
{
    public class MessageBoxTester
    {

        public enum Command
        {
            OK = 1,
            Cancel = 2,
            Abort = 3,
            Retry = 4,
            Ignore = 5,
            Yes = 6,
            No = 7,
            Close = 8,
            Help = 9
        }

        private IntPtr m_Hander;

        public MessageBoxTester(IntPtr Handler)
        {
            this.m_Hander = Handler;
        }

        public void Execute(Command cmd)
        {
            DialogExpectation.Win32.SendMessage(this.m_Hander, (uint)DialogExpectation.Win32.WindowMessages.WM_COMMAND, (UIntPtr)cmd, IntPtr.Zero);
        }
    }
}

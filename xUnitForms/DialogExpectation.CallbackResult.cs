using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xUnitForms
{
    public partial class DialogExpectation
    {

        public class CallbackResult
        {

            public enum ExpectedKind
            {
                Dialog = 0,
                MessageBox = 1
            }

            private ExpectedKind m_Kind;
            private FormTester m_FormTester;
            private MessageBoxTester m_MessageBoxTester;
            private int m_InvokedTimes;

            public ExpectedKind Kind
            {
                get { return this.m_Kind; }
            }

            public FormTester FormTester
            {
                get { return this.m_FormTester; }
            }

            public MessageBoxTester MessageBoxTester
            {
                get { return this.m_MessageBoxTester; }
            }

            public int InvokedTimes
            {
                get { return this.m_InvokedTimes; }
            }

            public CallbackResult(FormTester FormTestserInstace, int InvokedTimes)
            {
                this.m_Kind = ExpectedKind.Dialog;
                this.m_FormTester = FormTestserInstace;
                this.m_MessageBoxTester = null;
                this.m_InvokedTimes = InvokedTimes;
            }

            public CallbackResult(MessageBoxTester MessageBoxTestserInstace, int InvokedTimes)
            {
                this.m_Kind = ExpectedKind.MessageBox;
                this.m_FormTester = null;
                this.m_MessageBoxTester = MessageBoxTestserInstace;
                this.m_InvokedTimes = InvokedTimes;
            }

            public CallbackResult(System.Windows.Forms.Form FormInstance, int InvokedTimes)
                : this(new FormTester(FormInstance), InvokedTimes)
            {
            }

            public CallbackResult(IntPtr MessageBoxHandler, int InvokedTimes)
                : this(new MessageBoxTester(MessageBoxHandler), InvokedTimes)
            {
            }
        }

    }
}

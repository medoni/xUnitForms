using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace xUnitForms
{
    public partial class DialogExpectation
    {

        private class HandlerCollection : Dictionary<string, Handler>
        {

            public Handler Add(string Name, Delegate Handler, int ExpectedTimes)
            {
                Handler h = new Handler(Handler, ExpectedTimes, Name);
                this.Add(Name, h);
                return h;
            }
        }

        private class Handler
        {

            private int m_ExpectedTimes;
            private Delegate m_Hander;
            private string m_Name;
            private int m_InvokedTimes;

            public string Name
            {
                get { return this.m_Name; }
            }

            public int ExpectedTimes
            {
                get { return this.m_ExpectedTimes; }
            }

            public int InvokedTimes
            {
                get { return this.m_InvokedTimes; }
            }

            public Handler(Delegate Handler, int ExpectedTimes, string Name)
            {
                this.m_Hander = Handler;
                this.m_ExpectedTimes = ExpectedTimes;
                this.m_Name = Name;
            }

            private CallbackResult CreateCallbackResult(System.Windows.Forms.Form form, IntPtr msgboxHandler)
            {
                CallbackResult ret = null;
                if (form != null)
                {
                    ret = new CallbackResult(form, this.m_InvokedTimes);
                }
                else if (msgboxHandler != IntPtr.Zero)
                {
                    ret = new CallbackResult(msgboxHandler, this.m_InvokedTimes);
                }
                else
                {
                    Debug.Assert(false, "Both form instance and message box handler are null.");
                }
                return ret;
            }

            public void Invoke(System.Windows.Forms.Form form, IntPtr msgboxHandler, IntPtr hWnd)
            {
                // Increase the invoked times.
                this.m_InvokedTimes = this.m_InvokedTimes + 1;
                // Check the expected times and invoked times.
                if (this.m_InvokedTimes > this.m_ExpectedTimes)
                {
                    throw new OverflowException(string.Format("Invoked times ({0}) is more than the expected times ({1}) on this dialog.", this.m_InvokedTimes, this.m_ExpectedTimes));
                }
                else
                {
                    // Create callback result.
                    CallbackResult cbr = this.CreateCallbackResult(form, msgboxHandler);
                    // Dynamic invoke the delegate method after showing dialog.
                    try
                    {
                        if (this.m_Hander is ModalFormActivated)
                        {
                            this.m_Hander.DynamicInvoke(new object[] { cbr });
                        }
                        else if (this.m_Hander is ModalFormActivatedHwnd)
                        {
                            this.m_Hander.DynamicInvoke(new object[] { cbr, hWnd });
                        }
                    }
                    catch (System.Reflection.TargetInvocationException tiex)
                    {
                        if (tiex.InnerException != null)
                        {
                            throw tiex.InnerException;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            public bool Verify()
            {
                bool ret = true;
                if (this.m_ExpectedTimes < int.MaxValue)
                {
                    ret = (this.m_ExpectedTimes == this.m_InvokedTimes);
                }
                return ret;
            }
        }

    }
}

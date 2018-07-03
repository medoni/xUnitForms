using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace xUnitForms
{
    public delegate void ModalFormActivated(DialogExpectation.CallbackResult Result);
    internal delegate void ModalFormActivatedHwnd(DialogExpectation.CallbackResult Result, IntPtr hWnd);

    public partial class DialogExpectation : IDisposable
    {
        private const string UNEXP_DLGNAME = "Unexpected";

        private const int CBTHOOKTYPE = 5;
        private const int HCBT_DESTROYWND = 4;
        private const int HCBT_ACTIVATE = 5;
        private const int HCBT_MOVESIZE = 0;
        private const int HCBT_SETFOCUS = 9;

        private HandlerCollection m_Handers;
        private Win32.CBTCallback m_Callback;
        private IntPtr m_HanderToHook;

        private List<IntPtr> m_hWndList;
        private bool m_Listening;

        private ModalFormActivated m_UnexpectedCalllback;
        private bool m_ExpectAllDialog;
        private bool m_ExpectAllMessageBox;

        private System.Windows.Forms.Form m_HostingForm;

        public ModalFormActivated UnexpectedCalllback
        {
            get { return this.m_UnexpectedCalllback; }
            set { this.m_UnexpectedCalllback = value; }
        }

        internal DialogExpectation(System.Windows.Forms.Form HostingForm)
        {
            this.m_Handers = new HandlerCollection();
            this.m_Callback = null;
            this.m_HanderToHook = IntPtr.Zero;
            this.m_hWndList = new List<IntPtr>();
            this.m_Listening = false;

            this.m_ExpectAllDialog = false;
            this.m_ExpectAllMessageBox = false;

            this.m_UnexpectedCalllback = DefaultUnexpectedCallback;
            this.m_HostingForm = HostingForm;

            this.BeginListening();
        }

        /// <remarks>
        /// This constructor had been obsoleted. Please use <see cref="DialogExpectation(System.Windows.Forms.Form HostingForm)"/> instead.
        /// </remarks>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DialogExpectation(System.Windows.Forms.Form HostingForm, bool StrictExpectation)
            : this(HostingForm)
        {
        }

        private void BeginListening()
        {
            if (this.m_Listening == false)
            {
                this.m_Listening = true;
                this.m_Callback = Callback_ModalListener;
                this.m_HanderToHook = Win32.SetWindowsHookEx(HCBT_ACTIVATE, this.m_Callback, IntPtr.Zero, Win32.GetCurrentThreadId());
            }
        }

        private IntPtr Callback_ModalListener(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code == HCBT_ACTIVATE)
            {
                if (this.m_hWndList.Contains(wParam) == false)
                {
                    this.m_hWndList.Add(wParam);
                    this.FindWindowNameAndInvokeHandler(wParam);
                }
            }
            if (code == HCBT_DESTROYWND)
            {
                if (this.m_hWndList.Contains(wParam) == true)
                {
                    this.m_hWndList.Remove(wParam);
                }
            }
            return Win32.CallNextHookEx(this.m_HanderToHook, code, wParam, lParam);
        }

        private void FindWindowNameAndInvokeHandler(IntPtr hWnd)
        {
            string name = string.Empty;
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(hWnd);
            bool needInvoke = false;
            if (form == this.m_HostingForm)
            {
                needInvoke = false;
            }
            else if (form != null && form.Modal == true)
            {
                name = string.IsNullOrEmpty(form.Name) ? form.GetType().Name : form.Name;
                needInvoke = true;
            }
            else if (Win32.IsDialog(hWnd) == true)
            {
                name = Win32.GetText(hWnd);
                needInvoke = true;
            }
            if (needInvoke == true)
            {
                this.Invoke(name, form, hWnd, hWnd);
                if (form != null && form.Visible)
                {
                    try
                    {
                        form.Close();
                    }
                    catch { }
                }
            }
        }

        private void Invoke(string name, System.Windows.Forms.Form form, IntPtr msgboxHandler, IntPtr hWnd)
        {
            Handler namedHandler = null;
            // Get the named expected handler.
            if (this.m_Handers.ContainsKey(name))
            {
                namedHandler = this.m_Handers[name];
            }
            else
            {
                name = string.IsNullOrEmpty(name) ? UNEXP_DLGNAME : name;
                // Check if currently popping up unexpected dialog/message box need throw exception.
                if (form == null)
                {
                    if(!this.m_ExpectAllMessageBox)
                        throw new System.Collections.Generic.KeyNotFoundException(string.Format("Unexpected message box with text '{0}' is being brought up.", name));
                }
                else
                {
                    if(!this.m_ExpectAllDialog)
                        throw new System.Collections.Generic.KeyNotFoundException(string.Format("Unexpected dialog form named {0} is being brought up.", name));
                }
                // Add it into the named list as expecting all.
                namedHandler = this.m_Handers.Add(name, this.m_UnexpectedCalllback, int.MaxValue);
            }
            namedHandler.Invoke(form, msgboxHandler, hWnd);
        }

        #region " IDisposable Support "

        private void UnhookWindow()
        {
            if (this.m_HanderToHook != IntPtr.Zero)
            {
                Win32.UnhookWindowsHookEx(this.m_HanderToHook);
                this.m_HanderToHook = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        ~DialogExpectation()
        {
            this.UnhookWindow();
        }

        public void Dispose()
        {
            this.UnhookWindow();
        }

        #endregion

        private void DefaultUnexpectedCallback(DialogExpectation.CallbackResult Result)
        {
            switch (Result.Kind)
            {
                case CallbackResult.ExpectedKind.Dialog:
                    Result.FormTester.Control.Close();
                    break;
                case CallbackResult.ExpectedKind.MessageBox:
                    Result.MessageBoxTester.Execute(MessageBoxTester.Command.OK);
                    break;
                default:
                    Debug.Assert(false, "Invalid callback result kind.");
                    break;
            }
        }

        #region " Expact Any "

        public void ExpectAny(ModalFormActivated Handler)
        {
            this.m_ExpectAllDialog = true;
            this.m_ExpectAllMessageBox = true;
            this.m_UnexpectedCalllback = Handler;
        }

        public void ExpectAny()
        {
            this.ExpectAny(DefaultUnexpectedCallback);
        }

        #endregion

        #region " Expect Message Box "

        public void ExpectMessageBox(string Text)
        {
            this.ExpectMessageBox(Text, int.MaxValue, DefaultUnexpectedCallback);
        }

        public void ExpectMessageBox(string Text, int ExpectTimes)
        {
            this.ExpectMessageBox(Text, ExpectTimes, DefaultUnexpectedCallback);
        }

        public void ExpectMessageBox(string Text, ModalFormActivated Handler)
        {
            this.ExpectMessageBox(Text, int.MaxValue, Handler);
        }

        public void ExpectMessageBox(string Text, int ExpectedTimes, ModalFormActivated Handler)
        {
            this.m_Handers.Add(Text, Handler, ExpectedTimes);
        }

        public void ExpectMessageBox()
        {
            this.ExpectMessageBox(DefaultUnexpectedCallback);
        }

        public void ExpectMessageBox(ModalFormActivated Handler)
        {
            this.m_ExpectAllMessageBox = true;
            this.m_UnexpectedCalllback = Handler;
        }

        #endregion

        #region " Expect Dialog "

        public void ExpectDialog()
        {
            this.ExpectDialog(DefaultUnexpectedCallback);
        }

        public void ExpectDialog(ModalFormActivated Handler)
        {
            this.m_ExpectAllDialog = true;
            this.m_UnexpectedCalllback = Handler;
        }

        public void ExpectDialog<T>(int ExpectedTimes) where T : System.Windows.Forms.Form
        {
            this.ExpectDialog<T>(DefaultUnexpectedCallback, ExpectedTimes);
        }

        public void ExpectDialog<T>() where T : System.Windows.Forms.Form
        {
            this.ExpectDialog<T>(DefaultUnexpectedCallback);
        }

        public void ExpectDialog<T>(ModalFormActivated Handler) where T : System.Windows.Forms.Form
        {
            this.ExpectDialog<T>(Handler, int.MaxValue);
        }

        public void ExpectDialog<T>(ModalFormActivated Handler, int ExpectedTimes) where T : System.Windows.Forms.Form
        {
            this.m_Handers.Add(typeof(T).Name, Handler, ExpectedTimes);
        }

        #endregion

        public void Verify()
        {
            foreach (KeyValuePair<string, Handler> kvp in this.m_Handers)
            {
                if (!kvp.Value.Verify())
                {
                    throw new VerifyFailedException(kvp.Value.Name, kvp.Value.ExpectedTimes, kvp.Value.InvokedTimes);
                }
            }
        }

        public class VerifyFailedException : Exception
        {
            public VerifyFailedException(string Name, int ExpectedTimes, int InvokedTimes)
                : base(string.Format("Expected times {0} does not equal to invoked times {1} in dialog {2}.", ExpectedTimes, InvokedTimes, Name))
            {
            }
        }

    }
}

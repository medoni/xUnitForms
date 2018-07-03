using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xUnitForms
{
    public class FormTester<T> : FormTester where T : System.Windows.Forms.Form
    {

        public T Form
        {
            get { return (T)this.Control; }
        }

        public FormTester(T Form, bool AutoExpect)
            : base(Form, AutoExpect)
        {
        }

        public FormTester(T Form)
            : base(Form)
        {
        }
    }

    public class FormTester : ControlTester<System.Windows.Forms.Form>
    {
        public DialogExpectation DialogExpectation
        {
            get;
            private set;
        }

        public FormTester(System.Windows.Forms.Form Form, bool AutoExpect)
            : base(Form)
        {
            this.DialogExpectation = AutoExpect ? BeginDialogExpectation() : null;
        }

        public FormTester(System.Windows.Forms.Form Form)
            : this(Form, false)
        {
        }

        public DialogExpectation BeginDialogExpectation()
        {
            // Clear the original expectation object if it's not null.
            if (this.DialogExpectation != null)
            {
                this.DialogExpectation.Dispose();
                this.DialogExpectation = null;
            }
            // Create the dialog expectation instance for receiving the dialog message.
            this.DialogExpectation = new DialogExpectation(this.Control);
            // Show the hosting form is it's invisible in order to make the expectation be able to be run.
            if (this.Control.Visible == false)
            {
                this.Control.Show();
            }
            return this.DialogExpectation;
        }

        /// <remarks>
        /// This method had been obsoleted. Please use <see cref="BeginDialogExpectation()"/> instead.
        /// </remarks>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DialogExpectation BeginDialogExpectation(bool StrictExpectation)
        {
            // Create the dialog expectation instance for receiving the dialog message.
            this.DialogExpectation = new DialogExpectation(this.Control, StrictExpectation);
            // Show the hosting form is it's invisible in order to make the expectation be able to be run.
            if (this.Control.Visible == false)
            {
                this.Control.Show();
            }
            return this.DialogExpectation;
        }
    }
}

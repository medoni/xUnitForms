using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace xUnitFormsTests.Facts
{
    public class DialogExpectationFacts
    {
        [Fact]
        public void Strict_DefaultDelegation_NoExpectedTimes()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button1");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label2");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectDialog<Materials.Dialog1>();
                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1.
                exp.Verify();
            }

            Assert.Equal<string>("Dialog1 popped up.", lbl_tester.Control.Text);
        }

        [Fact]
        public void Strict_CustomizeDelegation_NoExpectedTimes()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button1");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label2");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectDialog<Materials.Dialog1>(
                    (xUnitForms.DialogExpectation.CallbackResult result) =>
                    {
                        Assert.Equal<xUnitForms.DialogExpectation.CallbackResult.ExpectedKind>(
                            xUnitForms.DialogExpectation.CallbackResult.ExpectedKind.Dialog,
                            result.Kind);
                        Assert.NotNull(result.FormTester);
                        Assert.IsType<Materials.Dialog1>(result.FormTester.Control);
                    });
                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1.
                exp.Verify();
            }

            Assert.Equal<string>("Dialog1 popped up.", lbl_tester.Control.Text);
        }

        [Fact]
        public void Strict_ShowTwiceAndExpectTwice_Correct()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button2");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label3");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectDialog<Materials.Dialog1>(2);
                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                exp.Verify();
            }

            Assert.Equal<string>("Dialog1 popped up twice.", lbl_tester.Control.Text);
        }

        [Fact]
        public void Strict_ShowTwiceButExpectOnce_OverflowException()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button2");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label3");

            Assert.Throws<OverflowException>(
                () =>
                {
                    using (var exp = f_tester.BeginDialogExpectation())
                    {
                        exp.ExpectDialog<Materials.Dialog1>(1);
                        btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                    }
                });
        }

        [Fact]
        public void Strict_ShowTwiceButExpect3Times_VerifyFailedException()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button2");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label3");

            Assert.Throws<xUnitForms.DialogExpectation.VerifyFailedException>(
                () =>
                {
                    using (var exp = f_tester.BeginDialogExpectation())
                    {
                        exp.ExpectDialog<Materials.Dialog1>(3);
                        btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                        exp.Verify();
                    }
                });
        }

        [Fact]
        public void Strict_ExpectMessageBox()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button3");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label4");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectMessageBox();
                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
            }

            Assert.Equal<string>("OK", lbl_tester.Control.Text);
        }

        [Fact]
        public void Strict_ExpectMessageBox_CustomizeHandler()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button3");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label4");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectMessageBox(
                    (xUnitForms.DialogExpectation.CallbackResult result) =>
                    {
                        Assert.Equal<xUnitForms.DialogExpectation.CallbackResult.ExpectedKind>(
                            xUnitForms.DialogExpectation.CallbackResult.ExpectedKind.MessageBox,
                            result.Kind);
                        Assert.NotNull(result.MessageBoxTester);
                        result.MessageBoxTester.Execute(xUnitForms.MessageBoxTester.Command.Cancel);
                    });
                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
            }

            Assert.Equal<string>("Cancel", lbl_tester.Control.Text);
        }

        [Fact]
        public void ExpectSpecificMessageBoxWithCorrectTimes()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button3");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label4");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectMessageBox("Clicked button 3.", 1);
                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                exp.Verify();
            }

            Assert.Equal<string>("OK", lbl_tester.Control.Text);
        }

        [Fact]
        public void ExpectSpecificMessageBoxButExpectMoreTimes()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button3");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label4");

            Assert.Throws<xUnitForms.DialogExpectation.VerifyFailedException>(() =>
            {
                using (var exp = f_tester.BeginDialogExpectation())
                {
                    exp.ExpectMessageBox("Clicked button 3.", 3);
                    btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                    exp.Verify();
                }
            });
        }

        [Fact]
        public void ExpectSpecificMessageBoxButUnexpectedPopped()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button3");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label4");

            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() =>
            {
                using (var exp = f_tester.BeginDialogExpectation())
                {
                    exp.ExpectMessageBox("I'm an unexpected message box.", 1);
                    btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                    exp.Verify();
                }
            });
        }

        [Fact]
        public void ExpectAny()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button4");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label5");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectAny();
                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                exp.Verify();
            }

            Assert.Equal<string>("Onz", lbl_tester.Control.Text);
        }

        [Fact]
        public void ComplexExpectation()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button4");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label5");

            using (var exp = f_tester.BeginDialogExpectation())
            {
                exp.ExpectDialog<Materials.Dialog1>(
                    (xUnitForms.DialogExpectation.CallbackResult result) =>
                    {
                        Assert.True(result.InvokedTimes == 1 || result.InvokedTimes == 2);
                    }, 2);

                exp.ExpectDialog<Materials.Dialog2>(1);

                exp.ExpectMessageBox("Clicked button 4 1st.", 1,
                    (xUnitForms.DialogExpectation.CallbackResult result) => result.MessageBoxTester.Execute(xUnitForms.MessageBoxTester.Command.OK));

                exp.ExpectMessageBox("Clicked button 4 2nd.", 1,
                    (xUnitForms.DialogExpectation.CallbackResult result) => result.MessageBoxTester.Execute(xUnitForms.MessageBoxTester.Command.Cancel));

                btn_tester.RaiseEvent("Click"); // This will pop up Dialog1 twice.
                exp.Verify();
            }

            Assert.Equal<string>("Onz", lbl_tester.Control.Text);
        }

        [Fact]
        public void AutoExpectDialogWhenConstructFormTester_Expected()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f, true);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button1");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label2");

            f_tester.DialogExpectation.ExpectDialog<Materials.Dialog1>();
            btn_tester.RaiseEvent("Click"); // This will pop up Dialog1.
            f_tester.DialogExpectation.Verify();

            Assert.Equal<string>("Dialog1 popped up.", lbl_tester.Control.Text);
        }

        [Fact]
        public void AutoExpectDialogWhenConstructFormTester_Unexpected_ExceptionThrough()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f, true);
            var btn_tester = f_tester.FindControl<System.Windows.Forms.Button>("button1");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label2");

            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(
                () => btn_tester.RaiseEvent("Click"));
        }

    }
}

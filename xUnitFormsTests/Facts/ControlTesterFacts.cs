using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace xUnitFormsTests.Facts
{
    public class ControlTesterFacts
    {
        [Fact]
        public void GetAControl_TopLevel()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);

            var txt2_tester = f_tester.Get<System.Windows.Forms.TextBox>("textBox2");

            Assert.NotNull(txt2_tester);
            Assert.NotNull(txt2_tester.Control);
            Assert.IsType<System.Windows.Forms.TextBox>(txt2_tester.Control);
            Assert.Equal<string>("textBox2", txt2_tester.Control.Text);
        }

        [Fact]
        public void GetAControl_DeepInto()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);

            var txt1_tester = f_tester
                .Get<System.Windows.Forms.GroupBox>("groupBox1")
                .Get<System.Windows.Forms.TabControl>("tabControl1")
                .Get<System.Windows.Forms.TabPage>("tabPage2")
                .Get<System.Windows.Forms.TextBox>("textBox1");

            Assert.NotNull(txt1_tester);
            Assert.NotNull(txt1_tester.Control);
            Assert.IsType<System.Windows.Forms.TextBox>(txt1_tester.Control);
            Assert.Equal<string>("textBox1", txt1_tester.Control.Text);
        }

        [Fact]
        public void GetAControl_UserControl_DeepInto()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);

            var txt1_tester = f_tester
                .Get<Materials.UserControl1>("userControl11")
                .Get<System.Windows.Forms.GroupBox>("groupBox1")
                .Get<System.Windows.Forms.TabControl>("tabControl1")
                .Get<System.Windows.Forms.TabPage>("tabPage2")
                .Get<System.Windows.Forms.TextBox>("textBox1");

            Assert.NotNull(txt1_tester);
            Assert.NotNull(txt1_tester.Control);
            Assert.IsType<System.Windows.Forms.TextBox>(txt1_tester.Control);
            Assert.Equal<string>("textBox1_user_control", txt1_tester.Control.Text);
        }

        [Fact]
        public void FindAControl_InForm()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);

            var llb_tester = f_tester.FindControl<System.Windows.Forms.LinkLabel>("linkLabel1");

            Assert.NotNull(llb_tester);
            Assert.NotNull(llb_tester.Control);
            Assert.IsType<System.Windows.Forms.LinkLabel>(llb_tester.Control);
            Assert.Equal<string>("linkLabel1", llb_tester.Control.Text);
        }

        [Fact]
        public void FindAControl_InUserControl()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);

            var cbo_tester = f_tester.FindControl<System.Windows.Forms.ComboBox>("comboBox1");

            Assert.NotNull(cbo_tester);
            Assert.NotNull(cbo_tester.Control);
            Assert.IsType<System.Windows.Forms.ComboBox>(cbo_tester.Control);
            Assert.Equal<int>(2, cbo_tester.Control.Items.Count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace xUnitFormsTests.Facts
{
    public class ComponentTesterFacts
    {
        [Fact]
        public void FindAComponent_ImageList()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);

            var imgs_tester = f_tester.FindComponent<System.Windows.Forms.ImageList>("imageList1");

            Assert.NotNull(imgs_tester);
            Assert.NotNull(imgs_tester.Component);
            Assert.IsType<System.Windows.Forms.ImageList>(imgs_tester.Component);
            Assert.Equal<int>(3, imgs_tester.Component.Images.Count);
        }

        [Fact]
        public void ComponentRaiseEvent_TimerTicked()
        {
            var f = new Materials.Form1();
            var f_tester = new xUnitForms.FormTester<Materials.Form1>(f);
            var tm_tester = f_tester.FindComponent<System.Windows.Forms.Timer>("timer1");
            var lbl_tester = f_tester.FindControl<System.Windows.Forms.Label>("label1");

            tm_tester.RaiseEvent("Tick"); // Will change the label1's text.

            Assert.Equal<string>("Timer ticked.", lbl_tester.Control.Text);
        }
    }
}

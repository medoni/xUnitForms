using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xUnitForms
{
    public class ControlTester<T> : ComponentTester<T> where T : System.Windows.Forms.Control
    {

        public T Control
        {
            get { return this.Component; }
        }

        public ControlTester(T control)
            : base(control)
        {
        }

        #region " Find Control "

        public ControlTester<TCtl> FindControl<TCtl>(string UniqueName) where TCtl : System.Windows.Forms.Control
        {
            return this.FindControl<TCtl>(UniqueName, true);
        }

        public ControlTester<TCtl> FindControl<TCtl>(string UniqueName, bool IgnoreCase) where TCtl : System.Windows.Forms.Control
        {
            System.Windows.Forms.Control ctl = this.FindControl<TCtl>(this.Control, UniqueName, IgnoreCase);
            ControlTester<TCtl> ret = null;
            if (ctl != null)
            {
                ret = new ControlTester<TCtl>((TCtl)ctl);
            }
            return ret;
        }

        private TCtl FindControl<TCtl>(System.Windows.Forms.Control BaseControl, string UniqueName, bool IgnoreCase) where TCtl : System.Windows.Forms.Control
        {
            TCtl ret = null;
            // Check myself control first.
            if (string.Compare(BaseControl.Name, UniqueName, IgnoreCase) == 0 && BaseControl is TCtl)
            {
                ret = (TCtl)BaseControl;
            }
            else
            {
                // Check all base control's children.
                foreach (System.Windows.Forms.Control ctl in BaseControl.Controls)
                {
                    ret = this.FindControl<TCtl>(ctl, UniqueName, IgnoreCase);
                    if (ret != null)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        #endregion

        #region " Find Component "

        public ComponentTester<TComp> FindComponent<TComp>(string UniqueName) where TComp : System.ComponentModel.Component
        {
            return this.FindComponent<TComp>(UniqueName, true);
        }

        public ComponentTester<TComp> FindComponent<TComp>(string UniqueName, bool IgnoreCase) where TComp : System.ComponentModel.Component
        {
            ComponentTester<TComp> ret = null;
            System.ComponentModel.Component comp = null;
            if (this.Control == null)
            {
                throw new NullReferenceException("Cannot find the container of current component.");
            }
            else
            {
                Type formType = this.Control.GetType();
                string compName = string.Format("_{0}", UniqueName);
                foreach (System.Reflection.FieldInfo field in formType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
                {
                    // VB.Net will prefixing with "_" for the components but will not in C# so here needs to check both of them.
                    if (string.Compare(field.Name, compName, IgnoreCase) == 0 ||
                        string.Compare(field.Name, UniqueName, IgnoreCase) == 0)
                    {
                        comp = (System.ComponentModel.Component)field.GetValue(this.Control);
                        if (comp is TComp)
                        {
                            break;
                        }
                    }
                }
            }
            if (comp != null)
            {
                ret = new ComponentTester<TComp>((TComp)comp);
            }
            return ret;
        }

        #endregion

        #region " Get Control "

        public ControlTester<TCtl> Get<TCtl>(string Name) where TCtl : System.Windows.Forms.Control
        {
            return this.Get<TCtl>(Name, true);
        }

        public ControlTester<TCtl> Get<TCtl>(string Name, bool IgnoreCase) where TCtl : System.Windows.Forms.Control
        {
            ControlTester<TCtl> ret = null;
            TCtl ctl = null;
            // Find the control under this control.
            foreach (System.Windows.Forms.Control c in this.Control.Controls)
            {
                if (string.Compare(c.Name, Name, IgnoreCase) == 0 && c is TCtl)
                {
                    ctl = (TCtl)c;
                    break;
                }
            }
            // Create the instance of the control and return.
            if (ctl != null)
            {
                ret = new ControlTester<TCtl>(ctl);
            }
            return ret;
        }

        #endregion
    }
}

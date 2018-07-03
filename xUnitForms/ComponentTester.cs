using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xUnitForms
{
    public class ComponentTester<T> where T : System.ComponentModel.Component
    {

        private T m_Component;

        public T Component
        {
            get { return this.m_Component; }
        }

        public ComponentTester(T Component)
        {
            this.m_Component = (T)Component;
        }

        #region " Raise Event "

        public void RaiseEvent(string EventName)
        {
            var raiseMethod = this.GetRaiseEventMethodInfo(EventName);
            if (raiseMethod != null)
            {
                this.DoRaiseEvent(raiseMethod);
            }
        }

        public void RaiseEvent(string EventName, object[] args)
        {
            var raiseMethod = this.GetRaiseEventMethodInfo(EventName);
            if (raiseMethod != null)
            {
                this.DoRaiseEvent(raiseMethod, args);
            }
        }

        public void RaiseEvent(string EventName, EventArgs arg)
        {
            var raiseMethod = this.GetRaiseEventMethodInfo(EventName);
            if (raiseMethod != null)
            {
                this.DoRaiseEvent(raiseMethod, arg);
            }
        }

        private System.Reflection.MethodInfo GetRaiseEventMethodInfo(string EventName)
        {
            Type ctlType = typeof(T);
            System.Reflection.EventInfo eveInfo = ctlType.GetEvent(EventName);
            System.Reflection.MethodInfo raiseMethod = null;
            if (eveInfo == null)
            {
                throw new NotImplementedException(string.Format("Event {0} has not been implemented.", EventName));
            }
            else
            {
                raiseMethod = eveInfo.GetRaiseMethod(true);
                if (raiseMethod == null)
                {
                    raiseMethod = ctlType.GetMethod(string.Format("On{0}", EventName), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    if (raiseMethod == null)
                    {
                        throw new NotImplementedException(string.Format("Raising method of event {0} has not been implemented.", EventName));
                    }
                }
            }
            return raiseMethod;
        }

        private void DoRaiseEvent(System.Reflection.MethodInfo raiseMethod, EventArgs eventArgs)
        {
            this.DoRaiseEvent(raiseMethod, new object[] { eventArgs });
        }

        private void DoRaiseEvent(System.Reflection.MethodInfo raiseMethod, object[] args)
        {
            try
            {
                raiseMethod.Invoke(this.Component, args);
            }
            catch (System.Reflection.TargetInvocationException tiex)
            {
                // Pass the inner exception out instead of just reporting the reflection calling exception.
                if (tiex.InnerException != null)
                {
                    throw tiex.InnerException;
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        private void DoRaiseEvent(System.Reflection.MethodInfo raiseMethod)
        {
            System.Reflection.ParameterInfo[] paraInfos = raiseMethod.GetParameters();
            List<object> @parameters = new List<object>();
            foreach (System.Reflection.ParameterInfo paraInfo in paraInfos)
            {
                @parameters.Add(System.Activator.CreateInstance(paraInfo.ParameterType));
            }
            this.DoRaiseEvent(raiseMethod, @parameters.ToArray());
        }
    }

        #endregion
}

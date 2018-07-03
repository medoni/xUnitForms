# xunitforms
Cloned from https://archive.codeplex.com/?p=xunitforms

xUnitForms is a xUnit extension for unit and acceptance testing of Windows Forms applications.

Your xUnit tests can interact with the controls, components and user controls. Your tests can manipulate and verify the properties of the GUI. Also xUnitForms can detect and handle the dialog forms and message boxes brought up within your test code, and verify that your expectations for the test are fulfilled.

The ideas of building xUnitForms is mostly came from a similar project named NUnitForms which is an extension for NUnit. As NUnit had been evolved to xUnit but it's not catching up we built this project.

Basically we used the core idea of NUnitForms which are:

    Retrieve an instance of control or component from an instance of windows form.
    Manipulate and verify the properties of them.
    Detect, handle and verify any bringing up dialog forms and message box.

Hence the size of this framework reduce a lot which means all classes you will use in your test code might be at most:

    ControlTester: Wrapping a control instance and giving you the possibility to raise event.
    ComponentTester: Wrapping a component instance and giving you the possibility to raise event.
    FormTester: Wrapping a form instance and giving you the possibility to raise event and detect the dialog windows and message box.
    DialogExpectation: Giving you the possibility to set how to deal with the popping up dialogs.


Regarding how to use it please refer to the test project of the source code and releases.
The documentation of the usage will be coming soon, hopefully...
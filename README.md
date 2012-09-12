LordDesign.Utilities
====================

This is a very simple (at first) class library for universal utilities such as logging.

### Dependencies
- ELMAH

### Logger.cs
This class simply wraps the basic ELMAH exception logger and will log to the elmah.axd source if the current context is an http application, and to a file if it is not. This is useful for business logic layers in which a class's usage may be over http or in a service.

### Usage

    using LordDesign.Utilities;
    
    public class MyClass
    {
        public void DoMyThing()
        {
            try
            {
                // Todo: Do something...
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
        }
    }

Now you can do all of your exception logging with just one simple line of code.
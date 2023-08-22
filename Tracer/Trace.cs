
namespace Cartheur.Ideal.Mooc.Tracer
{
    /**
 * The Class Trace provides static methods that can be called from anywhere in the project to record elements of trace.   
 * A new Event is created by the method startNewEvent(). Events have incremental time codes.
 * The other methods add new elements and sub-elements to the current Event.
 * @author Olivier
 */
    public class Trace
    {

        private static Tracer<Element> tracer;
        private static int time = 0;

        public static void Init(Tracer<Element> t)
        {
            tracer = t;
        }

        public static void StartNewEvent()
        {
            if (tracer != null)
                tracer.StartNewEvent(time++);
        }

        public static Element AddEventElement(string name)
        {
            if (tracer != null)
                return tracer.AddEventElement(name);
            else
                return null;
        }

        public static void AddEventElement(string name, string value)
        {
            if (tracer != null)
                tracer.AddEventElement(name, value);
        }

        public static Element AddSubelement(Element element, string name)
        {
            if (tracer != null)
                return tracer.AddSubelement(element, name);
            else
                return null;
        }

        public static void AddSubelement(Element element, string name, string textContent)
        {
            if (tracer != null)
                tracer.AddSubelement(element, name, textContent);
        }
    }
}

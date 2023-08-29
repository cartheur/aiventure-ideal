using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc.Tracer
{
    /// <summary>
    /// A lot to do in here.
    /// </summary>
    /// <seealso cref="ITrace&lt;Element&gt;" />
    public class AbstractTracer : ITrace<Element>
    {

        public AbstractTracer() { }

        public Element AddEventElement(string name)
        {
            throw new NotImplementedException();
        }

        public void AddEventElement(string name, string value)
        {
            throw new NotImplementedException();
        }

        public Element AddSubelement(Element element, string name)
        {
            throw new NotImplementedException();
        }

        public void AddSubelement(Element element, string name, string textContent)
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public void FinishEvent()
        {
            throw new NotImplementedException();
        }

        public Element NewEvent(string source, string type, int t)
        {
            throw new NotImplementedException();
        }

        public void StartNewEvent(int t)
        {
            throw new NotImplementedException();
        }
    }
}

namespace Cartheur.Ideal.Mooc.Interfaces
{
    /// <summary>
    /// Generates activity traces.
    /// </summary>
    /// <typeparam name="EventElement">The element of trace.</typeparam>
    public interface ITrace<EventElement>
    {
        /// <summary>
        /// Close the tracer.
        /// </summary>
        /// <returns>true if tracer ok</returns>
        public bool Close();
        /// <summary>
        /// Create a new event.
        /// </summary>
        /// <param name="t">The time stamp.</param>
        public void StartNewEvent(int t);
        /// <summary>
        /// Finishes the current event.
        /// </summary>
        public void FinishEvent();
        /// <summary>
        /// Add a new property to the current event.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The added event element</returns>
        public EventElement AddEventElement(string name);
        /// <summary>
        /// Adds the event element.
        /// </summary>
        /// <param name="name">The element's name.</param>
        /// <param name="value">The element's string value</param>
        public void AddEventElement(string name, string value);
        /// <summary>
        /// Adds a subelement.
        /// </summary>
        /// <param name="element">The subelement.</param>
        /// <param name="name">The name of the subelement.</param>
        /// <returns></returns>
        public EventElement AddSubelement(EventElement element, string name);
        /// <summary>
        /// Adds a subelement.
        /// </summary>
        /// <param name="element">The subelement.</param>
        /// <param name="name">The name of the subelement.</param>
        /// <param name="textContent">The text content of the sub element.</param>
        public void AddSubelement(EventElement element, string name, string textContent);
        /// <summary>
        /// Create an event that can be populated using its reference.
        /// </summary>
        /// <param name="source">The source of the event: Ernest or user.</param>
        /// <param name="type">The type.</param>
        /// <param name="t">The time stamp.</param>
        /// <returns>The pointer to the event.</returns>
        public EventElement NewEvent(string source, string type, int t);

    }
}

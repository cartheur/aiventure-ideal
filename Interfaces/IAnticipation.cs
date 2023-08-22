
namespace Cartheur.Ideal.Mooc.Interfaces
{
    /// <summary>
    /// The anticipation for the presence of its participant in an environment.
    /// </summary>
    /// <seealso cref="System.IComparable&lt;Cartheur.Ideal.Mooc.Interfaces.IAnticipation&gt;" />
    public interface IAnticipation : IComparable<IAnticipation>
    {
        void AddProclivity(int proclivity);
    }
}

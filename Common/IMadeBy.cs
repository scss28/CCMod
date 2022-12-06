using System.Drawing;

namespace CCMod.Common
{
    // Use when you want to credit artists or coders
    /// <summary>Give credit to artists or coders for a piece of content or system.</summary>
    public interface IMadeBy
    {
        public string CodedBy { get; }
        public string SpritedBy { get; }
        public string ConceptBy => CodedBy;
    }
}

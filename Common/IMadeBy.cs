using System.Drawing;

namespace CCMod.Common
{
    // Use when you want to credit artists or coders
    public interface IMadeBy
    {
        public string CodedBy { get; }
        public string SpritedBy { get; }
        public string ConceptBy => CodedBy;
    }
}

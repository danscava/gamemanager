using System.ComponentModel;

namespace GameManager.Data.Enums
{
    /// <summary>
    /// Enum for different game media types
    /// </summary>
    public enum MediaType : int
    {
        [Description("Not Specified")]
        NotSpecified = 0,
        [Description("CD")]
        CD = 1,
        [Description("Cartridge")]
        Cartridge = 2,
    }
}

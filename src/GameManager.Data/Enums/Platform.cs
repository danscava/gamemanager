using System.ComponentModel;

namespace GameManager.Data.Enums
{
    /// <summary>
    /// Enum for specifying game platforms
    /// </summary>
    public enum Platform : int
    {
        [Description("Not Specified")]
        NotSpecified = 0,
        [Description("PC")]
        PC = 1,
        [Description("SNES")]
        SNES = 2,
        [Description("PS")]
        PS = 3
    }
}
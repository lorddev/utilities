namespace Devlord.Utilities
{
    public enum MailFormat    {
        Plain,
        Html,
#if NETSTANDARD1_5 || NETSTANDARD1_3
        Enriched,
        Rtf
#endif
    }
}
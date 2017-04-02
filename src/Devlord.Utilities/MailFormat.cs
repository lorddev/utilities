namespace Devlord.Utilities
{
    public enum MailFormat    {
        Plain,
        Html,
#if NETSTANDARD1_5
        Enriched,
        Rtf
#endif
    }
}
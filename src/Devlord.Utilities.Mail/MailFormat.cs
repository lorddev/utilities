namespace Devlord.Utilities.Mail
{
    public enum MailFormat
    {
        Plain,
        Html,
#if NETSTANDARD1_5 || NETSTANDARD1_3
        Enriched,
        Rtf
#endif
    }
}
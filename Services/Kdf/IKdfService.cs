namespace ASP_SPR311.Services.Kdf
{
    // Key Derivation Function Service by https://datatracker.ietf.org/doc/html/rfc2898
    public interface IKdfService
    {
        String DerivedKey(String password, String salt);
    }
}

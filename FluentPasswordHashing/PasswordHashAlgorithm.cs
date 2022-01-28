namespace FluentPasswordHashing
{
    public enum PasswordHashAlgorithm
    {
        [EnumMember(Value= "argon2id")]
        ARGON2ID,
        [EnumMember(Value = "argon2d")]
        ARGON2D,
        [EnumMember (Value = "argon2i")]
        ARGON2I
    }
}

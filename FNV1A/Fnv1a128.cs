namespace FNV1A
{
    using static System.Globalization.CultureInfo;
    using static System.Globalization.NumberStyles;
    using static System.Numerics.BigInteger;

    public sealed class Fnv1a128 : Fnv1aBase
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="Fnv1a128"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentException">style is not a
        /// <see cref="System.Globalization.NumberStyles"></see> value.   -or-  style includes the
        /// <see cref="AllowHexSpecifier"></see> or <see cref="HexNumber"></see> flag along with another
        /// value.</exception>
        /// <exception cref="System.ArgumentNullException">value is null.</exception>
        /// <exception cref="System.FormatException">value does not comply with the input pattern specified by
        /// style.</exception>
        public Fnv1a128() : base(
            Parse("100000000000000000000000000000000", AllowHexSpecifier, InvariantCulture),
            Parse("0000000001000000000000000000013B", AllowHexSpecifier, InvariantCulture),
            Parse("6C62272E07BB014262B821756295C58D", AllowHexSpecifier, InvariantCulture),
            128)
        {
        }
    }
}

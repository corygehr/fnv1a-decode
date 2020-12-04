using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FNV1A
{
    /// <summary>
    /// Base class for FNV1A implementations
    /// </summary>
    public abstract class Fnv1aBase : System.Security.Cryptography.HashAlgorithm
    {
        /// <summary>
        /// The "wrap-around" modulo value for keeping multiplication within the number of bits.
        /// </summary>
        private readonly BigInteger _modValue;

        /// <summary>
        /// The prime.
        /// </summary>
        private readonly BigInteger _prime;

        /// <summary>
        /// The non-zero offset basis.
        /// </summary>
        private readonly BigInteger _offsetBasis;

        /// <summary>
        /// The hash.
        /// </summary>
        private BigInteger _hash;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="Fnv1aBigBase"/> class.
        /// </summary>
        /// <param name="modValue">The "wrap-around" modulo value for keeping multiplication within the number of
        /// bits.</param>
        /// <param name="prime">The FNV-1a prime.</param>
        /// <param name="offsetBasis">The FNV-1a offset basis.</param>
        /// <param name="hashSizeValue">The size, in bits, of the computed hash code.</param>
        // ReSharper disable once TooManyDependencies
        protected Fnv1aBase(BigInteger modValue, BigInteger prime, BigInteger offsetBasis, int hashSizeValue)
        {
            this._modValue = modValue;
            this._prime = prime;
            this._offsetBasis = offsetBasis;
            this.Init();
            this.HashSizeValue = hashSizeValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.
        /// </summary>
        public override sealed void Initialize() => this.Init();

        /// <inheritdoc />
        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing
        /// the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (int i = ibStart; i < cbSize; i++)
            {
                unchecked
                {
                    this._hash ^= array[i];
                    this._hash = (this._hash * this._prime) % this._modValue;
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the
        /// cryptographic stream object.
        /// </summary>
        /// <returns>
        /// The computed hash code.
        /// </returns>
        protected override byte[] HashFinal() => this._hash.ToByteArray();

        /// <summary>
        /// Initializes the hash for this instance.
        /// </summary>
        private void Init() => this._hash = this._offsetBasis;
    }
}
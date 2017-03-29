// Source: https://github.com/dotnet/corefx/commit/6d523750c4356af1d5c9e001c362baa833059833#diff-4d021d7c9e9a9ef5fdf2d896a928c0b6

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

// ReSharper disable CheckNamespace

namespace System.Security.Cryptography
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class AesManaged : Aes
    {
        private readonly Aes _impl;

        public AesManaged()
        {
            // This class wraps Aes
            _impl = Create();
        }

        public override int BlockSize
        {
            get => _impl.BlockSize;
            set => _impl.BlockSize = value;
        }

        public override byte[] IV
        {
            get => _impl.IV;
            set => _impl.IV = value;
        }

        public override byte[] Key
        {
            get => _impl.Key;
            set => _impl.Key = value;
        }

        public override int KeySize
        {
            get => _impl.KeySize;
            set => _impl.KeySize = value;
        }

        public override CipherMode Mode
        {
            get => _impl.Mode;
            set => _impl.Mode = value;
        }

        public override PaddingMode Padding
        {
            get => _impl.Padding;
            set => _impl.Padding = value;
        }

        public override KeySizes[] LegalBlockSizes => _impl.LegalBlockSizes;
        public override KeySizes[] LegalKeySizes => _impl.LegalKeySizes;

        public override ICryptoTransform CreateEncryptor()
        {
            return _impl.CreateEncryptor();
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return _impl.CreateEncryptor(rgbKey, rgbIV);
        }

        public override ICryptoTransform CreateDecryptor()
        {
            return _impl.CreateDecryptor();
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return _impl.CreateDecryptor(rgbKey, rgbIV);
        }

        public override void GenerateIV()
        {
            _impl.GenerateIV();
        }

        public override void GenerateKey()
        {
            _impl.GenerateKey();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
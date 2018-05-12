﻿using System.Collections.Generic;

namespace OpenStory.Framework.Model.Common
{
    /// <summary>
    /// Compares objects that refer to characters.
    /// </summary>
    public class CharacterExtensionComparer : EqualityComparer<ICharacterExtension>
    {
        private readonly IEqualityComparer<CharacterKey> _keyComparer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterExtensionComparer"/> class.
        /// </summary>
        /// <param name="keyComparer">The <see cref="IEqualityComparer{CharacterKey}" /> to use internally.</param>
        public CharacterExtensionComparer(IEqualityComparer<CharacterKey> keyComparer)
        {
            _keyComparer = keyComparer;
        }

        /// <inheritdoc />
        public override bool Equals(ICharacterExtension x, ICharacterExtension y)
        {
            return _keyComparer.Equals(x.Key, y.Key);
        }

        /// <inheritdoc />
        public override int GetHashCode(ICharacterExtension obj)
        {
            return obj != null ? _keyComparer.GetHashCode(obj.Key) : 0;
        }
    }
}
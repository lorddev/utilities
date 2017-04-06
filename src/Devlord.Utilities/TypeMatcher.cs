// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeMatcher.cs" company="Lord Design">
//   © 2017 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community. 
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Devlord.Utilities.Resources;

namespace Devlord.Utilities
{
    public class TypeMatcher<T>
    {
        private readonly Dictionary<string, string> _fieldTranslator = new Dictionary<string, string>();
        private readonly IEnumerable<PropertyInfo> _properties;

        public TypeMatcher(IEnumerable<PropertyInfo> properties)
        {
            _properties = properties;
        }

        public Dictionary<string, string> GetFieldTranslator()
        {
            if (_fieldTranslator?.Count > 0)
            {
                return _fieldTranslator;
            }

            throw new InvalidOperationException("Please call 'VerifyTypeMatch' first");
        }

        public TypeMatcher<T> VerifyTypeMatch(IDataRecord dr)
        {
            // Counter for when each field is found.
            var hashSet = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);

            // Throw an error if the class expects columns that aren't being returned.
            foreach (var pi in _properties)
            {
                // Increment
                hashSet.Add(pi.Name.ToLowerInvariant());
            }

            // Don't throw an error if the data set more verbose than the class we're filling
            for (var i = 0; i < dr.FieldCount; i++)
            {
                // Decrement.
                var column = dr.GetName(i).ToLowerInvariant();
                if (hashSet.Contains(column))
                {
                    hashSet.Remove(column);
                }

                _fieldTranslator.Add(column, dr.GetName(i));
            }

            if (hashSet.Count > 0)
            {
                var ex = new Exception(ExceptionText.DRMapperTypeTooComplex);
                ex.Data.Add("Type", typeof(T));
                ex.Data.Add("Missing fields", string.Join(", ", hashSet));
                throw ex;
            }

            return this;
        }

        private static Dictionary<string, string> TranslateFields(IDataRecord dr)
        {
            var fieldTranslator = new Dictionary<string, string>();
            for (var i = 0; i < dr.FieldCount; i++)
            {
                var name = dr.GetName(i);
                fieldTranslator.Add(name.ToLowerInvariant(), name);
            }
            return fieldTranslator;
        }
    }
}
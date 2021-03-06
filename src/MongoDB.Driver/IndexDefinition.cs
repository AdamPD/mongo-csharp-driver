﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver
{
    /// <summary>
    /// Base class for an index definition.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public abstract class IndexDefinition<TDocument>
    {
        /// <summary>
        /// Renders the indexes to a <see cref="BsonDocument"/>.
        /// </summary>
        /// <param name="documentSerializer">The document serializer.</param>
        /// <param name="serializerRegistry">The serializer registry.</param>
        /// <returns>A <see cref="BsonDocument"/>.</returns>
        public abstract BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry);

        /// <summary>
        /// Performs an implicit conversion from <see cref="BsonDocument"/> to <see cref="IndexDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator IndexDefinition<TDocument>(BsonDocument document)
        {
            if (document == null)
            {
                return null;
            }

            return new BsonDocumentIndexDefinition<TDocument>(document);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String" /> to <see cref="IndexDefinition{TDocument}" />.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator IndexDefinition<TDocument>(string json)
        {
            if (json == null)
            {
                return null;
            }

            return new JsonIndexDefinition<TDocument>(json);
        }
    }

    /// <summary>
    /// A <see cref="BsonDocument"/> based index definition.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public sealed class BsonDocumentIndexDefinition<TDocument> : IndexDefinition<TDocument>
    {
        private readonly BsonDocument _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonDocumentIndexDefinition{TDocument}"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        public BsonDocumentIndexDefinition(BsonDocument document)
        {
            _document = Ensure.IsNotNull(document, "document");
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        public BsonDocument Document
        {
            get { return _document; }
        }

        /// <inheritdoc />
        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            return _document;
        }
    }

    /// <summary>
    /// A JSON <see cref="String" /> based index definition.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public sealed class JsonIndexDefinition<TDocument> : IndexDefinition<TDocument>
    {
        private readonly string _json;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonIndexDefinition{TDocument}"/> class.
        /// </summary>
        /// <param name="json">The json.</param>
        public JsonIndexDefinition(string json)
        {
            _json = Ensure.IsNotNull(json, "json");
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        public string Json
        {
            get { return _json; }
        }

        /// <inheritdoc />
        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            return BsonDocument.Parse(_json);
        }
    }
}

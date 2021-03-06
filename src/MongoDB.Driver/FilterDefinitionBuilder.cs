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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MongoDB.Driver
{
    /// <summary>
    /// A builder for a <see cref="FilterDefinition{TDocument}"/>.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public sealed class FilterDefinitionBuilder<TDocument>
    {
        /// <summary>
        /// Creates an all filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>An all filter.</returns>
        public FilterDefinition<TDocument> All<TField, TItem>(FieldDefinition<TDocument, TField> field, IEnumerable<TItem> values)
            where TField : IEnumerable<TItem>
        {
            return new ArrayOperatorFilterDefinition<TDocument, TField, TItem>("$all", field, values);
        }

        /// <summary>
        /// Creates an all filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>An all filter.</returns>
        public FilterDefinition<TDocument> All<TItem>(string field, IEnumerable<TItem> values)
        {
            return new ArrayOperatorFilterDefinition<TDocument, IEnumerable<TItem>, TItem>(
                "$all",
                new StringFieldDefinition<TDocument, IEnumerable<TItem>>(field),
                values);
        }

        /// <summary>
        /// Creates an all filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>An all filter.</returns>
        public FilterDefinition<TDocument> All<TField, TItem>(Expression<Func<TDocument, TField>> field, IEnumerable<TItem> values)
            where TField : IEnumerable<TItem>
        {
            return All(new ExpressionFieldDefinition<TDocument, TField>(field), values);
        }

        /// <summary>
        /// Creates an and filter.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>A filter.</returns>
        public FilterDefinition<TDocument> And(params FilterDefinition<TDocument>[] filters)
        {
            return And((IEnumerable<FilterDefinition<TDocument>>)filters);
        }

        /// <summary>
        /// Creates an and filter.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>An and filter.</returns>
        public FilterDefinition<TDocument> And(IEnumerable<FilterDefinition<TDocument>> filters)
        {
            return new AndFilterDefinition<TDocument>(filters);
        }

        /// <summary>
        /// Creates an element match filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>An element match filter.</returns>
        public FilterDefinition<TDocument> ElemMatch<TField, TItem>(FieldDefinition<TDocument, TField> field, FilterDefinition<TItem> filter)
            where TField : IEnumerable<TItem>
        {
            return new ElementMatchFilterDefinition<TDocument, TField, TItem>(field, filter);
        }

        /// <summary>
        /// Creates an element match filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>An element match filter.</returns>
        public FilterDefinition<TDocument> ElemMatch<TItem>(string field, FilterDefinition<TItem> filter)
        {
            return ElemMatch(
                new StringFieldDefinition<TDocument, IEnumerable<TItem>>(field),
                filter);
        }

        /// <summary>
        /// Creates an element match filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>An element match filter.</returns>
        public FilterDefinition<TDocument> ElemMatch<TField, TItem>(Expression<Func<TDocument, TField>> field, FilterDefinition<TItem> filter)
            where TField : IEnumerable<TItem>
        {
            return ElemMatch(new ExpressionFieldDefinition<TDocument, TField>(field), filter);
        }

        /// <summary>
        /// Creates an element match filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>An element match filter.</returns>
        public FilterDefinition<TDocument> ElemMatch<TField, TItem>(Expression<Func<TDocument, TField>> field, Expression<Func<TItem, bool>> filter)
            where TField : IEnumerable<TItem>
        {
            return ElemMatch(new ExpressionFieldDefinition<TDocument, TField>(field), new ExpressionFilterDefinition<TItem>(filter));
        }

        /// <summary>
        /// Creates an equality filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>An equality filter.</returns>
        public FilterDefinition<TDocument> Eq<TField>(FieldDefinition<TDocument, TField> field, TField value)
        {
            return new SimpleFilterDefinition<TDocument, TField>(field, value);
        }

        /// <summary>
        /// Creates an equality filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>An equality filter.</returns>
        public FilterDefinition<TDocument> Eq<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            return Eq(new ExpressionFieldDefinition<TDocument, TField>(field), value);
        }

        /// <summary>
        /// Creates an exists filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="exists">if set to <c>true</c> [exists].</param>
        /// <returns>An exists filter.</returns>
        public FilterDefinition<TDocument> Exists(FieldDefinition<TDocument> field, bool exists = true)
        {
            return new OperatorFilterDefinition<TDocument>("$exists", field, exists);
        }

        /// <summary>
        /// Creates an exists filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="exists">if set to <c>true</c> [exists].</param>
        /// <returns>An exists filter.</returns>
        public FilterDefinition<TDocument> Exists(Expression<Func<TDocument, object>> field, bool exists = true)
        {
            return Exists(new ExpressionFieldDefinition<TDocument>(field), exists);
        }

        /// <summary>
        /// Creates a geo intersects filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="geometry">The geometry.</param>
        /// <returns>A geo intersects filter.</returns>
        public FilterDefinition<TDocument> GeoIntersects<TCoordinates>(FieldDefinition<TDocument> field, GeoJsonGeometry<TCoordinates> geometry)
            where TCoordinates : GeoJsonCoordinates
        {
            return new GeometryOperatorFilterDefinition<TDocument, TCoordinates>("$geoIntersects", field, geometry);
        }

        /// <summary>
        /// Creates a geo intersects filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="geometry">The geometry.</param>
        /// <returns>A geo intersects filter.</returns>
        public FilterDefinition<TDocument> GeoIntersects<TCoordinates>(Expression<Func<TDocument, object>> field, GeoJsonGeometry<TCoordinates> geometry)
            where TCoordinates : GeoJsonCoordinates
        {
            return GeoIntersects(new ExpressionFieldDefinition<TDocument>(field), geometry);
        }

        /// <summary>
        /// Creates a geo within filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="geometry">The geometry.</param>
        /// <returns>A geo within filter.</returns>
        public FilterDefinition<TDocument> GeoWithin<TCoordinates>(FieldDefinition<TDocument> field, GeoJsonGeometry<TCoordinates> geometry)
            where TCoordinates : GeoJsonCoordinates
        {
            return new GeometryOperatorFilterDefinition<TDocument, TCoordinates>("$geoWithin", field, geometry);
        }

        /// <summary>
        /// Creates a geo within filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="geometry">The geometry.</param>
        /// <returns>A geo within filter.</returns>
        public FilterDefinition<TDocument> GeoWithin<TCoordinates>(Expression<Func<TDocument, object>> field, GeoJsonGeometry<TCoordinates> geometry)
            where TCoordinates : GeoJsonCoordinates
        {
            return GeoWithin(new ExpressionFieldDefinition<TDocument>(field), geometry);
        }

        /// <summary>
        /// Creates a geo within box filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A geo within box filter.</returns>
        public FilterDefinition<TDocument> GeoWithinBox(FieldDefinition<TDocument> field, double x, double y)
        {
            return new OperatorFilterDefinition<TDocument>("$geoWithin", field, new BsonDocument("$box", new BsonArray { x, y }));
        }

        /// <summary>
        /// Creates a geo within box filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A geo within box filter.</returns>
        public FilterDefinition<TDocument> GeoWithinBox(Expression<Func<TDocument, object>> field, double x, double y)
        {
            return GeoWithinBox(new ExpressionFieldDefinition<TDocument>(field), x, y);
        }

        /// <summary>
        /// Creates a geo within center filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>A geo within center filter.</returns>
        public FilterDefinition<TDocument> GeoWithinCenter(FieldDefinition<TDocument> field, double x, double y, double radius)
        {
            return new OperatorFilterDefinition<TDocument>("$geoWithin", field, new BsonDocument("$center", new BsonArray { new BsonArray { x, y }, radius }));
        }

        /// <summary>
        /// Creates a geo within center filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>A geo within center filter.</returns>
        public FilterDefinition<TDocument> GeoWithinCenter(Expression<Func<TDocument, object>> field, double x, double y, double radius)
        {
            return GeoWithinCenter(new ExpressionFieldDefinition<TDocument>(field), x, y, radius);
        }

        /// <summary>
        /// Creates a geo within center sphere filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>A geo within center sphere filter.</returns>
        public FilterDefinition<TDocument> GeoWithinCenterSphere(FieldDefinition<TDocument> field, double x, double y, double radius)
        {
            return new OperatorFilterDefinition<TDocument>("$geoWithin", field, new BsonDocument("$centerSphere", new BsonArray { new BsonArray { x, y }, radius }));
        }

        /// <summary>
        /// Creates a geo within center sphere filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>A geo within center sphere filter.</returns>
        public FilterDefinition<TDocument> GeoWithinCenterSphere(Expression<Func<TDocument, object>> field, double x, double y, double radius)
        {
            return GeoWithinCenterSphere(new ExpressionFieldDefinition<TDocument>(field), x, y, radius);
        }

        /// <summary>
        /// Creates a geo within polygon filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="points">The points.</param>
        /// <returns>A geo within polygon filter.</returns>
        public FilterDefinition<TDocument> GeoWithinPolygon(FieldDefinition<TDocument> field, double[,] points)
        {
            var arrayOfPoints = new BsonArray(points.GetLength(0));
            for (var i = 0; i < points.GetLength(0); i++)
            {
                arrayOfPoints.Add(new BsonArray(2) { points[i, 0], points[i, 1] });
            }

            return new OperatorFilterDefinition<TDocument>("$geoWithin", field, new BsonDocument("$polygon", arrayOfPoints));
        }

        /// <summary>
        /// Creates a geo within polygon filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="points">The points.</param>
        /// <returns>A geo within polygon filter.</returns>
        public FilterDefinition<TDocument> GeoWithinPolygon(Expression<Func<TDocument, object>> field, double[,] points)
        {
            return GeoWithinPolygon(new ExpressionFieldDefinition<TDocument>(field), points);
        }

        /// <summary>
        /// Creates a greater than filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A greater than filter.</returns>
        public FilterDefinition<TDocument> Gt<TField>(FieldDefinition<TDocument, TField> field, TField value)
        {
            return new OperatorFilterDefinition<TDocument, TField>("$gt", field, value);
        }

        /// <summary>
        /// Creates a greater than filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A greater than filter.</returns>
        public FilterDefinition<TDocument> Gt<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            return Gt(new ExpressionFieldDefinition<TDocument, TField>(field), value);
        }

        /// <summary>
        /// Creates a greater than or equal filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A greater than or equal filter.</returns>
        public FilterDefinition<TDocument> Gte<TField>(FieldDefinition<TDocument, TField> field, TField value)
        {
            return new OperatorFilterDefinition<TDocument, TField>("$gte", field, value);
        }

        /// <summary>
        /// Creates a greater than or equal filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A greater than or equal filter.</returns>
        public FilterDefinition<TDocument> Gte<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            return Gte(new ExpressionFieldDefinition<TDocument, TField>(field), value);
        }

        /// <summary>
        /// Creates an in filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>An in filter.</returns>
        public FilterDefinition<TDocument> In<TField, TItem>(FieldDefinition<TDocument, TField> field, IEnumerable<TItem> values)
            where TField : IEnumerable<TItem>
        {
            return new ArrayOperatorFilterDefinition<TDocument, TField, TItem>("$in", field, values);
        }

        /// <summary>
        /// Creates an in filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>An in filter.</returns>
        public FilterDefinition<TDocument> In<TItem>(string field, IEnumerable<TItem> values)
        {
            return new ArrayOperatorFilterDefinition<TDocument, IEnumerable<TItem>, TItem>(
                "$in",
                new StringFieldDefinition<TDocument, IEnumerable<TItem>>(field),
                values);
        }

        /// <summary>
        /// Creates an in filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>An in filter.</returns>
        public FilterDefinition<TDocument> In<TField, TItem>(Expression<Func<TDocument, TField>> field, IEnumerable<TItem> values)
            where TField : IEnumerable<TItem>
        {
            return In(new ExpressionFieldDefinition<TDocument, TField>(field), values);
        }

        /// <summary>
        /// Creates a less than filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A less than filter.</returns>
        public FilterDefinition<TDocument> Lt<TField>(FieldDefinition<TDocument, TField> field, TField value)
        {
            return new OperatorFilterDefinition<TDocument, TField>("$lt", field, value);
        }

        /// <summary>
        /// Creates a less than filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A less than filter.</returns>
        public FilterDefinition<TDocument> Lt<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            return Lt(new ExpressionFieldDefinition<TDocument, TField>(field), value);
        }

        /// <summary>
        /// Creates a less than or equal filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A less than or equal filter.</returns>
        public FilterDefinition<TDocument> Lte<TField>(FieldDefinition<TDocument, TField> field, TField value)
        {
            return new OperatorFilterDefinition<TDocument, TField>("$lte", field, value);
        }

        /// <summary>
        /// Creates a less than or equal filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A less than or equal filter.</returns>
        public FilterDefinition<TDocument> Lte<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            return Lte(new ExpressionFieldDefinition<TDocument, TField>(field), value);
        }

        /// <summary>
        /// Creates a modulo filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="modulus">The modulus.</param>
        /// <param name="remainder">The remainder.</param>
        /// <returns>A modulo filter.</returns>
        public FilterDefinition<TDocument> Mod(FieldDefinition<TDocument> field, long modulus, long remainder)
        {
            return new OperatorFilterDefinition<TDocument>("$mod", field, new BsonArray { modulus, remainder });
        }

        /// <summary>
        /// Creates a modulo filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="modulus">The modulus.</param>
        /// <param name="remainder">The remainder.</param>
        /// <returns>A modulo filter.</returns>
        public FilterDefinition<TDocument> Mod(Expression<Func<TDocument, object>> field, long modulus, long remainder)
        {
            return Mod(new ExpressionFieldDefinition<TDocument>(field), modulus, remainder);
        }

        /// <summary>
        /// Creates a not equal filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A not equal filter.</returns>
        public FilterDefinition<TDocument> Ne<TField>(FieldDefinition<TDocument, TField> field, TField value)
        {
            return new OperatorFilterDefinition<TDocument, TField>("$ne", field, value);
        }

        /// <summary>
        /// Creates a not equal filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>A not equal filter.</returns>
        public FilterDefinition<TDocument> Ne<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            return Ne(new ExpressionFieldDefinition<TDocument, TField>(field), value);
        }

        /// <summary>
        /// Creates a near filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near filter.</returns>
        public FilterDefinition<TDocument> Near(FieldDefinition<TDocument> field, double x, double y, double? maxDistance = null, double? minDistance = null)
        {
            var document = new BsonDocument
            {
                { "$near", new BsonArray { x, y } },
                { "$maxDistance", () => maxDistance.Value, maxDistance.HasValue },
                { "$minDistance", () => minDistance.Value, minDistance.HasValue }
            };

            return new SimpleFilterDefinition<TDocument>(field, document);
        }

        /// <summary>
        /// Creates a near filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near filter.</returns>
        public FilterDefinition<TDocument> Near(Expression<Func<TDocument, object>> field, double x, double y, double? maxDistance = null, double? minDistance = null)
        {
            return Near(new ExpressionFieldDefinition<TDocument>(field), x, y, maxDistance, minDistance);
        }

        /// <summary>
        /// Creates a near filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="point">The geometry.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near filter.</returns>
        public FilterDefinition<TDocument> Near<TCoordinates>(FieldDefinition<TDocument> field, GeoJsonPoint<TCoordinates> point, double? maxDistance = null, double? minDistance = null)
            where TCoordinates : GeoJsonCoordinates
        {
            return new NearFilterDefinition<TDocument, TCoordinates>(field, point, false, maxDistance, minDistance);
        }

        /// <summary>
        /// Creates a near filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="point">The geometry.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near filter.</returns>
        public FilterDefinition<TDocument> Near<TCoordinates>(Expression<Func<TDocument, object>> field, GeoJsonPoint<TCoordinates> point, double? maxDistance = null, double? minDistance = null)
            where TCoordinates : GeoJsonCoordinates
        {
            return Near(new ExpressionFieldDefinition<TDocument>(field), point, maxDistance, minDistance);
        }

        /// <summary>
        /// Creates a near sphere filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near sphere filter.</returns>
        public FilterDefinition<TDocument> NearSphere(FieldDefinition<TDocument> field, double x, double y, double? maxDistance = null, double? minDistance = null)
        {
            var document = new BsonDocument
            {
                { "$nearSphere", new BsonArray { x, y } },
                { "$maxDistance", () => maxDistance.Value, maxDistance.HasValue },
                { "$minDistance", () => minDistance.Value, minDistance.HasValue }
            };

            return new SimpleFilterDefinition<TDocument>(field, document);
        }

        /// <summary>
        /// Creates a near sphere filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near sphere filter.</returns>
        public FilterDefinition<TDocument> NearSphere(Expression<Func<TDocument, object>> field, double x, double y, double? maxDistance = null, double? minDistance = null)
        {
            return NearSphere(new ExpressionFieldDefinition<TDocument>(field), x, y, maxDistance, minDistance);
        }

        /// <summary>
        /// Creates a near sphere filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="point">The geometry.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near sphere filter.</returns>
        public FilterDefinition<TDocument> NearSphere<TCoordinates>(FieldDefinition<TDocument> field, GeoJsonPoint<TCoordinates> point, double? maxDistance = null, double? minDistance = null)
            where TCoordinates : GeoJsonCoordinates
        {
            return new NearFilterDefinition<TDocument, TCoordinates>(field, point, true, maxDistance, minDistance);
        }

        /// <summary>
        /// Creates a near sphere filter.
        /// </summary>
        /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="point">The geometry.</param>
        /// <param name="maxDistance">The maximum distance.</param>
        /// <param name="minDistance">The minimum distance.</param>
        /// <returns>A near sphere filter.</returns>
        public FilterDefinition<TDocument> NearSphere<TCoordinates>(Expression<Func<TDocument, object>> field, GeoJsonPoint<TCoordinates> point, double? maxDistance = null, double? minDistance = null)
            where TCoordinates : GeoJsonCoordinates
        {
            return NearSphere(new ExpressionFieldDefinition<TDocument>(field), point, maxDistance, minDistance);
        }

        /// <summary>
        /// Creates a not in filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>A not in filter.</returns>
        public FilterDefinition<TDocument> Nin<TField, TItem>(FieldDefinition<TDocument, TField> field, IEnumerable<TItem> values)
            where TField : IEnumerable<TItem>
        {
            return new ArrayOperatorFilterDefinition<TDocument, TField, TItem>("$nin", field, values);
        }

        /// <summary>
        /// Creates a not in filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>A not in filter.</returns>
        public FilterDefinition<TDocument> Nin<TItem>(string field, IEnumerable<TItem> values)
        {
            return new ArrayOperatorFilterDefinition<TDocument, IEnumerable<TItem>, TItem>(
                "$nin",
                new StringFieldDefinition<TDocument, IEnumerable<TItem>>(field),
                values);
        }

        /// <summary>
        /// Creates a not in filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="values">The values.</param>
        /// <returns>A not in filter.</returns>
        public FilterDefinition<TDocument> Nin<TField, TItem>(Expression<Func<TDocument, TField>> field, IEnumerable<TItem> values)
            where TField : IEnumerable<TItem>
        {
            return Nin(new ExpressionFieldDefinition<TDocument, TField>(field), values);
        }

        /// <summary>
        /// Creates a not filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>A not filter.</returns>
        public FilterDefinition<TDocument> Not(FilterDefinition<TDocument> filter)
        {
            return new NotFilterDefinition<TDocument>(filter);
        }

        /// <summary>
        /// Creates an or filter.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>An or filter.</returns>
        public FilterDefinition<TDocument> Or(params FilterDefinition<TDocument>[] filters)
        {
            return Or((IEnumerable<FilterDefinition<TDocument>>)filters);
        }

        /// <summary>
        /// Creates an or filter.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>An or filter.</returns>
        public FilterDefinition<TDocument> Or(IEnumerable<FilterDefinition<TDocument>> filters)
        {
            return new OrFilterDefinition<TDocument>(filters);
        }

        /// <summary>
        /// Creates a regular expression filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="regex">The regex.</param>
        /// <returns>A regular expression filter.</returns>
        public FilterDefinition<TDocument> Regex(FieldDefinition<TDocument> field, BsonRegularExpression regex)
        {
            return new SimpleFilterDefinition<TDocument>(field, regex);
        }

        /// <summary>
        /// Creates a regular expression filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="regex">The regex.</param>
        /// <returns>A regular expression filter.</returns>
        public FilterDefinition<TDocument> Regex(Expression<Func<TDocument, object>> field, BsonRegularExpression regex)
        {
            return Regex(new ExpressionFieldDefinition<TDocument>(field), regex);
        }

        /// <summary>
        /// Creates a size filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size filter.</returns>
        public FilterDefinition<TDocument> Size(FieldDefinition<TDocument> field, int size)
        {
            return new OperatorFilterDefinition<TDocument>("$size", field, size);
        }

        /// <summary>
        /// Creates a size filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size filter.</returns>
        public FilterDefinition<TDocument> Size(Expression<Func<TDocument, object>> field, int size)
        {
            return Size(new ExpressionFieldDefinition<TDocument>(field), size);
        }

        /// <summary>
        /// Creates a size greater than filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size greater than filter.</returns>
        public FilterDefinition<TDocument> SizeGt(FieldDefinition<TDocument> field, int size)
        {
            return new ArrayIndexExistsFilterDefinition<TDocument>(field, size, true);
        }

        /// <summary>
        /// Creates a size greater than filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size greater than filter.</returns>
        public FilterDefinition<TDocument> SizeGt(Expression<Func<TDocument, object>> field, int size)
        {
            return SizeGt(new ExpressionFieldDefinition<TDocument>(field), size);
        }

        /// <summary>
        /// Creates a size greater than or equal filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size greater than or equal filter.</returns>
        public FilterDefinition<TDocument> SizeGte(FieldDefinition<TDocument> field, int size)
        {
            return new ArrayIndexExistsFilterDefinition<TDocument>(field, size - 1, true);
        }

        /// <summary>
        /// Creates a size greater than or equal filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size greater than or equal filter.</returns>
        public FilterDefinition<TDocument> SizeGte(Expression<Func<TDocument, object>> field, int size)
        {
            return SizeGte(new ExpressionFieldDefinition<TDocument>(field), size);
        }

        /// <summary>
        /// Creates a size less than filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size less than filter.</returns>
        public FilterDefinition<TDocument> SizeLt(FieldDefinition<TDocument> field, int size)
        {
            return new ArrayIndexExistsFilterDefinition<TDocument>(field, size - 1, false);
        }

        /// <summary>
        /// Creates a size less than filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size less than filter.</returns>
        public FilterDefinition<TDocument> SizeLt(Expression<Func<TDocument, object>> field, int size)
        {
            return SizeLt(new ExpressionFieldDefinition<TDocument>(field), size);
        }

        /// <summary>
        /// Creates a size less than or equal filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size less than or equal filter.</returns>
        public FilterDefinition<TDocument> SizeLte(FieldDefinition<TDocument> field, int size)
        {
            return new ArrayIndexExistsFilterDefinition<TDocument>(field, size, false);
        }

        /// <summary>
        /// Creates a size less than or equal filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="size">The size.</param>
        /// <returns>A size less than or equal filter.</returns>
        public FilterDefinition<TDocument> SizeLte(Expression<Func<TDocument, object>> field, int size)
        {
            return SizeLte(new ExpressionFieldDefinition<TDocument>(field), size);
        }

        /// <summary>
        /// Creates a text filter.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="language">The language.</param>
        /// <returns>A text filter.</returns>
        public FilterDefinition<TDocument> Text(string search, string language = null)
        {
            var document = new BsonDocument
            {
                { "$search", search },
                { "$language", language, language != null }
            };
            return new BsonDocumentFilterDefinition<TDocument>(new BsonDocument("$text", document));
        }

        /// <summary>
        /// Creates a type filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="type">The type.</param>
        /// <returns>A type filter.</returns>
        public FilterDefinition<TDocument> Type(FieldDefinition<TDocument> field, BsonType type)
        {
            return new OperatorFilterDefinition<TDocument>("$type", field, (int)type);
        }

        /// <summary>
        /// Creates a type filter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="type">The type.</param>
        /// <returns>A type filter.</returns>
        public FilterDefinition<TDocument> Type(Expression<Func<TDocument, object>> field, BsonType type)
        {
            return Type(new ExpressionFieldDefinition<TDocument>(field), type);
        }

        /// <summary>
        /// Creates a filter based on the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An expression filter.</returns>
        public FilterDefinition<TDocument> Where(Expression<Func<TDocument, bool>> expression)
        {
            return new ExpressionFilterDefinition<TDocument>(expression);
        }
    }

    internal sealed class AndFilterDefinition<TDocument> : FilterDefinition<TDocument>
    {
        private readonly List<FilterDefinition<TDocument>> _filters;

        public AndFilterDefinition(IEnumerable<FilterDefinition<TDocument>> filters)
        {
            _filters = Ensure.IsNotNull(filters, "filters").ToList();
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var document = new BsonDocument();

            foreach (var filter in _filters)
            {
                var renderedFilter = filter.Render(documentSerializer, serializerRegistry);
                foreach (var clause in renderedFilter)
                {
                    AddClause(document, clause);
                }
            }

            return document;
        }

        private static void AddClause(BsonDocument document, BsonElement clause)
        {
            if (clause.Name == "$and")
            {
                // flatten out nested $and
                foreach (var item in (BsonArray)clause.Value)
                {
                    foreach (var element in (BsonDocument)item)
                    {
                        AddClause(document, element);
                    }
                }
            }
            else if (document.ElementCount == 1 && document.GetElement(0).Name == "$and")
            {
                ((BsonArray)document[0]).Add(new BsonDocument(clause));
            }
            else if (document.Contains(clause.Name))
            {
                var existingClause = document.GetElement(clause.Name);
                if (existingClause.Value is BsonDocument && clause.Value is BsonDocument)
                {
                    var clauseValue = (BsonDocument)clause.Value;
                    var existingClauseValue = (BsonDocument)existingClause.Value;
                    if (clauseValue.Names.Any(op => existingClauseValue.Contains(op)))
                    {
                        PromoteFilterToDollarForm(document, clause);
                    }
                    else
                    {
                        existingClauseValue.AddRange(clauseValue);
                    }
                }
                else
                {
                    PromoteFilterToDollarForm(document, clause);
                }
            }
            else
            {
                document.Add(clause);
            }
        }

        private static void PromoteFilterToDollarForm(BsonDocument document, BsonElement clause)
        {
            var clauses = new BsonArray();
            foreach (var queryElement in document)
            {
                clauses.Add(new BsonDocument(queryElement));
            }
            clauses.Add(new BsonDocument(clause));
            document.Clear();
            document.Add("$and", clauses);
        }
    }

    internal sealed class ArrayOperatorFilterDefinition<TDocument, TField, TItem> : FilterDefinition<TDocument>
        where TField : IEnumerable<TItem>
    {
        private readonly string _operatorName;
        private readonly FieldDefinition<TDocument, TField> _field;
        private readonly IEnumerable<TItem> _values;

        public ArrayOperatorFilterDefinition(string operatorName, FieldDefinition<TDocument, TField> field, IEnumerable<TItem> values)
        {
            _operatorName = Ensure.IsNotNull(operatorName, operatorName);
            _field = Ensure.IsNotNull(field, "field");
            _values = values;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);

            var arraySerializer = renderedField.FieldSerializer as IBsonArraySerializer;
            if (arraySerializer == null)
            {
                var message = string.Format("The serializer for field '{0}' must implement IBsonArraySerializer.", renderedField.FieldName);
                throw new InvalidOperationException(message);
            }
            var itemSerializer = arraySerializer.GetItemSerializationInfo().Serializer;

            var document = new BsonDocument();
            using (var bsonWriter = new BsonDocumentWriter(document))
            {
                var context = BsonSerializationContext.CreateRoot(bsonWriter);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(renderedField.FieldName);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(_operatorName);
                bsonWriter.WriteStartArray();
                foreach (var value in _values)
                {
                    itemSerializer.Serialize(context, value);
                }
                bsonWriter.WriteEndArray();
                bsonWriter.WriteEndDocument();
                bsonWriter.WriteEndDocument();
            }

            return document;
        }
    }

    internal sealed class ElementMatchFilterDefinition<TDocument, TField, TItem> : FilterDefinition<TDocument>
    {
        private readonly FieldDefinition<TDocument, TField> _field;
        private readonly FilterDefinition<TItem> _filter;

        public ElementMatchFilterDefinition(FieldDefinition<TDocument, TField> field, FilterDefinition<TItem> filter)
        {
            _field = Ensure.IsNotNull(field, "field");
            _filter = filter;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);

            var arraySerializer = renderedField.FieldSerializer as IBsonArraySerializer;
            if (arraySerializer == null)
            {
                var message = string.Format("The serializer for field '{0}' must implement IBsonArraySerializer.", renderedField.FieldName);
                throw new InvalidOperationException(message);
            }
            var itemSerializer = (IBsonSerializer<TItem>)arraySerializer.GetItemSerializationInfo().Serializer;

            var renderedFilter = _filter.Render(itemSerializer, serializerRegistry);

            return new BsonDocument(renderedField.FieldName, new BsonDocument("$elemMatch", renderedFilter));
        }
    }

    internal sealed class ScalarElementMatchFilterDefinition<TDocument> : FilterDefinition<TDocument>
    {
        private readonly FilterDefinition<TDocument> _elementMatchFilter;

        public ScalarElementMatchFilterDefinition(FilterDefinition<TDocument> elementMatchFilter)
        {
            _elementMatchFilter = Ensure.IsNotNull(elementMatchFilter, "elementMatchFilter");
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var document = _elementMatchFilter.Render(documentSerializer, serializerRegistry);

            var elemMatch = (BsonDocument)document[0]["$elemMatch"];
            BsonValue condition;
            if (elemMatch.TryGetValue("", out condition))
            {
                elemMatch.Remove("");

                if (condition is BsonDocument)
                {
                    var nestedDocument = (BsonDocument)condition;
                    foreach (var element in nestedDocument)
                    {
                        elemMatch.Add(element);
                    }
                }
                else if (condition is BsonRegularExpression)
                {
                    elemMatch.Add("$regex", condition);
                }
                else
                {
                    elemMatch.Add("$eq", condition);
                }
            }
            
            return document;
        }
    }

    internal sealed class GeometryOperatorFilterDefinition<TDocument, TCoordinates> : FilterDefinition<TDocument>
        where TCoordinates : GeoJsonCoordinates
    {
        private readonly string _operatorName;
        private readonly FieldDefinition<TDocument> _field;
        private readonly GeoJsonGeometry<TCoordinates> _geometry;

        public GeometryOperatorFilterDefinition(string operatorName, FieldDefinition<TDocument> field, GeoJsonGeometry<TCoordinates> geometry)
        {
            _operatorName = Ensure.IsNotNull(operatorName, "operatorName");
            _field = Ensure.IsNotNull(field, "field");
            _geometry = Ensure.IsNotNull(geometry, "geometry");
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);

            var document = new BsonDocument();
            using (var bsonWriter = new BsonDocumentWriter(document))
            {
                var context = BsonSerializationContext.CreateRoot(bsonWriter);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(renderedField);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(_operatorName);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName("$geometry");
                serializerRegistry.GetSerializer<GeoJsonGeometry<TCoordinates>>().Serialize(context, _geometry);
                bsonWriter.WriteEndDocument();
                bsonWriter.WriteEndDocument();
                bsonWriter.WriteEndDocument();
            }

            return document;
        }
    }

    internal sealed class NearFilterDefinition<TDocument, TCoordinates> : FilterDefinition<TDocument>
        where TCoordinates : GeoJsonCoordinates
    {
        private readonly FieldDefinition<TDocument> _field;
        private readonly GeoJsonPoint<TCoordinates> _point;
        private readonly double? _maxDistance;
        private readonly double? _minDistance;
        private readonly bool _spherical;

        public NearFilterDefinition(FieldDefinition<TDocument> field, GeoJsonPoint<TCoordinates> point, bool spherical, double? maxDistance = null, double? minDistance = null)
        {
            _field = Ensure.IsNotNull(field, "field");
            _point = Ensure.IsNotNull(point, "point");
            _spherical = spherical;
            _maxDistance = maxDistance;
            _minDistance = minDistance;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);

            var document = new BsonDocument();
            using (var bsonWriter = new BsonDocumentWriter(document))
            {
                var context = BsonSerializationContext.CreateRoot(bsonWriter);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(renderedField);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(_spherical ? "$nearSphere" : "$near");
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName("$geometry");
                serializerRegistry.GetSerializer<GeoJsonPoint<TCoordinates>>().Serialize(context, _point);
                if (_maxDistance.HasValue)
                {
                    bsonWriter.WriteName("$maxDistance");
                    bsonWriter.WriteDouble(_maxDistance.Value);
                }
                if (_minDistance.HasValue)
                {
                    bsonWriter.WriteName("$minDistance");
                    bsonWriter.WriteDouble(_minDistance.Value);
                }
                bsonWriter.WriteEndDocument();
                bsonWriter.WriteEndDocument();
                bsonWriter.WriteEndDocument();
            }

            return document;
        }
    }

    internal sealed class NotFilterDefinition<TDocument> : FilterDefinition<TDocument>
    {
        private readonly FilterDefinition<TDocument> _filter;

        public NotFilterDefinition(FilterDefinition<TDocument> filter)
        {
            _filter = Ensure.IsNotNull(filter, "filter");
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedFilter = _filter.Render(documentSerializer, serializerRegistry);

            if (renderedFilter.ElementCount == 1)
            {
                return NegateSingleElementFilter(renderedFilter, renderedFilter.GetElement(0));
            }

            return NegateArbitraryFilter(renderedFilter);
        }

        private static BsonDocument NegateArbitraryFilter(BsonDocument filter)
        {
            // $not only works as a meta operator on a single operator so simulate Not using $nor
            return new BsonDocument("$nor", new BsonArray { filter });
        }

        private static BsonDocument NegateSingleElementFilter(BsonDocument filter, BsonElement element)
        {
            if (element.Name[0] == '$')
            {
                return NegateSingleElementTopLevelOperatorFilter(filter, element);
            }

            if (element.Value is BsonDocument)
            {
                var selector = (BsonDocument)element.Value;
                if (selector.ElementCount >= 1)
                {
                    var operatorName = selector.GetElement(0).Name;
                    if (operatorName[0] == '$' && operatorName != "$ref")
                    {
                        if (selector.ElementCount == 1)
                        {
                            return NegateSingleFieldOperatorFilter(element.Name, selector.GetElement(0));
                        }

                        return NegateArbitraryFilter(filter);
                    }
                }
            }

            if (element.Value is BsonRegularExpression)
            {
                return new BsonDocument(element.Name, new BsonDocument("$not", element.Value));
            }

            return new BsonDocument(element.Name, new BsonDocument("$ne", element.Value));
        }

        private static BsonDocument NegateSingleFieldOperatorFilter(string field, BsonElement element)
        {
            switch (element.Name)
            {
                case "$exists":
                    return new BsonDocument(field, new BsonDocument("$exists", !element.Value.ToBoolean()));
                case "$in":
                    return new BsonDocument(field, new BsonDocument("$nin", (BsonArray)element.Value));
                case "$ne":
                case "$not":
                    return new BsonDocument(field, element.Value);
                case "$nin":
                    return new BsonDocument(field, new BsonDocument("$in", (BsonArray)element.Value));
                default:
                    return new BsonDocument(field, new BsonDocument("$not", new BsonDocument(element)));
            }
        }

        private static BsonDocument NegateSingleElementTopLevelOperatorFilter(BsonDocument filter, BsonElement element)
        {
            switch (element.Name)
            {
                case "$or":
                    return new BsonDocument("$nor", element.Value);
                case "$nor":
                    return new BsonDocument("$or", element.Value);
                default:
                    return NegateArbitraryFilter(filter);
            }
        }
    }

    internal sealed class OperatorFilterDefinition<TDocument> : FilterDefinition<TDocument>
    {
        private readonly string _operatorName;
        private readonly FieldDefinition<TDocument> _field;
        private readonly BsonValue _value;

        public OperatorFilterDefinition(string operatorName, FieldDefinition<TDocument> field, BsonValue value)
        {
            _operatorName = Ensure.IsNotNull(operatorName, operatorName);
            _field = Ensure.IsNotNull(field, "field");
            _value = value;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);
            return new BsonDocument(renderedField, new BsonDocument(_operatorName, _value));
        }
    }

    internal sealed class OperatorFilterDefinition<TDocument, TField> : FilterDefinition<TDocument>
    {
        private readonly string _operatorName;
        private readonly FieldDefinition<TDocument, TField> _field;
        private readonly TField _value;

        public OperatorFilterDefinition(string operatorName, FieldDefinition<TDocument, TField> field, TField value)
        {
            _operatorName = Ensure.IsNotNull(operatorName, operatorName);
            _field = Ensure.IsNotNull(field, "field");
            _value = value;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);

            var document = new BsonDocument();
            using (var bsonWriter = new BsonDocumentWriter(document))
            {
                var context = BsonSerializationContext.CreateRoot(bsonWriter);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(renderedField.FieldName);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(_operatorName);
                renderedField.FieldSerializer.Serialize(context, _value);
                bsonWriter.WriteEndDocument();
                bsonWriter.WriteEndDocument();
            }

            return document;
        }
    }

    internal sealed class OrFilterDefinition<TDocument> : FilterDefinition<TDocument>
    {
        private readonly List<FilterDefinition<TDocument>> _filters;

        public OrFilterDefinition(IEnumerable<FilterDefinition<TDocument>> filters)
        {
            _filters = Ensure.IsNotNull(filters, "filters").ToList();
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var clauses = new BsonArray();

            foreach (var filter in _filters)
            {
                var renderedFilter = filter.Render(documentSerializer, serializerRegistry);
                AddClause(clauses, renderedFilter);
            }

            return new BsonDocument("$or", clauses);
        }

        private static void AddClause(BsonArray clauses, BsonDocument filter)
        {
            if (filter.ElementCount == 1 && filter.GetElement(0).Name == "$or")
            {
                // flatten nested $or
                clauses.AddRange((BsonArray)filter[0]);
            }
            else
            {
                // we could shortcut the user's query if there are no elements in the filter, but
                // I'd rather be literal and let them discover the problem on their own.
                clauses.Add(filter);
            }
        }
    }

    internal sealed class SimpleFilterDefinition<TDocument> : FilterDefinition<TDocument>
    {
        private readonly FieldDefinition<TDocument> _field;
        private readonly BsonValue _value;

        public SimpleFilterDefinition(FieldDefinition<TDocument> field, BsonValue value)
        {
            _field = Ensure.IsNotNull(field, "field");
            _value = value;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);
            return new BsonDocument(renderedField, _value);
        }
    }

    internal sealed class SimpleFilterDefinition<TDocument, TField> : FilterDefinition<TDocument>
    {
        private readonly FieldDefinition<TDocument, TField> _field;
        private readonly TField _value;

        public SimpleFilterDefinition(FieldDefinition<TDocument, TField> field, TField value)
        {
            _field = Ensure.IsNotNull(field, "field");
            _value = value;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry);

            var document = new BsonDocument();
            using (var bsonWriter = new BsonDocumentWriter(document))
            {
                var context = BsonSerializationContext.CreateRoot(bsonWriter);
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteName(renderedField.FieldName);
                renderedField.FieldSerializer.Serialize(context, _value);
                bsonWriter.WriteEndDocument();
            }

            return document;
        }
    }

    internal sealed class ArrayIndexExistsFilterDefinition<TDocument> : FilterDefinition<TDocument>
    {
        private readonly FieldDefinition<TDocument> _field;
        private readonly int _index;
        private readonly bool _exists;

        public ArrayIndexExistsFilterDefinition(FieldDefinition<TDocument> field, int index, bool exists)
        {
            _field = Ensure.IsNotNull(field, "field");
            _index = index;
            _exists = exists;
        }

        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            var renderedField = _field.Render(documentSerializer, serializerRegistry) + "." + _index;
            return new BsonDocument(renderedField, new BsonDocument("$exists", _exists));
        }
    }
}

﻿using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.Domain.Attributes;

/// <summary>
/// Represents an attribute used to provide metadata for the entity's property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NetForgePropertyAttribute : Attribute
{
    /// <inheritdoc cref="PropertyMetadataBase.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsHiddenFromListView"/>
    public bool IsHiddenFromListView { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsHiddenFromCreate"/>
    public bool IsHiddenFromCreate { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsHiddenFromDetails"/>
    public bool IsHiddenFromDetails { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsExcludedFromQuery"/>
    public bool IsExcludedFromQuery { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.DisplayName"/>
    public string? DisplayName { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.Order"/>
    /// <remarks>
    /// Default value is <c>-1</c> so it must not be used.
    /// </remarks>
    public int Order { get; set; } = -1;

    /// <inheritdoc cref="PropertyMetadataBase.FormOrder"/>
    /// <remarks>
    /// Default value is <c>-1</c> so it must not be used.
    /// </remarks>
    public int FormOrder { get; set; } = -1;

    /// <inheritdoc cref="PropertyMetadataBase.DisplayFormat"/>
    public string? DisplayFormat { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.EmptyValueDisplay"/>
    public string EmptyValueDisplay { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadataBase.DisplayAsHtml"/>
    public bool DisplayAsHtml { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.SearchType"/>
    public SearchType SearchType { get; set; } = SearchType.None;

    /// <inheritdoc cref="PropertyMetadataBase.IsSortable"/>
    public bool IsSortable { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsRichTextField"/>
    public bool IsRichTextField { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsReadOnly"/>
    public bool IsReadOnly { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.TruncationMaxCharacters"/>
    public int TruncationMaxCharacters { get; set; }
}

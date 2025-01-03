﻿namespace SiteVantagePro_API.Domain.Common;
public class TableConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    public TableConfiguration(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="schema">The schema.</param>
    public TableConfiguration(string name, string schema)
    {
        Name = name;
        Schema = schema;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the schema.
    /// </summary>
    /// <value>
    /// The schema.
    /// </value>
    public string? Schema { get; set; }
}


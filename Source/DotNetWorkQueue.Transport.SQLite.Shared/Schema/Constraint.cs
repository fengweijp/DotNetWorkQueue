﻿using System.Collections.Generic;
using DotNetWorkQueue.Exceptions;

namespace DotNetWorkQueue.Transport.SQLite.Shared.Schema
{
    /// <summary>
    /// Represents a SQLite constraint
    /// </summary>
    public class Constraint
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        public Constraint()
        {
        } 

        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="column">The column.</param>
        public Constraint(string name, ConstraintType type, string column) 
        {
			Name = name;
			Type = type;
            Columns = new List<string> {column};
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="columns">The columns.</param>
        public Constraint(string name, ConstraintType type, List<string> columns)
        {
            Name = name;
            Type = type;
            Columns = columns;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public List<string> Columns { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the table that this constraint belongs to
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public TableInfo Table { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public ConstraintType Type { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Constraint"/> is unique.
        /// </summary>
        /// <value>
        ///   <c>true</c> if unique; otherwise, <c>false</c>.
        /// </value>
        public bool Unique { get; set; }
        #endregion

        #region Script helpers
        /// <summary>
        /// Gets the unique text.
        /// </summary>
        /// <value>
        /// The unique text.
        /// </value>
		private string UniqueText => !Unique ? "" : "UNIQUE";

        #endregion

        #region Scripting
        /// <summary>
        /// Translates this constraint into a SQL script
        /// </summary>
        /// <returns></returns>
        public string Script()
        {
            switch (Type)
            {
                case ConstraintType.Constraint:
                case ConstraintType.Index:
                    return $"CREATE {UniqueText} INDEX {Name}{Table.Name} ON {Table.Name} ({string.Join(", ", Columns.ToArray())});";
                case ConstraintType.PrimaryKey:
                    return $"PRIMARY KEY ({string.Join("], [", Columns.ToArray())})";
            }
            throw new DotNetWorkQueueException($"Unhandled type of {Type}");
        }

        #endregion

        #region Clone
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Constraint Clone()
        {
            var temp = new List<string>();
            temp.AddRange(Columns);
            var rc = new Constraint(Name, Type, temp)
            {
                Unique = Unique
            };
            return rc;
        }
        #endregion
    }
    /// <summary>
    /// The type of the constraint
    /// </summary>
    public enum ConstraintType
    {
        /// <summary>
        /// index
        /// </summary>
        Index,
        /// <summary>
        /// primary key
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// constraint
        /// </summary>
        Constraint
    }
}
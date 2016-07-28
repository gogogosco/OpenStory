using System;

namespace OpenStory.Framework.Model.Common
{
    /// <summary>
    /// Represents a stack of identical items.
    /// </summary>
    /// <typeparam name="TItemInfo">The <see cref="ItemInfo"/> type of the items in this cluster.</typeparam>
    public abstract class ItemCluster<TItemInfo>
        where TItemInfo : ItemInfo
    {
        /// <summary>
        /// Gets the prototype of the items in this cluster.
        /// </summary>
        public TItemInfo Prototype { get; }

        /// <summary>
        /// Gets the number of items in this cluster.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets whether the cluster is empty.
        /// </summary>
        public bool IsEmpty => this.Quantity == 0;

        /// <summary>
        /// Gets the identifier for the prototype of this cluster.
        /// </summary>
        public int ItemId => this.Prototype.ItemId;

        /// <summary>
        /// Gets the item capacity of this cluster.
        /// </summary>
        public virtual int ClusterCapacity => this.Prototype.ClusterCapacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCluster{TItemInfo}"/> class.
        /// </summary>
        /// <param name="prototype">The <see cref="ItemInfo"/> to use as an item prototype.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="prototype"/> is <see langword="null"/>.
        /// </exception>
        protected ItemCluster(TItemInfo prototype)
        {
            if (prototype == null)
            {
                throw new ArgumentNullException(nameof(prototype));
            }

            this.Prototype = prototype;
            this.Quantity = 0;
        }

        /// <summary>
        /// Attempts to merge the specified ItemCluster with the current.
        /// </summary>
        /// <param name="other">The ItemCluster to merge.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="other"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="other"/> is for a different item prototype than the current instance.
        /// </exception>
        /// <returns>the number of items that were carried over to the current cluster.</returns>
        public int MergeWith(ItemCluster<TItemInfo> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            // Note: This is actually not quite necessary,
            // since Prototypes are immutable and only supplied from the cache,
            // we could go with just identity check.
            if (!this.Prototype.Equals(other.Prototype))
            {
                throw new ArgumentException(ModelStrings.DifferentItemClusterPrototype, nameof(other));
            }

            int freeSpace = this.ClusterCapacity - this.Quantity;
            int availableQuantity = Math.Min(freeSpace, other.Quantity);
            this.Quantity += availableQuantity;
            other.Quantity -= availableQuantity;

            return availableQuantity;
        }

        /// <summary>
        /// Gets the number of items that can be merged from the specified cluster into the current.
        /// </summary>
        /// <param name="other">The cluster to merge from.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="other"/> is <see langword="null"/>.
        /// </exception>
        /// <returns>
        /// <see langword="true"/> if the cluster is compatible and there is remaining capacity to contain more items; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanMergeWith(ItemCluster<TItemInfo> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (!this.Prototype.Equals(other.Prototype))
            {
                return false;
            }

            if (this.Quantity == this.ClusterCapacity)
            {
                return false;
            }

            return true;
        }
    }
}

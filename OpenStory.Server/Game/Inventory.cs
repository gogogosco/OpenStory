﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a container for items.
    /// </summary>
    public abstract class ItemContainer<TItemCluster> 
        where TItemCluster : ItemCluster
    {
        /// <summary>
        /// Gets the maximum capacity for this container.
        /// </summary>
        public abstract int MaxCapacity { get; }

        /// <summary>
        /// Gets the current capacity of this container.
        /// </summary>
        public int SlotCapacity { get; protected set; }

        /// <summary>
        /// Gets the number of free slots in this container.
        /// </summary>
        public int FreeSlots
        {
            get { return this.SlotCapacity - this.slots.Count; }
        }

        private Dictionary<int, ItemCluster> slots;

        /// <summary>
        /// Initializes a new ItemContainer instance with the specified slot capacity.
        /// </summary>
        /// <param name="slotCapacity">The slot capacity for this container.</param>
        protected ItemContainer(int slotCapacity)
        {
            this.SlotCapacity = slotCapacity;
            this.slots = new Dictionary<int, ItemCluster>(slotCapacity);
        }

        /// <summary>
        /// Expands the item container with the specified number of slots.
        /// </summary>
        /// <param name="slots"></param>
        public void Expand(int slots)
        {
            int newCapacity = this.SlotCapacity + slots;
            if (newCapacity > this.MaxCapacity)
            {
                throw new ArgumentException("You cannot expand this container past its max capacity.", "slots");
            }

            this.SlotCapacity = newCapacity;
        }
    }
}